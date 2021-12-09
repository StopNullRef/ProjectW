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
            // dtoCharacter에 내 캐릭터의 기획데이터 상의 인덱스 값이 존재함
            // 해당 데이터를 통해 내 캐릭터의 기획데이터를 불러옴
            sdCharacter = GameManager.SD.sdCharacters.Where(_ => _.index == dtoCharacter.index).SingleOrDefault();

            // 위의 과정을 통해 내 캐릭터의 기획데이터를 불러왔음
            // -> 기획데이터 중에 해당 캐릭터가 어떤 성장테이블의 로우(행)을 참조하는지에
            // 대한 데이터가 들어가 있음
            // -> 캐릭터 기획데이터가 존재한다면 성장스텟 데이터도 불러올 수 있음
            sdGrowthStat = GameManager.SD.sdGrowthStats.Where(_ => _.index == dtoCharacter.index).SingleOrDefault();
            level = dtoCharacter.level;
        }
    }
}
