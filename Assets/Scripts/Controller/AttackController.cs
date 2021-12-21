using ProjectW.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectW.Controller
{
    using Type = Define.Actor.Type;
    using State = Define.Actor.State;
    using AttackType = Define.Actor.AttackType;

    /// <summary>
    /// 액터의 공격 기능을 제어할 컨트롤러
    /// </summary>
    public class AttackController : MonoBehaviour
    {
        /// <summary>
        /// 공격 대상이 있는지
        /// </summary>
        public bool hasTarget;

        /// <summary>
        /// 공격 쿨타임을 체크할 수 있는지?
        /// (공격모션이 끝나기 전에는 쿨타임체크 막을것)
        /// </summary>
        public bool canCheckCoolTime;

        public bool isCoolTime;

        /// <summary>
        /// 공격 가능 상태인지?
        /// </summary>
        public bool canAtk;

        /// <summary>
        /// 이전 공격 시간 (이전 공격 시간을 이용해서 현재 시간에서 뺀 다음 뺀 값이 공격 쿨타임보다 크다면, 공격 가능 상태)
        /// </summary>
        private float prevAtkTime;

        /// <summary>
        /// 공격자 (해당 어택 컨트롤러 인스턴스를 갖는 액터)
        /// </summary>
        private Actor attacker;

        /// <summary>
        /// 피격 대상 액터들을 갖는 리스트
        /// </summary>
        private List<Actor> targets = new List<Actor>();

        /// <summary>
        /// 초기화 기능 (해당 어택 컨트롤러는 갖는 액터의 인스턴스를 받아온다)
        /// </summary>
        /// <param name="attacker"></param>
        public void Initialize(Actor attacker)
        {
            this.attacker = attacker;
        }

        /// <summary>
        /// 공격 가능 상태라면 공격자의 상태를 공격상태로 변경하는 기능
        /// </summary>
        public void CheckAttack()
        {
            // 타겟이 없다면 리턴
            if (!hasTarget)
                return;

            // 공격 쿨이라면 리턴
            if (isCoolTime)
                return;

            // 공격 불가능 상태라면 리턴
            if (!canAtk)
                return;

            attacker.SetState(State.Attack);
        }

        /// <summary>
        /// 액터의 공격 모션이 타격점에 도달했을 때 호출
        /// 근접 공격이라면 공격 범위 연산 후 데미지연산 실행
        /// 원거리 공격이라면 발사체를 생성
        /// </summary>
        public virtual void OnAttack()
        {
            switch (attacker.boActor.atkType)
            {
                case AttackType.Normal: // 근접
                    // 범위 연산
                    CalculateAttackRange();

                    // 데미지는 공격자의 공격력
                    var damage = attacker.boActor.atk;
                    // 위의 범위 연산을 통해 타겟리스트를 구했으므로
                    // 타겟 리스트를 순회하며 타겟들에게 데미지 연산을 실행
                    for (int i = 0; i < targets.Count; i++)
                        CalculateDamage(damage, targets[i]);

                    break;
                case AttackType.Projectile: // 원거리
                    OnFire();
                    break;
            }
        }

        /// <summary>
        /// 공격 범위에 적이 있는지 연산
        /// </summary>
        public virtual void CalculateAttackRange()
        {
            // 공격자의 액터 타입에 따라 타겟 레이어를 설정
            var targetLayer = attacker.boActor.actorType == Type.Character ?
                LayerMask.NameToLayer("Monster") : LayerMask.NameToLayer("Player");

            // 공격자의 위치에서 공격자의 앞쪽 방향으로 공격 범위만큼 레이를 발사,
            // 이 때 충돌을 체크하는 것은 0.5f의 반지름을 갖는 구 형태로 레이캐스트 히트 정보를 체크한다.
            var hits = Physics.SphereCastAll(attacker.transform.position, 0.5f,attacker.transform.forward,
                attacker.boActor.atkRange, 1<<targetLayer);

            // 타겟에 대한 정보를 새로 구했으니, 이전 타겟에 대한 정보를 지운다.
            targets.Clear();

            // 새로운 타겟 정보를 타겟 목록에 넣는다.
            for(int i=0; i< hits.Length; i++)
            {
                targets.Add(hits[i].transform.GetComponent<Actor>());
            }
        }

        /// <summary>
        /// 원거리 타입 발사체 생성
        /// </summary>
        public virtual void OnFire()
        {

        }

        /// <summary>
        /// 데미지를 공시겡 따라 연산하여 타겟에 적용
        /// </summary>
        /// <param name="damage">공격자가 가한 데미지</param>
        /// <param name="target">피격 대상</param>
        public virtual void CalculateDamage(float damage, Actor target)
        {
            // 데미지 계산
            // Max 메서드를 이용해서 방어력이 데미지보다 더 큰 경우에 음수가 되지 않고 0으로 고정되도록
            var calDamage = Mathf.Max(damage - target.boActor.def, 0);

            // 계산된 데미지를 타겟의 현재 체력에 적용
            // -> 데미지가 현재 체력보다 클 경우 음수가 되지 않고 0으로 고정되도록
            target.boActor.currentHp = Mathf.Max(target.boActor.currentHp - calDamage, 0);

            // 타겟의 체력이 0이라면 죽은 상태로 변경
            if(target.boActor.currentHp <= 0)
            {
                target.SetState(State.Dead);
            }
        }

        /// <summary>
        /// 공격 쿨타임을 업데이트 하는 기능
        /// FixedUpdate에서 호출할 것임..
        /// </summary>
        public void AttackIntervalUpdate()
        {
            // 쿨타임을 체크할 수 없다면 리턴
            if (!canCheckCoolTime)
                return;

            if (!isCoolTime)
                return;

            // 현재 시간에서 이전 공격시간을 뺀 시간이 공격 쿨타임 이상이라면
            if(Time.time - prevAtkTime>= attacker.boActor.atkInterval)
            {
                // 쿨타임 초기화
                InitAttackInterval();
            }
        }

        /// <summary>
        /// 공격 쿨타임 초기화
        /// </summary>
        public void InitAttackInterval()
        {
            prevAtkTime = Time.time;
            isCoolTime = false;
        }
    }
}