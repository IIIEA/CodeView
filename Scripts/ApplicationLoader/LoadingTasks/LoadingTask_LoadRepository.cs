using System;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.SaveLoadSystem;
using Asyncoroutine;
using UnityEngine;

namespace _Fly_Connect.Scripts.ApplicationLoader.LoadingTasks
{
    public sealed class LoadingTask_LoadRepository : ILoadingTask
    {
        private SaveLoadManager _saveLoadManager;
        private GameManager _gameManager;

        public float Weight { get; private set; } = 0.7f;

        [Inject]
        private void Construct(SaveLoadManager saveLoadManager, GameManager gameManager)
        {
            _gameManager = gameManager;
            _saveLoadManager = saveLoadManager;
        }

       async void ILoadingTask.Do(Action<LoadingResult> callback)
        {
            await new WaitForSeconds(0.5f);
            LoadingScreen.ReportProgress(Weight);
            _saveLoadManager.Load();
            _gameManager.StartGame();
            callback?.Invoke(LoadingResult.Success());
        }
    }
}