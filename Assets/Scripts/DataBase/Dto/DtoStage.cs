using ProjectW.Network;
using System;

namespace ProjectW.DB
{
    [Serializable]
    public class DtoStage : DtoBase
    {
        /// <summary>
        /// 플레이어가 마지막으로 위치한 스테이지의 기획 데이터상의 인덱스
        /// -> 게임을 다시 시작했을 때, 마지막으로 위치해있던 마을에서 게임을 진행할 수 있게
        /// </summary>
        public int index;

        /// <summary>
        /// 플레이어가 마지막으로 위치한 스테이지 상에서의 좌표
        /// </summary>
        public float posX;
        public float posY;
        public float posZ;
    }
}
