using System;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace _Fly_Connect.Scripts.TicketManager
{
    [Serializable]
    public sealed class TicketManager : IGameStartListener, IGameFinishListener
    {
        private const int MAX_REWARD_COUNT = 4;

        [SerializeField] private Countdown _timer;

        [ShowInInspector] public int CurrentCollectedReward { get; private set; }
        [ShowInInspector] public bool CanGetReward { get; private set; }

        private ICoroutineRunner _coroutineRunner;
        private MoneyStorage _moneyStorage;
        private IncomeCounter _incomeCounter;
        private float _remainingTime;
        private bool _isWork = true;
        private PurchasesManager _purchasesManager;
        private bool _wasSaveLoad;

        public Countdown Timer => _timer;
        public bool HasReward => CurrentCollectedReward != MAX_REWARD_COUNT;

        public event Action OnEnd;

        [Inject]
        private void Inject(ICoroutineRunner coroutineRunner, MoneyStorage moneyStorage, IncomeCounter incomeCounter,
            PurchasesManager purchasesManager)
        {
            _purchasesManager = purchasesManager;
            _incomeCounter = incomeCounter;
            _moneyStorage = moneyStorage;
            _coroutineRunner = coroutineRunner;
        }

        public void Setup(int collectedReward, bool canGetReward)
        {
            CurrentCollectedReward = collectedReward;
            CanGetReward = canGetReward;

            if (PlayerPrefs.HasKey("LastSession"))
            {
                var lastSession = DateTime.Parse(PlayerPrefs.GetString("LastSession"));

                // Debug.Log($"Выгрузил время {lastSession.Hour}:{lastSession.Minute}:{lastSession.Second}");

                var lastTime = DateTime.Now - lastSession;

                // Debug.Log($"Вычислил разницу времени {lastTime.Days}:{lastTime.Hours}:{lastTime.Minutes}:{lastTime.Seconds}");

                // Debug.Log(lastTime.TotalSeconds + " всего секунд");
                var remainingTime = _timer.Duration - lastTime.TotalSeconds;
                // Debug.Log(remainingTime + " оставшееся время");


                _remainingTime = (float) remainingTime;
            }
        }

        public void OnStartGame()
        {
            _timer = new Countdown(_timer.Duration, _coroutineRunner);

            _timer.OnEnded += OnTimerEnded;

            PlayTimer();

            if (_remainingTime <= 0)
            {
                OnEnd?.Invoke();
            }
        }

        public void OnFinishGame()
        {
            _timer.OnEnded -= OnTimerEnded;
        }

        private void PlayTimer()
        {
            if (HasReward && CanGetReward == false)
            {
                _timer.Play();
            }
            else if (HasReward == false && CanGetReward == false)
            {
                _timer.Play();
            }

            if (_isWork == true && PlayerPrefs.HasKey("LastSession"))
            {
                _isWork = false;
                _timer.RemainingTime = _remainingTime;
                // Debug.Log($"Установил оставшееся время {_remainingTime}");

                if (_remainingTime <= 0)
                {
                    OnEnd?.Invoke();
                }
            }
        }

        private void OnTimerEnded()
        {
            CanGetReward = true;
        }

        [Button]
        public void AddReward()
        {
            if (HasReward)
            {
                AwardReward();
                CurrentCollectedReward++;
                CanGetReward = false;

                _timer.ResetTime();
                PlayTimer();

                if (HasReward == false)
                {
                    ClearRewards();
                }
            }
        }

        private void AwardReward()
        {
            var income = _incomeCounter.IncomePerMinute;

            if (CurrentCollectedReward == 0)
            {
                _moneyStorage.EarnMoney(new BigNumber(income * 2));
            }
            else if (CurrentCollectedReward == 1)
            {
                _moneyStorage.EarnMoney(new BigNumber(income * 4));
            }
            else if (CurrentCollectedReward == 2)
            {
                _moneyStorage.EarnMoney(new BigNumber(income * 6));
            }
            else if (CurrentCollectedReward == 3)
            {
                _moneyStorage.EarnTicket(1);
            }
        }

        [Button]
        private void ClearRewards()
        {
            CurrentCollectedReward = 0;
            CanGetReward = false;
            PlayTimer();
        }
    }
}