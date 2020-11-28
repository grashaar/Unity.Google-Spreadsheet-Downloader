using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Unity.GoogleSpreadsheetDownloader
{
    public partial class SheetName
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool isAdded = default;

#if  ODIN_INSPECTOR
        [Button, ShowIf(nameof(Validate))]
#endif
        private void Download()
            => SpreadsheetDownloader.DownloadSheet(this.name);

        private bool Validate()
            => this.isAdded && !string.IsNullOrWhiteSpace(this.name);
#endif
    }
}