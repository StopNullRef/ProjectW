using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Util
{
    /// <summary>
    /// 여러 종류의 오브젝트 풀들을 관리할 오브젝트 풀 매니저 클래스
    /// </summary>
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        /// <summary>
        /// 모든 풀을 보관할 딕셔너리
        /// ObjectPool<Monster>, ObjectPool<Character> 등의 서로 다른 타입의 풀을 편리하게
        /// 보관하기위해 object 타입으로 박싱하여 관리 -> 대신 성능면에서 좋은 방법은 아님, 생산성은 좋음..
        /// -> 가장 베스트는 각각의  서로 다른 타입의 오브젝트 풀을 필드로 하나씩 선언하는 것
        /// </summary>
        private Dictionary<Type, object> poolDic = new Dictionary<Type, object>();

        /// <summary>
        /// 풀 딕셔너리에 새로운 풀을 생성하여 등록하는 기능
        /// </summary>
        /// <typeparam name="T">생성할 풀의 타입</typeparam>
        /// <param name="obj">풀에 poolableObject 를  생성하여 등록하기 위한 원형객체(프리팹)</param>
        /// <param name="poolCount">초기에 생성할 poolableObejct의 수</param>
        public void ResgistPool<T>(T obj,int poolCount =1) where T : MonoBehaviour, IPoolableObject
        {
            ObjectPool<T> pool = null;
            
            // 딕셔너리의 키 값을 오브젝트 풀의 T타입으로 사용하고 있음
            // -> 따라서 딕셔너리에 키 값 확인 및 키를 추가할 용도로 쓰기 위해 키  값이되는 타입을 받아둠
            var type = typeof(T);

            // 동일한 키의 풀이 이미 존재하는지 검사
            if (poolDic.ContainsKey(type))
                // 키가 이미 존재하므로 해당 키에 접근하여 value인 오브젝트풀을 해당 T타입으로 캐스팅
                pool = poolDic[type] as ObjectPool<T>;
            else
            {
                // 해당 키값의 풀이 등록되어있지 않다면 생성하여 딕셔너리에 추가
                pool = new ObjectPool<T>();
                poolDic.Add(type, pool);
            }

            // 위의 과정을 통해 결과적으로 T타입의 풀을 받아둔다..
            // -> 풀에 초기에 객체를 생성해서 넣어두는 작업을 할 것이므로

            // 해당 풀이 사용할 홀더(하이라키 상의 부모 객체)가 있는지 확인하여 없다면 생성
            if(pool.holder == null)
            {
                // 사용하고자 하는 풀의 타입이름으로 홀더를 생성
                pool.holder = new GameObject($"{type.Name}Holder").transform;
                // 홀더의 부모를 오브젝트 풀 매니저로 설정
                pool.holder.SetParent(transform);
                pool.holder.position = Vector3.zero;
            }

            // 위의 과정을 통해 결과적으로 (풀)과 (풀 객체들의 하이라키 상의 부모인 홀더)를
            // 얻었으므로, 이제 풀에서 들고 있을 객체를 생성만 하면됨

            // 이 때 초기 생성할 객체의 수는 파라미터로 받은 poolCount만큼 생성한다.
            for(int i=0; i<poolCount; i++)
            {
                // 원형 객체(프리팹)을 이용하여 풀에 넣어 사용할 객체 생성
                var poolableObj = Instantiate(obj);
                poolableObj.name = obj.name;

                pool.Regist(poolableObj);
            }
        }

        /// <summary>
        /// PoolDic에 등록된 풀을 찾아서 반환하는 기능
        /// -> 해당 기능을 이용하여 내가 원하는 풀에 접근하여 풀에 있는 기능들을 사용할 수 있음
        /// </summary>
        /// <typeparam name="T">찾고자 하는 풀의 타입</typeparam>
        /// <returns>찾고자 하는 풀의 인스턴스</returns>
        public ObjectPool<T> GetPool<T>() where T: MonoBehaviour, IPoolableObject
        {
            var type = typeof(T);

            // 딕셔너리에 해당 타입(키)의 풀이 존재하는지 검사
            if (!poolDic.ContainsKey(type))
                return null;

            return poolDic[type] as ObjectPool<T>;
        }

        /// <summary>
        /// 특정 풀이 들고 있는 객체들을 전부 비우는 기능
        /// </summary>
        /// <typeparam name="T">비우고자하는 풀의 타입</typeparam>
        public void ClearPool<T>() where T : MonoBehaviour,IPoolableObject
        {
            // 오브젝트 풀은 객체를 파괴하지 않고 재사용한다고 했는데
            // 왜 풀 안에 있는 객체들을 전부 비우나요(파괴하나요)??
            // -> 특정 시점에서 더이상 들고 있을 피룡가 없는 풀이 발생하는 경우가 있음
            // 그런 경우에 해당 풀을 비우기 위해서

            // T 타입 풀의 인스턴스를 찾음
            // 찾은 인스턴스가 존재한다면 내부의 pool 리스트를 담음, 없을 경우 null이 담김
            var pool = GetPool<T>()?.Pool;

            // 풀 리스트가 없다면 리턴
            if (pool == null)
                return;

            // 있다면 풀 리스트 안에 있는 객체들을 전부 파괴
            for (int i = 0; i < pool.Count; i++)
            {
                if(pool[i] != null)
                        Destroy(pool[i].gameObject);
            }

            pool.Clear();
        }
    }
}
