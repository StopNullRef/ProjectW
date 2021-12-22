using ProjectW.SD;
using System;
using UnityEngine;

namespace ProjectW.DB
{
    [Serializable]
    public class BoMonster : BoActor
    {
        // 몬스터가 특정 위치까지 정찰 후 대기시간을 갖는다.
        // 이때 아래의 데이터를 이용
        // -> 현재 정찰 대기시간이 정찰대기시간 체크지점이 될 때까지 대기
        public float startPatrolWaitTime; // 정찰 대기시간
        public float patrolWaitTime; // 정찰대기 시간 체크 지점
        public Vector3 desPos; // 목적지 위치 (정찰 지점, 타겟 위치)

        public SDMonster sdMonster;


        public BoMonster(SDMonster sdMonster)
        {
            this.sdMonster = sdMonster;
        }
    }
}
