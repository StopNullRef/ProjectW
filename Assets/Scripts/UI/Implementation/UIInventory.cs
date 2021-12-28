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
        /// ��� ������ ������ ������ �ִ� ���̶�Ű ���� �θ� ��ü�� Ʈ������ ����
        /// </summary>
        private Transform itemSlotHolder;
        /// <summary>
        /// �κ��丮 ���� ��� �����۽��� ��ü ������ ���� �÷���
        /// </summary>
        private List<ItemSlot> itemSlots = new List<ItemSlot>();

        public override void Start()
        {
            base.Start();

            // ������ ���� Ȧ�� ���� ���ε�
            itemSlotHolder = transform.Find("Frame/ItemSlotHolder");

            // Linq���� ToList�� ����Ʈ ��ȯ ����
            //itemSlots = itemSlotHolder.GetComponentsInChildren<ItemSlot>().ToList();

            // ��� ������ ������ ������ ã�Ƽ� ����Ʈ�� ����
            for(int i=0; i<itemSlotHolder.childCount; i++)
            {
                itemSlots.Add(itemSlotHolder.GetChild(i).GetComponent<ItemSlot>());
            }
            
            // ��� ������ ���� �ʱ�ȭ
            InitAllSlot();

            // ������ ������ �ִ� ������ �����͸� �κ��丮 ���Կ� ���� �������ش�.

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
        /// �κ��丮�� ������ �����͸� �߰��ϴ� ���
        /// 1. �������� �ʱ⿡ ������ �����͸� �޾Ƽ� �κ��丮�� ������ ��
        /// 2. ���͸� ��Ƽ� ����� �������� Ŭ�󿡼� ���� �߰��� ��
        /// </summary>
        /// <param name="boItem"></param>
        public void AddItem(BoItem boItem)
        {
            // �߰��� ������ ���� �ε����� ���������� �����Ѵٸ�
            // �����ε����� ���� ��ġ��Ű��, �ƴ϶�� ��� ������ �������� ����ִ� ���Կ� �־��ش�.
            // -> �����ε����� �����Ѵٴ� ���� ������ ������ �ִ� ������
            // ���ٴ� ���� ���� ������ ������

            // ������ ������ ������ �ִ� ������ �����͸� �� ���
            if(boItem.slotIndex >=0)
            {
                // ���� ������ �ִ� ������
                itemSlots[boItem.slotIndex].SetSlot(boItem);
                return;
            }

            // Ŭ�󿡼� ���͸� ��� ������ ������ ������
            for(int i =0; i< itemSlots.Count; i++)
            {
                // ����ִ� ����(�����۵����Ͱ� �������� �ʴ� ����)�̶��
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
