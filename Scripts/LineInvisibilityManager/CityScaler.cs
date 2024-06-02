using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Tutorial;
using DG.Tweening;
using Lean.Touch;
using Sirenix.Utilities;
using UnityEngine;

namespace _Fly_Connect.Scripts.LineInvisibilityManager
{
    public class CityScaler : IGameStartListener, IGameFinishListener, IGameUpdateListener
    {
        private List<CityPoint> _city = new();
        private Map _map;
        private LeanPinchCamera _leanPinchCamera;
        private float _previousZoom;
        private CountryService _countryService;
        private List<CityPoint> _cities = new();
        private CityShower _cityShower;

        [Inject]
        private void Construct(LeanPinchCamera leanPinchCamera, Map map, CountryService countryService, CityShower cityShower)
        {
            _cityShower = cityShower;
            _countryService = countryService;
            _leanPinchCamera = leanPinchCamera;
            _map = map;

            foreach (var country in countryService.Country)
            {
                foreach (var city in country.GameCities)
                {
                    _cities.Add(city);
                }
            }

        }

        public void OnFinishGame()
        {
        }


        public void OnStartGame()
        {
            _cities = _map.GetGameCities();
            OnUpdate(0);
            _previousZoom = _leanPinchCamera.Zoom;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_cityShower.IsPlayAnimation)
                return;

            if (!TutorialManager.Instance.IsCompleted)
                return;

            if (_previousZoom != _leanPinchCamera.Zoom)
            {
                _previousZoom = _leanPinchCamera.Zoom;

                if (_leanPinchCamera.Zoom >= 7 && _leanPinchCamera.Zoom <= 14)
                {
                    foreach (var country in _countryService.Country)
                    {
                        foreach (var city in country.GameCities)
                        {
                            if (city.gameObject.gameObject.activeSelf && city.CityUpLevelController != null)
                            {
                                if (city.CurrentAirportLevel == 4)
                                {
                                    if (city.ShowplaceTransform == null)
                                        return;

                                    if (city.ShowplaceTransform.transform.localPosition == city.ShowPlaceMaxSize)
                                        return;

                                    city.CityUpLevelController.EnabledObjects.ForEach(obj =>
                                        obj.gameObject.SetActive(false));

                                    var spriteRenderers = city.CityUpLevelController.EnabledObjects[^1]
                                        .GetComponentsInChildren<SpriteRenderer>(true);


                                    if (spriteRenderers.Length > 0)
                                    {
                                        foreach (var sprite in spriteRenderers)
                                        {
                                            sprite.gameObject.SetActive(false);
                                        }

                                        if (city.ShowplaceTransform.transform.localPosition != city.ShowPlaceMaxSize)
                                        {
                                            city.CityUpLevelController.EnabledObjects[^1].gameObject.SetActive(true);
                                            city.ShowplaceTransform.gameObject.SetActive(true);
                                            city.ShowplaceTransform.DOScale(city.ShowPlaceMaxSize, 0.2f);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (_leanPinchCamera.Zoom <= 7 && _leanPinchCamera.Zoom < 14)
                {
                    foreach (var country in _countryService.Country)
                    {
                        foreach (var city in country.GameCities)
                        {
                            if (city.gameObject.gameObject.activeSelf && city.CityUpLevelController != null)
                            {
                                if (city.CurrentAirportLevel == 4)
                                {
                                    if (city.CityUpLevelController != null)
                                    {
                                        if (city.CityUpLevelController.EnabledObjects.Count > 0)
                                        {
                                            city.CityUpLevelController.EnableObjects();
                                        }

                                        if (city.ShowplaceTransform != null)
                                        {
                                            if (city.ShowplaceTransform.localScale != city.StartShowplaceSize)
                                            {
                                                city.ShowplaceTransform.DOScale(city.StartShowplaceSize, 0.2f);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _previousZoom = _leanPinchCamera.Zoom;
        }
    }
}