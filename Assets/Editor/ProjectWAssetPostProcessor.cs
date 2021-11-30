using UnityEditor;

namespace ProjectW.Editor
{
    // AssetPostprocessor unity���� asset������ �ִ� ������ ���� �Ǿ����� �۾��� �Ҽ� �ִ� Ŭ����

    /// <summary>
    /// ��� �ڻ��� ��ȭ�� �����Ͽ� �ݹ��� �߻���Ŵ
    /// -> �ݹ鿡���� ��ȭ�� ������ �ڻ꿡 ���� ������ �Ķ���ͷ� ���޵�
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