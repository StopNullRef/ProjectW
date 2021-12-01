using ProjectW.SD;
using ProjectW.Util;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW
{
    /// <summary>
    /// 게임에 사용하는 모든 데이터를 관리하는 클래스
    /// 추가로 게임의 씬 변경 등과 같은 큰 흐름 등을 제어하기도 함
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// 해당 필드 true라면 더미서버를 사용
        /// </summary>
        public bool useDummyServer;

        /// <summary>
        /// 기획 데이터를 갖는 객체
        /// </summary>
        [SerializeField]
        private StaticDataModule sd = new StaticDataModule();
        public static StaticDataModule SD => Instance.sd;

        private void Start()
        {
            var titleController = FindObjectOfType<TitleController>();
            titleController?.Initialize();
        }

        /// <summary>
        /// 앱 기본 설정
        /// </summary>
        public void OnApplicationSetting()
        {
            // 수직동기화 끄기
            QualitySettings.vSyncCount = 0;
            // 렌더 프레임을 60으로 설정
            Application.targetFrameRate = 60;
            // 앱 실행 중 장시간 대기 시에도 화면이 꺼지지 않게
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
