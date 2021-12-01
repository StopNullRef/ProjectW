using ProjectW.Network;
using ProjectW.Util;


namespace ProjectW.Dummy
{
    public class DummyServer : Singleton<DummyServer>
    {
        /// <summary>
        /// 더미서버에서 갖는 유저 데이터 (더미서버에서의 유저 DB라고  생각하면 됨)
        /// </summary>
        public UserDataSO userData;

        /// <summary>
        /// 통신 메서드를 갖는 인터페이스를 더미서버에 맞게  구현한 객체
        /// </summary>
        public INetworkClient dummyModule;

        public void Initialize()
        {
            dummyModule = new ServerModuleDummy();
        }
    }
}
