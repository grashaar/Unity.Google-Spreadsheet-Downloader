using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Unity.GoogleSpreadsheetDownloader
{
#if ODIN_INSPECTOR
    [InlineProperty]
#endif
    [Serializable]
    public sealed class SheetName : IEquatable<SheetName>
    {
#if ODIN_INSPECTOR
        [HideLabel]
#endif
        [SerializeField]
        private string name = string.Empty;

        public string Name
        {
            get => this.name;
            set => this.name = value ?? string.Empty;
        }

        public override int GetHashCode()
            => this.name.GetHashCode();

        public override bool Equals(object obj)
            => obj is SheetName other && string.Equals(this.name, other.name);

        public bool Equals(SheetName other)
            => other != null && string.Equals(this.name, other.name);

        public override string ToString()
            => this.name;

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

        public static implicit operator string(SheetName value)
            => value?.name ?? string.Empty;

        public static implicit operator SheetName(string value)
            => new SheetName() { Name = value };
    }
}