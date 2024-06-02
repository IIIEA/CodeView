using System;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using Asyncoroutine;
using JetBrains.Annotations;
using UnityEngine;

namespace _Fly_Connect.Scripts.ApplicationLoader.LoadingTasks
{
    [UsedImplicitly]
    public sealed class LoadingTask_GameInit : ILoadingTask
    {
        private GameContext _gameContext;

        public float Weight { get; private set; } = 0.5f;

        [Inject]
        private void Construct(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public async void Do(Action<LoadingResult> callback)
        {
            await new WaitForSeconds(0.5f);
            LoadingScreen.ReportProgress(Weight);
            _gameContext.StartInject();
            callback.Invoke(LoadingResult.Success());
        }
    }
}