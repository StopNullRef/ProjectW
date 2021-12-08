using ProjectW.Network;
using System;

namespace ProjectW.DB
{
    [Serializable]
    public class DtoCharacter : DtoBase
    {
        /// <summary>
        /// 유저가 사용하는 캐릭터의 기획 데이터 상의 인덱스
        /// </summary>
        public int index;

        /// <summary>
        /// 유저의 캐릭터 레벨
        /// </summary>
        public int level;
    }
}
