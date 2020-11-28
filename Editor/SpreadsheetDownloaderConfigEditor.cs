using UnityEngine;
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
#endif

namespace Unity.GoogleSpreadsheetDownloader.Editor
{
    [CustomEditor(typeof(SpreadsheetDownloaderConfig))]
    public sealed class SpreadsheetDownloaderConfigEditor :
#if ODIN_INSPECTOR
        OdinEditor
#else
        UnityEditor.Editor
#endif
    {
        private SerializedProperty entries;
        private SpreadsheetDownloaderConfig config;
        private int total;

#if ODIN_INSPECTOR
        protected override void OnEnable()
        {
            base.OnEnable();
#else
        private void OnEnable()
        {
#endif
            this.config = this.target as SpreadsheetDownloaderConfig;
            this.entries = this.serializedObject.FindProperty("sheetDefinitions").FindPropertyRelative("entries");
        }

        public override void OnInspectorGUI()
        {
#if !ODIN_INSPECTOR
            EditorGUILayout.HelpBox("Odin Inspector is required.", MessageType.Warning);
#endif

            this.serializedObject.Update();

            ApplyIsAdded();

            EditorGUILayout.HelpBox("Google Spreadsheet document must be published to the web before using this tool.", MessageType.Info);

            GUILayout.Space(16);

            base.OnInspectorGUI();

            var directoryPath = SpreadsheetDownloader.GetDirectoryPath(this.config);
            EditorGUILayout.HelpBox($"{directoryPath}", MessageType.Info);

            GUILayout.Space(16);

            if (GUILayout.Button("Download All", GUILayout.Height(32)))
            {
                this.total = SpreadsheetDownloader.Download(this.config, UpdateDownloadProgress, FinishDownload);
                ShowProgressBar();
            }
        }

        private void ApplyIsAdded()
        {
            var changed = false;

            for (var i = 0; i < this.entries.arraySize; i++)
            {
                var elem = this.entries.GetArrayElementAtIndex(i);
                var key = elem.FindPropertyRelative("Key");
                var isAdded = key.FindPropertyRelative("isAdded");

                if (!isAdded.boolValue)
                {
                    isAdded.boolValue = true;
                    changed = true;
                }
            }

            if (changed)
            {
                this.serializedObject.ApplyModifiedProperties();
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