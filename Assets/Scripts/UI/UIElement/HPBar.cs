using ProjectW.Object;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.UI
{
    /// <summary>
    /// UI ��� �� HpBar ������ UI�� ������ ��ũ��Ʈ
    /// </summary>
    public class HPBar : MonoBehaviour, IPoolableObject
    {
        /// <summary>
        /// �ش� hp bar �� ǥ���� hp �����͸� ���� ������ �ν��Ͻ�
        /// </summary>
        public Actor target;

        public Image hpGauge;

        public bool CanRecycle { get; set; }

        public void Initialize(Actor target)
        {
            this.target = target;
            hpGauge.fillAmount = 1f;

            // ������Ʈ Ǯ�� ����ϴ� ��ü���� ������ �ش� Ǯ�� Ȧ���� �θ�� �Ͽ� �����ϰ� ����
            // �� ��, Ǯ���� ��ü�� ������ Ȧ������ ���� ĵ������ �θ� �����Ų�ٸ�,
            // Transform -> Rect Transform ���� �θ� ����Ǹ鼭 �����ϸ��� �߻���
            // ����, �θ� �������� �⺻ ������ ������ �ٽ� �ǵ���
            transform.localScale = Vector3.one;
        }

        public void HpBarUpdate()
        {
            // Ÿ���� ���ų� Ÿ���� �ݶ��̴��� ������ ����
            // �ݶ��̴��� ��? Ŭ���̴��� �ش� ��ü(����)�� ũ�� ������ ������ �����Ƿ�
            // �ݶ��̴��� ���� ������ ũ�⿡ �����Ͽ� ������ �Ӹ� ���ʿ� ü�¹ٸ� ���� ����
            if (target == null || target.Coll == null)
                return;

            // ���Ͱ� �׾��ٸ�
            if(target.State == Define.Actor.State.Dead)
            {
                // ü�¹ٴ� �� �̻� �ʿ� �����Ƿ� Ǯ�� ��ȯ
                ObjectPoolManager.Instance.GetPool<HPBar>().Return(this);
                return;
            }

            // ���� �ʾҴٸ� ü�¹��� ��ġ�� ������ ���� ����
            transform.position = target.transform.position + Vector3.up * target.Coll.bounds.size.y * 1.2f;
            hpGauge.fillAmount = target.boActor.currentHp / target.boActor.maxHp;
        }
    }
}
