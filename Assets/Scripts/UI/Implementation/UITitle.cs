using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.UI
{
    public class UITitle : MonoBehaviour
    {
        /// <summary>
        /// ���� �ε� ���� ����
        /// </summary>
        public TextMeshProUGUI loadStateDesc;

        /// <summary>
        /// �ε� ���� ������
        /// </summary>
        public Image loadFillGauge;

        /// <summary>
        /// �ε� ���� ���� �ؽ�Ʈ�� �����ϴ� ���
        /// </summary>
        /// <param name="loadState">���� �ε� ���¸� ��Ÿ���� ���ڿ�</param>
        public void SetLoadStateDescription(string loadState)
        {
            loadStateDesc.text = $"{loadState}...";
        }

        // ������ �ִϸ��̼� ó���� ���� Unity�� Coroutine�� ���
        // -> �ڷ�ƾ�� ����Ƽ���� Ư�� �۾��� �񵿱�� ������ �� �ְ� ���ִ� ���
        // (���� �񵿱�� �ƴ� -> �񵿱�ó�� ���̰� ����Ǵ� ��)
        // �ڷ�ƾ�� ��ȯ ���� �׻� IEumerator

        /// <summary>
        /// �ε� ������ �ִϸ��̼� ó��
        /// </summary>
        /// <param name="loadPer">�����ϰ����ϴ� �ε� �ۼ�������</param>
        /// <returns></returns>
        public IEnumerator LoadGaugeUpdate(float loadPer)
        {
            // UI�� fillAmount ���̶� ���� �ε� �ۼ������� ���̶� �ٻ����� �ʴٸ�
            // �ٻ��� ������ ���� �����ǵ��� �ݺ�

            // float ���� �ε��Ҽ��� ����� �����ʹ� == ������ �̿��ؼ� ��Ȯ�� ���� �񱳰� �Ұ�����
            // ����  float ������ ������ ������ ���� ���� ��Ȯȯ ���� ���ϴ� ���� �ƴ϶�
            // �ٻ��� �̿��Ͽ� �� ���� �ٻ������� ���ϴ� ����� �ùٸ� �񱳸� �� �� ����
            while (!Mathf.Approximately(loadFillGauge.fillAmount, loadPer))
            {
                // Mathf.Lerp(float a, float b, float t) :
                // a���� b�� ������ �������� (a�� ������ b�� ������ ������ ����) ��Ų��.
                // a���� b�� �ѹ� ���� �� �ִ� �󸶸�ŭ ������ �Ǵ����� ��Ÿ���� ���� t 
                loadFillGauge.fillAmount = Mathf.Lerp(loadFillGauge.fillAmount,loadPer,Time.deltaTime * 2f);

                yield return null;
            }


        }
    }
}
