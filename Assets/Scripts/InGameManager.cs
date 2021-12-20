using ProjectW.Controller;
using ProjectW.DB;
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
    using Random = UnityEngine.Random;

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
        /// 현재 스테이지 내에서 몬스터 스폰영역에 대한 정보를 들고 있는 컬렉션
        /// Key : 현재 스테이지의 SpawnHolder 객체의 배치된 순서 (하이라키상의 순서)
        /// Value : 영역 데이터
        /// </summary>
        private Dictionary<int, Bounds> spawnAreaBounds = new Dictionary<int, Bounds>();

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
            actor.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (isReady)
                CheckSpawnTime();
        }

        private void FixedUpdate()
        {
            ActorUpdate(Characters);
            ActorUpdate(Monsters);
        }

        private void ActorUpdate(List<Actor> actors)
        {
            for (int i = 0; i < actors.Count; i++)
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

            // 현재 스테이가 변경되었으므로, 변경된 스테이지에서 사용될 몬스터 리소스를 불러와 몬스터풀을 등록
            // 추가로 스테이지의 몬스터 스폰 영역에 대한 정보도 미리 찾아둔다.

            var sd = GameManager.SD;

            // 하이라키상에 스폰 영역들을 갖는 홀더의 참조를 담을 변수
            Transform spawnAreaHolder = null;

            // 하이라키 상에서 스폰영역을 찾는 것을 시도한다.
            try
            {
                spawnAreaHolder = currentStage.transform.Find("SpawnAreaHolder");
            }
            catch
            {
                // 이 곳에 들어왔다는 것은 스폰 영역 홀더를 차지 못했다는 것
                // -> 스폰 영역 홀더가 없는 경우는 몬스터를 생성하지 않는 스테이지라는 뜻
                // 따라서, 코루틴을 종료시킨다.
                yield break;
            }

            // 해당 스테이지에서 생성될 수 있는 몬스터의 종류만큼 반복
            for (int i = 0; i < sdStage.genMonsters.Length; i++)
            {
                // 몬스터 기획 데이터를 하나씩 불러온다.
                var sdMonster = sd.sdMonsters.Where(_ => _.index == sdStage.genMonsters[i]).SingleOrDefault();

                // 불러온 데이터가 존재한다면  몬스터 풀에 등록
                if (sdMonster != null)
                {
                    resourceManager.LoadPoolableObject<Object.Monster>(sdMonster.resourcePath, 10);
                }
                // 존재하지 않는다면 다음으로 넘김
                else
                    continue;

                // 해당 몬스터의 스폰구역에 대한 정보를 가져온다.
                var spawnAreaIndex = sdStage.spawnArea[i];
                // 스폰 영역 인덱스가 -1 이라면, 몬스터를 생성하지 않는 스테이지
                if (spawnAreaIndex != -1)
                {
                    // 해당 스폰 영역 인덱스가 딕셔너리에 이미 존재하는지 체크
                    // -> (중복되는 영역을 갖는 몬스터가 있다면 이밈 딕셔너리에 해당 영역이 등록 되어있으므로)
                    if (!spawnAreaBounds.ContainsKey(spawnAreaIndex))
                    {
                        // 이 곳에 들어왔다는 것은 등록되지 않았다는 것, 따라서 새로 등록
                        var spawnArea = spawnAreaHolder.GetChild(spawnAreaIndex);
                        spawnAreaBounds.Add(spawnAreaIndex, spawnArea.GetComponent<Collider>().bounds);
                    }
                }
            }

            yield return null;
        }

        /// <summary>
        /// 위의 ChangeStage 메서드가 씬 전환 도중에 실행되는 작업이라면
        /// 해당 메서드는 씬 전환이 완료된 후에 실행될 작업
        /// </summary>
        public void OnChangeStageComplete()
        {
            InitSpawnTime();

            SpawnCharacter();
            SpawnMonster();

            isReady = true;
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
            if (playerController.PlayerCharacter != null)
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

            var sd = GameManager.SD;
            // 현재 스테이지에 대한 기획데이터 참조를 받아둠
            var sdStage = GameManager.User.boStage.sdStage;

            // 미리 설정한 최소, 최대 값 사이의 랜덤값을 뽑아 그 수만큼 몬스터를 생성
            var monsterSpawnCnt = Random.Range(Define.Monster.MinSpawnCnt, Define.Monster.MaxSpawnCnt);

            // 몬스터를 생성하기 위해 (정확히는 몬스터 객체는 이미 생성되어있음) 몬스터 풀을 가져옴
            // -> 몬스터는 오브젝트 풀을 사용하기 때문에 로딩 과정 또는 씬 전환 과정 사이에
            // 이미 몬스터 풀에 몬스터들은 생성되어 있는 상태

            var monsterPool = ObjectPoolManager.Instance.GetPool<Object.Monster>();

            // 랜덤 수만큼 몬스터를 생성
            for (int i = 0; i < monsterSpawnCnt; i++)
            {
                // 이 때 어떤 종류의 몬스터를 생성할 것인가?
                // -> 현재 스테이지에서 생성할 수 있는 몬스터 중에 랜덤하게 생성
                // 스테이지가 생성할 수 있는 몬스터 인덱스를 갖는 배열 데이터 중에
                // 랜덤하게 하나의 몬스터 인덱스를 가져온다.
                var randIndex = UnityEngine.Random.Range(0, sdStage.genMonsters.Length);

                // 생성할 수 있는 몬스터 인덱스를 갖는 배열 중에 랜덤한 배열인덱스 값을 뽑았으므로
                // 해당 인덱스로 몬스터 인덱스를 불러온다.
                var genMonster = sdStage.genMonsters[randIndex];

                // -1과 같다면, 몬스터가 존재하지 않는 스테이지이므로, 강제로 메서드를 종료한다.
                if (genMonster == -1)
                    return;

                // 생성할 몬스터의 기획 데이터를 가져온다.
                var sdMonster = sd.sdMonsters.Where(_ => _.index == genMonster).SingleOrDefault();

                // 몬스터 기획데이터를 이용하여 몬스터 풀에서 해당 데이터와 일치하는 몬스터를 가져온다.

                // 몬스터 리소스에 설정된 이름을 가져온다 (리소스 패스에서 마지막 슬래쉬가 있는 부분까지 전부 문자열 삭제)
                // -> 결과적으로 프리팹이름만 남음 (ex: Radish, Cat, Mushroom)
                // 비용이 좋지 못한 작업이므로 나중에 다른 방안으로 수정해보는 것 추천
                var monsterName = sdMonster.resourcePath.Remove(0, sdMonster.resourcePath.LastIndexOf('/') + 1);

                var monster = monsterPool.GetObj(_ => _.name == monsterName);

                // 풀에서 몬스터를 가져오지 못했을 시 다음으로 넘긴다.
                if (monster == null)
                    continue;

                // 이 곳의 로직이 실행된다는 것은 몬스터 풀에서 정상적으로 몬스터를 가져왔다는 뜻
                // 가져온 몬스터를 스폰 영역에 맞는 위치에 설정하고 초기화 후
                // 활성 몬스터 목록(인게임 매니저에 몬스터 리스트)에 넣어준다.

                // 스폰 영역 내에서 랜덤하게 스폰할 위치를 뽑음
                var bounds = spawnAreaBounds[sdStage.spawnArea[randIndex]];
                var spawnPosX = UnityEngine.Random.Range(-bounds.size.x * 0.5f, bounds.size.x * 0.5f);
                var spawnPosZ = UnityEngine.Random.Range(-bounds.size.z * 0.5f, bounds.size.z * 0.5f);

                monster.transform.position = bounds.center + new Vector3(spawnPosX, 0, spawnPosZ);
                // 풀에서 가져온 몬스터는 활성화하여 사용할 것이므로, 몬스터 홀더를 부모를 변경해준다.
                // -> 풀에 있는 객체들은 별도로 풀이 만든 홀더에 존재하고 있음
                monster.transform.SetParent(monsterHolder);
                monster.Initialize(new BoMonster(sdMonster));

                AddActor(monster);
            }

        }

        /// <summary>
        /// 몬스터 스폰시간 초기화
        /// </summary>
        private void InitSpawnTime()
        {
            var boStage = GameManager.User.boStage;

            if (Time.time - boStage.prevSpawnTime > boStage.spawnCheckTime)
            {
                // 스폰을 한 번 진쟁하므로, 이전 스폰시간을 현재 시간으로 변경
                boStage.prevSpawnTime = Time.time;
                // 스폰을 한 번 진행하므로, 스폰체크시간을 정해놓은 범위 안에 랜덤한 값으로 변경
                // -> 결과적으로 몬스터를 한 번 스폰한 후, 스폰시간이 랜덤하게 변경됨
                boStage.spawnCheckTime = UnityEngine.Random.Range(Define.Monster.MinSpawnTime, Define.Monster.MaxSpawnTime);
            }
        }

        /// <summary>
        /// 몬스터 스폰 시간을 체크하는 기능
        /// </summary>
        private void CheckSpawnTime()
        {
            if (currentStage == null)
                return;

            var boStage = GameManager.User.boStage;

            if (Time.time - boStage.prevSpawnTime > boStage.spawnCheckTime)
            {
                InitSpawnTime();
                SpawnMonster();
            }
        }

        /// <summary>
        /// 몬스터 인덱스를 받아 해당 몬스터의 스폰 구역을 찾아
        /// 해당 스폰 구역 내에서 랜덤한 위치를 반환
        /// </summary>
        /// <param name="monsterindex"></param>
        /// <returns></returns>
        public Vector3 GenRandPosInArea(int monsterindex)
        {
            var sdStage = GameManager.User.boStage.sdStage;

            int index = sdStage.genMonsters.Where(_ => _ == monsterindex).SingleOrDefault();

            // 스폰 영역 내에서 랜덤하게 스폰할 위치를 뽑음
            var bounds = spawnAreaBounds[sdStage.spawnArea[index]];
            var spawnPosX = UnityEngine.Random.Range(-bounds.size.x * 0.5f, bounds.size.x * 0.5f);
            var spawnPosZ = UnityEngine.Random.Range(-bounds.size.z * 0.5f, bounds.size.z * 0.5f);

            return bounds.center + new Vector3(spawnPosX, 0, spawnPosZ);
        }
    }
}
