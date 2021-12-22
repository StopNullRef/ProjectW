/// <summary>
/// 프로젝트에서 사용되는 상수나 열거형 등을 정의
/// </summary>
namespace ProjectW.Define
{
    /// <summary>
    /// 게임에서 사용되는 씬 종류
    /// </summary>
    public enum SceneType { Title, Ingame, Loading, }

    /// <summary>
    /// 타이틀 씬에서 순차적으로 수행할 작업을 열거
    /// </summary>
    public enum IntroPhase
    {
        Start, //시작
        ApplicationSetting, // 앱 세팅
        Server, // 서버 연결 및 초기화
        StaticData, // 기획 데이터 로드 (고정된 데이터)
        UserData, // 유저 데이터 로드 (유동적인 데이터)
        Resource, // 리소스 로드
        UI, // UI 매니저 초기화
        Comepelte, // 완료
    }

    public class Camera
    {
        public enum View { Standard,Front}
        public const float RotSpeed = 3f;
        public const string CamPosPath = "Prefabs/CamPos";
    }

    public class Input
    {
        public const string AxisX = "Horizontal"; // 캐릭터 좌우 이동에 사용 (a,d 만 사용가능하게)
        public const string AxisZ = "Vertical"; // 캐릭터 앞뒤 이동에 사용 (w,s 만 사용)
        public const string MouseX = "Mouse X"; // 마우스 수평(좌우) 이동에 대한 축값
        public const string MouseY = "Mouse Y"; // 마우스 수직 이동에 대한 축값
        public const string FrontCam = "Fire3"; // 카메라가 캐릭터 후방(3인칭)에서 전방 카메라로 변경(leftctrl만 사용)
        public const string Jump = "Jump"; // 캐릭터 점프 (spaceBar만 사용)
        public const string MouseLeft = "Fire1"; // 마우스 왼쪽 클릭
        public const string MouseRight = "Fire2"; // 마우스 오른쪽 클릭
    }

    public class Actor
    {
        /// <summary>
        /// 액터의 타입
        /// </summary>
        public enum Type { Character, Monster, }

        /// <summary>
        /// 액터의 상태
        /// </summary>
        public enum State { Idle, Walk, Jump, Attack, Dead, }

        /// <summary>
        /// 액터의 일반공격 타입
        /// </summary>
        public enum AttackType { Normal, Projectile, }
    }

    public class Monster
    {
        /// <summary>
        /// 최소 몬스터 스폰 수
        /// </summary>
        public const int MinSpawnCnt = 1;
        /// <summary>
        /// 최대 몬스터 스폰 수
        /// </summary>
        public const int MaxSpawnCnt = 5;
        /// <summary>
        /// 다음 스폰까지의 최소 시간
        /// </summary>
        public const float MinSpawnTime = 10f;
        /// <summary>
        /// 다음 스폰까지의 최대 시간
        /// </summary>
        public const float MaxSpawnTime = 20f;
        /// <summary>
        /// 정찰 후 최소 대기 시간
        /// </summary>
        public const float MinPatrolWaitTime = 1f;
        /// <summary>
        /// 정찰 후 최대 대기 시간
        /// </summary>
        public const float MaxPatrolwaitTime = 3f;
    }

    public class StaticData
    {
        public const string SDPath = "Assets/StaticData";
        public const string SDExcelPath = "Assets/StaticData/Excel";
        public const string SDJsonPath = "Assets/StaticData/Json";
    }

}
