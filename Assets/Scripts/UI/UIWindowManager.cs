using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectW.Util;

namespace ProjectW.UI
{
    /// <summary>
    /// 모든 UIWindow 객체를 관리하는 매니저
    /// </summary>
    public class UIWindowManager : Singleton<UIWindowManager>
    {
        // 주석에서 아래와 같이 표현
        // UIWindowManager == UWM, UIWindow == UW

        /// <summary>
        /// UWM에 등록된 현재 열려있는 모든 UW를 갖는 리스튼
        /// </summary>
        private List<UIWindow> totalOpenWindows = new List<UIWindow>();
        /// <summary>
        /// UWM에 등록된 모든 UW를 갖는 리스트
        /// -> 반복용(순회용)
        /// </summary>
        private List<UIWindow> totalUIWindows = new List<UIWindow>();

        /// <summary>
        /// UWM에 등록 시 UW 객체를 담아둘 딕셔너리
        /// -> 탐색용
        /// </summary>
        private Dictionary<string, UIWindow> cachedTotalUIWindows = new Dictionary<string, UIWindow>();

        /// <summary>
        /// UWM에 등록된 UW의 인스턴스 접근 시 (UWM을 이용해서 특정 인스턴스 접근 메서드를 사용할 시에)
        /// 해당 인스턴스를 담아둘 딕셔너리(내가 코드로 직접 접근한 UW 객체만 담김)
        /// </summary>
        private Dictionary<string, object> cachedInstance = new Dictionary<string, object>();

        public void Initilaize()
        {
            InitAllWindow();
        }

        private void Update()
        {
            // esc 키를 누른다면
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                // 현재 열려있는 UI 중 가장 마지막에 열린 UI를 가져옴
                var targetWindow = GetTopWindow();

                // 가져온 UW가 null이 아니고 esc키로 닫을 수 있다면
                if(targetWindow != null && targetWindow.canCloseESC)
                {
                    targetWindow.Close();
                }
            }
        }


        /// <summary>
        /// UWM에 등록된 모든 UW를 초기화하는 기능
        /// </summary>
        public void InitAllWindow()
        {
            for (int i=0; i< totalUIWindows.Count; i++)
            {
                totalUIWindows[i]?.InitWindow();
            }
        }

        /// <summary>
        /// UWM에 등록된 모든 UW를 닫는 기능
        /// </summary>
        public void CloseAll()
        {
            for (int i = 0; i < totalUIWindows.Count; i++)
            {
                totalUIWindows[i]?.Close(true);
            }
        }

        /// <summary>
        /// 토탈 UW 리스트, 토탈 UW 딕셔너리에 UW를 등록하는 기능
        /// </summary>
        /// <param name="uiWindow">등록하고자하는 UW 인스턴스</param>
        public void AddTotalWindow(UIWindow uiWindow)
        {
            // 파생클래스 타입이름을 키 값으로 지정
            var key = uiWindow.GetType().Name;

            // 키값이 딕셔너리에 존재하는지를 나타내는 필드
            bool hasKey = false;

            // 전체 UW 리스트에 등록하고자 하는 인스턴스가 있는지
            // 또는 전체 UW 딕셔너리에 해당 인스턴스 타입 이름의 키각 존재하는지 확인
            if(totalUIWindows.Contains(uiWindow)|| cachedTotalUIWindows.ContainsKey(key))
            {
                // 딕셔너리에 키값으로 밸류에 접근 시, 해당 밸류가 null이 아니라면
                // 등록하고자 하는 UW가 이미 존재하므로 리턴
                if (cachedTotalUIWindows[key] != null)
                    return;

                // 키 값은 있는데 밸류가 null 이라면 참조하고 있는 UW의 인스턴스가 없다는 것
                // ex> 로비씬에서 사용하는 UW가 UWM에 등록되어있는데, 인게임으로 씬이 전환되면서
                // 해당 UW의 인스턴스가 파괴되서, 키는 존재하는데 밸류(인스턴스)가 null이 됨
                else
                {
                    hasKey = true;

                    // 리스트 내에 인스턴스가 존재하지 않는 원소가 있는지 확인하여 제거 한다.
                    for(int i = totalUIWindows.Count -1; i>0; i--)
                    {
                        if (totalUIWindows[i] == null)
                            totalUIWindows.RemoveAt(i);
                    }
                }
            }

            totalUIWindows.Add(uiWindow);

            // 키가 존재한다면 인스턴스를 넣어줌
            if (hasKey)
                cachedTotalUIWindows[key] = uiWindow;
            // 없다면 키와 인스턴스를 추가시킴
            else
                cachedTotalUIWindows.Add(key, uiWindow);

        }

        /// <summary>
        /// 전체 열려있는 UW 리스트에 활성화된 UW를 등록하는 기능
        /// </summary>
        /// <param name="uIWindow">등록하고자하는 활성화된 UW 인스턴스</param>
        public void AddOpenWindow(UIWindow uIWindow)
        {
            // 전체 열려있는 UW리스트에 이미 존재하지 않다면 넣음
            if (!totalOpenWindows.Contains(uIWindow))
                totalOpenWindows.Add(uIWindow);
        }

        /// <summary>
        /// 전체 열려있는 UW 리스트에 비활성화된 UW를 제거하는 기능
        /// </summary>
        /// <param name="uIWindow">비활성화 하고자하는 UW 인스턴스</param>
        public void RemoveOpenWindow(UIWindow uIWindow)
        {
            // 전체 열려있는 UW리스트에 존재한다면 지움
            if (totalOpenWindows.Contains(uIWindow))
                totalOpenWindows.Remove(uIWindow);
        }

        /// <summary>
        /// UWM에 등록된 T타입 유형의 UW를 반환하는 기능
        /// </summary>
        /// <typeparam name="T">찾고자하는 T타입의 UW 인스턴스</typeparam>
        /// <returns>찾은 T타입의 UW 인스턴스</returns>
        public T GetWindow<T>() where T : UIWindow
        {
            string key = typeof(T).Name;

            // 토탈 UW  딕셔너리에 없으면 null 반환
            if (!cachedTotalUIWindows.ContainsKey(key))
                return null;

            // 현재 메서드를 통해 접근하는 UW는 모두 토탈 인스턴스 딕셔너리에 등록된다

            // 인스턴스 딕셔너리에 키가 없다면 전체 딕셔너리에서 UW 인스턴스를 가져와
            // T타입으로 캐스팅 후 등록

            if(!cachedInstance.ContainsKey(key))
            {
                cachedInstance.Add(key, (T)Convert.ChangeType(cachedTotalUIWindows[key], typeof(T)));
            }
            else if (!cachedInstance.ContainsKey(key) || cachedInstance[key].Equals(null))
            {
                cachedInstance[key] = (T)Convert.ChangeType(cachedTotalUIWindows[key], typeof(T));
            }

            // 인스턴스 딕셔너리를 통해서 UW 베이스 형태가 아닌 최종 파생클래스형태 (T타입)으로
            // 원하는 UW의 파생 인스턴스를 가져올 수 잇음
            return (T)cachedInstance[key];
        }

        /// <summary>
        /// 현재 열려있는 UW 중 가장 최상위(가장 마지막에 열린) UW  인스턴스를 반환하는 기능
        /// </summary>
        /// <returns></returns>
        public UIWindow GetTopWindow()
        {
            for(int i= totalUIWindows.Count-1; i>=0; i--)
            {
                if (totalUIWindows[i] != null)
                    return totalUIWindows[i];
            }
            return null;
        }
    }
}
