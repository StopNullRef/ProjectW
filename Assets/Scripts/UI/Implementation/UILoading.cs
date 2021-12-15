using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.UI
{
    public class UILoading : MonoBehaviour
    {
        // 로딩 상태 텍스트 애니메이션에 사용할 필드 (... 애니메이션)
        private string dot = string.Empty;
        private const string loadStateText = "Load Next Scene";

        public TextMeshProUGUI loadStateDesc;
        public Image loadGauge;

        // Loading 씬의 카메라 객체 참조
        public Camera cam;

        private void Update()
        {
            loadGauge.fillAmount = GameManager.Instance.loadProgress;

            // 20 프레임마다 . 이 하나씩 증가하게
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
