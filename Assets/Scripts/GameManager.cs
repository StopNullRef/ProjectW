using ProjectW.DB;
using ProjectW.Define;
using ProjectW.SD;
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
        public void LoadScene(SceneType sceneName, IEnumerator loadCoroutine = null,Action loadComplete = null)
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
                var asyncOper =  SceneManager.LoadSceneAsync(sceneName.ToString(),LoadSceneMode.Additive);

                // ��� �߰��� ���� ��Ȱ��ȭ
                // ����? �ε� ������ ���ο� ���� �ϳ��� �߰��Ǿ� 2���� ���� Ȱ��ȭ�� �����ε�..
                // �� �� ���� ��Ʈ������ �ʴ�  �ΰ��� ���� ��� ȭ�鿡 �������� ��
                // ����, ���� �����ϰ��� �ϴ� ���� �߰��ص� ���� ����ڿ��� �ε� ���� �����ְ�
                // �ε����� Ȱ��ȭ�Ǿ��ִ� ���� ���� �����ϰ��� �ϴ� ���� �ʿ��� ������ ���� �ε� �ϴ� �۾��� �ϱ� ���ؼ�
                asyncOper.allowSceneActivation = false;

                // �����ϰ��� �ϴ� ���� �ʿ��� �۾��� �����Ѵٸ� ����
                if(loadCoroutine != null)
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
                    if(loadProgress >= 0.9f)
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

    }
}
