using System;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Tutorial;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable]
    public class AirportUpgradeSeller
    {
        [SerializeField] private CityUpgradeSuperDuperUltraFormula _airportUpgrade;
        [SerializeField] private float _multiplier = 0;
        [SerializeField] private float _time = 0;
        private IncomeCounter _incomeCounter;
        [field: SerializeField] public int OrderUpgradeAirport { get; private set; } = 1;
        [field: SerializeField] public int MaxLevelCityCount { get; private set; } = 0;
        [field: SerializeField] public int CityCount { get; private set; } = 0;

        [Inject]
        private void Construct(IncomeCounter incomeCounter)
        {
            _incomeCounter = incomeCounter;
        }

        public void Setup(int orderUpgradeAirport, int maxLevelCityCount, int cityCount)
        {
            OrderUpgradeAirport = orderUpgradeAirport;
            MaxLevelCityCount = maxLevelCityCount;
            CityCount = cityCount;
        }

        public long GetCurrentPrice(int level)
        {
            if (!TutorialManager.Instance.IsCompleted && _incomeCounter.IncomePerMinute == 0)
                return 20;

            return (long)_airportUpgrade.Get(_incomeCounter.IncomePerMinute, _time, level, _multiplier);
        }

        public void IncreaseUpgradeOrderAirport()
        {
            OrderUpgradeAirport++;
        }

        public void IncreaseMaxUpgradedCity()
        {
            MaxLevelCityCount++;
        }
        
        public bool CanBuy(BigNumber moneyStorageMoney, int level)
        {
            if (moneyStorageMoney.ToLong() >= GetCurrentPrice(level))
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

        public void IncreaseCityCount()
        {
            CityCount++;
        }
    }
}