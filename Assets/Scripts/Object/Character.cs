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
                    break;
            }
        }

        public override void ActorUpdate()
        {
            base.ActorUpdate();
        }

        public override void MoveUpdate()
        {
            base.MoveUpdate();
        }
    }

}
