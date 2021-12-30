using ProjectW.DB;
using ProjectW.Object;
using ProjectW.UI;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectW.Object
{
    /// <summary>
    /// 몬스터는 인게임 도중에 빈번히 생성되고, 제거되므로 오브젝트 풀을 이용한다.
    /// -> 오브젝트를 풀을 이용할 객체에게는 오브젝트 풀 기능을 수행할 수 있게 IPoolableObject 인터페이스를 상속시킨다.
    /// </summary>
    public class Monster : Actor, IPoolableObject
    {
        public BoMonster boMonster;

        private NavMeshAgent agent;
        private NavMeshPath path;

        // 재사용 가능 여부를 나타내는 프로퍼티, 처음 생성 시 재사용 가능한 상태로 설정
        public bool CanRecycle { get; set; } = true;

        public override void Initialize(BoActor boActor)
        {
            base.Initialize(boActor);
            // 접근의 편의를 위해 BoMonster 형태로 캐스팅하여 담아둠
            boMonster = boActor as BoMonster;

            // 초기 스텟 설정
            SetStats();
            // 정찰 대기 시간 초기화
            InitPatrolWaitTime();

            // 처음 목적지를 몬스터 자기 자신의 위치로 설정한다.
            // 이유? 처음에 목적지 데이터가 비어있음 (기본 값인 0,0,0이 목적지가 됨)
            // 목적지를 몬스터의 현재 위치로 설정함으로써, 목적지 도착 체크 로직을 통해
            // 바로 도착 상태로 인식하게 하여, 몬스터의 새로운 목적지를 설정할 수 있게 하기 위해서
            boMonster.desPos = transform.position;
        }

        protected override void Start()
        {
            base.Start();

            agent = GetComponent<NavMeshAgent>();
            path = new NavMeshPath();
        }

        public override void SetStats()
        {
            if (boMonster == null)
                return;

            boMonster.level = 1f;
            boMonster.actorType = Define.Actor.Type.Monster;
            boMonster.moveSpeed = boMonster.sdMonster.moveSpeed;
            boMonster.currentHp = boMonster.maxHp = boMonster.sdMonster.maxHp;
            boMonster.currentMp = boMonster.maxMp = boMonster.sdMonster.maxMp;
            boMonster.atkRange = boMonster.sdMonster.atkRange;
            boMonster.atkInterval = boMonster.sdMonster.atkInterval;
            boMonster.atk = boMonster.sdMonster.atk;
            boMonster.def = boMonster.sdMonster.def;
        }

        public override void ActorUpdate()
        {
            CheckDetection();

            base.ActorUpdate();
        }

        public override void MoveUpdate()
        {
            // 몬스터가 움직일 수 있는 상태인지 확인
            var isMove = GetMovement();

            // 움직일 수 있다면
            if (isMove)
            {
                // 정찰 중이거나 타겟을 추적중이므로 이동상태로 변경
                SetState(Define.Actor.State.Walk);

                // 속력 설정
                agent.speed = boMonster.moveSpeed;
                // 목적지 설정
                agent.SetDestination(boMonster.desPos);
            }
            // 움직일 수 없다면
            else
            {
                // 정찰 대기상태이므로 대기 상태로 변경
                SetState(Define.Actor.State.Idle);
            }
        }

        /// <summary>
        /// 상황에 따라 움직임을 설정하고 움직임 여부를 반환
        /// </summary>
        /// <returns></returns>
        private bool GetMovement()
        {
            // 타겟에 대한 정보를 가지고 있는지?
            if (attackController.hasTarget)
            {
                // 공격 범위 내에 캐릭터가 있다면 true, 아니라면 false
                // !canAtk은 공격이 가능하면 움직임 여부를 false, 공격 불가능한 상태라면 true
                return !attackController.canAtk;
            }

            // 상태가 대기상태라면
            if (State == Define.Actor.State.Idle)
            {
                // 정찰 후 몬스터는 해당 위치에서 대기함
                // 이 때 대기시간이 끝났다면
                if (Time.time - boMonster.startPatrolWaitTime > boMonster.patrolWaitTime)
                {
                    // 대기가 끝났으므로 이동할 수 있게 true 반환
                    return true;
                }

                // 대기시간이 지나지 않았으므로 이동할 수 없게 false 반환
                return false;
            }

            // 정찰 위치에 도착했다면 정찰 위치를 변경
            // false를 반환시켜서 바로 이동하는 것이 아닌 설정된 대기시간 동안 대기 후 이동하도록

            var distance = (boMonster.desPos - transform.position).magnitude;

            if (distance < agent.stoppingDistance)
            {
                // 목적지에 도착했으므로 정찰 대기시간 초기화
                InitPatrolWaitTime();

                // 정찰 위치 변경
                ChangeDestPos();
                // 정찰 위치에 도달했으므로, 움직일 수 없도록 false 반환
                return false;
            }

            // 이 코드가 실행되었다는 것은 아직 정찰 중이라는 뜻
            // -> 정찰 시 이동을 해야하므로 true 반환
            return true;

        }

        /// <summary>
        /// 정찰 대기시간 초기화
        /// </summary>
        private void InitPatrolWaitTime()
        {
            boMonster.startPatrolWaitTime = Time.time;
            // 정찰 대기시간을 매 번 랜덤하게 변경
            boMonster.patrolWaitTime = Random.Range(Define.Monster.MinPatrolWaitTime, Define.Monster.MaxPatrolwaitTime);
        }

        /// <summary>
        /// 정찰 위치를 변경하는 기능
        /// </summary>
        private void ChangeDestPos()
        {
            // 몬스터마다 스폰 구역이 다름으로, 몬스터의 인덱스 값을 넘겨 해당 몬스터의
            // 스폰 구역 내에서 랜던함 위치를 받아온다.
            boMonster.desPos = InGameManager.Instance.GetRandPosInArea(boMonster.sdMonster.index);

            // 위의 과정을 통해 스폰 구역 내에서 랜덤한 위치를 뽑았음
            // 예외처리
            // 1. 랜덤 위치가 네비메쉬 경로 상에 존재하는지 확인
            // 2. 랜덤 위치가 네비메쉬 경로상에 존재하지만, 목적지에 도착할 수 없는 경우인지를 확인

            var isExist = agent.CalculatePath(boMonster.desPos, path);

            // 해당 위치가 네비메쉬에 존재하지 않는 경우
            if (!isExist)
            {
                // 재귀 호출을 통해 랜덤위치를 새로 뽑고 경로를 다시 검사
                ChangeDestPos();
            }
            // 해당 위치가 네비메쉬에 존재하지만, 목적지에 도착할  수 없는 경우
            else if (path.status == NavMeshPathStatus.PathPartial)
            {
                ChangeDestPos();
            }
        }

        /// <summary>
        /// 적 감지 범위 내에 적이 존재하는지 체크
        /// </summary>
        private void CheckDetection()
        {
            // 기획데이터에 설정된 감지범위 값으로 x,y,z축으로 감지를 위한 박스영역을 설정
            var value = boMonster.sdMonster.detectionRange;
            // 이 때 박스 영역은 detectionRange의 값의 2배가 된다.
            var halfExtents = new Vector3(value, value, value);

            // 설정한 박스 영역 안에 플레이어 레이어를 가진 객체가 겹침이 발생했는지 확인
            // -> 겹침이 발생했다면 겹침이 발생한 콜라이더의 정보가 반환된다.
            var colls = Physics.OverlapBox(transform.position, halfExtents, transform.rotation, 1 << LayerMask.NameToLayer("Player"));

            if (colls.Length == 0)
            {
                attackController.hasTarget = false;
                return;
            }

            // 이 곳에 들어왔다는 것은 플레이어를 감지했다는 것
            attackController.hasTarget = true;
            // 플레리어를 추적할 수 있도록 목적지를 플레이어로 변경
            boMonster.desPos = colls[0].transform.position;

            // 플레이어와의 거리가  공격 범위 안이라면 공격가능 상태로 변경
            var distance = boMonster.desPos - transform.position;
            // 플레이어와의 거리가 공격범위 안이라면
            if (distance.magnitude <= boMonster.atkRange)
            {
                // 공격 가능 상태로 변경
                attackController.canAtk = true;
                // 공격 가능 상태가 되면 이동보다 공격이 우선시 되어 이동이나 회전을 하지 않는다.
                // 이 때, 캐릭터가 이동 시에도 몬스터가 캐릭터를 바라볼 수 있도록 회전값을 변경
                transform.rotation = Quaternion.LookRotation(distance.normalized);
            }
            else
            {
                // 공격 불가능 상태로 변경
                attackController.canAtk = false;
            }
        }

        /// <summary>
        /// 몬스터의 사망 모션이 종료됐을 때, 발생시킬 이벤트
        /// </summary>
        public override void OnDeadEnd()
        {
            // 몬스터 풀에 몬스터를 다시 넣음
            ObjectPoolManager.Instance.GetPool<Monster>().Return(this);

            // 여기서 아이템 드랍해주기
            ItemDrop();
        }

        public void ItemDrop()
        {
           var uiIngame = UIWindowManager.Instance.GetWindow<UIIngame>();

            var sdMonster = boMonster.sdMonster;
            //아이템 드랍 로직 적기

            for (int i=0; i<boMonster.sdMonster.dropItemRef.Length;i++)
            {
                if(Random.Range(1,100) <= (sdMonster.dropItemPer[i]*100))
                {
                    uiIngame.AddItem(this, new BoItem(GameManager.SD.sdItems.Where(_ => _.index == sdMonster.dropItemRef[i]).SingleOrDefault()));
                }
            }

            //uiIngame.AddItem(this,);

        }

    }
}
