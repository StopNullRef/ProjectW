using System;

namespace ProjectW.SD
{
    [Serializable]
    public class SDMonster : StaticData
    {
        /// <summary>
        /// 몬스터 이름
        /// </summary>
        public string name;
        /// <summary>
        /// 몬스터의 공격 타입
        /// </summary>
        public Define.Actor.AttackType atkType;
        /// <summary>
        /// 이동 속도
        /// </summary>
        public float moveSpeed;
        /// <summary>
        /// 몬스터가 적을 감지할 수 있는 범위
        /// </summary>
        public float detectionRange;
        /// <summary>
        /// 몬스터 공격 범위
        /// </summary>
        public float atkRange;
        /// <summary>
        /// 공격 딜레이
        /// </summary>
        public float atkInterval;
        public float maxHp;
        public float maxMp;
        public float atk;
        public float def;
        /// <summary>
        /// 몬스터 프리팹 경로
        /// </summary>
        public string resourcePath;
    }
}
