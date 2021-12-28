
using ProjectW.DB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.UI
{
    public class ItemSlot : MonoBehaviour
    {
        // 아이템 수량을 나타내는 컴포넌트의 참조
        private TextMeshProUGUI itemAmount;
        // 아이템 이미지를 나타내는 컴포넌트의 참조
        public Image ItemImage { get; private set; }

        public BoItem BoItem { get; private set; }

        public void Initialize()
        {
            // 아이템 수량 필드가 null 이라면 자식객체 (본인포함) 중에 텍스트메쉬프로 컴포넌트를 찾아온다
            itemAmount ??= GetComponent<TextMeshProUGUI>();
            // 아이템 이미지 필드가 null 이라면 0번째(첫번째) 자식에 접근하여 이미지 컴포넌트를 찾아온다.
            // -> 직접적으로 0번째에 접근하는 이유, ItemSlot 컴포넌트를 붙일 객체도 이미지 컴포넌트를
            // 가지고 있으므로, 명확하게 첫번째 자식(아이템이미지)에 접근하여 컴포넌트를 가져옴
            ItemImage ??= transform.GetChild(0).GetComponent<Image>();
        }

        /// <summary>
        /// 슬롯에 아이템 데이터를 세팅하는 기능
        /// </summary>
        /// <param name="boItem"></param>
        public void SetSlot(BoItem boItem)
        {
            BoItem = boItem;

            // 추후에 아이템을 드래그해서 아이템슬롯 스왑 기능을 만들 예정
            // 아이템 스왑 시, 스왑의 대상이 되는 두 슬롯의 데이터를 단수히 서로 바꿔줄 것임
            // 이 때, SetSlot 메서드를 이용하는데 두 슬롯 중 하나의 슬롯이 비어있는 슬롯이라면
            // 결과적으로 한 쪽에는 null인 상태의 boItem 인자가 전달됨
            if(BoItem == null)
            {
                itemAmount.text = "";
                ItemImage.sprite = null;
                ItemImage.color = new Color(1, 1, 1, 0);
            }
            else
            {
                itemAmount.text = boItem.amount.ToString();
                //스프라이트 부르는 기능 하고나서 한다
                //ItemImage.sprite = 
                ItemImage.color = Color.white;
            }
        }
    }
}
