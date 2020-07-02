#if UNITY_EDITOR

using System.IO;
using UnityEngine;

#if UNITY_GOOGLESPREADSHEET_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

using UnityEditor;

namespace Unity.GoogleSpreadsheet
{
    public static partial class GoogleSpreadsheetHelper
    {
        internal static void DownloadSheet(string sheetName)
        {
#if UNITY_GOOGLESPREADSHEET_UNITASK
            DownloadSheetInternal(sheetName).Forget();
#else
            DownloadSheetInternal(sheetName);
#endif
        }

#if UNITY_GOOGLESPREADSHEET_UNITASK
        private static async UniTaskVoid DownloadSheetInternal(string sheetName)
#else
        private static async void DownloadSheetInternal(string sheetName)
#endif
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                Debug.LogError($"Sheet name is null or empty");
                return;
            }

            if (!(Selection.activeObject is GoogleSpreadsheetConfig config))
            {
                Debug.LogError($"The current selected object is not an instance of {nameof(GoogleSpreadsheetConfig)}");
                return;
            }

            if (!config.SheetGids.TryGetValue(sheetName, out var sheetDef))
            {
                Debug.LogError($"The instance of {nameof(GoogleSpreadsheetConfig)} does not contain any sheet whose name is {sheetName}", Selection.activeObject);
                return;
            }

            var directory = GetDownloadDirectory(config);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var url = config.GetDownloadUrl(sheetDef.Gid);
            var ext = string.IsNullOrEmpty(sheetDef.CustomExtension) ? $"{config.Format}" : sheetDef.CustomExtension;
            var path = Path.Combine(directory, $"{sheetName}.{ext}");

            Debug.Log($"Begin downloading <b>{sheetName}.{ext}</b>");

            await DownloadDataAsync(url, path);

            Debug.Log($"Downloaded <b>{sheetName}.{ext}</b> to {path}");
        }
    }
}

#endif