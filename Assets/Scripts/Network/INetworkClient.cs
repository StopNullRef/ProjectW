﻿using ProjectW.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectW.Network
{
    /// <summary>
    /// 서버와 통시하는 프로토콜 메서드를 갖는 인터페이스
    /// 프로토콜이란?
    /// 서버와 클라이언트 간에 통신에 사용되는 API
    /// </summary>
    public interface INetworkClient
    {
        /// <summary>
        /// 서버에 계정 정보를 요청하는 메서드
        /// </summary>
        /// <param name="uniqueId">
        /// 서버에 계정 정보를 요청하면서 보내는 각 계정마다 부여된 고유한 아이디
        ///  -> 임의로 디바이스의 유니크 아이디를 이용
        /// </param>
        /// <param name="responseHandler">
        /// 서버에 요청한 데이터를 받아서 처리할 핸들러
        /// </param>
        void Login(int uniqueId, ResponseHandler<DtoAccount> responseHandler);
    }
}
