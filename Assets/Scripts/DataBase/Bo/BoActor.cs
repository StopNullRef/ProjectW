using System;
using UnityEngine;
using ProjectW.Define;

namespace ProjectW.DB
{
    /// <summary>
    /// BoActor는 캐릭터와 몬스터의 공통된 데이터를 정리
    /// </summary>
    [Serializable]
    public class BoActor
    {
        public float level;
        public Actor.Type actorType;
        public Actor.AttackType atkType;
        public float moveSpeed;
        public Vector3 moveDir;
        public Vector3 rotDir;
        public float currentHp;
        public float maxHp;
        public float currentMp;
        public float maxMp;
        public float atk;
        public float def;
        public float atkRange;
        public float atkInterval;
        public bool isGround;
    }
}
