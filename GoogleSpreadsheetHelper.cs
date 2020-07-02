using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_GOOGLESPREADSHEET_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Unity.GoogleSpreadsheet
{
    public static partial class GoogleSpreadsheetHelper
    {
        public static string GetDownloadDirectory(GoogleSpreadsheetConfig config)
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
        public static int Download(GoogleSpreadsheetConfig config, string downloadPath,
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

#if UNITY_GOOGLESPREADSHEET_UNITASK
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
        public static int Download(GoogleSpreadsheetConfig config,
                                   Action<int> onUpdateProgress = null, Action onCompleted = null)
        {
            if (!config)
                return 0;

            if (config.SheetGids.Count <= 0)
                return 0;

            var directory = GetDownloadDirectory(config);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

#if UNITY_GOOGLESPREADSHEET_UNITASK
            DownloadSheets(config, directory, onUpdateProgress, onCompleted).Forget();
#else
            DownloadSheets(config, directory, onUpdateProgress, onCompleted);
#endif

            return config.SheetGids.Count;
        }

#if UNITY_GOOGLESPREADSHEET_UNITASK
        private static async UniTaskVoid DownloadSheets(GoogleSpreadsheetConfig config, string directory,
                                                        Action<int> onUpdateProgress, Action onCompleted)
#else
        private static async void DownloadSheets(GoogleSpreadsheetConfig config, string directory,
                                                 Action<int> onUpdateProgress, Action onCompleted)
#endif
        {
            var i = 1;

            foreach (var kv in config.SheetGids)
            {
                var sheetName = kv.Key;
                var sheetDef = kv.Value;
                var url = config.GetDownloadUrl(sheetDef.Gid);
                var ext = string.IsNullOrEmpty(sheetDef.CustomExtension) ? $"{config.Format}" : sheetDef.CustomExtension;
                var path = Path.Combine(directory, $"{sheetName}.{ext}");

                await DownloadDataAsync(url, path);

                onUpdateProgress?.Invoke(i);

                i += 1;
            }

            onCompleted?.Invoke();
        }

#if UNITY_GOOGLESPREADSHEET_UNITASK
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
