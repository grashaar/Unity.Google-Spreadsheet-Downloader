# Unity Google Spreadsheet Downloader

## Usage

- Publish your Google Spreadsheet to the web.
- Create a `SpreadsheetDownloaderConfig` asset via `Assets > Create > Spreadsheet Downloader Config` menu.
- Fill in all required information needed for the download process.

### Notes
- Spreadsheet Id in published link: `docs.google.com/spreadsheets/d/e/<SPREADSHEET_ID>/pubhtml`
- Sheet Gid in the URL: `docs.google.com/spreadsheets/d/<...>/edit#gid=<SHEET_GID>`

## Changelog

### 2.0.0
- Rename `GoogleSpreadsheetConfig` to `SpreadsheetDownloaderConfig`
- Rename `Unity.GoogleSpreadsheet` namespace to `Unity.GoogleSpreadsheetDownloader`
- Rename `GoogleSpreadsheetHelper` to `SpreadsheetDownloader`
- Change assembly definition name to `Unity.GoogleSpreadsheetDownloader`
- Remove Unity Supplements dependency

## Dependencies

- [UniTask 2.0.20+](https://openupm.com/packages/com.cysharp.unitask/)
- Automatically use `UniTask` instead of `Task` if the UniTask package is present.

## Editor

- Currently require [Odin Inspector](https://odininspector.com/) to properly show `SpreadsheetDownloaderConfig`.