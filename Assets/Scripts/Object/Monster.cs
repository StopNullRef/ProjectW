using ProjectW.DB;
using ProjectW.Object;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Object
{
    /// <summary>
    /// ���ʹ� �ΰ��� ���߿� ����� �����ǰ�, ���ŵǹǷ� ������Ʈ Ǯ�� �̿��Ѵ�.
    /// -> ������Ʈ�� Ǯ�� �̿��� ��ü���Դ� ������Ʈ Ǯ ����� ������ �� �ְ� IPoolableObject �������̽��� ��ӽ�Ų��.
    /// </summary>
    public class Monster : Actor, IPoolableObject
    {
        public BoMonster boMonster;

        // ���� ���� ���θ� ��Ÿ���� ������Ƽ, ó�� ���� �� ���� ������ ���·� ����
        public bool CanRecyle { get; set; } = true;

        public override void Initialize(BoActor boActor)
        {
            base.Initialize(boActor);
            // ������ ���Ǹ� ���� BoMonster ���·� ĳ�����Ͽ� ��Ƶ�
            boMonster = boActor as BoMonster;

            // �ʱ� ���� ����
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
