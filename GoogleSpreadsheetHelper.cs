using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unity.GoogleSpreadsheet
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
                var sheetName = kv.Key;
                var sheetDef = kv.Value;
                var url = config.GetDownloadUrl(sheetDef.Gid);
                var ext = string.IsNullOrEmpty(sheetDef.CustomExtension) ? $"{config.Format}" : sheetDef.CustomExtension;
                var path = Path.Combine(directory, $"{sheetName}.{ext}");

                await Download(UnityWebRequest.Get(url), path);

                progressCallback?.Invoke(i);

                i += 1;
            }

            finishCallback?.Invoke();
        }

        private static async UniTask Download(UnityWebRequest req, string filePath)
        {
            await req.SendWebRequest();

            File.WriteAllText(filePath, req.downloadHandler.text ?? string.Empty);
        }

#if UNITY_EDITOR
        internal static async void Download(string sheetName)
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

            await Download(UnityWebRequest.Get(url), path);

            Debug.Log($"Downloaded <b>{sheetName}.{ext}</b> to {path}");
        }
#endif
    }
}
