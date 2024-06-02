using System;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Reward;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable, InlineProperty]
    public class MoneyBoosterModel
    {
        [SerializeField] private Countdown _timer;
        private ICoroutineRunner _coroutineRunner;
        private RewardManager _rewardManager;

        public event Action OnFinish;
        public event Action<float> OnTimerChanged;

        [Inject]
        private void Construct(ICoroutineRunner coroutineRunner, RewardManager rewardManager)
        {
            _rewardManager = rewardManager;
            _coroutineRunner = coroutineRunner;
            _timer = new Countdown(_timer.Duration,_coroutineRunner);
        }

        public void StartTimer()
        {
            _timer.Play();
            _timer.OnTimeChanged += OnTimeChanged;
            _timer.OnEnded += OnTimerFinish;
            _rewardManager.EnableX2Money();
        }

        private void OnTimeChanged()
        {
            OnTimerChanged?.Invoke(_timer.RemainingTime);
        }

        private void OnTimerFinish()
        {
            OnFinish?.Invoke();
            _timer.ResetTime();
            _timer.OnTimeChanged -= OnTimeChanged;
            _timer.OnEnded -= OnTimerFinish;
            _rewardManager.DisableX2Money();
        }
    }
}