using System;
using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CamerasScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using Lean.Touch;
using MAXHelper;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    public class AirportPresenter : IAirportPresenter
    {
        private AirportSeller _airportSeller;
        private AirportUpgradeSeller _airportUpgradeSeller;
        private CityPoint _city;
        private MoneyStorage _moneyStorage;
        private AirplaneLaunchManager _airplaneLaunchManager;
        private LeanPinchCamera _leanPinchCamera;
        private AirplaneSeller _airplaneSeller;
        private IncomeCounter _incomeCounter;
        private bool _canShowReward = true;
        public CameraController CameraController { get; private set; }
        public BuildingCameraService BuildingCameraService { get; private set; }
        public bool IsShowTicketIcon => _moneyStorage.Ticket > 0;

        public bool IsAirportButtonInteractable => _city.CurrentAirportLevel != _city.MaxAirportLevel;

        public string AirportLevelText => "Level " + _city.CurrentAirportLevel;
        public string AirportButtonText => _city.CurrentAirportLevel == 0 ? "Buy" : "Upgrade";
        public string AirportLevel => _city.CurrentAirportLevel.ToString();
        public string AirplaneCount => GetAirplaneCount();

        public string AirportPrice => GetPrice();

        public Transform PopupTransform => _city.CityPointView.transform;
        public Vector3 CityPosition => _city.CityPosition.transform.position;

        public bool IsRouteBuild => _city.Routes.Count > 0;

        public bool IsRouteRedTextRed => _city.Routes.Count != _city.CurrentAirportLevel;

        public string RouteText
        {
            get
            {
                if (_city == null)
                {
                    return 0.ToString();
                }

                return _city.Routes.Count + "/" + _city.CurrentAirportLevel;
            }
        }

        public Vector3 PopupScale => GetPopupScale();

        public event Action OnBuyButtonStateChanged;

        public event Action<bool> HasMoney;

        public AirportPresenter(CityPoint cityPoint, AirportSeller airportSeller,
            AirportUpgradeSeller airportUpgradeSeller, MoneyStorage moneyStorage, AirplaneSeller airplaneSeller,
            AirplaneLaunchManager airplaneLaunchManager, LeanPinchCamera leanPinchCamera,
            BuildingCameraService buildingCameraService, CameraController cameraController, IncomeCounter incomeCounter)
        {
            _incomeCounter = incomeCounter;
            CameraController = cameraController;
            _airplaneSeller = airplaneSeller;
            BuildingCameraService = buildingCameraService;
            _leanPinchCamera = leanPinchCamera;
            _airplaneLaunchManager = airplaneLaunchManager;
            _moneyStorage = moneyStorage;
            _airportSeller = airportSeller;
            _city = cityPoint;
            _airportUpgradeSeller = airportUpgradeSeller;
        }

        public void Enable()
        {
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
            CheckHasMoney();
            _city.CityUpLevelController.Enable();
        }

        public void Disable()
        {
            _moneyStorage.OnMoneyChanged -= OnMoneyChanged;
            _city.CityUpLevelController.Disable();
        }

        private string GetAirplaneCount()
        {
            if (_city.Routes.Count == 0)
            {
                return "0";
            }

            int airplaneCount = 0;

            foreach (var route in _city.Routes)
            {
                if (route.Airplane != null)
                {
                    airplaneCount++;
                }
            }

            return airplaneCount.ToString();
        }

        private void OnMoneyChanged(BigNumber _)
        {
            OnBuyButtonStateChanged?.Invoke();
            CheckHasMoney();
        }

        private Vector3 GetPopupScale()
        {
            float scale = Remap.DoRemap(_leanPinchCamera.ClampMax, _leanPinchCamera.ClampMin, 8, 0.4f,
                _leanPinchCamera.Zoom);

            return new Vector3(scale, scale, 1);
        }

        private void CheckHasMoney()
        {
            var airportPrice = _airportSeller.GetCurrentPrice();

            if (_city.CurrentAirportLevel == 0)
            {
                BigNumber bigNumber = new BigNumber(airportPrice);

                HasMoney?.Invoke(_moneyStorage.Money >= bigNumber);
            }
            else
            {
                var airportUpgradePrice = _airportUpgradeSeller.GetCurrentPrice(_city.CurrentAirportLevel);
                HasMoney?.Invoke(_moneyStorage.Money.ToLong() >= airportUpgradePrice);
            }
        }

        private string GetPrice()
        {
            var airportPrice = _airportSeller.GetCurrentPrice();

            if (_city.CurrentAirportLevel == 0)
            {
                var currentPrice = airportPrice;
                BigNumber price = new BigNumber(currentPrice);

                return price.ToString();
            }
            else
            {
                var airportUpgradePrice = _airportUpgradeSeller.GetCurrentPrice(_city.CurrentAirportLevel);

                BigNumber upgradePrice = new BigNumber(airportUpgradePrice);

                return upgradePrice.ToString();
            }
        }

        public void OnRewardButtonClicked()
        {
            if (_moneyStorage.Ticket > 0)
            {
                _moneyStorage.SpendTicket(1);
                OnFinishAds(true);
            }
            else
            {
                if (_canShowReward == false)
                    return;

                _canShowReward = false;

                AdsManager.EResultCode result =
                    AdsManager.ShowRewarded(_city.gameObject, OnFinishAds, "Airport_Popup");
                MusicManager.Pause();

                if (result != AdsManager.EResultCode.OK)
                {
                    MusicManager.Resume();
                    _canShowReward = true;
                }
            }

        }

        private void OnFinishAds(bool isSuccess)
        {
            if (isSuccess)
            {
                OnBuyAirportButtonClicked();
            }

            _canShowReward = true;
            MusicManager.Resume();
        }

        public void OnBuyAirportButtonClicked()
        {
            if (_city.CurrentAirportLevel == 0)
            {
                var price = _airportSeller.GetCurrentPrice();

                BigNumber bigNumber = new BigNumber(price);

                if (_moneyStorage.Money >= bigNumber)
                    _moneyStorage.SpendMoney(price);

                _airplaneSeller.IncreaseOrderAirplaneBuyed();
                _airportSeller.IncreaseOpenedCity();
            }
            else
            {
                var price = _airportUpgradeSeller.GetCurrentPrice(_city.CurrentAirportLevel);

                if (_moneyStorage.Money.ToLong() >= price)
                    _moneyStorage.SpendMoney(_airportUpgradeSeller.GetCurrentPrice(_city.CurrentAirportLevel));

                _airportUpgradeSeller.IncreaseUpgradeOrderAirport();
            }

            _city.LevelUp();

            if (_city.CurrentAirportLevel == 4)
                _airportUpgradeSeller.IncreaseMaxUpgradedCity();

            if (_city.CurrentAirportLevel == 1)
            {
                _airportUpgradeSeller.IncreaseCityCount();

                MadPixelAnalytics.AnalyticsManager.CustomEvent("unlock_city", new Dictionary<string, object>()
                {
                    {"name", _city.name},
                    {"maxLevelCityCount", _airportUpgradeSeller.MaxLevelCityCount},
                    {"cityCount", _airportUpgradeSeller.CityCount},
                });
            }

            _incomeCounter.CalculateIncome();

            OnBuyButtonStateChanged?.Invoke();
            CheckHasMoney();
        }
    }
}