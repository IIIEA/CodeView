using System;
using System.Collections;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Extensions;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using Lean.Touch;
using UniLinq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Fly_Connect.Scripts.ActionPopupSystem
{
    [Serializable]
    public sealed class ActionPopupManager : IGameStartListener, IGameFinishListener, IGameUpdateListener
    {
        private List<CityPoint> _gameCities = new();
        private List<CityPoint> _upgradedCities = new();
        private ICoroutineRunner _coroutineRunner;
        private MoneyStorage _moneyStorage;
        private ActionPopupPool _actionPopupPool;
        private Map _map;
        private int _openedPopupCount = 0;
        private int _openedPopupMax = 3;
        private float _timeBetweenClosedPopup = 10;
        private float _timeBetweenShowPopup = 60;
        private float _timeBetweenOpenPopup = 10f;
        private int _buyedCitiesCount;
        private ShowplaceRewardSeller _showplaceRewardSeller;
        private LeanPinchCamera _leanPinchCamera;
        private float _previousZoom;
        private List<ActionPopupView> _cachedViews = new();

        [Inject]
        private void Construct(MoneyStorage moneyStorage, ICoroutineRunner coroutineRunner,
            ActionPopupPool actionPopupPool, Map map, ShowplaceRewardSeller showplaceRewardSeller,
            LeanPinchCamera leanPinchCamera)
        {
            _leanPinchCamera = leanPinchCamera;
            _showplaceRewardSeller = showplaceRewardSeller;
            _map = map;
            _actionPopupPool = actionPopupPool;
            _coroutineRunner = coroutineRunner;
            _moneyStorage = moneyStorage;
        }

        public void OnStartGame()
        {
            _gameCities = _map.GetGameCities();

            foreach (var gameCity in _gameCities)
            {
                gameCity.OnCityUpgrade += OnCityUpgrade;
            }

            _upgradedCities = _gameCities.Where(city => city.CurrentAirportLevel == city.MaxAirportLevel).ToList();
            _previousZoom = _leanPinchCamera.Zoom;
            _coroutineRunner.StartCoroutine(ShowPopupRoutine());
        }

        public void OnFinishGame()
        {
            foreach (var gameCity in _gameCities)
            {
                gameCity.OnCityUpgrade -= OnCityUpgrade;
            }
        }

        private void OnCityUpgrade(CityPoint cityPoint)
        {
            if (cityPoint.CurrentAirportLevel == cityPoint.MaxAirportLevel)
            {
                _upgradedCities.Add(cityPoint);
            }

            if (cityPoint.CurrentAirportLevel == 1)
            {
                _buyedCitiesCount++;

                if (_buyedCitiesCount % 20 == 0)
                {
                    _openedPopupMax++;
                }
            }
        }

        private CityPoint GetCity()
        {
            List<CityPoint> cities = new();

            for (var i = 0; i < _upgradedCities.Count; i++)
            {
                if (_upgradedCities[i].ShowActionPopup == false)
                    cities.Add(_upgradedCities[i]);
            }

            int randomIndex = Random.Range(0, cities.Count);
            return cities[randomIndex];
        }

        private IEnumerator ShowPopupRoutine()
        {
            while (true)
            {
                if (_upgradedCities.Count != 0)
                {
                    for (int i = 0; i < _upgradedCities.Count; i++)
                    {
                        if (_openedPopupCount < _openedPopupMax)
                        {
                            _openedPopupCount++;
                            var city = GetCity();
                            var view = _actionPopupPool.Get();
                            view.Enable(city);
                            var offset = new Vector3(0, 0.1f, -0.5f);
                            view.transform.position = city.CityPointView.transform.position + offset;
                            view.OnButtonClick += OnButtonClicked;
                            float scale = Remap.DoRemap(_leanPinchCamera.ClampMax, _leanPinchCamera.ClampMin, 4, 0.3f,
                                _leanPinchCamera.Zoom);
                            view.View.transform.localScale = new Vector3(scale, scale, 1);
                            _cachedViews.Add(view);
                            _coroutineRunner.StartCoroutine(DisablePopup(view, _timeBetweenClosedPopup, true));
                            yield return new WaitForSeconds(_timeBetweenOpenPopup);
                        }
                    }
                }

                yield return new WaitForSeconds(_timeBetweenShowPopup);
            }
        }

        private void OnButtonClicked(ActionPopupView view)
        {
            _coroutineRunner.StartCoroutine(DisablePopup(view));
        }

        private IEnumerator DisablePopup(ActionPopupView actionPopupView, float time = 0, bool isAutoclose = false)
        {
            yield return new WaitForSeconds(time);
            var money = _showplaceRewardSeller.GetCurrentPrice();
            BigNumber bigNumber = new BigNumber(money);

            if (actionPopupView.IsActive)
            {
                if (!isAutoclose)
                {
                    _moneyStorage.EarnMoney(bigNumber);
                }

                actionPopupView.Disable(money.ToString(), isAutoclose);
                actionPopupView.OnButtonClick -= OnButtonClicked;
                _cachedViews.Remove(actionPopupView);
                _actionPopupPool.Release(actionPopupView);
                _openedPopupCount--;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (_previousZoom != _leanPinchCamera.Zoom)
            {
                foreach (var view in _cachedViews)
                {
                    if (view.gameObject.activeSelf)
                    {
                        float scale = Remap.DoRemap(_leanPinchCamera.ClampMax, _leanPinchCamera.ClampMin, 4, 0.3f,
                            _leanPinchCamera.Zoom);
                        view.View.transform.localScale = new Vector3(scale, scale, 1);
                    }

                    _previousZoom = _leanPinchCamera.Zoom;
                }
            }
        }
    }
}