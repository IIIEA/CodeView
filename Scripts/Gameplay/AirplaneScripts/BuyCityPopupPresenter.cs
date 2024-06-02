using System;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using Lean.Touch;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public sealed class BuyCityPopupPresenter : IBuyCityPresenter
    {
        private MoneyStorage _moneyStorage;
        private AirportSeller _airportSeller;
        private LeanPinchCamera _leanPinchCamera;
        public Transform CityTransform { get; private set; }
        public string Price => GetPrice();

        private string GetPrice()
        {
            return new BigNumber(_airportSeller.GetCurrentPrice()).ToString();
        }

        public bool HasMoney => _airportSeller.CanBuy(_moneyStorage.Money.ToLong());

        public event Action OnBuyButtonStateChanged;

        public BuyCityPopupPresenter(MoneyStorage moneyStorage, AirportSeller airportSeller, Transform cityTransform,
            LeanPinchCamera leanPinchCamera)
        {
            _leanPinchCamera = leanPinchCamera;
            CityTransform = cityTransform;
            _airportSeller = airportSeller;
            _moneyStorage = moneyStorage;
            CityTransform = cityTransform;
        }

        public Vector3 GetPopupScale()
        {
            float scale = Remap.DoRemap(_leanPinchCamera.ClampMax, _leanPinchCamera.ClampMin, 8, 0.4f,
                _leanPinchCamera.Zoom);

            return new Vector3(scale, scale, 1);
        }
    }
}