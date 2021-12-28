using ProjectW.DB;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectW.UI
{

    public class UIInventory : UIWindow
    {
        /// <summary>
        /// 모든 아이템 슬롯을 가지고 있는 하이라키 상의 부모 객체의 트랜스폼 참조
        /// </summary>
        private Transform itemSlotHolder;
        /// <summary>
        /// 인벤토리 상의 모든 아이템슬롯 객체 참조를 갖는 컬렉션
        /// </summary>
        private List<ItemSlot> itemSlots = new List<ItemSlot>();

        public override void Start()
        {
            base.Start();

            // 아이템 슬롯 홀더 참조 바인딩
            itemSlotHolder = transform.Find("Frame/ItemSlotHolder");

            // Linq쓰면 ToList로 리스트 변환 가능
            //itemSlots = itemSlotHolder.GetComponentsInChildren<ItemSlot>().ToList();

            // 모든 아이템 슬롯의 참조를 찾아서 리스트에 담음
            for(int i=0; i<itemSlotHolder.childCount; i++)
            {
                itemSlots.Add(itemSlotHolder.GetChild(i).GetComponent<ItemSlot>());
            }
            
            // 모든 아이템 슬롯 초기화
            InitAllSlot();

            // 유저가 가지고 있는 아이템 데이터를 인벤토리 슬롯에 전부 갱신해준다.

        }

        private void InitAllSlot()
        {
            //itemSlots.ForEach(_ => _.Initialize());

            for(int i=0; i< itemSlots.Count; i++)
            {
                itemSlots[i].Initialize();
            }
        }

        /// <summary>
        /// 인벤토리에 아이템 데이터를 추가하는 기능
        /// 1. 서버에서 초기에 아이템 데이터를 받아서 인벤토리를 갱신할 때
        /// 2. 몬스터를 잡아서 드랍된 아이템을 클라에서 직접 추가할 떄
        /// </summary>
        /// <param name="boItem"></param>
        public void AddItem(BoItem boItem)
        {
            // 추가된 아이템 슬롯 인덱스가 정상적으로 존재한다면
            // 슬롯인덱스에 맞춰 배치시키고, 아니라면 상단 좌측을 기준으로 비어있는 슬롯에 넣어준다.
            // -> 슬롯인덱스가 존재한다는 것은 기존에 가지고 있던 아이템
            // 없다는 것은 새로 루팅한 아이템

            // 서버가 유저가 가지고 있던 아이템 데이터를 준 경우
            if(boItem.slotIndex >=0)
            {
                // 원래 가지고 있던 아이템
                itemSlots[boItem.slotIndex].SetSlot(boItem);
                return;
            }

            // 클라에서 몬스터를 잡고 루팅한 아이템 데이터
            for(int i =0; i< itemSlots.Count; i++)
            {
                // 비어있는 슬롯(아이템데이터가 존재하지 않는 슬롯)이라면
                if(itemSlots[i].BoItem == null)
                {
                    boItem.slotIndex = i;
                    itemSlots[i].SetSlot(boItem);
                    break;
                }
            }
        }
    }
}
