using System;

namespace ProjectW.SD
{
    [Serializable]
    public class SDCharacter : StaticData
    {
        /// <summary>
        /// 캐릭터의 이름
        /// </summary>
        public string name;

        /// <summary>
        /// 캐릭터의 공격 타입
        /// </summary>
        public Define.Actor.AttackType atkType;

        /// <summary>
        /// 이동 속력
        /// </summary>
        public float moveSpeed;

        /// <summary>
        /// 점프 시 지면에 가해지는 힘
        /// </summary>
        public float jumpForce;

        /// <summary>
        /// 일반 공격 범위
        /// </summary>
        public float atkRange;

        /// <summary>
        /// 일반 공격 간격 (공격 딜레이)
        /// </summary>
        public float atkInterval;

        /// <summary>
        /// ?
        /// </summary>
        public int growthStatRef;

        /// <summary>
        /// 리소스(prefabs) 경로
        /// </summary>
        public string resourcePath;
    }
}
