using ProjectW.Define;
using ProjectW.Dummy;
using ProjectW.Network;
using ProjectW.Resource;
using ProjectW.UI;
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
        /// 인트로 페이즈에 따른 로딩 진행상황을 UI에 전달하여 출력하기 위해 uiTitle를 참조 받음
        /// </summary>
        public UITitle uiTitle;

        /// <summary>
        /// 로딩 게이지 애니메이션 처리에 사용될 코루틴
        /// </summary>
        private Coroutine loadGaugeUpdateCoroutine;


        /// <summary>
        /// 타이틀 컨트롤러 초기화
        /// </summary>
        public void Initialize()
        {
            OnPhase(introPhase);
        }

        /// <summary>
        /// 현재 페이즈에 대한 로직 실행
        /// </summary>
        /// <param name="phase">진행 시키고자 하는 현재 페이즈</param>
        private void OnPhase(IntroPhase phase)
        {
            // 현재 페이즈에 대한 열거형을 문자열로 변경하여 로딩 상태를 나타내는 텍스트에 전달
            uiTitle.SetLoadStateDescription(phase.ToString());
            
            // 로딩게이지 ui의 fillAmount가 아직 실제 로딩 게이지 퍼센트로 값이 끝까지 보간이 안된 상태에서
            // 실제 로딩 페이즈는 다음 페이즈로 변경이 이루어진다면??
            // 이 때 문제가 발생함
            // -> 기존의 코루틴이 이미 실행되고 있는데, 또 새로운 코루틴을 추가적으로 실행시키게 되면
            // 오류가 발생됨

            // 해결방법
            // 아직 실행중인 코루틴이 존재한다면 코루틴을 강제로 멈춘 후에, 새로운 로딩 게이지 퍼센트를
            // 넘겨서 코루틴을 다시 시작시킨다.

            // 로딩 게이지 애니메이션 처리에 사용될 코루틴이 존재한다면
            if(loadGaugeUpdateCoroutine != null)
            {
                // 코루틴을 강제로 멈추로 null로 초기화시킨다.
                StopCoroutine(loadGaugeUpdateCoroutine);
                loadGaugeUpdateCoroutine = null;
            }

            // 변경된 페이즈가 전체 페이즈 완료가 아니라면
            if (phase != IntroPhase.Comepelte)
            {
                // 현재 로드 퍼센테이즈를 구한다.
                var loadPer = (float)phase / (float)IntroPhase.Comepelte;
                // 구한 퍼센테이지를 로딩바에 적용
                // 코루틴 실행 시에는 StartCoroutine() 메서드를 이용
                loadGaugeUpdateCoroutine = StartCoroutine(uiTitle.LoadGaugeUpdate(loadPer));
            }
            else
                uiTitle.loadFillGauge.fillAmount = 1f;

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
                    GameManager.SD.Initialize();
                    LoadComplete = true;
                    break;
                case IntroPhase.UserData:
                    new LoginHandler().Connect();
                    LoadComplete = true;
                    break;
                case IntroPhase.Resource:
                    ResourceManager.Instance.Initialize();
                    LoadComplete = true;
                    break;
                case IntroPhase.UI:

                    LoadComplete = true;
                    break;
                case IntroPhase.Comepelte:
                    allLoaded = true;
                    var ingameManager = InGameManager.Instance;
                    GameManager.Instance.LoadScene(SceneType.Ingame, ingameManager.ChangeStage());
                    LoadComplete = true;

                    break;
            }
        }

        /// <summary>
        /// 현재 페이즈를 다음 페이즈로 변경하는 기능
        /// </summary>
        private void NextPhase()
        {
            // 실제 각 페이즈에서 진행하는 작업이 많은 처리가 필요한 작업이 아니므로
            // 현재 페이즈가 너무 빨리 병경되어 페이즈 변경과정을 확인하기 어려움..
            // 따라서 일부러 페이즈 변경에 딜레이를 주어 변경과정을 확인할  수 있게 만듬..
            StartCoroutine(WaitForSeconds());
        }

        private IEnumerator WaitForSeconds()
        {
            // 대략 1초 후에 아래 코드가 실행됨
            yield return new WaitForSeconds(0.2f);
            loadComplete = false;
            OnPhase(++introPhase);
        }

    }
}