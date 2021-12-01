using ProjectW.SD;
using ProjectW.Util;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW
{
    /// <summary>
    /// ���ӿ� ����ϴ� ��� �����͸� �����ϴ� Ŭ����
    /// �߰��� ������ �� ���� ��� ���� ū �帧 ���� �����ϱ⵵ ��
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// �ش� �ʵ� true��� ���̼����� ���
        /// </summary>
        public bool useDummyServer;

        /// <summary>
        /// ��ȹ �����͸� ���� ��ü
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
        /// �� �⺻ ����
        /// </summary>
        public void OnApplicationSetting()
        {
            // ��������ȭ ����
            QualitySettings.vSyncCount = 0;
            // ���� �������� 60���� ����
            Application.targetFrameRate = 60;
            // �� ���� �� ��ð� ��� �ÿ��� ȭ���� ������ �ʰ�
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
