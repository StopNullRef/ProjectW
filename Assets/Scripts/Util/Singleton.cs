using UnityEngine;

namespace ProjectW.Util
{
    /// <summary>
    /// 디자인 패턴 중 하나, 싱글턴 패턴
    ///  -> 씬이 변경되어도 데이터가 유지되어야 하고,
    /// 외부에서 호출이 잦음.
    /// 여러 객체를 사용하는 것이 아닌 단일 객체 사용.
    /// 이러한 경우 싱글턴 패턴을 적용
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary>
        /// 싱글턴에서 파생된 클래스 객체의 인스턴스
        /// </summary>
        private static T instance = null;

        /// <summary>
        /// 외부에서 싱글턴 인스턴스에 접근하기 위한 프로퍼티
        /// </summary>
        public static T Instance
        {
            get
            {
                // 인스턴스가 존재하는지 체크
                if (instance == null)
                {
                    // 인스턴스를 씬에서 찾음
                    instance = FindObjectOfType<T>();
                    // 인스턴스를 찾았는지 체크
                    if (instance == null)
                    {
                        // 없다면 게임오브젝트를 생성하여
                        GameObject obj = new GameObject(typeof(T).Name);
                        // T타입의 컴포넌트를 붙여준다.
                        instance = obj.AddComponent<T>();
                    }
                }

                // 결과적으로, 처음에 존재하지않는다면 찾거나 생성하여 반환 후
                // 이후에는 동일한 인스턴스를 계속 반환하게 된다.
                //  -> 단 하나의 인스턴스만을 가리킨다.
                return instance;
            }
        }

        // Awake가 호출되었다는 것은 하이라키상에 T타입의 컴포넌트를 가진
        // 객체가 존재한다는 뜻
        protected virtual void Awake()
        {
            // 인스턴스가 없다면
            if (instance == null)
            {
                // 인스턴스를 미리 넣어주는 작업
                //  -> Instance 프로퍼티를 통해 접근 시 객체를 찾거나 생성하는 과정을 생략
                instance = this as T;
                // 씬이 변경되도 게임 오브젝트가 파괴되지 않도록
                // gameObject : Mono를 상속받았다면, 컴포넌트 형태로 붙일 수 있음
                // 컴포넌트를 붙이는 게임오브젝트가 존재한다는 뜻
                //  -> gameObject는 해당 컴포넌트 객체를 갖는 게임 오브젝트를 가르키는 프로퍼티
                DontDestroyOnLoad(gameObject);
            }
            // 인스턴스가 있다면
            else
            {
                // 이 시점에 인스턴스가 존재한다는 것은 잘못된 사용으로 인한 복수의 인스턴스 생성
                // Destroy : 파라미터로 넘긴 게임오브젝트를 파괴하는 기능
                Destroy(gameObject);
            }
        }
    }

}
