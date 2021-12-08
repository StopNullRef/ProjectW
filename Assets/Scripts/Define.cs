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

    public class StaticData
    {
        public const string SDPath = "Assets/StaticData";
        public const string SDExcelPath = "Assets/StaticData/Excel";
        public const string SDJsonPath = "Assets/StaticData/Json";
    }

}
