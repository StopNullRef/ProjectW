using ProjectW.DB;
using ProjectW.Resource;
using ProjectW.UI;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.Object
{
    /// <summary>
    /// 아이템 클래스
    /// </summary>
    public class ItemBase : MonoBehaviour, IPoolableObject
    {
        Actor target;

        public Image itemImage;
        public BoItem DropItem
        {
            get;
            set;
        }

        public void Initialize(Actor target)
        {
            this.target = target;

            itemImage = GetComponent<Image>();

            transform.localScale = new Vector3 (0.3f,0.3f,0.3f);

            Vector3 dropPos = target.transform.position;
            dropPos.y += 0.5f; 

            transform.position = dropPos;
           
            //itemImage.sprite = SpriteLoader.GetSprite(Define.Resource.AtlasType.ItemAtlas, DropItem.sdItem.name);
        }

        public bool CanRecycle { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            // 충돌한 콜라이더가 플레이어 일때
            if (other.CompareTag("Player"))
            {
                // 인벤토리 정보 받고 해당아이템 추가
                var inven = UIWindowManager.Instance.GetWindow<UIInventory>();

                inven.AddItem(DropItem);
                // 다시 풀에 넣어줌
                ObjectPoolManager.Instance.GetPool<ItemBase>().Return(this);
            }
        }
    }
}
