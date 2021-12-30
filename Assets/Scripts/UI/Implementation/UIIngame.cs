using ProjectW.Controller;
using ProjectW.DB;
using ProjectW.Object;
using ProjectW.Resource;
using ProjectW.SD;
using ProjectW.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectW.UI
{
    /// <summary>
    /// 인게임에서 일반적으로 사용하는 UI들을 관리하는 클래스
    /// -> 커서, 몬스터 hp바, 캐릭터 hud 등
    /// </summary>
    public class UIIngame : UIWindow
    {
        /// <summary>
        /// 플레이어가 마우스 입력을 통해 적을 타겟팅하고 있는지에 대한 데이터를
        /// 얻기 위해 참조를 받아둠
        /// </summary>
        public PlayerController playerController;

        public Texture2D defaultCursor; // 기본 커서 텍스쳐
        public Texture2D targetPointerCursor; // 몬스터에 마우스 포인팅 시 커서 텍스쳐

        public List<BubbleGauge> playerHpBubbles;
        public List<BubbleGauge> playerMpBubbles;

        private List<HPBar> monsterHpBars = new List<HPBar>();

        private List<ItemBase> items = new List<ItemBase>();

        public Canvas worldUICanvas;

        private void Update()
        {
            CursorUpdate();
            BubbleGaugeUpdate();
            HpBarUpdate();
            BillboardUpdate();
        }

        private void BillboardUpdate()
        {
            // 2d UI가 항상 카메라를 바라보게 만들기 위해서
            // 카메라의 트랜스폼 참조가 필요하다
            var camTrans = CameraController.Cam.transform;

            // 월드 공간의 UI는 모두 월드 UI 캔버스에 존재하므로
            // 캔버스에 접근하여 자식 객체들을 전부 카메라를 바라보게 만듬
            for (int i = 0; i < worldUICanvas.transform.childCount; i++)
            {
                // 하이라키상의 자식 객체를 순서대로 하나씩 가져옴
                var child = worldUICanvas.transform.GetChild(i);

                child.LookAt(camTrans, Vector3.up);
                // y축 회전만 원하므로, 나머지 회전값은 0으로 초기화
                var newRot = child.eulerAngles;
                newRot.x = 0;
                newRot.z = 0;
                child.eulerAngles = newRot;
            }
        }


        private void CursorUpdate()
        {
            Cursor.SetCursor(playerController.HasPointTarget ? targetPointerCursor : defaultCursor, Vector2.zero, CursorMode.Auto);
        }

        private void BubbleGaugeUpdate()
        {
            // 체력, 마력 UI 업데이트를 위해 플레이어의 데이터를 가져온다.
            // 이때, 플레이어 캐릭터가 존재하는지?, 플레이어 캐릭터의 데이터는 존재하는지? 확인
            var boActor = playerController.PlayerCharacter?.boActor;
            if (boActor == null)
                return;

            // 현재 체력 및 마력을 0~1사이 값으로 정규화
            var hpGauge = boActor.currentHp / boActor.maxHp;
            var mpGauge = boActor.currentMp / boActor.maxMp;

            // 정규화된 값을 버블 게이즈들에 적용
            Progress(playerHpBubbles, hpGauge);
            Progress(playerMpBubbles, mpGauge);

            void Progress(List<BubbleGauge> list, float value)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].SetGauge(value);
                }
            }
        }

        private void HpBarUpdate()
        {
            for (int i = 0; i < monsterHpBars.Count; i++)
                monsterHpBars[i].HpBarUpdate();
        }

        /// <summary>
        /// 매개변수로 받은 액터의 정보를 기준으로 체력바를 생성하여 monsterHpBars 리스트에 추가
        /// </summary>
        /// <param name="target"></param>
        public void AddHpBar(Actor target)
        {
            var hpBar = ObjectPoolManager.Instance.GetPool<HPBar>().GetObj();

            // hpBar의 부모를 월드 UI 캔버스로 설정
            hpBar.transform.SetParent(worldUICanvas.transform);
            // hpBar 초기화 및 활성화
            hpBar.Initialize(target);
            hpBar.gameObject.SetActive(true);
            // 초기화 및 활성화가 끝난 hpBar를 monsterHpBars 리스트에 추가
            monsterHpBars.Add(hpBar);
        }

        public void AddItem(Monster monster,BoItem itemInfo)
        {
            var item = ObjectPoolManager.Instance.GetPool<ItemBase>().GetObj();
            item.DropItem = itemInfo;
            item.transform.SetParent(UIWindowManager.Instance.GetWindow<UIIngame>().worldUICanvas.transform);
            item.Initialize(monster);
            item.itemImage.sprite = SpriteLoader.GetSprite(Define.Resource.AtlasType.ItemAtlas, itemInfo.sdItem.resourcePath);
            item.gameObject.SetActive(true);
            items.Add(item);
        }

        /// <summary>
        /// 스테이지 전환 시 현재 스테이지에 있는 hpBar 객체를 전부 푸렝 반환
        /// </summary>
        public void Clear()
        {
            var hpBarPool = ObjectPoolManager.Instance.GetPool<HPBar>();

            for (int i = 0; i < monsterHpBars.Count; i++)
            {
                hpBarPool.Return(monsterHpBars[i]);
            }
            monsterHpBars.Clear();
        }

    }
}
