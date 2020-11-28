using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Unity.GoogleSpreadsheetDownloader
{
#if ODIN_INSPECTOR
    [InlineProperty(LabelWidth = 120)]
#endif
    [Serializable]
    public sealed class SheetDefinition
    {
        [SerializeField]
        private string gid = string.Empty;

        [SerializeField]
        private string customExtension = string.Empty;

        public string Gid
        {
            get => this.gid;
            set => this.gid = value ?? string.Empty;
        }

        public string CustomExtension
        {
            get => this.customExtension;
            set => this.customExtension = value ?? string.Empty;
        }

        public SheetDefinition() { }

        public SheetDefinition(string gid, string customExtension = null)
        {
            this.Gid = gid;
            this.CustomExtension = customExtension;
        }

        internal SheetDefinition Clone()
            => new SheetDefinition(this.gid, this.customExtension);
    }
}