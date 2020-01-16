using UnityEngine;
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Unity.GoogleSpreadsheet.Editor
{
    [CustomEditor(typeof(GoogleSpreadsheetConfig))]
    public sealed class GoogleSpreadsheetConfigEditor :
#if ODIN_INSPECTOR
        OdinEditor
#else
        UnityEditor.Editor
#endif
    {
        private GoogleSpreadsheetConfig config;
        private int total;

#if ODIN_INSPECTOR
        protected override void OnEnable()
        {
            base.OnEnable();
#else
        private void OnEnable()
        {
#endif
            this.config = this.target as GoogleSpreadsheetConfig;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Google Spreadsheet document must be published " +
                                    "to the web before using this tool.", MessageType.Warning);

            GUILayout.Space(16);

            base.OnInspectorGUI();

            var downloadDirectory = GoogleSpreadsheetHelper.GetDownloadDirectory(this.config);
            EditorGUILayout.HelpBox($"Download directory: {downloadDirectory}", MessageType.Info);

            GUILayout.Space(16);

            if (GUILayout.Button("Download sheets", GUILayout.Height(32)))
            {
                this.total = GoogleSpreadsheetHelper.Download(this.config, UpdateDownloadProgress, FinishDownload);
                ShowProgressBar();
            }
        }

        private void ShowProgressBar()
        {
            if (this.total > 0)
                EditorUtility.DisplayProgressBar("Google Spreadsheet Downloader", "Downloading...", 0f);
        }

        private void UpdateDownloadProgress(int count)
        {
            if (count >= this.total)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            EditorUtility.DisplayProgressBar("Google Spreadsheet Downloader", "Downloading...", count * 1f / this.total);
        }

        private void FinishDownload()
        {
            AssetDatabase.Refresh();
        }
    }
}
