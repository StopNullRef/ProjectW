using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.UI
{
    /// <summary>
    /// UI 내에 UIElement(큰 틀의 UI 스크립트가 아닌 UI에서 부분적으로 사용되는 스크립트)를
    /// 제외한 모든 UI의 베이스 클래스 (큰 틀의 UI들의 베이스 클래스)
    /// UIWindow는 캔버스 그룹 컴포넌트를 강제로 갖게 됨
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIWindow : MonoBehaviour
    {
        /// <summary>
        /// 강제로 달아둔 캔버스 그룹을 담아둘 필드
        /// 캔버스그룹을 통해 UI를 활성/비활성화 하는 효과를 줌 (알파값 0~1을 사용)
        /// 비활성화 시 실제로 비활성화되는 것이 아니므로 UI 입력과 감지를 차단
        /// (인터렉터블, 블록 레이캐스트 false)
        /// </summary>
        private CanvasGroup cachedCanvasGroup;

        public CanvasGroup CachedCanvasGroup
        {
            get
            {
                if (cachedCanvasGroup == null)
                    cachedCanvasGroup = GetComponent<CanvasGroup>();

                return cachedCanvasGroup;
            }


        }
        /// <summary>
        /// 해당 UI를 esc 키로 닫을 수 있게 설정할건지?
        /// </summary>
        public bool canCloseESC;

        /// <summary>
        /// UI의 활성화 상태
        /// -> 씬에서 UI 배치 시 처음에 화면에 보이지 않는 UI라도 무조건 활성화시키고
        /// 대신 게임 시작 시에 꺼진 상태로 두려면 isOpen을 false로 두면
        /// 내부 로직을 통해 시작 시에 자동으로 비활성화 됨
        /// </summary>
        public bool isOpen;

        public virtual void Start()
        {


            InitWindow();
        }

        public virtual void InitWindow()
        {
            // UWM에 해당 UW 인스턴스(자기자신)을 등록
            UIWindowManager.Instance.AddTotalWindow(this);

            // 초기에 인스펙터 상에 isOpen을 체크했다면 강제로 열고,아니면 닫고
            if (isOpen)
                Open(true);
            else
                Close(true);
        }

        /// <summary>
        /// UI 활성화 기능
        /// </summary>
        /// <param name="force">강제로 활성화시킬건지?</param>
        public virtual void Open(bool force = false)
        {
            if (!isOpen || force)
            {
                isOpen = true;
                UIWindowManager.Instance.AddOpenWindow(this);
                SetCanvasGroup(true);
            }
        }

        /// <summary>
        /// UI를 비활성화하는 기능
        /// </summary>
        /// <param name="force"></param>
        public virtual void Close(bool force = false)
        {
            if (isOpen || force)
            {
                isOpen = false;
                UIWindowManager.Instance.RemoveOpenWindow(this);
                SetCanvasGroup(false);
            }
        }

        /// <summary>
        /// 활성화 상태에 따라 캔버스 그룹 내 필드를 설정
        /// </summary>
        /// <param name="isActive"></param>
        private void SetCanvasGroup(bool isActive)
        {
            CachedCanvasGroup.alpha = Convert.ToInt32(isActive);
            CachedCanvasGroup.interactable = isActive;
            CachedCanvasGroup.blocksRaycasts = isActive;
        }
    }
}
