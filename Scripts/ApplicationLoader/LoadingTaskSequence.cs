using System;
using System.Collections.Generic;

namespace _Fly_Connect.Scripts.ApplicationLoader
{
    public class LoadingTaskSequence : ILoadingTask
    {
        private readonly IReadOnlyList<ILoadingTask> _childrens;

        private Action<LoadingResult> _callback;
        private int _pointer;

        public float Weight { get; }

        public LoadingTaskSequence(IReadOnlyList<ILoadingTask> children)
        {
            _childrens = children;
        }

        public void Do(Action<LoadingResult> callback)
        {
            _pointer = 0;
            _callback = callback;
           _childrens[_pointer].Do(OnTaskCompleted);
        }

        private void OnTaskCompleted(LoadingResult result)
        {
            if (result.IsSuccess)
            {
                _callback.Invoke(result);
                return;
            }

            _pointer++;

            if(_pointer >= _childrens.Count)
                _callback.Invoke(LoadingResult.Success());
            else
                _childrens[_pointer].Do(OnTaskCompleted);
        }
    }
}
