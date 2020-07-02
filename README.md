# Unity Google Spreadsheet

## Notes

- Automatically switch to UniTask if the package is present.

## Usage

- Publish your Google Spreadsheet to the web.
- Create a `GoogleSpreadsheetConfig` asset via `Assets > Create > Google Spreadsheet
Config` menu.
- Fill in all required information needed for downloading sheets.

### Notes
- Spreadsheet Id in published link: `docs.google.com/spreadsheets/d/e/<SPREADSHEET_ID>/pubhtml`
- Sheet Gid in the URL: `docs.google.com/spreadsheets/d/<...>/edit#gid=<SHEET_GID>`

## Dependencies

- [Unity Supplements 1.0.0+](https://openupm.com/packages/com.laicasaane.unity-supplements/)
- [UniTask 2.0.20+](https://openupm.com/packages/com.cysharp.unitask/)

## Editor

- Currently require [Odin Inspector](https://odininspector.com/) to properly show `GoogleSpreadsheetConfig`.