﻿using System;
using _Fly_Connect.Scripts.Tutorial;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable]
    public class AirportCapacityUpgradeSeller
    {
        [SerializeField] private PeopleCountUpgrade _upgradeConfig;
        [SerializeField] private float _exponent = 0;
        [SerializeField] private float _multiplier = 0;
        [field: SerializeField] public float AddedValue { get; private set; }

        public int GetCurrentPrice(int level)
        {
            if (level == 1)
            {
                return 100;
            }

            return (int) _upgradeConfig.Get(level, _multiplier, _exponent);
        }

        public bool CanBuy(BigNumber moneyStorageMoney, int level)
        {
            if (moneyStorageMoney >= GetCurrentPrice(level))
            {
                return true;
            }

            return false;
        }

        public void SetMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }

        public void SetExponent(float exponent)
        {
            _exponent = exponent;
        }
    }
}