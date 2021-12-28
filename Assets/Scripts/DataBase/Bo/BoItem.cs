using ProjectW.SD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectW.DB
{
    [Serializable]
    public class BoItem
    {
        public int slotIndex; // 아이템이 인벤토리 상에 위치하고 있는 아이템 슬롯의 인덱스
        public int amount; // 아이템 수량
        public SDItem sdItem; // 아이템 기획 데이터

        /// <summary>
        /// 아이템 드롭 후 루팅 시 사용 
        /// -> 실제 rpg 겜이라면 (드롭된 아이템의 정보를 서버에서 받아와서 BoItem을 생성해야함)
        /// 
        /// 위의 과정을 전부 구현하기 힘드므로
        /// 아이템 정보를 서버에서 받는건 처음 로그인 시, 딱 한 번
        /// 그 이후 몬스터를 잡고 아이템이 루팅되면, 아이템정보를 클라이언트에서 바로 생성해서
        /// 더미 db에 추가함
        /// </summary>
        /// <param name="sdItem"></param>
        public BoItem(SDItem sdItem)
        {
            // 슬롯 인덱스 초기값은 실제 사용하지 않는 슬롯 인덱스 값으로 부여
            slotIndex = -1;
            amount = 1; 
            this.sdItem = sdItem;
        }
    }

    [Serializable]
    public class BoEquipment : BoItem
    {
        public bool isEquip; // 장비를 착용중인지
        public int reinforceValue; // 강화수치

        public BoEquipment(SDItem sdItem) : base(sdItem)
        {
            isEquip = false;
            reinforceValue = 0;
        }
    }
}

