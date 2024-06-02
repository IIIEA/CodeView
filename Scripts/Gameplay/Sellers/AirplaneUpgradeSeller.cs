using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable]
    public class AirplaneUpgradeSeller
    {
        [SerializeField] private UpgradeAirplanePriceFormula _airplaneBuyFormula;
        [SerializeField] private float _exponent = 0;
        [SerializeField] private float _multiplier = 0;
        [field: SerializeField] public int OrderAirplaneUpgrade { get; private set; } = 1;

        public void Setup(int orderAirplaneBuyed)
        {
            OrderAirplaneUpgrade = orderAirplaneBuyed;
        }

        public void IncreaseOrderAirplaneBuyed()
        {
            OrderAirplaneUpgrade++;
        }

        public int GetCurrentPrice(int level)
        {
            return (int) (_airplaneBuyFormula.Get(OrderAirplaneUpgrade, _multiplier, _exponent) * (level * 0.1f + 1));
        }

        public bool CanBuy(BigNumber moneyStorageMoney, int level)
        {
            if (moneyStorageMoney.ToInt() >= GetCurrentPrice(level))
            {
                return true;
            }

            return false;
        }
    }
}