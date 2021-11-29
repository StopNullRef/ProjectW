using ProjectW.Define;
using ProjectW.Dummy;
using ProjectW.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW
{
    /// <summary>
    /// Ÿ��Ʋ ������ ���� ���� ���� �ʿ��� �������� �ʱ�ȭ ��
    /// ������ �ε� ���� �����ϴ� Ŭ����
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        /// <summary>
        /// ���� �������� �Ϸ� ����
        /// </summary>
        private bool loadComplete;

        /// <summary>
        /// �ܺο��� loadComplete�� �����ϱ� ���� ������Ƽ
        /// �߰��� ���� ������ �Ϸ�� ���ǿ� ���� ���� ������� ����
        /// </summary>
        public bool LoadComplete
        {
            get => loadComplete;
            set
            {
                loadComplete = value;

                //���� ����� �Ϸ�Ǿ���, ��� ����� �Ϸ���� �ʾҴٸ�
                if (loadComplete && !allLoaded)
                    //���� ������� ����
                    NextPhase();
            }
        }

        /// <summary>
        /// ��� �������� �Ϸ� ����
        /// </summary>
        private bool allLoaded;

        /// <summary>
        /// ���� ����� ��Ÿ��
        /// </summary>
        private IntroPhase introPhase = IntroPhase.Start;

        /// <summary>
        /// ���� ����� ���� ���� ����
        /// </summary>
        /// <param name="phase">���� ��Ű���� �ϴ� ���� ������</param>
        private void OnPhase(IntroPhase phase)
        {
            switch (phase)
            {
                case IntroPhase.Start:
                    LoadComplete = true;
                    break;
                case IntroPhase.ApplicationSetting:
                    GameManager.Instance.OnApplicationSetting();
                    LoadComplete = true;
                    break;
                case IntroPhase.Server:
                    DummyServer.Instance.Initialize();
                    ServerManager.Instance.Initialize();
                    LoadComplete = true;
                    break;
                case IntroPhase.StaticData:

                    LoadComplete = true;
                    break;
                case IntroPhase.UserData:

                    LoadComplete = true;
                    break;
                case IntroPhase.Resource:

                    LoadComplete = true;
                    break;
                case IntroPhase.UI:

                    LoadComplete = true;
                    break;
                case IntroPhase.Comepelte:

                    LoadComplete = true;
                    break;
            }
        }

        /// <summary>
        /// ���� ����� ���� ������� �����ϴ� ���
        /// </summary>
        private void NextPhase()
        {

        }

    }
}