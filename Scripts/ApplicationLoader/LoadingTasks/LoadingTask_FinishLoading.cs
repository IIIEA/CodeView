using System;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using Asyncoroutine;
using JetBrains.Annotations;
using UnityEngine;

namespace _Fly_Connect.Scripts.ApplicationLoader.LoadingTasks
{
    [UsedImplicitly]
    public sealed class LoadingTask_FinishLoading : ILoadingTask
    {
        public float Weight { get; private set; } = 1f;

        public async void Do(Action<LoadingResult> callback)
        {
            await new WaitForSeconds(1f);
            LoadingScreen.ReportProgress(Weight);
            callback.Invoke(LoadingResult.Success());
        }
    }
}