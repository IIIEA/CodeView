using System;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable]
    public class ShowplaceRewardSeller
    {
        [SerializeField] private CityUpgradeMPM _showplaceRewardFormula;
        [SerializeField] private float _multiplier = 0;
        [SerializeField] private float _time = 0;

        private AirplaneSeller _airplaneSeller;
        private IncomeCounter _incomeCounter;

        [Inject]
        private void Construct(AirplaneSeller airplaneSeller, IncomeCounter incomeCounter)
        {
            _incomeCounter = incomeCounter;
            _airplaneSeller = airplaneSeller;
        }

        public long GetCurrentPrice()
        {
            return (long) _showplaceRewardFormula.Get(_incomeCounter.IncomePerMinute, _time, _multiplier);
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