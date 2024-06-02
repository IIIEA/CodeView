using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Elementary.Timer;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.Upgrades;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Reward
{
    [Serializable]
    public sealed class RewardManager
    {
        [SerializeField] private float _multiplier;

        private float _incomeUpgradeLevel;
        private UpgradePresenter _upgradePresenter;
        private bool _isEnabledX2Money;

        public event Action<int, int, bool> OnRewardAdded;

        [Inject]
        private void Construct(UpgradePresenter upgradePresenter)
        {
            _upgradePresenter = upgradePresenter;
        }

        public (int, int) CalculateReward((int, int) cityLevels, Airplane airplane, bool isNeedMultiplier = true)
        {
            float currentDistance = airplane.GetRoute().Distance;
            _incomeUpgradeLevel = _upgradePresenter.GetLevelIncomeUpgrade() - 1;

            var cityLevel1 = cityLevels.Item1 * 2;
            var cityLevel2 = cityLevels.Item1 * 2;

            int rewardCity1 = (int) ((cityLevel1 * currentDistance) * (_incomeUpgradeLevel * 0.1f + 1));
            int rewardCity2 = (int) ((cityLevel2 * currentDistance) * (_incomeUpgradeLevel * 0.1f + 1));

            if (isNeedMultiplier)
                rewardCity1 = (int) (rewardCity1 * _multiplier);

            if (isNeedMultiplier)
                rewardCity2 = (int) (rewardCity2 * _multiplier);

            if (rewardCity1 <= 0)
                rewardCity1 = 1;

            if (rewardCity2 <= 0)
                rewardCity2 = 1;

            if (_isEnabledX2Money)
            {
                rewardCity1 *= 2;
                rewardCity2 *= 2;
            }

            OnRewardAdded?.Invoke(rewardCity1, rewardCity2, airplane.IsForwardDirection());

            return (rewardCity1, rewardCity2);
        }

        public void SetAddedMoneyOnAirplaneCapacity(float addedValue)
        {
            _incomeUpgradeLevel = addedValue;
        }

        public void EnableX2Money()
        {
            _isEnabledX2Money = true;
        }

        public void DisableX2Money()
        {
            _isEnabledX2Money = false;
        }

        public void SetTotalMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }
    }
}