using ProjectW.SD;
using System;
using UnityEngine;
using System.Linq;

namespace ProjectW.DB
{
    [Serializable]
    public class BoStage
    {
        /// <summary>
        /// 다른 스테이즈로 워프 시 이전 스테이지에 대한 인덱스를 들고 있을 필드
        /// 서버에서는 존재하지 않는 데이터, 클라에서만 사용
        /// -> 워프 시에 어떤 마을에서 워프를 해서 왔는지 알아야만
        /// 해당 마을의 워프가 존재하는 위치에 캐릭터를 배치시킬 수 있으므로
        /// </summary>
        public int prevStageIndex;
        /// <summary>
        /// 플레이어가 위치한 좌표
        /// </summary>
        public Vector3 prevPos;

        /// <summary>
        /// 이전 몬스터 스폰 시간
        /// </summary>
        public float prevSpawnTime;
        /// <summary>
        /// 몬스터 스폰 체크 시간
        /// </summary>
        public float spawnCheckTime;

        /// <summary>
        /// 플레이어가 현재/마지막으로 위치한 스테이지의 기획데이터 (없다면 시작마을로 설정)
        /// </summary>
        public SDStage sdStage;



        public BoStage(DtoStage dtoStage)
        {
            // dtoStage에 index와 동일한 인덱스를 갖는 기획 데이터가 존재한다면
            // 가져오고, 아니라면 null
            sdStage = GameManager.SD.sdStages.Where(_ => _.index == dtoStage.index).SingleOrDefault();
            prevPos = new Vector3(dtoStage.posX, dtoStage.posY, dtoStage.posZ);

        }
    }
}
