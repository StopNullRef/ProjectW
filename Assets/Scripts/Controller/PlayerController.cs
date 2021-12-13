using ProjectW.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectW.Controller
{

    using Input = Define.Input;

    /// <summary>
    /// 플레이어 캐릭터의 입력 처리 등 다양한 제어를 할 클래스
    /// 캐릭터 클래스에서 처리하지 않고 별도로 플레이어 컨트롤러를 작성하는 이유?
    /// 캐릭터와 플레이어의 입력을 분리함으로써 캐릭터 클래스를 더 다양하게
    /// 사용할 수 있기 때문에 
    /// (ex: 멀티 환경에서와 같이 캐릭터 객체가 여러개 존재한다면, 입력 처리가 분리되지 않을 시 코드 작성이 어려움)
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// 플레이어 캐릭터의 인스턴스 참조
        /// </summary>
        public Character PlayerCharacter { get; private set; }

        private InputController inputController = new InputController();

        /// <summary>
        /// 플레이어 컨트롤러 초기화 시 내 캐릭터의 인스턴스를 받아와
        /// 컨트롤러를 초기화 한다.
        /// </summary>
        /// <param name="character"></param>
        public void Initialize(Character character)
        {
            // 내 캐릭터의 하이라키 상의 부모를 플레이어 컨트롤러로 지성
            character.transform.SetParent(transform);

            PlayerCharacter = character;

            inputController.AddAxis(Input.AxisZ, GetAxisZ); // 캐릭터 앞뒤 이동 키
            inputController.AddAxis(Input.MouseX, GetMouseX); // 캐릭터 좌우 회전

            inputController.AddButton(Input.MouseLeft, OnDownMouseLeft); // 일반공격 키, 눌렀을 때 한 번 공격 실행
            inputController.AddButton(Input.MouseRight, OnDownMouseRight, OnUpMouseRight); // 누르고 있을 시 회전 가능 상태, 아니라면 회전 불가능 상태
            inputController.AddButton(Input.Jump, OnDownJump); // 점프 키, 눌렀을 때 한 번 점프 실행
        }

        #region Input Implementation (입력 구현부)
        private void GetAxisZ(float value)
        {
            PlayerCharacter.boActor.moveDir.z = value;
        }

        private void GetMouseX(float value)
        {
            PlayerCharacter.boActor.rotDir.y = PlayerCharacter.boActor.canRot ? value : 0;
        }

        private void OnDownMouseLeft()
        {

        }

        private void OnDownMouseRight()
        {
            PlayerCharacter.boActor.canRot = true;
        }

        private void OnUpMouseRight()
        {
            PlayerCharacter.boActor.canRot = false;
        }

        private void OnDownJump()
        {
            PlayerCharacter.SetState(Define.Actor.State.Jump);
        }

        #endregion
    }
}
