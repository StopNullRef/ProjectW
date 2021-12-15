using UnityEngine;
using System.Linq;


namespace ProjectW.Object
{

    public class Warp : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.CompareTag("Player"))
                return;

            // 객체의 이름을 워프할 스테이지이의 인덱스톨 미리 설정하였기 때문에
            // 해당 이름을 동해 워프할 스테이지 인덱스 값을 받아온다.
            var warpStageIndex = int.Parse(name);

            var boStage = GameManager.User.boStage;

            // 스테이지 이동할 것이므로, 이동 전에 이전 스테이지 인덱스에 현재 스테이지 인덱스를 넣는다.
            boStage.prevStageIndex = boStage.sdStage.index;
            // 새로 이동하는 스테이지의 기획 데이터를 불러온다.
            boStage.sdStage = GameManager.SD.sdStages.Where(_ => _.index == warpStageIndex).SingleOrDefault();

            var inGameManager = FindObjectOfType<InGameManager>();

            

            GameManager.Instance.OnAdditiveLoadingScene(inGameManager.ChangeStage(), inGameManager.OnChangeStageComplete);
        }

    }
}
