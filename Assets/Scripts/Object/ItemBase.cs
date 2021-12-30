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
    /// ������ Ŭ����
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
            // �浹�� �ݶ��̴��� �÷��̾� �϶�
            if (other.CompareTag("Player"))
            {
                // �κ��丮 ���� �ް� �ش������ �߰�
                var inven = UIWindowManager.Instance.GetWindow<UIInventory>();

                inven.AddItem(DropItem);
                // �ٽ� Ǯ�� �־���
                ObjectPoolManager.Instance.GetPool<ItemBase>().Return(this);
            }
        }
    }
}
