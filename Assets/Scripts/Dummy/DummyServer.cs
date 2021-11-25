using PorjectW.Network;
using ProjectW.Util;


namespace ProjectW.Dummy
{
    public class DummyServer : Singleton<DummyServer>
    {

        /// <summary>
        /// 통신 메서드를 갖는 인터페이스를 더미서버에 맞게  구현한 객체
        /// </summary>
        public INetworkClient dummyModule;
    }
}
