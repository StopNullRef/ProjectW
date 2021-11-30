using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ProjectW.Editor
{
    /// <summary>
    /// StaticData 파일이 추가되었을 때 후처리를 진행
    /// excel 파일의 추가를 감지하고, json 파일로 변환한다.
    /// </summary>
    public static class StaticDataImporter
    {

        public static void Import(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetsPaths)
        {

        }

        /// <summary>
        /// 파일을 삭제한 경우 실행할 기능
        /// </summary>
        /// <param name="deletedAssets">삭제할 에셋 정보</param>
        private static void Delete(string[] deletedAssets)
        {

        }

        /// <summary>
        /// 파일이 이동 되었을때 실행할 기능
        /// </summary>
        /// <param name="movedAssets">새로운 경로(이동 후)의 에셋 정보</param>
        /// <param name="movedFromAssetPath">이전 경로(이동 전)의 에셋 정보</param>
        private static void Move(string[] movedAssets, string[] movedFromAssetPath)
        {
            // 이전 경로 에셋 삭제
            Delete(movedFromAssetPath);
            // 새로운 경로 에셋 수정
            ImportNewOrModified(movedAssets);
        }

        /// <summary>
        /// 파일을 새로 임포트하거나 수정했을 때
        /// </summary>
        /// <param name="importedAssets">임포트하거나 수정한 에셋 정보</param>
        private static void ImportNewOrModified(string[] importedAssets)
        {

        }

        /// <summary>
        /// 엑셀 파일을 감지하여 json 파일을 생성
        /// </summary>
        /// <param name="assets">변화가 감지된 에셋들의 정보(경로)</param>
        /// <param name="isDeleted">변화가 감지된 에셋의 변화 종류가 삭제인지?</param>
        private static void ExcelToJson(string[] assets, bool isDeleted)
        {
            // 엑셀 파일들의 경로를 담을 리스트
            List<string> staticDataAssets = new List<string>();

            // 파라미터로 받은 에셋 경로에서 엑셀파일만 걸러낸다.
            foreach(var asset in assets)
            {
                if (IsStaticData(asset, isDeleted))
                    staticDataAssets.Add(asset);
            }

            // 걸러낸 excel 기획데이터를 json으로 변환을 시도한다.
            foreach (var staticDataAsset in staticDataAssets)
            {
                try
                {
                    // 경로에서 파일이름과 확장자만 남김다
                    var fileName = staticDataAsset.Substring(staticDataAsset.LastIndexOf("/")+1);
                    // 확장자를 제거해서 파일이름만 남긴다.
                    fileName = fileName.Remove(fileName.LastIndexOf("."));

                    // 상대 경로를 통해 프로젝트의 Asset 폴더까지의 경로를 읽음
                    var rootPath = Application.dataPath;
                    // Assets 폴더 경로만 지운다. 프로젝트 폴더 경로까지만 남음
                    rootPath = rootPath.Remove(rootPath.LastIndexOf('/'));

                    var fileFullPath = $"{rootPath}/{staticDataAsset}";

                    // 변환을 위해 Json Converter 객체를 생성
                    var excelToJsonConvert = new ExcelToJsonConvert(fileFullPath, $"{rootPath}/{Define.StaticData.SDJsonPath}");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    Debug.LogErrorFormat("Couldn't convert assets = {0}", staticDataAsset);
                    EditorUtility.DisplayDialog("Error Convert",string.Format("Couldn't convert assets = {0}", staticDataAsset),"OK");
                }
            }
        }

        /// <summary>
        /// 파일이 엑셀 파일이면서 기획데이터인지 확인하는 기능
        /// </summary>
        /// <param name="path">확인하고자 하는 파일 경로</param>
        /// <param name="isDeleted">해당 파일의 발생된 변화가 삭제인지?</param>
        /// <returns>엑셀이면서 기획데이터라면 true, 아니라면 false</returns>
        private static bool IsStaticData(string path, bool isDeleted)
        {
            // excel 파일이 아니라면 리턴
            if(path.EndsWith(".xlsx") == false)
                return false;

            // 확인하고자 하는 파일의 경로가 Assets부터 시작하는 경로로 들어옴
            // -> 이 때 파일의 존재를 확인하기 위해 전체 경로가 필요하여
            // Application.dataPath를 통해 프로젝트의 Assets 폴더까지의 경로를 구한뒤
            // 중복되는 Assets 경로 부분만 제거
            var absoultePath = Application.dataPath + path.Remove(0, "Assets".Length);

            // 삭제하는 파일 이거나 존재하는 파일이여하고, 경로는 excel 데이터 경로에 있어야한다.
            // -> 해당 파일이 엑셀이면서 기획데이터라는 뜻
            return ((isDeleted || File.Exists(absoultePath)) && path.StartsWith(Define.StaticData.SDExcelPath));
        }
    }
}
