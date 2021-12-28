using System;


namespace ProjectW.SD
{
    [Serializable]
    public class SDItem : StaticData
    {
        public string name;
        public Define.Item.ItemType itemType;
        /// <summary>
        /// 아이템 사용 시 영향을 미치는 스텟들
        /// (이 때 스텟의 이름을 실제로 캐릭터가  사용하고 있는 스텟 필드 변수명으로 설정)
        /// </summary>
        public string[] affectingStats;

        /// <summary>
        /// 위쪽에서 영향을 미치는 스텟에 적용되는 값들
        /// -> 결과적으로 위의 필드와 연동되므로 위의 필드와 배열의 길이가 항상 동일해야한다
        /// </summary>
        public float[] affectingStatsValue;
        public string description;
        public string resourcePath;
    }
}
