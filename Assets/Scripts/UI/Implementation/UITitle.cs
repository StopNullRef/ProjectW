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
        /// 현재 로딩 상태 설명
        /// </summary>
        public TextMeshProUGUI loadStateDesc;

        /// <summary>
        /// 로딩 상태 게이지
        /// </summary>
        public Image loadFillGauge;

        /// <summary>
        /// 로딩 상태 설명 텍스트를 설정하는 기능
        /// </summary>
        /// <param name="loadState">현재 로딩 상태를 나타내는 문자열</param>
        public void SetLoadStateDescription(string loadState)
        {
            loadStateDesc.text = $"{loadState}...";
        }

        // 동적인 애니메이션 처리를 위해 Unity의 Coroutine을 사용
        // -> 코루틴은 유니티에서 특정 작업을 비동기로 실행할 수 있게 해주는 기능
        // (실제 비동기는 아님 -> 비동기처럼 보이게 실행되는 것)
        // 코루틴의 반환 값은 항상 IEumerator

        /// <summary>
        /// 로딩 게이지 애니메이션 처리
        /// </summary>
        /// <param name="loadPer">변경하고자하는 로딩 퍼센테이지</param>
        /// <returns></returns>
        public IEnumerator LoadGaugeUpdate(float loadPer)
        {
            // UI의 fillAmount 값이랑 현재 로딩 퍼센테이지 값이랑 근사하지 않다면
            // 근사할 때까지 값이 보간되도록 반복

            // float 같은 부동소수점 방식의 데이터는 == 연산을 이용해서 정확한 값의 비교가 불가능함
            // 따라서  float 값끼리 동일한 값인지 비교할 때는 정확환 값을 비교하는 것이 아니라
            // 근삿값을 이용하여 두 값이 근사한지를 비교하는 방식이 올바를 비교를 할 수 있음
            while (!Mathf.Approximately(loadFillGauge.fillAmount, loadPer))
            {
                // Mathf.Lerp(float a, float b, float t) :
                // a에서 b의 값으로 선형보간 (a의 값에서 b의 값으로 서서히 변경) 시킨다.
                // a에서 b로 한번 보간 시 최대 얼마만큼 보간이 되는지를 나타내느 값이 t 
                loadFillGauge.fillAmount = Mathf.Lerp(loadFillGauge.fillAmount,loadPer,Time.deltaTime * 2f);

                yield return null;
            }


        }
    }
}
