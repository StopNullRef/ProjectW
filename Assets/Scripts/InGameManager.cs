using ProjectW.Controller;
using ProjectW.Define;
using ProjectW.Object;
using ProjectW.Resource;
using ProjectW.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectW
{
    using Actor = Object.Actor;
    /// <summary>
    /// 인게임 내 객체들을 관리할 클래스
    /// 스테이지 전환 시 처리작업 등을 수행 (해당 스테이지에 필요한 리소스 로드 및 인스턴스 생성)
    /// </summary>
    public class InGameManager : Singleton<InGameManager>
    {
        /// <summary>
        /// 스테이지 전환 작업이 완료되어 인게임을 시작할 준비가 되었는지
        /// </summary>
        private bool isReady;

        /// <summary>
        /// 활성화된 몬스터 객체들을 인게임에서 들고 있을 홀더 (하이라키상의 부모)
        /// </summary>
        private Transform monsterHolder;

        /// <summary>
        /// 현재 스테이지 인스턴스를 들고 있을 필드
        /// </summary>
        private GameObject currentStage;

        public List<Actor> Characters { get; private set; } = new List<Actor>();
        public List<Actor> Monsters { get; private set; } = new List<Actor>();

        /// <summary>
        /// 활성화된 액터를 인게임 매니저에 등록하는 기능
        /// 이 곳에 등록된 액터만 업데이트가 된다.
        /// </summary>
        /// <param name="actor">등록할 활성화된 액터의 인스턴스</param>
        public void AddActor(Actor actor)
        {
            switch (actor)
            {
                // actor 타입이 몬스터와 같다면 임시로 actor를 monster라는 임시 변수로 사용
                case var monster when actor.boActor.actorType == Define.Actor.Type.Monster:
                    Monsters.Add(monster);
                    break;
                case var character when actor.boActor.actorType == Define.Actor.Type.Character:
                    Characters.Add(character);
                    break;
            }

        }

        private void FixedUpdate()
        {
            ActorUpdate(Characters);
            ActorUpdate(Monsters);
        }

        private void ActorUpdate(List<Actor> actors)
        {
            for(int i =0; i<actors.Count;i++)
            {
                // 액터가 죽지 않았따면 업데이트
                if (actors[i].State != Define.Actor.State.Dead)
                    actors[i].ActorUpdate();
                else
                {
                    // 죽었다면 액터 컬렉션에서 제거
                    actors.RemoveAt(i);
                    // 반복되는 곳에서 리스트 안에 원소를 제거할 때 주의할 점
                    // 원소 제거시 리스트 공간이 해당 원소 뒤쪽부터 한 칸씩 땡겨짐
                    i--;
                }
            }
        }


        /// <summary>
        /// 스테이지 전환 시 필요한 리소스를 불러오고 리소스를 통해 인스턴스를 생성,
        /// 생성한 인스턴스에 필요한 데이터를 바인딩 작업
        /// -> 해당 메서드를 호출하는 시점은 로딩 씬이 활성화되어있는 상태
        /// 씬 전환 시에 다음 씬에 필요한 스테이지 리소스를 불러오는 작업
        /// </summary>
        /// <returns></returns>
        public IEnumerator ChangeStage()
        {
            isReady = false;

            // 외부(서버)에서 새로 불러올 스테이지 정보를 받은 상태
            // 스테이지 정보(스테이지에 대한 기획데이터)를 이용하여 스테이지 객체를 생성
            var sdStage = GameManager.User.boStage.sdStage;

            var resourceManager = ResourceManager.Instance;

            // 현재 스테이지 객체가 이미 존재하는지
            if (currentStage != null)
            {
                // 존재한다면 새로운 스테이지를 로드할 것이므로 파괴
                Destroy(currentStage);
            }
            // 새로 불러올 스테이지 객체를 생성하여 현재 스테이지를 나타내는 필드에 대입
            currentStage = Instantiate(resourceManager.LoadObject(sdStage.resourcePath));

            // 여기서 한가지 문제점이 발생
            // 현재 ChangeStage 메서드가 호출되고 있는 시점은 씬이 2개인 상태 (로딩, 인게임)
            // 이 때 객체를 생성하면 활성화되어있는 씬에 객체가 생성됨 (로딩 씬이 활성화, 인게임은 비활성화)
            // 그럼 현재 활성화 되어있는 로딩씬에 생성된 객체가 귀속됨
            // -> 로딩이 끝나면 로딩씬은 언로드 되고 인게임이 활성화되므로, 결과적으로 생성한 맵을 확인할 수가 없음

            // 그럼 어떻게 해결?
            // 생성한 객체를 로딩씬에서 인게임씬으로 이동
            SceneManager.MoveGameObjectToScene(currentStage, SceneManager.GetSceneByName(SceneType.Ingame.ToString()));

            // 위의 과정을 통해 스테이지 객체만 로딩 과정에서 미리 만들어두고
            // 그외 캐릭터나 몬스터 (액터)들은 씬이 완전히 전환된 후에 따로 생성


            yield return null;
        }

        /// <summary>
        /// 위의 ChangeStage 메서드가 씬 전환 도중에 실행되는 작업이라면
        /// 해당 메서드는 씬 전환이 완료된 후에 실행될 작업
        /// </summary>
        public void OnChangeStageComplete()
        {
            SpawnCharacter();
        }

        /// <summary>
        /// 플레이어 캐릭터 생성 또는 스테이지 이동 시 플레이어 위치 설정
        /// </summary>
        private void SpawnCharacter()
        {
            // 전환이 완료된 씬에서 플레이어 캐릭터 인스턴스를 찾는다.
            // PlayerController는 내 캐릭터를 조작할 수 있는 컨트롤러
            var playerController = FindObjectOfType<PlayerController>();
            if (playerController == null)
                return;

            // 플레이어의 캐릭터 인스턴스가 존재한다면
            // -> 타이틀에서 인게임 씬으로의 변경이 아닌, 인게임 씬에서 스테이지 이동(워프)했을 경우
            if(playerController.PlayerCharacter !=null)
            {
                // 새로 이동한 스테이지에 이전 스테이지와 연결된 워프를 찾음
                var warp = currentStage.transform.Find($"WarpHolder/{GameManager.User.boStage.prevStageIndex}/EntryPos").transform;

                
                // 플레이어의 위치와 바라보는 방향을 이동한 워프에 설정된 위치와 방향으로 바꿔준다
                playerController.PlayerCharacter.transform.position = warp.position;
                playerController.PlayerCharacter.transform.forward = warp.forward;

                // 플레이어가 워프로 인해 갑작스롭게 이동하였으므로, 카메라도 동일하게 강제로 이동시켜준다.
                playerController.cameraController.SetForceStandardView();
                return;
            }

            // 존재하지 않는다면
            // -> 타이틀에서 인게임 씬으로 변경한 경우, 이 때 캐릭터가 없으므로 생성
            var characterObj = Instantiate(ResourceManager.Instance.LoadObject(GameManager.User.boCharacter.sdCharacter.resourcePath));

            // 유저의 캐릭터가 마지막으로 종료한 위치(처음 접속일 경우 0,0,0)로 캐릭터를 이동
            characterObj.transform.position = GameManager.User.boStage.prevPos;

            var playerCharacter = characterObj.GetComponent<Character>();
            // 생성한 캐릭터 객체가 갖는 캐릭터 컴포넌트에 유저의 캐릭터 정보를 전달하여 초기화시킨다.
            playerCharacter.Initialize(GameManager.User.boCharacter);
            // 초기화가 끝난 캐릭터 객체를 유저가 제어할 수 있게 플레이어 컨트롤러에 전달하여 초기화시킨다.
            playerController.Initialize(playerCharacter);

            // 모든 초기화가 완료된 캐릭터 객체를 정상적으로 업데이트할 수 있게 IngameManger에 등록시킨다.
            AddActor(playerCharacter);
        }

        /// <summary>
        /// 몬스터 스폰 기능
        /// </summary>
        private void SpawnMonster()
        {
            if (monsterHolder == null)
            {
                //없다면 몬스터 홀더를 생성
                monsterHolder = new GameObject("MonsterHolder").transform;
                monsterHolder.position = Vector3.zero;
            }
        }
    }
}
