using ProjectW.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectW.UI
{
    /// <summary>
    /// 인게임에서 일반적으로 사용하는 UI들을 관리하는 클래스
    /// -> 커서, 몬스터 hp바, 캐릭터 hud 등
    /// </summary>
    public class UIIngame : UIWindow
    {
        /// <summary>
        /// 플레이어가 마우스 입력을 통해 적을 타겟팅하고 있는지에 대한 데이터를
        /// 얻기 위해 참조를 받아둠
        /// </summary>
        public PlayerController playerController;

        public Texture2D defaultCursor; // 기본 커서 텍스쳐
        public Texture2D targetPointerCursor; // 몬스터에 마우스 포인팅 시 커서 텍스쳐
    }
}
