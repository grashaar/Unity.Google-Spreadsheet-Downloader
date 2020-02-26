using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Unity.GoogleSpreadsheet
{
    [CreateAssetMenu(fileName = nameof(GoogleSpreadsheetConfig), menuName = "Google Spreadsheet Config", order = 1)]
    public sealed class GoogleSpreadsheetConfig : ScriptableObject
    {
        private const string _url = "https://docs.google.com/spreadsheets/d/e/{0}/pub?gid={1}&single=true&output={2}";

        [Space]
        [SerializeField]
        private string spreadsheetId = string.Empty;

        public string SpreadsheetId
            => this.spreadsheetId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(formats))]
#endif
        [SerializeField]
        private FormatType format = FormatType.csv;

        public FormatType Format
            => this.format;

#if ODIN_INSPECTOR
        [DictionaryDrawerSettings(KeyLabel = "File Name", ValueLabel = "Sheet Definition")]
#endif
        [Space]
        [SerializeField]
        private SheetDictionary sheetDefinitions = new SheetDictionary();

        public IReadOnlyDictionary<string, SheetDefinition> SheetGids
            => this.sheetDefinitions;

        [Space]
        [SerializeField]
        private string downloadFolder = "GoogleSpreadsheet";

        public string DownloadFolder
            => this.downloadFolder;

        public string GetDownloadUrl(string gid)
            => string.Format(_url, this.spreadsheetId, gid, this.format);

#if ODIN_INSPECTOR
        private readonly ValueDropdownList<FormatType> formats = new ValueDropdownList<FormatType> {
            { ".csv"  , FormatType.csv  },
            { ".tsv"  , FormatType.tsv  },
            { ".xlsx" , FormatType.xlsx },
            { ".ods"  , FormatType.ods  },
        };
#endif

        [Serializable]
        private sealed class SheetDictionary : SerializableDictionary<string, SheetDefinition> { }
    }

    public enum FormatType
    {
        csv, tsv, xlsx, ods
    }
}
