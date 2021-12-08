using ProjectW.SD;
using System;
using System.Linq;
using UnityEngine;

namespace ProjectW.DB
{
    [Serializable]
    public class BoCharacter : BoActor
    {
        /// <summary>
        /// 캐릭터의 기본 정보 및 레벨에 영향을 받지 않는 데이터
        /// </summary>
        public SDCharacter sdCharacter;

        

        /// <summary>
        /// 레벨의 영향을 받는 기획 데이터
        /// </summary>
        public SDGrowthStat sdGrowthStat;

        public BoCharacter(DtoCharacter dtoCharacter)
        {
            sdCharacter = GameManager.SD.sdCharacters.Where(_ => _.index == dtoCharacter.index).SingleOrDefault();
            sdGrowthStat = GameManager.SD.sdGrowthStats.Where(_ => _.index == dtoCharacter.index).SingleOrDefault();
            level = dtoCharacter.level;
        }
    }
}
