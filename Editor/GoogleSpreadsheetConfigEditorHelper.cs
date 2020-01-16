using UnityEditor;

namespace Unity.GoogleSpreadsheet.Editor
{
    public static class GoogleSpreadsheetConfigEditorHelper
    {
        [MenuItem("Tools/Google Spreadsheet/Locate config file")]
        public static void LocateSpreadsheet()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(GoogleSpreadsheetConfig)}");

            if (guids.Length <= 0)
                return;

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<GoogleSpreadsheetConfig>(path);
        }
    }
}