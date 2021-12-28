using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.UI
{
    /// <summary>
    /// UI 요소 중 버블게이지 형태의 UI를 제어할 스크립트 
    /// </summary>
    public class BubbleGauge : MonoBehaviour
    {
        private Image gauge;

        private void Start()
        {
            gauge = GetComponent<Image>();
        }

        /// <summary>
        /// 이미지 fillamount 값을 컨트롤하는 기능
        /// </summary>
        /// <param name="value"></param>
        public void SetGauge(float value)
        {
            gauge.fillAmount = value;
        }
    }
}
