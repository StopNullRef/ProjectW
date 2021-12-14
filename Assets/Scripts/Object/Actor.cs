using ProjectW.DB;
using UnityEngine;

namespace ProjectW.Object
{
    using State = Define.Actor.State;
    /// <summary>
    /// 인게임 내에 다이나믹하게 행동하는 객체들의 추상화된 베이스 클래스
    /// 캐릭터, 몬스터 등 Actor의 파생클래스에서 공통되는 기능은 최대한 Actor에 정의
    /// 파생 클래스에 따라 다른 기능은 해당 파생클래스에서 별도로 정의
    /// </summary>
    public abstract class Actor : MonoBehaviour
    {
        /// <summary>
        /// 액터의 현재 상태를 나타내는 필드
        /// </summary>
        public State State { get; private set; }

        /// <summary>
        /// 액터의 bo 데이터
        /// -> 액터가 인게임에서 사용하는 모든 데이터가 포함되어있다
        /// </summary>
        public BoActor boActor;

        public Collider Coll { get; private set; }
        protected Rigidbody rig;
        protected Animator anim;

        /// <summary>
        /// 액터 초기화 기능
        /// 초기화 시 외부에서 boActor 데이터를 받는다.
        /// </summary>
        /// <param name="boActor"></param>
        public virtual void Initialize(BoActor boActor)
        {
            this.boActor = boActor;
        }

        protected virtual void Start()
        {
            // 컴포넌트 참조 바인딩
            Coll = GetComponent<Collider>();
            rig = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
        }

        /// <summary>
        /// 액터 스텟 설정 추상메서드 (실제 추상메서드로 작성되진 않음, 그냥 가상 메서드)
        /// </summary>
        public virtual void SetStats() { }

        /// <summary>
        /// 액터 객체의 모든 업데이트를 담당
        /// -> Unity의 Update 콜백을 사용하지 않음
        /// 기존의 방식처럼 Update를 사용하게 된다면 Update 콜백 이용시 모든 객체가 객체마다 Update 를 갖게 됨
        /// 하지만 객체마다 Update 콜백을 가지 않고 Update 용도로 사용할 메서드를 직접 작성한 뒤
        /// 한 곳에서 모든 객체의 인스턴스를 컬렉션으로 보관한 뒤, 반복문을 통해 모든 객체의 Update 용 메서드를
        /// 직접 호출하게 되면 기존의 방식보다 많은 비용을 아낄 수 있다.
        /// -> 기존 방식 : 객체마다 Update 콜백을 갖음, 작성은 편리하나 비용이 비쌈
        /// -> 새로운 방식 : 객체마다 Update 콜백을 갖지 않음, 단 하나의 인스턴스가 Update 콜백을 갖고
        /// 그곳에서 모든 객체를 한 번에 Update를 처리함, 기존 방식보다 비용이 싸다
        /// </summary>
        public virtual void ActorUpdate()
        {
            MoveUpdate();
        }

        /// <summary>
        /// 이동 업데이트 추상 메서드
        /// </summary>
        public virtual void MoveUpdate() { }

        /// <summary>
        /// 액터의 현재 상태 변경 기능
        /// -> 상태 변경 시 상태에 따라 특정 기능을 한 번 실행할 수 있음
        /// </summary>
        /// <param name="state">변경하고자 하는 상태</param>
        public virtual void SetState(State state)
        {
            State = state;

            anim.SetInteger("state", (int)State);

            // 상태 변경 후 변경된 상태에 따른 처리를 switch/case로 검사하여 실행
            // 액터에서 파생 객체들이 공통으로 갖는 상태만을 처리한다.
            // 그 후 파생 객체에 따라 추가적으로 갖는 상태는 해당 파생클래스에서 별도로 처리
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Walk:
                    break;
                case State.Attack:
                    break;
                case State.Dead:
                    break;
            }
        }
    }
}
