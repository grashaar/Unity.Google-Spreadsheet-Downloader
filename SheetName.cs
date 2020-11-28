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
    public sealed partial class SheetName : IEquatable<SheetName>
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

        public SheetName() { }

        public SheetName(string name)
        {
            this.Name = name;
        }

        internal SheetName Clone()
            => new SheetName(this.name);

        public override int GetHashCode()
            => this.name.GetHashCode();

        public override bool Equals(object obj)
            => obj is SheetName other && string.Equals(this.name, other.name);

        public bool Equals(SheetName other)
            => other != null && string.Equals(this.name, other.name);

        public override string ToString()
            => this.name;

        public static implicit operator string(SheetName value)
            => value?.name ?? string.Empty;

        public static implicit operator SheetName(string value)
            => new SheetName(value);
    }
}