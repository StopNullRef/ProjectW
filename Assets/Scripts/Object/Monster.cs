using ProjectW.DB;
using ProjectW.Object;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Object
{
    /// <summary>
    /// 몬스터는 인게임 도중에 빈번히 생성되고, 제거되므로 오브젝트 풀을 이용한다.
    /// -> 오브젝트를 풀을 이용할 객체에게는 오브젝트 풀 기능을 수행할 수 있게 IPoolableObject 인터페이스를 상속시킨다.
    /// </summary>
    public class Monster : Actor, IPoolableObject
    {
        public BoMonster boMonster;

        // 재사용 가능 여부를 나타내는 프로퍼티, 처음 생성 시 재사용 가능한 상태로 설정
        public bool CanRecyle { get; set; } = true;

        public override void Initialize(BoActor boActor)
        {
            base.Initialize(boActor);
            // 접근의 편의를 위해 BoMonster 형태로 캐스팅하여 담아둠
            boMonster = boActor as BoMonster;

            // 초기 스텟 설정
            SetStats();
        }

        protected override void Start()
        {
            base.Start();
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
            base.ActorUpdate();
        }

        public override void MoveUpdate()
        {

        }

    }
}
