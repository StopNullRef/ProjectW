using System;

namespace ProjectW.SD
{
    [Serializable]
    public class SDGrowthStat : StaticData
    {
        /// <summary>
        /// 스텟 계산 공식
        /// 레벨 * 기본스텟 * 스텟 계수
        /// </summary>
        public float maxHp;
        public float maxHpFactor;
        public float maxMp;
        public float maxMpFactor;
        public float atk;
        public float atkFactor;
        public float def;
        public float defFactor;
        public float behaviour;
        public float behaviourFactor;
    }
}
