using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable]
    public sealed class Countdown : ICountdown
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
            get => 1 - _remainingTime / _duration;
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
        public float RemainingTime
        {
            get => _remainingTime;
            set => _remainingTime = Mathf.Clamp(value, 0, _duration);
        }

        [Space] [SerializeField] private float _duration;

        private float _remainingTime;
        private Coroutine _coroutine;
        private ICoroutineRunner _coroutineRunner;

        public event Action OnStarted;
        public event Action OnTimeChanged;
        public event Action OnStopped;
        public event Action OnEnded;
        public event Action OnReset;

        public Countdown(float timerDuration, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _duration = timerDuration;
            _remainingTime = _duration;
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
                _coroutine = null;
            }

            if (IsPlaying)
            {
                IsPlaying = false;
                OnStopped?.Invoke();
            }
        }

        public void ResetTime()
        {
            _remainingTime = _duration;
            OnReset?.Invoke();
        }

        private IEnumerator TimerRoutine()
        {
            while (_remainingTime > 0)
            {
                yield return null;
                _remainingTime -= Time.deltaTime;
                OnTimeChanged?.Invoke();
            }

            IsPlaying = false;
            OnEnded?.Invoke();
        }

        private void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            _remainingTime = _duration * (1 - progress);
            OnTimeChanged?.Invoke();
        }
    }
}