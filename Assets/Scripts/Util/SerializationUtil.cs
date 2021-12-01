using System.Collections.Generic;
using Newtonsoft.Json;


namespace ProjectW.Util
{
    /// <summary>
    /// 이번에는 Untiy의 JsonUtility를 사용하지 않음
    /// 이유? Unity의 JsonUtility는 직렬화하고자하는 객체가
    /// 다른 객체에서 파생되는 개채라면 정상적으로 기반에 있던 필드를 받아올 수 없음
    /// 쓰려면 별도로 JsonUtility를 커스텀해야함..
    /// 
    /// 일반적으로 JsonUtility를 사용하지 않고, 외부 라이브러리 중에 LitJson, NewtonsoftJson을
    /// 주로 많이 사용함... 저희는 그 중에 NewtonsoftJson을 사용
    /// </summary>
    public static class SerializationUtil
    {
        /// <summary>
        /// 파라미터로 받은 json을 지정한 T타입으로 역직렬화하여 반환
        /// </summary>
        /// <typeparam name="T">역직렬화하고자 하는 타입</typeparam>
        /// <param name="json">역직렬화할 json 데이터</param>
        /// <returns>역직렬화된 json 데이터</returns>
        public static T JsonToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 파라미터로 받은 json을 지정한 T타입의 목록 형태로 역직렬화하여 반환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        /// <summary>
        /// T타입 데이터를 json으로 직렬화하여 변환
        /// </summary>
        /// <typeparam name="T">직렬화하고자 하는 타입</typeparam>
        /// <param name="obj">직렬화할 T타입 데이터</param>
        /// <returns>직렬화된 json 데이터</returns>
        public static string ToJSon<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

    }
}
