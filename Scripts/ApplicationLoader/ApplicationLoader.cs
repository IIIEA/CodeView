using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Infrastructure.Locator;
using UnityEngine;

namespace _Fly_Connect.Scripts.ApplicationLoader
{
    public sealed class ApplicationLoader : MonoBehaviour
    {
        [SerializeField] private bool _isLoadOnStart;
        [SerializeField] private ServiceLocator _serviceLocator;
        [SerializeField] private LoadingPipeline _pipeline;

        private List<ILoadingTask> _loadingTasks = new();

        public Action<float> OnProgressChanged;
        public event Action OnLoadingCompleted;
        public event Action<string> OnLoadingFailed;

        private void Start()
        {
            if (_isLoadOnStart)
            {
                LoadApplication();
            }
        }

        private async void LoadApplication()
        {
            float sum = 0;
            float acc = 0;

            LoadingScreen.Show();
            CreateTasks(_pipeline.GetTaskList());

            foreach (ILoadingTask task in _loadingTasks)
            {
                var tcs = new TaskCompletionSource<LoadingResult>();
                task.Do(result => tcs.TrySetResult(result));

                LoadingResult result = await tcs.Task;

                if (!result.IsSuccess)
                {
                    LoadingScreen.ReportError(result.Error);
                    return;
                }
            }

            LoadingScreen.Hide();
            OnLoadingCompleted?.Invoke();
        }

        private void CreateTasks(Type[] tasks)
        {
            foreach (Type taskType in tasks)
            {
                var instance = Activator.CreateInstance(taskType);
                ILoadingTask task = (ILoadingTask) instance;
                DependencyInjector.Inject(instance, _serviceLocator);
                _loadingTasks.Add(task);
            }
        }
    }
}