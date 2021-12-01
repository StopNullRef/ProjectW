using System;
using System.Linq;
using ProjectW.DB;
using ProjectW.Network;
using ProjectW.Util;

namespace ProjectW.Dummy
{
    /// <summary>
    /// 더미서버에서의 통신 프로토콜 구현부를 갖는 클래스
    /// </summary>
    public class ServerModuleDummy : INetworkClient
    {
        /// <summary>
        /// 더미서버에서는 로그인 요청 메서드를 어떤식으로 처리할 것인지를 구현
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="responseHandler"></param>
        public void Login(int uniqueId, ResponseHandler<DtoAccount> responseHandler)
        {
            // 더미서버이므로 실제로 클라이언트에서 클라이언트의 요청을 처리하는 것과 같음 (원맨쇼)
            // 한마디로 통신 요청에 대한 실패가 발생할 일이 일반적으로 없음
            // 강제로 요청 성공 메서드를 실행시킴
            responseHandler.HandleSuccess(SerializationUtil.ToJSon(DummyServer.Instance.userData.dtoAccount));
        }
    }
}
