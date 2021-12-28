using ProjectW.Object;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW.UI
{
    /// <summary>
    /// UI 요소 중 HpBar 형태의 UI를 제어할 스크립트
    /// </summary>
    public class HPBar : MonoBehaviour, IPoolableObject
    {
        /// <summary>
        /// 해당 hp bar 가 표현할 hp 데이터를 갖는 액터의 인스턴스
        /// </summary>
        public Actor target;

        public Image hpGauge;

        public bool CanRecycle { get; set; }

        public void Initialize(Actor target)
        {
            this.target = target;
            hpGauge.fillAmount = 1f;

            // 오브젝트 풀로 사용하는 객체들은 별도의 해당 풀의 홀더를 부모로 하여 관리하고 있음
            // 이 때, 풀에서 객체를 꺼내서 홀더에서 월드 캔버스로 부모를 변경시킨다면,
            // Transform -> Rect Transform 으로 부모가 변경되면서 스케일링이 발생됨
            // 따라서, 부모를 기준으로 기본 스케일 값으로 다시 되돌림
            transform.localScale = Vector3.one;
        }

        public void HpBarUpdate()
        {
            // 타겟이 없거나 타겟의 콜라이더가 없으면 리턴
            // 콜라이더는 왜? 클라이더가 해당 객체(몬스터)의 크기 정보를 가지고 있으므로
            // 콜라이더를 통해 몬스터의 크기에 접근하여 몬스터의 머리 위쪽에 체력바를 띄우기 위해
            if (target == null || target.Coll == null)
                return;

            // 몬스터가 죽었다면
            if(target.State == Define.Actor.State.Dead)
            {
                // 체력바는 더 이상 필요 없으므로 풀에 반환
                ObjectPoolManager.Instance.GetPool<HPBar>().Return(this);
                return;
            }

            // 죽지 않았다면 체력바의 위치와 게이지 값을 갱신
            transform.position = target.transform.position + Vector3.up * target.Coll.bounds.size.y * 1.2f;
            hpGauge.fillAmount = target.boActor.currentHp / target.boActor.maxHp;
        }
    }
}
