using System;
using JetBrains.Annotations;
using Asyncoroutine;
using UnityEngine;

namespace _Fly_Connect.Scripts.ApplicationLoader.LoadingTasks
{
    [UsedImplicitly]
    public sealed class LoadingTask_Vibration : ILoadingTask
    {
        public float Weight { get; private set; } = 0.3f;

        public async void Do(Action<LoadingResult> callback)
        {
            await new WaitForSeconds(0.5f);
            LoadingScreen.ReportProgress(Weight);                         
            callback.Invoke(LoadingResult.Success());
        }
    }
}