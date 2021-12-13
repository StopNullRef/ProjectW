using System;
using System.Collections.Generic;

namespace ProjectW.Controller
{
    /// <summary>
    /// 입력 처리를 수행하는 클래스
    /// 입력 처리 시 특정 키에 대해 어떠한 기능을 수행할 것인지에 대한 정보를 갖는 핸들러를 갖는다.
    /// </summary>
    public class InputController
    {
        /// <summary>
        /// 버튼 타입의 키 입력 실행할 메서드를 갖는 대리자
        /// </summary>
        public delegate void ButtonEvent();

        /// <summary>
        /// 축 타입의 키 입력 시 실행할 메서드를 갖는 대리자
        /// -> 파라미터로 검사한 축의 값을 받는다.
        /// </summary>
        public delegate void AxisEvent(float value);

        /// <summary>
        /// 사용하고자 하는 축 타입의 이름을 key 값으로,
        /// 해당 축 타입의 입력을 통해 실행시키고자 하는 기능들을 value로
        /// pair (한 쌍)으로 묶어 리스트에 담아둔다.
        /// </summary>
        public List<KeyValuePair<string, AxisHandler>> inputAxes = new List<KeyValuePair<string, AxisHandler>>();

        public List<KeyValuePair<string, ButtonHandler>> inputButtons = new List<KeyValuePair<string, ButtonHandler>>();

        /// <summary>
        /// 사용하고자 하는 축타입의 키와 기능을 inputAxes 리스트에 추가하는 기능
        /// </summary>
        /// <param name="key"></param>
        /// <param name="axisEvent"></param>
        public void AddAxis(string key, AxisEvent axisEvent)
        {
            inputAxes.Add(new KeyValuePair<string, AxisHandler>(key , new AxisHandler(axisEvent)));
        }

        public void AddButton(string key, ButtonEvent downEvent = null, ButtonEvent upEvent =null , 
            ButtonEvent pressEvent= null, ButtonEvent notPressEvnet =null)
        {
            inputButtons.Add(new KeyValuePair<string, ButtonHandler>(key, new ButtonHandler(downEvent, upEvent, pressEvent, notPressEvnet)));
        }

        public class AxisHandler
        {
            /// <summary>
            /// 해당 필드가 특정 축 타입 키에 대한 기능을 대리한다.
            /// </summary>
            private AxisEvent axisEvent;

            public AxisHandler(AxisEvent axisEvent)
            {
                this.axisEvent = axisEvent;
            }

            /// <summary>
            /// axisEvent에 담긴 기능을 실행시킬 수 있는 메서드
            /// </summary>
            /// <param name="value"></param>
            public void GetAxisValue(float value)
            {
                axisEvent?.Invoke(value);
            }
        }

        public class ButtonHandler
        {
            /// <summary>
            /// 키를 처음 누른 순간 (1번) 호출될 기능을 담아둘 대리자
            /// </summary>
            private ButtonEvent downEvent;
            /// <summary>
            /// 키를 누르고 있다가 떼는 순간 (1번) 호출될 기능을 담아둘 대리자
            /// </summary>
            private ButtonEvent upEvent;
            /// <summary>
            ///  키를 계속해서 누르고 있을 때, 매 번 실행시킬 기능을 담아둘 대리자
            /// </summary>
            private ButtonEvent pressEvent;
            /// <summary>
            /// 키를 계속해서 떼고 있을 때, 매 번 실행시킬 기능을 담아둘 대리자
            /// </summary>
            private ButtonEvent notPressEvent;

            public ButtonHandler(ButtonEvent downEvent = null, ButtonEvent upEvent= null, ButtonEvent pressEvent= null, ButtonEvent notPressEvent = null)
            {
                this.downEvent = downEvent;
                this.upEvent = upEvent;
                this.pressEvent = pressEvent;
                this.notPressEvent = notPressEvent;
            }

            public void OnDown()
            {
                downEvent?.Invoke();
            }

            public void OnUp()
            {
                upEvent?.Invoke();
            }

            public void OnPress()
            {
                pressEvent?.Invoke();
            }

            public void OnNotPress()
            {
                notPressEvent?.Invoke();
            }
        }
    }
}
