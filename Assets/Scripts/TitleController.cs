using ProjectW.Define;
using ProjectW.Dummy;
using ProjectW.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW
{
    /// <summary>
    /// 타이틀 씬에서 게임 시작 전에 필요한 전반적인 초기화 및
    /// 데이터 로드 등을 수행하는 클래스
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        /// <summary>
        /// 현재 페이즈의 완료 상태
        /// </summary>
        private bool loadComplete;

        /// <summary>
        /// 외부에서 loadComplete에 접근하기 위한 프로퍼티
        /// 추가로 현재 페이즈 완료시 조건에 따라 다음 페이즈로 변경
        /// </summary>
        public bool LoadComplete
        {
            get => loadComplete;
            set
            {
                loadComplete = value;

                //현재 페이즈가 완료되었고, 모든 페이즈가 완료되지 않았다면
                if (loadComplete && !allLoaded)
                    //다음 페이즈로 변경
                    NextPhase();
            }
        }

        /// <summary>
        /// 모든 페이즈의 완료 상태
        /// </summary>
        private bool allLoaded;

        /// <summary>
        /// 현재 페이즈를 나타냄
        /// </summary>
        private IntroPhase introPhase = IntroPhase.Start;

        /// <summary>
        /// 현재 페이즈에 대한 로직 실행
        /// </summary>
        /// <param name="phase">진행 시키고자 하는 현재 페이즈</param>
        private void OnPhase(IntroPhase phase)
        {
            switch (phase)
            {
                case IntroPhase.Start:
                    LoadComplete = true;
                    break;
                case IntroPhase.ApplicationSetting:
                    GameManager.Instance.OnApplicationSetting();
                    LoadComplete = true;
                    break;
                case IntroPhase.Server:
                    DummyServer.Instance.Initialize();
                    ServerManager.Instance.Initialize();
                    LoadComplete = true;
                    break;
                case IntroPhase.StaticData:

                    LoadComplete = true;
                    break;
                case IntroPhase.UserData:

                    LoadComplete = true;
                    break;
                case IntroPhase.Resource:

                    LoadComplete = true;
                    break;
                case IntroPhase.UI:

                    LoadComplete = true;
                    break;
                case IntroPhase.Comepelte:

                    LoadComplete = true;
                    break;
            }
        }

        /// <summary>
        /// 현재 페이즈를 다음 페이즈로 변경하는 기능
        /// </summary>
        private void NextPhase()
        {

        }

    }
}