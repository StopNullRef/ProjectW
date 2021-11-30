using UnityEditor;

namespace ProjectW.Editor
{
    // AssetPostprocessor unity에서 asset폴더에 있는 내용이 변경 되었을때 작업을 할수 있는 클래스

    /// <summary>
    /// 모든 자산의 변화를 감지하여 콜백을 발생시킴
    /// -> 콜백에서는 변화가 감지된 자산에 대한 정보가 파라미터로 전달됨
    /// </summary>
    public class ProjectWAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, 
            string[] movedAssets, string[] movedFromAssetsPaths)
        {
            StaticDataImporter.Import(importedAssets, deletedAssets, movedAssets, movedFromAssetsPaths);
        }
    }
}