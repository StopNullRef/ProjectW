using ProjectW.DB;
using ProjectW.Define;
using ProjectW.SD;
using ProjectW.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectW
{
    /// <summary>
    /// 게임에 사용하는 모든 데이터를 관리하는 클래스
    /// 추가로 게임의 씬 변경 등과 같은 큰 흐름 등을 제어하기도 함
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// 로딩 씬에서 로딩 진행상태를 나타낼 필드 (0~1)
        /// </summary>
        public float loadProgress;

        /// <summary>
        /// 해당 필드 true라면 더미서버를 사용
        /// </summary>
        public bool useDummyServer;

        /// <summary>
        /// 유저 데이터 (DB에서 받아온 데이터)
        /// </summary>
        [SerializeField]
        private BoUser boUser = new BoUser();

        public static BoUser User => Instance.boUser;

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

        /// <summary>
        /// 씬을 비동기로 로드하는 기능
        /// 다른 씬 간의 전환에 사용 (ex: Title-> InGame)
        /// </summary>
        /// <param name="sceneName">로드할 씬의 이름을 갖는 열거형</param>
        /// <param name="loadCoroutine">씬 전환 시 로딩씬에서 다음 씬에 필요한 미리 처리할 작업</param>
        /// <param name="loadComplete">씬 전환 완료 후 실행할 기능</param>
        public void LoadScene(SceneType sceneName, IEnumerator loadCoroutine = null,Action loadComplete = null)
        {
            // SceneManager.LoadScene(); 동기화 방식의 씬 전환
            // -> 씬 전환 작업을 처리하느라, 현재 씬이 그동안 멈춰있음

            // SceneManager.LoadSceneAsync(); 비동기 방식의 씬 전환
            // -> 씬 전환 시에 기존의 현재씬이 멈추지 않음

            // 이번에 씬 전환 시에 바로 씬을 전환하는 것이 아닌 현재씬 -> 다음 씬 사이에 로딩 씬을 추가하여
            // 로딩씬에서 다음씬에 필요한 데이터 로드 작업을 진행할 것임
            // 그럼 이 때 데이터 로드 상황에 대한 내용을 로딩씬에 출력해주려면 씬 전환 과정에서
            // 현재 씬이 멈춰있으면, 변경상황을 제대로 출력하지 못함
            // 따라서, 기존의 동기 방식의 씬 로드가 아닌 비동기 방식의 씬 로드가 필요함

            StartCoroutine(WaitForLoad());

            // LoadScene 메서드에서만 사용가능한 로컬 함수 선언
            IEnumerator WaitForLoad()
            {
                // 로딩 진행상태를 나타냄 (0 ~ 1)
                // 처음 씬 로딩을 실행하기전 로딩 진행상태를 0으로 초기화
                loadProgress = 0;

                // 코루틴을 사용하는 가장 큰 이유?
                // yield return 키워드를 이용해서 내가 원하는 타이밍(시점)에 특정 코드를
                // 실행시키기 편하기 때문에

                // 비동기로 로딩 씬을 불러옴
                yield return SceneManager.LoadSceneAsync(SceneType.Loading.ToString());

                // 로딩 씬으로 전환 완료 후에 아래 조직이 실행됨

                // 내가 불러오고자 하는 씬을 추가
                // -> 씬 변경 시, 로드씬 옵션을 additive로 주면 기존 씬이 변경되는 것이 아닌
                // 기존 씬에 새로운 씬이 추가되어 하이라키에 복수의 씬이 존재하게 됨
                var asyncOper =  SceneManager.LoadSceneAsync(sceneName.ToString(),LoadSceneMode.Additive);

                // 방금 추가한 씬을 비활성화
                // 이유? 로딩 씬에서 새로운 씬이 하나가 추가되어 2개의 씬이 활성화된 상태인데..
                // 이 떄 따로 컨트롤하지 않는  두개의 씬이 모두 화면에 비춰지게 됨
                // 따라서, 실제 변경하고자 하는 씬을 추가해둔 다음 사용자에게 로딩 씬을 보여주고
                // 로딩씬이 활성화되어있는 동안 실제 변경하고자 하는 씬에 필요한 데이터 등을 로드 하는 작업을 하기 위해서
                asyncOper.allowSceneActivation = false;

                // 변경하고자 하는 씬에 필요한 작업이 존재한다면 실행
                if(loadCoroutine != null)
                {
                    // 해당 작업이 완료될 때까지 대기
                    yield return StartCoroutine(loadCoroutine);
                }

                // 위에 작업이 완료된 후에 아래 로직이 실행됨

                // 로딩 진행 상태를 나타내는 loadProgress 값을 이용해서 사용자한테 로딩 진행 상태를 알려줌
                // -> 사실상 데이터 로딩 작업은 위쪽에서 이미 끝났음.. 
                //  loadProgress를 이용한 나타내고자 하는 것은 내가 변경하고자하는 씬의 로딩 상태
                // 현재 데이터 로드만 끝났고, 변경하고자 하는 씬은 부르자마자 비활성화 하였기 때문에
                // 실제로 전부 로드된 상태가 아님.. 데이터 로드가 끝났기 때문에 비활성화된 씬을 다시 활성화시켜
                // 실제 씬 로드를 진행시키며, 씬 로드 진행을 loadProgress로 나타냄

                // 비동기로 로드한 씬이 활성화가 완료되지 않았다면 특정 작업을반복
                while (!asyncOper.isDone)
                {
                    // isDone 이 false라면 씬이 아직 로드가 끝나지 않은 상태
                    // 그럼 이때 현재 로드 상황을 loadProgress로 나타낸다.
                    if(loadProgress >= 0.9f)
                    {
                        loadProgress = 1f;

                        // 로딩바가 마지막까지 차는 것을 확인하기 위해 1초정도 대기
                        yield return new WaitForSeconds(1f);

                        // 변경하고자하는 씬을 다시 활성화
                        // isDone은 씬이 활성상태가 아니라면 progress가 1이 되어도
                        // true가 안됨
                        asyncOper.allowSceneActivation = true;
                    }

                    loadProgress = asyncOper.progress;

                    // 코루틴 내에서 반복문 사용 시 로직을 한 번 실행 후, 코루틴을 탈출하여
                    // 메인 로직을  실행할 수 있게 yield return을 사용
                    yield return null;
                }

                // 위에 반복작업이 다 끝난 후에 아래로직 실행

                // 로딩씬에서 다음 씬에 필요한 작업을 전부 수행했으므로 로딩씬을 다시 비활성화 시킴
                yield return SceneManager.UnloadSceneAsync(SceneType.Loading.ToString());

                // 모든 작업이 완료되었으므로 모든 작업 완료 후 실행시킬 로직이 있다면 실행
                loadComplete?.Invoke();
            }

        }

    }
}
