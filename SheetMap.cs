using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.GoogleSpreadsheetDownloader
{
    [Serializable]
    public sealed class SheetMap : Dictionary<SheetName, SheetDefinition>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<Entry> entries = new List<Entry>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();

            foreach (var entry in this.entries)
            {
                if (ContainsKey(entry.Key))
                    continue;

                this[entry.Key] = entry.Value;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.entries.Clear();

            foreach (var kv in this)
            {
                var index = this.entries.FindIndex(x => string.Equals(x.Key?.Name, kv.Key?.Name));

                if (index >= 0)
                    continue;

                this.entries.Add(new Entry { Key = kv.Key.Clone(), Value = kv.Value.Clone() });
            }
        }

        public Entry[] ToArray()
            => this.entries.ToArray();

        [Serializable]
        public class Entry
        {
            public SheetName Key;
            public SheetDefinition Value;
        }
    }
}
