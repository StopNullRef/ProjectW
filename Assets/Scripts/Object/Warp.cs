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

            // ��ü�� �̸��� ������ ������������ �ε����� �̸� �����Ͽ��� ������
            // �ش� �̸��� ���� ������ �������� �ε��� ���� �޾ƿ´�.
            var warpStageIndex = int.Parse(name);

            var boStage = GameManager.User.boStage;

            // �������� �̵��� ���̹Ƿ�, �̵� ���� ���� �������� �ε����� ���� �������� �ε����� �ִ´�.
            boStage.prevStageIndex = boStage.sdStage.index;
            // ���� �̵��ϴ� ���������� ��ȹ �����͸� �ҷ��´�.
            boStage.sdStage = GameManager.SD.sdStages.Where(_ => _.index == warpStageIndex).SingleOrDefault();

            var inGameManager = FindObjectOfType<InGameManager>();

            

            GameManager.Instance.OnAdditiveLoadingScene(inGameManager.ChangeStage(), inGameManager.OnChangeStageComplete);
        }

    }
}
