using System;


namespace ProjectW.SD
{
    /// <summary>
    /// 모든 기획데이터의 베이스가 되는 클래스
    /// 모든 기획데이터는 해당 클래스에서 파생이 됨
    /// </summary>
    [Serializable]
    public class StaticData
    {
        /// <summary>
        /// 기획 데이터의 인덱스
        /// -> 인덱스는 해당 기획 테이블 내에서 유일함
        /// </summary>
        public int index;
    }
}
