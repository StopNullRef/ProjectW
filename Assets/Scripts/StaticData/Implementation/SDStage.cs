using System;


namespace ProjectW.SD
{
    [Serializable]
    public class SDStage : StaticData
    {
        // 스테이지 이름
        public string name;
        // 스테이지에서 스폰될 수 있는 몬스터들의 기획 데이터 상의 인덱스들
        public int[] genMonsters;
        // 위의 스폰될 수 있는 몬스터들이 스폰되는 영역에 대한 데이터들
        // -> 해당 배열의 길이와 genMonsters의 배열의 길이는 동일하게 사용됨
        // 한마디로 spawnArea는 genMonster[i] 번째 몬스터가 어느 영역에 스폰이 되는지에 대한 데이터를 갖음
        public int[] spawnArea;
        // 해당 스테이지에서 워프할 수 있는 스테이지들의 인덱스 값을 갖는 배열
        public int[] warpStageRef;
        // 해당 스테이지 리소스(프리팹) 경로
        public string resourcePath;
    }
}
