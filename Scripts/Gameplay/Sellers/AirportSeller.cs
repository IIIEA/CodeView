using System;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Tutorial;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable]
    public class AirportSeller
    {
        [SerializeField] private CityUpgradeMPM _cityConfig;
        [SerializeField] private float _multiplier = 0;
        [SerializeField] private float _time = 0;
        [field: SerializeField] public int CurrentOpenedCity { get; private set; }

        private IncomeCounter _incomeCounter;

        [Inject]
        private void Construct( IncomeCounter incomeCounter)
        {
            _incomeCounter = incomeCounter;
        }

        public void Setup(int currentOpenedCountry)
        {
            CurrentOpenedCity = currentOpenedCountry;
        }

        public long GetCurrentPrice()
        {
            if (!TutorialManager.Instance.IsCompleted && _incomeCounter.IncomePerMinute == 0)
                return 10;

            return (long) _cityConfig.Get(_incomeCounter.IncomePerMinute, _time, _multiplier);
        }

        public void IncreaseOpenedCity()
        {
            CurrentOpenedCity++;
        }

        public bool CanBuy(long moneyStorageMoney)
        {
            long price = GetCurrentPrice();

            if (moneyStorageMoney >= price)
            {
                return true;
            }

            return false;
        }

        public void SetTime(float time)
        {
            _time = time;
        }

        public void SetMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }
    }
}