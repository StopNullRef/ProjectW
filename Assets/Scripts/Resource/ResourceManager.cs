using ProjectW.Object;
using ProjectW.UI;
using ProjectW.Util;
using System;
using UnityEngine;
using UnityEngine.U2D;

namespace ProjectW.Resource
{
    /// <summary>
    /// 런타임(실행 시간)에 필요한 리소스를 불러오는 기능을 담당할 클래스
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        /// <summary>
        /// 초기화 기능
        /// </summary>
        public void Initialize()
        {
            LoadAllPrefabs();
            LoadAllAtlas();
        }

        /// <summary>
        /// Resources 폴더 내의 프리팹을 불러와 반환하는 기능
        /// </summary>
        /// <param name="path">Resource 폴더 내 불러올 에셋의 경로</param>
        /// <returns>불러온 프리팹 게임오브젝트</returns>
        public GameObject LoadObject(string path)
        {
            // Resources.Load()
            // -> Assets 폴더 내의 Resources 라는 이름의 폴더가 존재한다면
            // 해당 경로로부터 path를 읽음, 해당 경로에 파일이 GameObject로
            // 부를 수 있다면 불러옴
            return Resources.Load<GameObject>(path);
        }

        /// <summary>
        /// 오브젝트 풀 등록 시에 사용할 프리팹(원형 객체)를 불러오는 기능
        /// -> 프리팹 로드에 성공 후, 해당 메서드에서 바로 오브젝트 풀 매니저를 통해 등록시킨다.
        /// </summary>
        /// <typeparam name="T">불러오고자하는 프리팹이 갖는 타입</typeparam>
        /// <param name="path">프리팹 경로</param>
        /// <param name="poolCount">생성시키고자하는 초기 객체 수</param>
        /// <param name="complete">프리팹을 로드하고 오브젝트 풀에 등록 후 추가적으로 실행시킬 기능</param>
        public void LoadPoolableObject<T>(string path, int poolCount = 1, Action complete = null) where T : MonoBehaviour, IPoolableObject
        {
            // 프리팹 경로를 통해 프리팹을 불러온다.
            var obj = LoadObject(path);
            // 프리팹이 가지고 있는 T타입의 컴포넌트를 가져온다
            // -> 실질적으로 T타입의 컴포넌트가 IPoolableObject를 상속받고 있으므로
            var tComponent = obj.GetComponent<T>();

            // t타입의 풀을 등록
            ObjectPoolManager.Instance.ResgistPool<T>(tComponent, poolCount);

            // 위의 작업이 모두 끝난 후 추가적으로 실행시키고자 하는 기능이 있다면 실행
            complete?.Invoke();
        }

        /// <summary>
        /// 인게임에서 사용할 모든 프리팹을 부르는 기능
        /// </summary>
        private void LoadAllPrefabs()
        {
            LoadPoolableObject<HPBar>("Prefabs/UI/HPBar", 10);
            LoadPoolableObject<ItemBase>("Prefabs/UI/Item", 10);
        }

        /// <summary>
        /// Resources 폴더 내의 모든 아틀라스를 불러와 스프라이트 로더에 등록시키는 기능
        /// </summary>
        private void LoadAllAtlas()
        {
            // 현재는 한 번에 모든 아틀라스를 불러와서 스프라이트 로더에 등록시키는데
            // 최적화한다면...

            // ex) 아틀라스가 씬 별로 존재한다.
            // 씬 변경 시마다 변경된 씬에 맞는 아틀라스만을 불러오도록 수정

            // Resources/Sprite 폴더 경로에 있는 SpriteAtlas 타입의 에셋을 모두 불러온다.
            var atlases = Resources.LoadAll<SpriteAtlas>("Sprite");
            SpriteLoader.SetAtlas(atlases);
        }
    }
}
