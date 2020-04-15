using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Unity.GoogleSpreadsheet
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
        public string Name = string.Empty;

        public override int GetHashCode()
            => this.Name.GetHashCode();

        public override bool Equals(object obj)
            => obj is SheetName other ? string.Equals(this.Name, other.Name) : false;

        public bool Equals(SheetName other)
            => other != null ? string.Equals(this.Name, other.Name) : false;

#if UNITY_EDITOR
        [HideInInspector]
        [SerializeField]
        private bool isAdded;

#if  ODIN_INSPECTOR
        [Button, ShowIf(nameof(isAdded))]
#endif
        internal void Download()
        {
            GoogleSpreadsheetHelper.Download(this.Name);
        }
#endif

        public static implicit operator string(SheetName value)
            => value?.Name ?? string.Empty;

        public static implicit operator SheetName(string value)
            => new SheetName() { Name = value };
    }
}