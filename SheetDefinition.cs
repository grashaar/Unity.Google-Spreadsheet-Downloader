using System;

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
        public string Gid;
        public string CustomExtension;
    }
}