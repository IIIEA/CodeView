using System;
using Asyncoroutine;
using JetBrains.Annotations;
using UnityEngine;

namespace _Fly_Connect.Scripts.ApplicationLoader.LoadingTasks
{
    [UsedImplicitly]
    public sealed class LoadingTask_Stub : ILoadingTask
    {
        public float Weight { get; private set; } = 0.95f;

        public async void Do(Action<LoadingResult> callback)
        {
            await new WaitForSeconds(1f);
            LoadingScreen.ReportProgress(Weight);
            callback.Invoke(LoadingResult.Success());
        }
    }
}