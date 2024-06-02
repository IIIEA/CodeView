using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.SaveLoadSystem;
using Sirenix.Utilities;
using UniLinq;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [Serializable]
    public class Map : IGameStartListener, IGameFinishListener
    {
        private Country[] _countries;

        public List<int> OrderBuyedCountry { get; private set; } = new();

        private bool _canAddCountry;
        private bool _isSetupCalled;
        private bool IsFirstCountry;
        private AirplaneRouteFactory _airplaneRouteFactory;

        [Inject]
        public void Construct(CountryService countryService, AirplaneRouteFactory airplaneRouteFactory,
            AirplaneLaunchManager airplaneLaunchManager, GameRepository repository, CountrySeller countrySeller)
        {
            _airplaneRouteFactory = airplaneRouteFactory;
            _countries = countryService.Country;
            _countries.ForEach(country => { country.OnCountryOpened += OnCountryOpened; });
        }

        public void Setup(List<int> orderBuyedCountry)
        {
            _isSetupCalled = true;
            
            OrderBuyedCountry = orderBuyedCountry;

            foreach (var index in OrderBuyedCountry)
            {
                var country = _countries.FirstOrDefault(c => c.CountryData.Index == index);

                if (country != null)
                {
                    if (IsFirstCountry == false)
                    {
                        country.IsFirstOpened = true;
                        IsFirstCountry = true;
                    }

                    country.OpenCountry();

                    MeshMarker meshMarker = country.gameObject.GetComponentInChildren<MeshMarker>(true);

                    if (meshMarker != null)
                        meshMarker.gameObject.layer = (int) PHYSICS_LAYER.ACTIVE_COUNTRY;
                }
            }

            var gameCities = GetGameCities();

            foreach (var gameCity in gameCities)
            {
                if (gameCity.RouteIndexes != null)
                {
                    var routes = gameCity.RouteIndexes;

                    for (int i = 0; i < routes.Count; i++)
                    {
                        var destinationCity = gameCities.FirstOrDefault(сity => сity.Id == routes[i]);

                        _airplaneRouteFactory.GenerateRoute(gameCity, destinationCity);
                    }
                }

                gameCity.SetCurrentLevel(gameCity.SavedAirportLevel);
            }

            _canAddCountry = true;
        }

        private void OnCountryOpened(int index)
        {
            if (!_isSetupCalled)
                _canAddCountry = true;

            if (_canAddCountry == false)
                return;

            OrderBuyedCountry.Add(index);
        }

        public void OnFinishGame()
        {
            _countries.ForEach(country => country.OnCountryOpened -= OnCountryOpened);
        }

        private List<CityPoint> GetAllBuyedCities()
        {
            List<CityPoint> allBuyedCities = new List<CityPoint>();

            foreach (var country in _countries)
            {
                foreach (var city in country.BuyedCities)
                {
                    allBuyedCities.Add(city);
                }
            }

            return allBuyedCities;
        }

        public CityPoint FindClosestCity(CityPoint cityPoint)
        {
            List<CityPoint> buyedCities = GetAllBuyedCities();

            CityPoint closestCity = null;
            float closestDistanceSquared = float.MaxValue;

            foreach (var city in buyedCities)
            {
                if (city != cityPoint)
                {
                    float distanceSquared = (cityPoint.transform.position - city.transform.position).sqrMagnitude;

                    if (distanceSquared < closestDistanceSquared)
                    {
                        closestDistanceSquared = distanceSquared;
                        closestCity = city;
                    }
                }
            }

            return closestCity;
        }

        public List<CityPoint> GetGameCities()
        {
            List<CityPoint> allGameCities = new List<CityPoint>();

            foreach (var country in _countries)
            {
                allGameCities.AddRange(country.GameCities);
            }

            return allGameCities;
        }

        public void OnStartGame()
        {
          
        }

        // private void OnIncreaseOpenedCountry()
        // {
        //     foreach (var country in _countries)
        //     {
        //         if (country.GameCities.Count == 1)
        //         {
        //             _countryWithOneCity.Add(country);
        //             country.ChangeMaterial(COUNTRY_MATERIAL.DEFAULT);
        //             country.EnableTitle();
        //             country.EnableCollider();
        //         }
        //     }
        //
        //     _countrySeller.OnIncreaseOpenedCountry -= OnIncreaseOpenedCountry;
        // }

        public List<CityPoint> GetBuyedCities()
        {
            List<CityPoint> allGameCities = new List<CityPoint>();

            foreach (var country in _countries)
            {
                allGameCities.AddRange(country.BuyedCities);
            }

            return allGameCities;
        }

        public List<CityPoint> GetOpenedCity()
        {
            List<CityPoint> allGameCities = new List<CityPoint>();

            foreach (var country in _countries)
            {
                if (country.IsBuy)
                    allGameCities.AddRange(country.BuyedCities);
            }

            return allGameCities;
        }

        public void EnableCity()
        {
            var gameCities = GetGameCities();

            if (gameCities.Count > 0)
            {
                bool active = gameCities[0].CityPointView.gameObject.activeSelf;

                if (active)
                {
                    foreach (var gameCity in gameCities)
                    {
                        if (gameCity.CityPointView != null  && !gameCity.IsNotHide)
                        {
                            gameCity.CityPointView.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        public void DisableCity()
        {
            var gameCities = GetGameCities();

            if (gameCities.Count > 0)
            {
                bool active = gameCities[0].CityPointView.gameObject.activeSelf;

                if (active == false)
                {
                    foreach (var gameCity in gameCities)
                    {
                        if (gameCity.CityPointView != null)
                        {
                            gameCity.CityPointView.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}