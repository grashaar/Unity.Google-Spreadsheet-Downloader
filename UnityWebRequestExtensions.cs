#if !UNITY_GOOGLE_SPREADSHEET_DOWNLOADER_UNITASK

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Unity.GoogleSpreadsheetDownloader
{
    public sealed class UnityWebRequestAwaiter : INotifyCompletion
    {
        private readonly UnityWebRequestAsyncOperation operation;
        private Action continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation operation)
        {
            this.operation = operation;
            operation.completed += OnRequestCompleted;
        }

        public bool IsCompleted { get { return operation.isDone; } }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }

        private void OnRequestCompleted(AsyncOperation operation)
        {
            this.continuation();
        }
    }

    public static class UnityWebRequestExtensions
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation operation)
        {
            return new UnityWebRequestAwaiter(operation);
        }
    }
}

#endif