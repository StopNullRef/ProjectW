using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.UI
{
    /// <summary>
    /// UI ���� UIElement(ū Ʋ�� UI ��ũ��Ʈ�� �ƴ� UI���� �κ������� ���Ǵ� ��ũ��Ʈ)��
    /// ������ ��� UI�� ���̽� Ŭ���� (ū Ʋ�� UI���� ���̽� Ŭ����)
    /// UIWindow�� ĵ���� �׷� ������Ʈ�� ������ ���� ��
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIWindow : MonoBehaviour
    {
        /// <summary>
        /// ������ �޾Ƶ� ĵ���� �׷��� ��Ƶ� �ʵ�
        /// ĵ�����׷��� ���� UI�� Ȱ��/��Ȱ��ȭ �ϴ� ȿ���� �� (���İ� 0~1�� ���)
        /// ��Ȱ��ȭ �� ������ ��Ȱ��ȭ�Ǵ� ���� �ƴϹǷ� UI �Է°� ������ ����
        /// (���ͷ��ͺ�, ��� ����ĳ��Ʈ false)
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
        /// �ش� UI�� esc Ű�� ���� �� �ְ� �����Ұ���?
        /// </summary>
        public bool canCloseESC;

        /// <summary>
        /// UI�� Ȱ��ȭ ����
        /// -> ������ UI ��ġ �� ó���� ȭ�鿡 ������ �ʴ� UI�� ������ Ȱ��ȭ��Ű��
        /// ��� ���� ���� �ÿ� ���� ���·� �η��� isOpen�� false�� �θ�
        /// ���� ������ ���� ���� �ÿ� �ڵ����� ��Ȱ��ȭ ��
        /// </summary>
        public bool isOpen;

        public virtual void Start()
        {


            InitWindow();
        }

        public virtual void InitWindow()
        {
            // UWM�� �ش� UW �ν��Ͻ�(�ڱ��ڽ�)�� ���
            UIWindowManager.Instance.AddTotalWindow(this);

            // �ʱ⿡ �ν����� �� isOpen�� üũ�ߴٸ� ������ ����,�ƴϸ� �ݰ�
            if (isOpen)
                Open(true);
            else
                Close(true);
        }

        /// <summary>
        /// UI Ȱ��ȭ ���
        /// </summary>
        /// <param name="force">������ Ȱ��ȭ��ų����?</param>
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
        /// UI�� ��Ȱ��ȭ�ϴ� ���
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
        /// Ȱ��ȭ ���¿� ���� ĵ���� �׷� �� �ʵ带 ����
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
