using System;
using UnityEngine;
using ProjectW.Util;


namespace ProjectW.Network
{
    /// <summary>
    /// 통신에  사용되는 모든 데이터셋의 베이스 클래스
    /// Dto -> data transfer object
    /// </summary>
    [Serializable]
    public class DtoBase
    {
        // 직접 작성하는 값이 아니라 통신 결과에 따라 채워지는 데이터이므로
        // 인스펙터에 노출되지 않도록 HideInInspector 명령어로 숨긴다.

        [HideInInspector]
        // 통신 결과에 대한 에러코드
        public int errorCode;

        [HideInInspector]
        // 에러에 대한 내용
        public string errorMessage;

    }

    /// <summary>
    /// 서버 통신 후 데이터에 대한 처리를 일반화하여 수행할 클래스
    /// DtoBase 클래스는?
    /// 서버와의 통신에 사용되는 데이터를 클래스로 전부 만들어둘건데
    /// 이 통신에 사용될 데이터를 Dto라고 표현
    /// 통신에 사용되는 모든 데이터들은 DtoBase 라는 클래스에서 파생되어 만들어짐
    /// 결과적으로 해당 클래스의 T타입에는 통신에 사용되는 데이터들만 올 수 있음
    /// </summary>
    public class ResponseHandler<T> where T: DtoBase
    {
        /// <summary>
        /// 응답 성공  시 호출될 델리게이트
        /// </summary>
        /// <param name="result">요청해서 받은 데이터</param>
        public delegate void OnSuccess(T result);

        /// <summary>
        /// 응답 실패시 호출될 델리게이트
        /// </summary>
        /// <param name="error">에러코드와 에러메세지</param>
        public delegate void OnFailed(DtoBase error);

        private OnSuccess successDel;
        private OnFailed failedDel;

        /// <summary>
        /// 생성 시에 응답 성공, 실패 시에 실행시킬 메서드를 받는다
        /// </summary>
        /// <param name="success">응답 성공 시 실행할 메서드</param>
        /// <param name="failed">응답 실패 시 실행할 메서드</param>
        public ResponseHandler(OnSuccess success, OnFailed failed)
        {
            successDel = success;
            failedDel = failed;
        }

        /// <summary>
        /// 서버에 데이터 요청 성공 시 응답 처리
        /// </summary>
        /// <param name="response">요청한 데이터</param>
        public void HandleSuccess(String response)
        {
            T data = null;

            // 서버에서 받은 json이 없다면 리턴
            if (response == null)
                return;

            // json을 임의의 T 타입으로 변환
            data = SerializationUtil.JsonToObject<T>(response);

            // 에러코드가 존재하는지 체크
            if (data.errorCode > 0)
            {
                // 에러가 존재하므로 실패 처리
                failedDel?.Invoke(data);
                return;
            }

            // 요청 성공 시 실행할 델리게이트가 존재한다면 실행
            successDel?.Invoke(data);
        }
    }
}
