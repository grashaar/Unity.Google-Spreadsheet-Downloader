#if UNITY_EDITOR

using System.IO;
using UnityEngine;

#if UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

using UnityEditor;

namespace Unity.GoogleSpreadsheetDownloader
{
    public static partial class SpreadsheetDownloader
    {
        internal static void DownloadSheet(string sheetName)
        {
#if UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK
            DownloadSheetInternal(sheetName).Forget();
#else
            DownloadSheetInternal(sheetName);
#endif
        }

#if UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK
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

            if (!(Selection.activeObject is SpreadsheetDownloaderConfig config))
            {
                Debug.LogError($"The current selected object is not an instance of {nameof(SpreadsheetDownloaderConfig)}");
                return;
            }

            if (!config.SheetGids.TryGetValue(sheetName, out var sheetDef))
            {
                Debug.LogError($"The instance of {nameof(SpreadsheetDownloaderConfig)} does not contain any sheet whose name is {sheetName}", Selection.activeObject);
                return;
            }

            var directory = GetDirectoryPath(config);

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

            AssetDatabase.Refresh();
        }
    }
}

#endif