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
                this[entry.Key] = entry.Value;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.entries.Clear();

            foreach (var kv in this)
            {
                var index = this.entries.FindIndex(x => x.Key == kv.Key);

                if (index < 0)
                    this.entries.Add(new Entry { Key = kv.Key, Value = kv.Value });
            }
        }

        [Serializable]
        private class Entry
        {
            public SheetName Key;
            public SheetDefinition Value;
        }
    }
}
