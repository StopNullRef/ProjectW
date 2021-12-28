using ProjectW.Network;
using System;
using System.Collections.Generic;

namespace ProjectW.DB
{
    [Serializable]
    public class DtoItem : DtoBase
    {
        public List<DtoItemElement> items;

        public DtoItem() { }
        /// <summary>
        /// 유저가 가지고 있는 모든 아이템 정보를 한 번에 Dto로 편하게 변환하기 위한  생성자
        /// </summary>
        /// <param name="boItems"></param>
        public DtoItem(List<BoItem> boItems)
        {
            items = new List<DtoItemElement>();

            for(int i=0; i< boItems.Count; i++)
            {
                items.Add(new DtoItemElement(boItems[i]));
            }
        }
    }

    [Serializable]
    public class DtoItemElement
    {
        public int slotIndex; // 아이템의 인벤토리 상의 인덱스
        public int index; // 아이템의 기획데이터 상의 인덱스
        public int amoint; // 아이템 수량
        public int reinforceValue; // 장비일 시 강화수치, 아니라면 사용하지 않음
        public bool isEquip; // 장비라면 착용중인지, 아니라면 사용하지 않음

        public DtoItemElement() { }

        /// <summary>
        /// 게임에서  사용중이던 아이템 데이터를 받아 Dto로 변환시키기 위한 생성자
        /// </summary>
        /// <param name="boItem"></param>
        public DtoItemElement(BoItem boItem)
        {
            slotIndex = boItem.slotIndex;
            index = boItem.sdItem.index;
            amoint = boItem.amount;

            // boItem의 타입이 장비 데이터라면
            if(boItem is BoEquipment)
            {
                var boEquipment = boItem as BoEquipment;
                reinforceValue = boEquipment.reinforceValue;
                isEquip = boEquipment.isEquip;
            }
        }
    }
}
