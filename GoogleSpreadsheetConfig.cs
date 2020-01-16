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
        [ValueDropdown(nameof(fileTypes))]
#endif
        [SerializeField]
        private FileType fileType = FileType.csv;

        public FileType FileType
            => this.fileType;

#if ODIN_INSPECTOR
        [DictionaryDrawerSettings(KeyLabel = "Sheet Name", ValueLabel = "Gid")]
#endif
        [Space]
        [SerializeField]
        private StringDictionary sheetGids = new StringDictionary();

        public IReadOnlyDictionary<string, string> SheetGids
            => this.sheetGids;

        [Space]
        [SerializeField]
        private string downloadFolder = "GoogleSpreadsheet";

        public string DownloadFolder
            => this.downloadFolder;

        public string GetDownloadUrl(string gid)
            => string.Format(_url, this.spreadsheetId, gid, this.fileType);

#if ODIN_INSPECTOR
        private readonly ValueDropdownList<FileType> fileTypes = new ValueDropdownList<FileType> {
            { ".csv"  , FileType.csv  },
            { ".tsv"  , FileType.tsv  },
            { ".xlsx" , FileType.xlsx },
            { ".ods"  , FileType.ods  },
        };
#endif

        [Serializable]
        private sealed class StringDictionary : SerializableDictionary<string, string> { }
    }

    public enum FileType
    {
        csv, tsv, xlsx, ods
    }
}
