using ProjectW.DB;
using ProjectW.Define;
using ProjectW.SD;
using ProjectW.UI;
using ProjectW.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectW
{
    /// <summary>
    /// ���ӿ� ����ϴ� ��� �����͸� �����ϴ� Ŭ����
    /// �߰��� ������ �� ���� ��� ���� ū �帧 ���� �����ϱ⵵ ��
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// �ε� ������ �ε� ������¸� ��Ÿ�� �ʵ� (0~1)
        /// </summary>
        public float loadProgress;

        /// <summary>
        /// �ش� �ʵ� true��� ���̼����� ���
        /// </summary>
        public bool useDummyServer;

        /// <summary>
        /// ���� ������ (DB���� �޾ƿ� ������)
        /// </summary>
        [SerializeField]
        private BoUser boUser = new BoUser();

        public static BoUser User => Instance.boUser;

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

        /// <summary>
        /// ���� �񵿱�� �ε��ϴ� ���
        /// �ٸ� �� ���� ��ȯ�� ��� (ex: Title-> InGame)
        /// </summary>
        /// <param name="sceneName">�ε��� ���� �̸��� ���� ������</param>
        /// <param name="loadCoroutine">�� ��ȯ �� �ε������� ���� ���� �ʿ��� �̸� ó���� �۾�</param>
        /// <param name="loadComplete">�� ��ȯ �Ϸ� �� ������ ���</param>
        public void LoadScene(SceneType sceneName, IEnumerator loadCoroutine = null, Action loadComplete = null)
        {
            // SceneManager.LoadScene(); ����ȭ ����� �� ��ȯ
            // -> �� ��ȯ �۾��� ó���ϴ���, ���� ���� �׵��� ��������

            // SceneManager.LoadSceneAsync(); �񵿱� ����� �� ��ȯ
            // -> �� ��ȯ �ÿ� ������ ������� ������ ����

            // �̹��� �� ��ȯ �ÿ� �ٷ� ���� ��ȯ�ϴ� ���� �ƴ� ����� -> ���� �� ���̿� �ε� ���� �߰��Ͽ�
            // �ε������� �������� �ʿ��� ������ �ε� �۾��� ������ ����
            // �׷� �� �� ������ �ε� ��Ȳ�� ���� ������ �ε����� ������ַ��� �� ��ȯ ��������
            // ���� ���� ����������, �����Ȳ�� ����� ������� ����
            // ����, ������ ���� ����� �� �ε尡 �ƴ� �񵿱� ����� �� �ε尡 �ʿ���

            StartCoroutine(WaitForLoad());

            // LoadScene �޼��忡���� ��밡���� ���� �Լ� ����
            IEnumerator WaitForLoad()
            {
                // �ε� ������¸� ��Ÿ�� (0 ~ 1)
                // ó�� �� �ε��� �����ϱ��� �ε� ������¸� 0���� �ʱ�ȭ
                loadProgress = 0;

                // �ڷ�ƾ�� ����ϴ� ���� ū ����?
                // yield return Ű���带 �̿��ؼ� ���� ���ϴ� Ÿ�̹�(����)�� Ư�� �ڵ带
                // �����Ű�� ���ϱ� ������

                // �񵿱�� �ε� ���� �ҷ���
                yield return SceneManager.LoadSceneAsync(SceneType.Loading.ToString());

                // �ε� ������ ��ȯ �Ϸ� �Ŀ� �Ʒ� ������ �����

                // ���� �ҷ������� �ϴ� ���� �߰�
                // -> �� ���� ��, �ε�� �ɼ��� additive�� �ָ� ���� ���� ����Ǵ� ���� �ƴ�
                // ���� ���� ���ο� ���� �߰��Ǿ� ���̶�Ű�� ������ ���� �����ϰ� ��
                var asyncOper = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);

                // ��� �߰��� ���� ��Ȱ��ȭ
                // ����? �ε� ������ ���ο� ���� �ϳ��� �߰��Ǿ� 2���� ���� Ȱ��ȭ�� �����ε�..
                // �� �� ���� ��Ʈ������ �ʴ�  �ΰ��� ���� ��� ȭ�鿡 �������� ��
                // ����, ���� �����ϰ��� �ϴ� ���� �߰��ص� ���� ����ڿ��� �ε� ���� �����ְ�
                // �ε����� Ȱ��ȭ�Ǿ��ִ� ���� ���� �����ϰ��� �ϴ� ���� �ʿ��� ������ ���� �ε� �ϴ� �۾��� �ϱ� ���ؼ�
                asyncOper.allowSceneActivation = false;

                // �����ϰ��� �ϴ� ���� �ʿ��� �۾��� �����Ѵٸ� ����
                if (loadCoroutine != null)
                {
                    // �ش� �۾��� �Ϸ�� ������ ���
                    yield return StartCoroutine(loadCoroutine);
                }

                // ���� �۾��� �Ϸ�� �Ŀ� �Ʒ� ������ �����

                // �ε� ���� ���¸� ��Ÿ���� loadProgress ���� �̿��ؼ� ��������� �ε� ���� ���¸� �˷���
                // -> ��ǻ� ������ �ε� �۾��� ���ʿ��� �̹� ������.. 
                //  loadProgress�� �̿��� ��Ÿ������ �ϴ� ���� ���� �����ϰ����ϴ� ���� �ε� ����
                // ���� ������ �ε常 ������, �����ϰ��� �ϴ� ���� �θ��ڸ��� ��Ȱ��ȭ �Ͽ��� ������
                // ������ ���� �ε�� ���°� �ƴ�.. ������ �ε尡 ������ ������ ��Ȱ��ȭ�� ���� �ٽ� Ȱ��ȭ����
                // ���� �� �ε带 �����Ű��, �� �ε� ������ loadProgress�� ��Ÿ��

                // �񵿱�� �ε��� ���� Ȱ��ȭ�� �Ϸ���� �ʾҴٸ� Ư�� �۾����ݺ�
                while (!asyncOper.isDone)
                {
                    // isDone �� false��� ���� ���� �ε尡 ������ ���� ����
                    // �׷� �̶� ���� �ε� ��Ȳ�� loadProgress�� ��Ÿ����.
                    if (loadProgress >= 0.9f)
                    {
                        loadProgress = 1f;

                        // �ε��ٰ� ���������� ���� ���� Ȯ���ϱ� ���� 1������ ���
                        yield return new WaitForSeconds(1f);

                        // �����ϰ����ϴ� ���� �ٽ� Ȱ��ȭ
                        // isDone�� ���� Ȱ�����°� �ƴ϶�� progress�� 1�� �Ǿ
                        // true�� �ȵ�
                        asyncOper.allowSceneActivation = true;
                    }

                    loadProgress = asyncOper.progress;

                    // �ڷ�ƾ ������ �ݺ��� ��� �� ������ �� �� ���� ��, �ڷ�ƾ�� Ż���Ͽ�
                    // ���� ������  ������ �� �ְ� yield return�� ���
                    yield return null;
                }

                // ���� �ݺ��۾��� �� ���� �Ŀ� �Ʒ����� ����

                // �ε������� ���� ���� �ʿ��� �۾��� ���� ���������Ƿ� �ε����� �ٽ� ��Ȱ��ȭ ��Ŵ
                yield return SceneManager.UnloadSceneAsync(SceneType.Loading.ToString());

                // ��� �۾��� �Ϸ�Ǿ����Ƿ� ��� �۾� �Ϸ� �� �����ų ������ �ִٸ� ����
                loadComplete?.Invoke();
            }

        }

        /// <summary>
        /// �ΰ��� ������ �������� ��ȯ �� ���
        /// -> ���� ���� �����ϴ� ���� �ƴ�, �ε� ���� �̿��Ͽ� �� ��ȯó�� �������� ���̵��� ��
        /// �� ��, �ε� ���� �̿��Ͽ� �����ϰ��� �ϴ� ���������� �ʿ��� ���ҽ� �ε峪 �ʱ�ȭ �۾� ���� ó��
        /// ex) �ΰ��Ӿ�(���۸���) -> �ΰ��Ӿ�(�ʺ��ڻ����)
        /// </summary>
        /// <param name="loadCoutine">�ε� �߿� ó���� �۾�</param>
        /// <param name="loadComplete">�ε� �Ϸ� �� ó���� �۾�</param>
        public void OnAdditiveLoadingScene(IEnumerator loadCoutine = null, Action loadComplete = null)
        {
            StartCoroutine(WaitForLoad());

            IEnumerator WaitForLoad()
            {

                // �ε� ������ ���� �ε� ���� ���¸� ��Ÿ�� ���� �ʱ�ȭ
                loadProgress = 0;

                // �ε� ���� �񵿱�� �ҷ���, �� �� ����ȯ�� �ƴ� �� �߰� ������� �ε�
                var asyncOper = SceneManager.LoadSceneAsync(SceneType.Loading.ToString(), LoadSceneMode.Additive);

                // �� �ε尡 �Ϸ���� �ʾҴٸ� �ݺ�
                while(!asyncOper.isDone)
                {
                    // �� �ε� ���� ���¸� ����ؼ� ������Ʈ�Ѵ�.
                    loadProgress = asyncOper.progress;
                    yield return null;
                }

                UILoading uiLoading = null;

                // uiLoading�� null �̶�� �ݺ�
                while(uiLoading == null)
                {
                    // uiLoading �ν��Ͻ� ��ü�� ������ ����ؼ� ã�´�.
                    // ����ؼ� ã�� ����?
                    // �� ���� Ž���� ��������� ���, ���������� �� ã�� Ȯ���� ���� ����
                    // �� �� ���� ���������� �� ã����?
                    // -> �ε� ���� �ε� �� �ٷ� �ش� ������ ����Ǳ� ������ �� ������
                    // uiLoading ��ü�� �ʱ�ȭ�� �Ϸ�� �������� �� �� ����.
                    // �ʱ�ȭ�� �Ϸ���� �ʾҴٸ� ���������� ��ü�� ã�� �� ����.
                    // ���
                    // -> �ش� �ڵ带 ���� UILoading ��ü�� �� �ε� �� �ʱ�ȭ�� �� ������ ��ٸ��� �۾�
                    uiLoading = FindObjectOfType<UILoading>();

                    yield return null;
                }

                // �ε����� ī�޶� ��Ȱ��ȭ ��Ŵ
                // -> ���� �߰��ϴ� ������� �ε��Ͽ����Ƿ�, ���� 2���� ���� Ȱ��ȭ�� ����
                // �� ��, �ΰ��� ���� �����ϴ� ī�޶�� ��� �� ����� �� �ֱ� ������
                // �ε����� ī�޶� ��Ȱ��ȭ�� (�ʿ���� ������ ����)
                uiLoading.cam.enabled = false;

                // �ε� ���� Ȱ��ȭ �� ���¿��� ó���� �۾�
                // -> �� ������ �ҷ����� �� �ʱ�ȭ
                if(loadCoutine != null)
                    yield return StartCoroutine(loadCoutine);

                // ���� �ε� �۾��� ���� ���� ������ �ε� ���� �ʹ� ���� �Ѿ�Ƿ� Ȯ���� �� �ְ�
                yield return new WaitForSeconds(1f);


                // ���� ���� ���¶��� ���̰� ����
                // -> ���� ������ �ε� ������¿ʹ� ���̰� ����
                // �������� �����ֱ� ��
                loadProgress = 1f;

                // �ʿ��� �۾��� �������Ƿ�, �ε� ���� ��ε�

                yield return SceneManager.UnloadSceneAsync(SceneType.Loading.ToString());

                // ��� ��ȯ �۾��� �Ϸ�Ǿ����Ƿ�, ��ȯ �Ϸ� �� ������ �۾��� ����
                loadComplete?.Invoke();
            }
        }

    }
}
