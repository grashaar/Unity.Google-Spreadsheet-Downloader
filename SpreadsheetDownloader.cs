using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Unity.GoogleSpreadsheetDownloader
{
    public static partial class SpreadsheetDownloader
    {
        public static string GetDirectoryPath(SpreadsheetDownloaderConfig config)
        {
            if (!config || string.IsNullOrEmpty(config.DownloadFolder))
                return Application.dataPath;

            return Path.Combine(Application.dataPath, config.DownloadFolder).Replace("\\", "/");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        /// <param name="downloadPath"></param>
        /// <param name="onUpdateProgress"></param>
        /// <param name="onCompleted"></param>
        /// <returns>Total sheet count</returns>
        public static int Download(SpreadsheetDownloaderConfig config, string downloadPath,
                                   Action<int> onUpdateProgress = null, Action onCompleted = null)
        {
            if (!config)
                return 0;

            if (config.SheetGids.Count <= 0)
                return 0;

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

#if UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK
            DownloadSheets(config, downloadPath, onUpdateProgress, onCompleted).Forget();
#else
            DownloadSheets(config, downloadPath, onUpdateProgress, onCompleted);
#endif

            return config.SheetGids.Count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        /// <param name="onUpdateProgress"></param>
        /// <param name="onCompleted"></param>
        /// <returns>Total sheet count</returns>
        public static int Download(SpreadsheetDownloaderConfig config,
                                   Action<int> onUpdateProgress = null, Action onCompleted = null)
        {
            if (!config)
                return 0;

            if (config.SheetGids.Count <= 0)
                return 0;

            var directory = GetDirectoryPath(config);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

#if UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK
            DownloadSheets(config, directory, onUpdateProgress, onCompleted).Forget();
#else
            DownloadSheets(config, directory, onUpdateProgress, onCompleted);
#endif

            return config.SheetGids.Count;
        }

#if UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK
        private static async UniTaskVoid DownloadSheets(SpreadsheetDownloaderConfig config, string directory,
                                                        Action<int> onUpdateProgress, Action onCompleted)
#else
        private static async void DownloadSheets(SpreadsheetDownloaderConfig config, string directory,
                                                 Action<int> onUpdateProgress, Action onCompleted)
#endif
        {
            var i = 1;
            var sheets = config.SheetGids.ToArray();

            foreach (var sheet in sheets)
            {
                var sheetName = sheet.Key;
                var sheetDef = sheet.Value;
                var url = config.GetDownloadUrl(sheetDef.Gid);
                var ext = string.IsNullOrEmpty(sheetDef.CustomExtension) ? $"{config.Format}" : sheetDef.CustomExtension;
                var path = Path.Combine(directory, $"{sheetName}.{ext}");

                Debug.Log($"Begin downloading <b>{sheetName}.{ext}</b>");

                await DownloadDataAsync(url, path);

                Debug.Log($"Downloaded <b>{sheetName}.{ext}</b> to {path}");

                onUpdateProgress?.Invoke(i);

                i += 1;
            }

            onCompleted?.Invoke();
        }

#if UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK
        private static async UniTask DownloadDataAsync(string url, string filePath)
#else
        private static async Task DownloadDataAsync(string url, string filePath)
#endif
        {
            var req = UnityWebRequest.Get(url);
            await req.SendWebRequest();

            File.WriteAllText(filePath, req.downloadHandler.text ?? string.Empty);
        }
    }
}
