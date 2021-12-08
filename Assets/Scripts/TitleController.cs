using ProjectW.Define;
using ProjectW.Dummy;
using ProjectW.Network;
using ProjectW.Resource;
using ProjectW.UI;
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
        /// ��Ʈ�� ����� ���� �ε� �����Ȳ�� UI�� �����Ͽ� ����ϱ� ���� uiTitle�� ���� ����
        /// </summary>
        public UITitle uiTitle;

        /// <summary>
        /// �ε� ������ �ִϸ��̼� ó���� ���� �ڷ�ƾ
        /// </summary>
        private Coroutine loadGaugeUpdateCoroutine;


        /// <summary>
        /// Ÿ��Ʋ ��Ʈ�ѷ� �ʱ�ȭ
        /// </summary>
        public void Initialize()
        {
            OnPhase(introPhase);
        }

        /// <summary>
        /// ���� ����� ���� ���� ����
        /// </summary>
        /// <param name="phase">���� ��Ű���� �ϴ� ���� ������</param>
        private void OnPhase(IntroPhase phase)
        {
            // ���� ����� ���� �������� ���ڿ��� �����Ͽ� �ε� ���¸� ��Ÿ���� �ؽ�Ʈ�� ����
            uiTitle.SetLoadStateDescription(phase.ToString());
            
            // �ε������� ui�� fillAmount�� ���� ���� �ε� ������ �ۼ�Ʈ�� ���� ������ ������ �ȵ� ���¿���
            // ���� �ε� ������� ���� ������� ������ �̷�����ٸ�??
            // �� �� ������ �߻���
            // -> ������ �ڷ�ƾ�� �̹� ����ǰ� �ִµ�, �� ���ο� �ڷ�ƾ�� �߰������� �����Ű�� �Ǹ�
            // ������ �߻���

            // �ذ���
            // ���� �������� �ڷ�ƾ�� �����Ѵٸ� �ڷ�ƾ�� ������ ���� �Ŀ�, ���ο� �ε� ������ �ۼ�Ʈ��
            // �Ѱܼ� �ڷ�ƾ�� �ٽ� ���۽�Ų��.

            // �ε� ������ �ִϸ��̼� ó���� ���� �ڷ�ƾ�� �����Ѵٸ�
            if(loadGaugeUpdateCoroutine != null)
            {
                // �ڷ�ƾ�� ������ ���߷� null�� �ʱ�ȭ��Ų��.
                StopCoroutine(loadGaugeUpdateCoroutine);
                loadGaugeUpdateCoroutine = null;
            }

            // ����� ����� ��ü ������ �Ϸᰡ �ƴ϶��
            if (phase != IntroPhase.Comepelte)
            {
                // ���� �ε� �ۼ������ ���Ѵ�.
                var loadPer = (float)phase / (float)IntroPhase.Comepelte;
                // ���� �ۼ��������� �ε��ٿ� ����
                // �ڷ�ƾ ���� �ÿ��� StartCoroutine() �޼��带 �̿�
                loadGaugeUpdateCoroutine = StartCoroutine(uiTitle.LoadGaugeUpdate(loadPer));
            }
            else
                uiTitle.loadFillGauge.fillAmount = 1f;

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
                    GameManager.SD.Initialize();
                    LoadComplete = true;
                    break;
                case IntroPhase.UserData:
                    new LoginHandler().Connect();
                    LoadComplete = true;
                    break;
                case IntroPhase.Resource:
                    ResourceManager.Instance.Initialize();
                    LoadComplete = true;
                    break;
                case IntroPhase.UI:

                    LoadComplete = true;
                    break;
                case IntroPhase.Comepelte:
                    allLoaded = true;
                    var ingameManager = InGameManager.Instance;
                    GameManager.Instance.LoadScene(SceneType.Ingame, ingameManager.ChangeStage());
                    LoadComplete = true;

                    break;
            }
        }

        /// <summary>
        /// ���� ����� ���� ������� �����ϴ� ���
        /// </summary>
        private void NextPhase()
        {
            // ���� �� ������� �����ϴ� �۾��� ���� ó���� �ʿ��� �۾��� �ƴϹǷ�
            // ���� ����� �ʹ� ���� ����Ǿ� ������ ��������� Ȯ���ϱ� �����..
            // ���� �Ϻη� ������ ���濡 �����̸� �־� ��������� Ȯ����  �� �ְ� ����..
            StartCoroutine(WaitForSeconds());
        }

        private IEnumerator WaitForSeconds()
        {
            // �뷫 1�� �Ŀ� �Ʒ� �ڵ尡 �����
            yield return new WaitForSeconds(0.2f);
            loadComplete = false;
            OnPhase(++introPhase);
        }

    }
}