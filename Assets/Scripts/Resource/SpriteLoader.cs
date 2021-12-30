using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace ProjectW.Resource
{
    /// <summary>
    /// 게임에서 런타임에 사용되는 모든 아틀라스를 관리하는 클래스
    /// 아틀라스란?
    /// 여러 개의 스프라이트를 하나의 텍스쳐로 만들어 사용하는 것 (메모리 최적화)
    /// </summary>
    public static class SpriteLoader
    {
        // 보통 아틀라스를 분류하는 방법?
        // 게임의 규모 또는 장르에 따라 달라질 수 있음
        // 일반적으로 씬 단위로 관리
        // ex ) 타이틀 씬에서 사용되는 스프라이트 -> TitleAtlas
        // 여러 씬에서 사용되는 스프라이트들
        // ex ) CommonAtlas

        /// <summary>
        /// 모든 아틀라스들을 담아둘 딕셔너리
        /// </summary>
        private static Dictionary<Define.Resource.AtlasType, SpriteAtlas> atlases = new Dictionary<Define.Resource.AtlasType, SpriteAtlas>();

        // 열거형, ItemAltas
        // itematals, sprites

        /// <summary>
        /// 매개변수로 받은 아틀라스 목록의 아틀라스들을 딕셔너리에 등록하는 기능
        /// </summary>
        /// <param name="atlases"></param>
        public static void SetAtlas(SpriteAtlas[] atlases)
        {
            for (int i = 0; i < atlases.Length; i++)
            {
                // 아틀라스 이름이 아틀라스 타입 열거형이름과 동일하므로, 아틀라스의 이름을
                // 열거형으로 파싱하여 키값을 구한다
                var key = (Define.Resource.AtlasType)Enum.Parse(typeof(Define.Resource.AtlasType), atlases[i].name);

                // this 키워드 처럼 자기자신의 필드에 있는 atlase 필드에 접근
                // this를 쓰지 않는 것은 static 클래스이기 때문에 인스턴스를 갖지 않으므로
                // this를 사용할 수 없다.
                SpriteLoader.atlases.Add(key, atlases[i]);
            }
        }

        /// <summary>
        /// 특정 아틀라스에서 원하는 스프라이트를 찾아서 반환하는 기능
        /// </summary>
        /// <param name="type">찾고자 하는 스프라이트가 들어있는 아틀라스의 딕셔너리 상의 키 값</param>
        /// <param name="spriteKey">찾고자 하는 스프라이트의 이름</param>
        /// <returns>찾은 스프라이트 참조</returns>
        public static Sprite GetSprite(Define.Resource.AtlasType type, string spriteKey)
        {
            // 딕셔너리에 키 값이 존재하지 않는다면 아틀라스가 없는 것이므로 null
            if (!atlases.ContainsKey(type))
                return null;

            return atlases[type].GetSprite(spriteKey);
        }
    }
}
