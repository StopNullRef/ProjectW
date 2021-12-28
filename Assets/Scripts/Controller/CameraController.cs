using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectW.Resource;
using UnityEngine;
using static ProjectW.Define.Camera;

namespace ProjectW.Controller
{
    /// <summary>
    /// 인게임 내에서의 카메라를 제어할 클래
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// 현재 카메라 뷰를 나타내는 필드
        /// </summary>
        public View view;

        /// <summary>
        /// 카메라 이동 시 선형보간을 이용한 이동을 사용할 것임
        /// -> 이 때 선형보간에 사용할 값
        /// </summary>
        public float smooth = 3f;

        /// <summary>
        /// 뒤쪽에서 3인칭으로 캐릭터를 찍을 때의 위치를 갖는 트랜스폼 참조
        /// </summary>
        private Transform standardPos;

        /// <summary>
        /// 앞쪽에서 3인칭으로 캐릭터를 찍을 때의 위치를 갖는 트랜스폼 참조
        /// </summary>
        private Transform frontPos;

        /// <summary>
        /// 카메라가 추적할 타겟의 트랜스폼 참조 (플레이어)
        /// </summary>
        private Transform target;

        /// <summary>
        /// 카메라 컴포넌트를 외부에서  사용할 일이 잦으므로
        /// 미리 이 곳에서 참조를 받아두고 정적 필드에 담아둠으로써
        /// 편리하게 접글할 수 있도록 한다
        /// </summary>
        public static Camera Cam { get; private set; }

        private void Start()
        {
            Cam = GetComponent<Camera>();
        }

        /// <summary>
        /// 카메라의 추적 타겟을 설정하는 기능
        /// </summary>
        /// <param name="newTarget">추적하고자 하는 타겟의 트랜스폼 참조</param>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;

            // CamPos 프리팹을 이용하여 타겟의 자식으로 CamPos 객체를 생성함
            // -> 결과적으로 CamPos의 부모는 타겟이 되고, 카메라가 CamPos에  설정된
            // 위치나 회전 값을 가지고, 그대로 타겟을 추적할 수 있게 된다.

            var camPos = Instantiate(ResourceManager.Instance.LoadObject(Define.Camera.CamPosPath).transform);
            camPos.SetParent(target.transform);
            // 부모 변경 후 , 기존 월드를 기준으로 사용하던 위치가 변경되므로
            // 부모를 기준으로 0,0,0의 위치에 존재하게 한다.
            camPos.localPosition = Vector3.zero;

            // 위의 과정을 통해 결과적으로 CamPos의 자식으로 배치된 스탠다드 뷰와 프론트 뷰의 트랜스폼이
            // 타겟을 기준으로 기존에 설정해둔 위치와 회전값을 유지할 수 있게 된다.

            standardPos = camPos.Find("StandardPos");
            frontPos = camPos.Find("FrontPos");

            // 타겟 설정 시 카메라의 초기 위치와 바라보는 방향을 스탠다드 뷰의 위치와 방향으로 설정
            transform.position = standardPos.position;
            transform.forward = standardPos.forward;
        }

        private void FixedUpdate()
        {
            if (target == null)
                return;

            switch (view)
            {
                case View.Standard:
                    SetPosition(false, standardPos);
                    break;
                case View.Front:
                    SetPosition(true, frontPos);
                    break;
            }
        }

        /// <summary>
        /// 강제로 스탠다드 뷰의 위치와 회전 값을 갖도록 설정하는 기능
        /// </summary>
        public void SetForceStandardView()
        {
            SetPosition(false, standardPos);
        }

        /// <summary>
        /// 카메라의 이동 및 회전 연산
        /// </summary>
        /// <param name="isLerp">이동/회전 시 보간할 것인지? 보간 하지 않을 시 한 번에 이동/회전</param>
        /// <param name="target">스탠다드 뷰 또는 프론트 뷰, 둘 중 하나</param>
        private void SetPosition(bool isLerp,Transform target)
        {
            if(isLerp)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Time.fixedDeltaTime * smooth);
                transform.forward = Vector3.Lerp(transform.forward, target.forward, Time.fixedDeltaTime * smooth);
            }
            else
            {
                transform.position = target.position;
                transform.forward = target.forward;
            }
        }
    }
}
