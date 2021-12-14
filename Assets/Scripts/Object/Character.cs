using ProjectW.DB;
using UnityEngine;

namespace ProjectW.Object
{
    using State = Define.Actor.State;

    public class Character : Actor
    {
        /// <summary>
        /// 캐릭터의 bo 데이터 (사실상 베이스에 작성된 boActor와 동일한 데이터, boActor는 BoActor 타입으로 선언이 되어있으니까,
        /// BoCharacter에 선언된 필드로의 접근이 어려움 따라서, 자주 접근하게될 bo 데이터를 편리하게 사용할 수 있도록
        /// boActor를 BoCharacter로 캐스팅하여 담아둘 필드)
        /// </summary>
        public BoCharacter boCharacter;

        public override void Initialize(BoActor boActor)
        {
            base.Initialize(boActor);

            boCharacter = boActor as BoCharacter;

            SetStats();
        }

        public override void SetStats()
        {
            // 캐릭터 기본 스텟 설정
            boCharacter.actorType = Define.Actor.Type.Character;
            // 기획데이터에 설정되어있는 기본데이터를 인게임 데이터에 넣고 있음
            // -> 인게임 데이터는 기본적으로 기획데이터에 설정된 기본값으로 세팅이 됨
            // 추후 외부 요인(버프, 아이템 등)에 의해 스텟이 변경될 수도 있음
            boCharacter.atkType = boCharacter.sdCharacter.atkType;
            boCharacter.moveSpeed = boCharacter.sdCharacter.moveSpeed;

            boCharacter.maxHp = boCharacter.level * boCharacter.sdGrowthStat.maxHp * boCharacter.sdGrowthStat.maxHpFactor;

            boCharacter.currentHp = boCharacter.maxHp;

            boCharacter.maxMp = boCharacter.level * boCharacter.sdGrowthStat.maxMp * boCharacter.sdGrowthStat.maxMpFactor;

            boCharacter.currentMp = boCharacter.maxMp;

            boCharacter.atk = boCharacter.level * boCharacter.sdGrowthStat.atk * boCharacter.sdGrowthStat.atkFactor;

            boCharacter.def = boCharacter.level * boCharacter.def * boCharacter.sdGrowthStat.defFactor;

            boCharacter.atkRange = boCharacter.sdCharacter.atkRange;

            boCharacter.atkInterval = boCharacter.sdCharacter.atkInterval;

            //float GetGrowthData(float stat, float statFactor)
            //{
            //    return boCharacter.level * stat * statFactor;
            //}
        }

        public override void SetState(State state)
        {
            // actor가 공통으로 갖는 상태 처리
            base.SetState(state);

            // 캐릭터만 갖는 상태 처리
            switch (state)
            {
                case State.Jump:
                    OnJump();
                    break;
            }
        }

        public override void ActorUpdate()
        {
            base.ActorUpdate();
        }

        public override void MoveUpdate()
        {
            var velocity = boActor.moveSpeed * boActor.moveDir;
            velocity = transform.TransformDirection(velocity);

            transform.localPosition += velocity * Time.fixedDeltaTime;
            transform.Rotate(boActor.rotDir * Define.Camera.RotSpeed);

            // 속도의 벡터의 길이가 0과 같다면 안 움직인다는 뜻
            // -> 모션을 대기 모션으로
            if(Mathf.Approximately(velocity.sqrMagnitude,0))
            {
                SetState(State.Idle);
            }
            // 아니라면 움직인다는 뜻이므로, 모션을 걷기 모션으로
            else
            {
                SetState(State.Walk);
            }
        }

        /// <summary>
        /// 캐릭터가 땅에 있는지 체크하는 기능
        /// -> 충돌/겹침 체크하는 방법
        /// 1. 콜라이더를 이용하는 방법
        /// -> 레이캐스팅에 비해 비용이 싸다
        /// -> 대신 레이캐스팅 보다 충돌/겹침 체크가 정교하지 않다.
        /// 2. 레이캐스팅을 이용하는 방법
        /// ->
        /// </summary>
        private void CheckGround()
        {

        }

        /// <summary>
        /// 점프 기능 실행 (점프 키를 눌렀을 때 한 번 호출)
        /// </summary>
        public void OnJump()
        {
            // 강체에 순간적인 힘을 주는 방식으로 점프를 구현
            rig.AddForce(Vector3.up * boCharacter.sdCharacter.jumpForce, ForceMode.Impulse);
        }

    }

}
