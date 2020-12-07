using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Unity.GoogleSpreadsheetDownloader
{
    [CreateAssetMenu(fileName = nameof(SpreadsheetDownloaderConfig), menuName = "Spreadsheet Downloader Config", order = 1)]
    public sealed class SpreadsheetDownloaderConfig : ScriptableObject
    {
        private const string _url = "https://docs.google.com/spreadsheets/d/e/{0}/pub?gid={1}&single=true&output={2}";

        [Space]
        [SerializeField]
        private string spreadsheetId = string.Empty;

        public string SpreadsheetId
        {
            get => this.spreadsheetId;
            set => this.spreadsheetId = value ?? string.Empty;
        }

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(formats))]
#endif
        [SerializeField]
        private FormatType format = FormatType.csv;

        public FormatType Format
        {
            get => this.format;
            set => this.format = value;
        }

#if ODIN_INSPECTOR
        [DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "Definition")]
#endif
        [Space]
        [SerializeField]
        private SheetMap sheetDefinitions = new SheetMap();

        public SheetMap SheetGids
            => this.sheetDefinitions;

        [Space]
        [SerializeField]
        private string downloadFolder = "GoogleSpreadsheet";

        public string DownloadFolder
        {
            get => this.downloadFolder;
            set => this.downloadFolder = value ?? string.Empty;
        }

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
    }

    public enum FormatType
    {
        csv, tsv, xlsx, ods
    }
}
