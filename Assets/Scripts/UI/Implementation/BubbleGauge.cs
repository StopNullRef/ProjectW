using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.UI
{
    /// <summary>
    /// UI ��� �� ��������� ������ UI�� ������ ��ũ��Ʈ 
    /// </summary>
    public class BubbleGauge : MonoBehaviour
    {
        private Image gauge;

        private void Start()
        {
            gauge = GetComponent<Image>();
        }

        /// <summary>
        /// �̹��� fillamount ���� ��Ʈ���ϴ� ���
        /// </summary>
        /// <param name="value"></param>
        public void SetGauge(float value)
        {
            gauge.fillAmount = value;
        }
    }
}
