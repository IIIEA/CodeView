using System;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.Upgrades;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable]
    public class UFOSeller
    {
        [SerializeField] private CityUpgradeMPM _ufoFormula;
        [SerializeField] private float _multiplier = 0;
        [SerializeField] private float _time;
        [field:SerializeField] public float AdMultiplier { get; private set; }
        [field:SerializeField] public float TakenUFO { get; private set; }

        private IncomeCounter _incomeCounter;
        private UpgradePresenter _upgradePresenter;

        [Inject]
        private void Construct(IncomeCounter incomeCounter, UpgradePresenter upgradePresenter)
        {
            _upgradePresenter = upgradePresenter;
            _incomeCounter = incomeCounter;
        }

        public long GetCurrentPrice()
        {
            TakenUFO++;
            float percent = _upgradePresenter.GetUFOIncome();
            long reward = (long) _ufoFormula.Get(_incomeCounter.IncomePerMinute, _time, _multiplier);

            long percentAtNumber = (long)(reward * (percent / 100));   
            reward += percentAtNumber;
            return reward;
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