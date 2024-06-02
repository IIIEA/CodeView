using System;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable, InlineProperty]
    public class SpeedBoosterModel
    {
        [SerializeField] private Countdown _timer;
        private ICoroutineRunner _coroutineRunner;
        private AirplaneLaunchManager _airplaneLaunchManager;

        public bool IsPlaying => _timer.IsPlaying;

        public event Action OnFinish;
        public event Action<float> OnTimerChanged;

        [Inject]
        private void Construct(ICoroutineRunner coroutineRunner, AirplaneLaunchManager airplaneLaunchManager,
            IncomeCounter incomeCounter)
        {
            _airplaneLaunchManager = airplaneLaunchManager;
            _coroutineRunner = coroutineRunner;
            _timer = new Countdown(_timer.Duration, _coroutineRunner);
        }

        public void StartTimer()
        {
            _timer.Play();
            _timer.OnTimeChanged += OnTimeChanged;
            _timer.OnEnded += OnTimerFinish;
            _airplaneLaunchManager.EnableBoosterSpeed();
        }

        private void OnTimeChanged()
        {
            OnTimerChanged?.Invoke(_timer.RemainingTime);
        }

        private void OnTimerFinish()
        {
            OnFinish?.Invoke();
            _airplaneLaunchManager.DisableBoosterSpeed();
            _timer.ResetTime();
            _timer.OnTimeChanged -= OnTimeChanged;
            _timer.OnEnded -= OnTimerFinish;
        }
    }
}