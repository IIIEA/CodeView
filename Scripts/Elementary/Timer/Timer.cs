using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Elementary.Timer
{
    [Serializable]
    public sealed class Timer : ITimer
    {
        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-10)]
        [PropertySpace(8)]
        public bool IsPlaying { get; private set; }

        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-9)]
        [ProgressBar(0, 1)]
        public float Progress
        {
            get => _currentTime / _duration;
            set => SetProgress(value);
        }

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-8)]
        public float CurrentTime
        {
            get => _currentTime;
            set => _currentTime = Mathf.Clamp(value, 0, this._duration);
        }

        [Space] [SerializeField]
        private float _duration;

        private float _currentTime;
        private Coroutine _coroutine;
        private ICoroutineRunner _coroutineRunner;

        public event Action OnStarted;
        public event Action OnTimeChanged;
        public event Action OnCanceled;
        public event Action OnFinished;
        public event Action OnReset;

        public Timer(float duration, ICoroutineRunner coroutineRunner)
        {
            _duration = duration;
            _coroutineRunner = coroutineRunner;
            _currentTime = 0;
        }

        public void Play()
        {
            if (IsPlaying)
            {
                return;
            }

            IsPlaying = true;
            OnStarted?.Invoke();
            _coroutine = _coroutineRunner.StartCoroutine(TimerRoutine());
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                _coroutineRunner.StopCoroutine(_coroutine);
            }

            if (IsPlaying)
            {
                IsPlaying = false;
                OnCanceled?.Invoke();
            }
        }

        public void ResetTime()
        {
            _currentTime = 0;
            OnReset?.Invoke();
        }

        private void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            _currentTime = _duration * progress;
            OnTimeChanged?.Invoke();
        }

        public IEnumerator TimerRoutine()
        {
            while (_currentTime < _duration)
            {
                yield return null;
                _currentTime += Time.deltaTime;
                OnTimeChanged?.Invoke();
            }

            IsPlaying = false;
            OnFinished?.Invoke();
        }
    }
}