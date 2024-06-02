using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using MAXHelper;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class CountryPresenter : ICountryPresenter
    {
        private readonly MoneyStorage _moneyStorage;
        private readonly CountrySeller _countrySeller;
        private readonly Country _country;
        private CameraController _cameraController;

        public string Title => _country.CountryData.Name;

        public string BuyButtonText =>
            new BigNumber(_countrySeller.GetCurrentPrice(_country.GameCities.Count)).ToString();

        public bool IsButtonInteractable =>
            _countrySeller.CanBuy(_moneyStorage.Money.ToLong(), _country.GameCities.Count) && _canShowReward;

        public bool IsShowTicketIcon => _moneyStorage.Ticket > 0;

        private bool _canShowReward = true;

        public event Action OnBuyButtonStateChanged;

        public CountryPresenter(MoneyStorage moneyStorage, CountrySeller countrySeller,
            Country country, CameraController cameraController)
        {
            _cameraController = cameraController;
            _moneyStorage = moneyStorage;
            _country = country;
            _countrySeller = countrySeller;
        }

        public void Enable()
        {
            _country.IsOpenPopup = true;
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
        }

        public void Disable()
        {
            _country.IsOpenPopup = false;
            _moneyStorage.OnMoneyChanged -= OnMoneyChanged;
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
                    AdsManager.ShowRewarded(_country.gameObject, OnFinishAds, "clicked_on_country");
                MusicManager.Pause();

                if (result != AdsManager.EResultCode.OK)
                {
                    _canShowReward = true;
                    MusicManager.Resume();
                }
            }
        }

        private void OnFinishAds(bool isSuccess)
        {
            if (isSuccess)
            {
                _country.ChangeMaterial(COUNTRY_MATERIAL.DEFAULT);
                _country.OpenCountry();
                _countrySeller.IncreaseOpenedCountry();
                _cameraController.MoveTo(_country.GetFirstCity().position);
                Disable();
                OnBuyButtonStateChanged?.Invoke();

                MadPixelAnalytics.AnalyticsManager.CustomEvent("country_unlock", new Dictionary<string, object>()
                {
                    {"name", _country.name},
                    {"countryCount", _countrySeller.CurrentOpenedCountry - 2},
                });
            }


            _canShowReward = true;
            MusicManager.Resume();
        }

        public void OnBuyButtonClicked()
        {
            _country.ChangeMaterial(COUNTRY_MATERIAL.DEFAULT);
            long currentPrice = _countrySeller.GetCurrentPrice(_country.GameCities.Count);
            _moneyStorage.SpendMoney(currentPrice);
            _country.OpenCountry();
            _countrySeller.IncreaseOpenedCountry();
            _cameraController.MoveTo(_country.GetFirstCity().position);
            Disable();
            OnBuyButtonStateChanged?.Invoke();

            MadPixelAnalytics.AnalyticsManager.CustomEvent("country_unlock", new Dictionary<string, object>()
            {
                {"name", _country.name},
                {"countryCount", _countrySeller.CurrentOpenedCountry - 2},
            });
        }

        public void OnExitButtonClicked()
        {
            _country.ChangeMaterial(COUNTRY_MATERIAL.DEFAULT);
            Disable();
        }

        private void OnMoneyChanged(BigNumber money)
        {
            OnBuyButtonStateChanged?.Invoke();
        }
    }
}