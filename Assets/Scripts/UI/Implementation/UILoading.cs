using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.UI
{
    public class UILoading : MonoBehaviour
    {
        // �ε� ���� �ؽ�Ʈ �ִϸ��̼ǿ� ����� �ʵ� (... �ִϸ��̼�)
        private string dot = string.Empty;
        private const string loadStateText = "Load Next Scene";

        public TextMeshProUGUI loadStateDesc;
        public Image loadGauge;

        // Loading ���� ī�޶� ��ü ����
        public Camera cam;

        private void Update()
        {
            loadGauge.fillAmount = GameManager.Instance.loadProgress;

            // 20 �����Ӹ��� . �� �ϳ��� �����ϰ�
            if(Time.frameCount % 20 == 0)
            {
                if (dot.Length >= 3)
                    dot = string.Empty;
                else
                    dot = string.Concat(dot, ".");

                loadStateDesc.text = $"{loadStateText}{dot}";
            }
        }

    }
}
