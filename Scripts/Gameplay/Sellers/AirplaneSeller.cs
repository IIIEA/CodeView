using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable]
    public class AirplaneSeller
    {
        [SerializeField] private BuyingAirplanePriceFormula _airplaneBuyFormula;
        [SerializeField] private float _exponent = 0;
        [SerializeField] private float _multiplier = 0;
        [field: SerializeField] public int OrderAirplaneBuyed { get; private set; } = 1;

        public void Setup(int orderAirplaneBuyed)
        {
            OrderAirplaneBuyed = orderAirplaneBuyed;
        }

        public void IncreaseOrderAirplaneBuyed()
        {
            OrderAirplaneBuyed++;
        }

        public int GetCurrentPrice()
        {
            return (int) _airplaneBuyFormula.Get(OrderAirplaneBuyed, _multiplier, _exponent);
        }

        public bool CanBuy(BigNumber moneyStorageMoney)
        {
            if (moneyStorageMoney >= GetCurrentPrice())
            {
                return true;
            }

            return false;
        }
    }
}