using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Util
{
    /// <summary>
    /// 오브젝트 풀링을 수행할 클래스
    /// -> 어떤 타입의 객체를 담는 풀이 필요하지 모르므로, 클래스를 일반화시켜
    /// 내가 만들고 하는 타입의 풀을 생성할 수 있도록 한다.
    /// </summary>
    /// <typeparam name="T">모노를 상속 받고, IPoolableObject를 구현하는 타입만 올수 있도록</typeparam>
    public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
    {
        /// <summary>
        /// T타입의 객체들을 갖는 리스트
        /// </summary>
        public List<T> Pool { get; private set; } = new List<T>();

        /// <summary>
        /// 해당 풀의 객체들을 하이에라키상에서 들고 있을 부모 객체의 트랜스폼
        /// -> 하이라키상에서 보기좋게 정리용으로 사용할 홀더
        /// </summary>
        public Transform holder;

        /// <summary>
        /// 풀에 새로운 T타입 오브젝트를 등록하는 기능
        /// </summary>
        /// <param name="obj">풀에 등록하고자 하는 T타입 인스턴스</param>
        public void Regist(T obj)
        {
            obj.transform.SetParent(holder);
            obj.gameObject.SetActive(false);
            obj.CanRecyle = true;

            Pool.Add(obj);
        }

        /// <summary>
        /// 객체를 다시 풀에 반환하는 기능
        /// (정확히는 객체는 항상 똑같은 풀에 있고, 풀에서 객체를 꺼낸다고 표현을 하고 있지만
        /// 실제로 이루어지는 작업은, 재사용 가능한 객체를 찾아서 활성/비활성화
        /// 풀에 있을 경우(비활성화) -> 객체의 부모는 오브젝트 풀의 holder
        /// 풀에서 꺼낸 경우(활성화) -> 상황에 따라 다른 부모(내가 원하는 부모)를 가짐
        /// </summary>
        /// <param name="obj">풀에 다시 반환할 T타입 인스턴스</param>
        public void Return(T obj)
        {
            obj.transform.SetParent(holder);
            obj.gameObject.SetActive(false);
            obj.CanRecyle = true;
        }

        /// <summary>
        /// 풀에서 재사용 가능한 객체를 반환하는 기능
        /// </summary>
        /// <param name="pred">풀 내에서 재사용 객체를 특정 조건으로 검사하고자 할 시
        /// 해당 조건을 갖는 대리자(method)</param>
        /// <returns></returns>
        public T GetObj(Func<T,bool>pred)
        {
            // 풀 내에서 재사용 가능한 객체가 존재하는지 검사
            if(!(Pool.Find(obj => obj.CanRecyle) != null))
            {
                // 재사용 가능한 객체가 없을 경우 들어옴

                // 풀에서 조건과 동일한 객체가 있을 경우,
                // 해당 객체를 찾아서 새로운 객체를 생성
                // 없을 경우 null을 반환

                var protoObj = Pool.Find(obj => pred(obj));

                if (protoObj != null)
                {
                    var newObj = GameObject.Instantiate(protoObj.gameObject, holder);
                    newObj.name = protoObj.name;
                    // 새로 생성한 객체를 풀에 등록
                    Regist(newObj.GetComponent<T>());
                }
                else
                    return null;
            }

            // 파라미터로 받은 조건에 해당하는 객체가 존재하는지 검사
            var recycleObj = Pool.Find(obj => pred(obj) && obj.CanRecyle);

            if (recycleObj == null)
                return null;

            // 해당 객체를 반환해서 재사용할 것이므로, 더이상 재사용할 수 없게 재사용 불가능 상태로 변경
            recycleObj.CanRecyle = false;

            return recycleObj;
        }
    }
}
