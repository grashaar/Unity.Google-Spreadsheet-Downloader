using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UniRx.Async;

namespace GoogleSpreadsheet
{
    public static class GoogleSpreadsheetHelper
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
        /// <param name="progressCallback"></param>
        /// <param name="finishCallback"></param>
        /// <returns>Total sheet count</returns>
        public static int Download(GoogleSpreadsheetConfig config, string downloadPath,
                                   Action<int> progressCallback = null, Action finishCallback = null)
        {
            if (!config)
                return 0;

            if (config.SheetGids.Count <= 0)
                return 0;

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            DownloadAsync(config, downloadPath, progressCallback, finishCallback);
            return config.SheetGids.Count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        /// <param name="progressCallback"></param>
        /// <param name="finishCallback"></param>
        /// <returns>Total sheet count</returns>
        public static int Download(GoogleSpreadsheetConfig config,
                                   Action<int> progressCallback = null, Action finishCallback = null)
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

            DownloadAsync(config, directory, progressCallback, finishCallback);
            return config.SheetGids.Count;
        }

        private static async void DownloadAsync(GoogleSpreadsheetConfig config, string directory,
                                                Action<int> progressCallback, Action finishCallback)
        {
            var i = 1;

            foreach (var kv in config.SheetGids)
            {
                var url = config.GetDownloadUrl(kv.Value);
                var path = Path.Combine(directory, $"{kv.Key}.{config.FileType}");

                await Download(UnityWebRequest.Get(url), path);

                progressCallback?.Invoke(i);

                i += 1;
            }

            finishCallback?.Invoke();
        }

        private static async UniTask Download(UnityWebRequest req, string filePath)
        {
            var operation = await req.SendWebRequest();

            File.WriteAllText(filePath, operation.downloadHandler.text ?? string.Empty);
        }
    }
}
