using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ProjectW.Util;

namespace ProjectW.SD
{
    /// <summary>
    /// 모든 기획 데이터를 들고 있을 클래스
    /// 데이터를 로드하고 들고 있기만 할 것이므로, 모노를 상속받을 필요가 없음
    /// 모노를 갖지 않는 일반 C# 클래스를 인스펙터에 노출시키기 위해 직렬화
    /// </summary>
    [Serializable]
    public class StaticDataModule
    {
        public List<SDCharacter> sdCharacters;

        public void Initialize()
        {
            var loader = new StaticDataLoader();
            loader.Load(out sdCharacters);
        }

        /// <summary>
        /// 기획 데이터를 불로올 클래스
        /// </summary>
        private class StaticDataLoader
        {
            private string path;

            public StaticDataLoader()
            {
                path = $"{Application.dataPath}/StaticData/Json";
            }

            /// <summary>
            /// 기획데이터를 불러오는 기능
            /// </summary>
            /// <typeparam name="T">불러오고자하는 기획데이터의 타입</typeparam>
            /// <param name="data">불러온 데이터를 담을 리스트 참조</param>
            public void Load<T>(out List<T> data) where T : StaticData
            {
                // 파일이름이 타입이름에서 SD만 제거하면 동일하다는 규칙이 있음
                var fileName = typeof(T).Name.Remove(0, "SD".Length);

                // json 파일을 문자열로 읽어온다.
                var json = File.ReadAllText($"{path}/{fileName}.json");

                data = SerializationUtil.FromJson<T>(json);
            }
        }
    }
}
