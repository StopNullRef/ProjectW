
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
        /// 플레이어가 적을 포인팅하고 있는지에 대한 필드
        /// </summary>
        public bool HasPointTarget { get; set; }

        /// <summary>
        /// 포인팅하고 있는 타겟에 대한 트랜스폼 참조
        /// </summary>
        private Transform pointingTarget;

        /// <summary>
        /// 플레이어 캐릭터의 인스턴스 참조
        /// </summary>
        public Character PlayerCharacter { get; private set; }

        public CameraController cameraController;

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
            // 내가 사용할 캐릭터에게 플레이어 태그를 부여
            character.tag = "Player";
            character.gameObject.layer = LayerMask.NameToLayer("Player");

            PlayerCharacter = character;

            cameraController.SetTarget(PlayerCharacter.transform);

            inputController.AddAxis(Input.AxisZ, GetAxisZ); // 캐릭터 앞뒤 이동 키
            inputController.AddAxis(Input.MouseX, GetMouseX); // 캐릭터 좌우 회전

            inputController.AddButton(Input.MouseLeft, OnDownMouseLeft); // 일반공격 키, 눌렀을 때 한 번 공격 실행
            inputController.AddButton(Input.MouseRight, OnDownMouseRight, OnUpMouseRight, null, OnUpMouseRight); // 누르고 있을 시 회전 가능 상태, 아니라면 회전 불가능 상태
            inputController.AddButton(Input.Jump, OnDownJump); // 점프 키, 눌렀을 때 한 번 점프 실행
        }

        private void FixedUpdate()
        {
            if (PlayerCharacter == null)
            {
                return;
            }

            if (PlayerCharacter.State == Define.Actor.State.Dead)
            {
                return;
            }

            CheckMousePointTarget();
            InputUpdate();
        }

        /// <summary>
        /// 플레이어가 마우스로 특정 타겟을 포인팅하는지 대한 연산
        /// </summary>
        private void CheckMousePointTarget()
        {
            // 현재 씬에서 사용하는 카메라에서 스크린 좌표계의 마우스 위치로 레이 생성
            var ray = CameraController.Cam.ScreenPointToRay(UnityEngine.Input.mousePosition);

            // 생성한 레이를 통해 해당 레이 방향에 몬스터가 존재하는지 체크
            var hits = Physics.RaycastAll(ray, 1000f, 1 << LayerMask.NameToLayer("Monster"));

            // 레이캐스팅의 결과가 담긴 배열의 길이가 0이 아니라면 타겟 존재
            HasPointTarget = hits.Length != 0;
            // 추후 포인팅한 타겟을 공격 시, 캐릭터를 타겟쪽으로 회전시키기 위해 타겟의 트랜스폼을 받음
            pointingTarget = HasPointTarget ? hits[0].transform : null;

        }

        private void InputUpdate()
        {
            for (int i = 0; i < inputController.inputAxes.Count; i++)
            {
                var value = UnityEngine.Input.GetAxisRaw(inputController.inputAxes[i].Key);
                inputController.inputAxes[i].Value.GetAxisValue(value);
            }

            for (int i = 0; i < inputController.inputButtons.Count; i++)
            {
                var key = inputController.inputButtons[i].Key;
                var value = inputController.inputButtons[i].Value;

                if (UnityEngine.Input.GetButtonDown(key))
                {
                    value.OnDown();
                }
                else if (UnityEngine.Input.GetButton(key))
                {
                    value.OnPress();
                }
                else if (UnityEngine.Input.GetButtonUp(key))
                {
                    value.OnUp();
                }
                else
                {
                    value.OnNotPress();
                }

            }
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
            // 마우스가 가리키는 객체(몬스터)의 정보가 존재한다면
            if(pointingTarget != null)
            {
                // y축 회전만 실행하고, 나머지 축은 기존 회전 값을 유지할 수 있도록
                var origintRot = PlayerCharacter.transform.eulerAngles;
                // 플레이어가 타겟의 트랜스폼을 바라보도록
                PlayerCharacter.transform.LookAt(pointingTarget);
                // 변경된 회전값에서 y축 값은 유지한 채, x,z축 회전 값을 원래 회전값으로 변경
                var newRot = PlayerCharacter.transform.eulerAngles;
                newRot.x = origintRot.x;
                newRot.z = origintRot.z;
                PlayerCharacter.transform.eulerAngles = newRot;
            }

            PlayerCharacter.SetState(Define.Actor.State.Attack);
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
            // 이미 공중이라면 점프할 수 없게 리턴
            if (!PlayerCharacter.boActor.isGround)
                return;

            PlayerCharacter.SetState(Define.Actor.State.Jump);
        }

        #endregion
    }
}
