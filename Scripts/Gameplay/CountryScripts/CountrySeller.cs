using System;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Tutorial;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [Serializable]
    public class CountrySeller
    {
        [SerializeField] private CountrySellerMPM _countryConfig;
        [SerializeField] private float _multiplier = 0;

        private IncomeCounter _incomeCounter;
        // [SerializeField] private float _basePrice = 0;
        [field: SerializeField] public int CurrentOpenedCountry { get; private set; } = -1;

        [Inject]
        private void Construct(IncomeCounter incomeCounter)
        {
            _incomeCounter = incomeCounter;
        }

        public void Setup(int currentOpenedCountry)
        {
            CurrentOpenedCountry = currentOpenedCountry;
        }

        public long GetCurrentPrice(int cityInCountry)
        {
            if (_incomeCounter.IncomePerMinute == 0 && !TutorialManager.Instance.IsCompleted)
                return 25;

            return (long) Mathf.Abs(_countryConfig.Get(_incomeCounter.IncomePerMinute, cityInCountry, _multiplier));
        }

        public void IncreaseOpenedCountry()
        {
            CurrentOpenedCountry++;
        }

        public bool CanBuy(long moneyStorageMoney, int cityInCountry)
        {
            var price =  GetCurrentPrice(cityInCountry);

            if (moneyStorageMoney >= price)
            {
                return true;
            }

            return false;
        }

        public void SetMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }

        public void SetBasePrice(float basePrice)
        {
            // _exponent = basePrice;
        }
    }
}