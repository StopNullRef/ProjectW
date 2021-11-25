using ProjectW.Util;


namespace ProjectW.Network
{
    /// <summary>
    /// 클라이언트 내 전체적인 서버 통신을 관리하는 매니저
    /// 상황에 따라 서버 모듈을 생성하여 통신을 처리한다.
    ///  -> 여기서 말하는 상황
    ///  더미서버를 사용할 경우 더미서버 모듈을 생성
    ///  라이브서버를 사용할 경우 라이브서버 모듈을 생성
    /// </summary>
    public class SeverManager : Singleton<SeverManager>
    {

    }
}
