using System;
using JetBrains.Annotations;

namespace _Fly_Connect.Scripts.ApplicationLoader.LoadingTasks
{
    [UsedImplicitly]
    public sealed class LoadingTask_SaveData : ILoadingTask
    {
        public float Weight { get; private set; } = 0.7f;

        public void Do(Action<LoadingResult> callback)
        {
            callback.Invoke(LoadingResult.Success());
        }
    }
}