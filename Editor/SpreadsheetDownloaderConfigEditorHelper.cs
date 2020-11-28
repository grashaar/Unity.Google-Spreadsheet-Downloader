using UnityEditor;

namespace Unity.GoogleSpreadsheetDownloader.Editor
{
    public static class SpreadsheetDownloaderConfigEditorHelper
    {
        [MenuItem("Tools/Google Spreadsheet/Locate Config File")]
        public static void LocateSpreadsheet()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(SpreadsheetDownloaderConfig)}");

            if (guids.Length <= 0)
                return;

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<SpreadsheetDownloaderConfig>(path);
        }
    }
}