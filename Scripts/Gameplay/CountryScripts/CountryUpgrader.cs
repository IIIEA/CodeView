using System;
using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [Serializable]
    public class CountryUpgrader
    {
        private readonly List<CityPoint> _gameCities;
        private readonly HashSet<CityPoint> _buyedCities;
        
        private int _purchasedUpgradeCount = 0;
        private int _openedCity = 0;

        public CountryUpgrader (List<CityPoint> gameCities, HashSet<CityPoint> buyedCities)
        {
            _gameCities = gameCities;
            _buyedCities = buyedCities;
            _gameCities.ForEach(city => city.OnCityUpgrade += LevelUp);
        } 
        
        ~CountryUpgrader()
        {
            _gameCities.ForEach(city => city.OnCityUpgrade -= LevelUp);
        }

        public void Setup(int openedCountry, int purchasedUpgradeCount)
        {
            _purchasedUpgradeCount = openedCountry;
            _purchasedUpgradeCount = purchasedUpgradeCount;
        }
        
        public void GetPurchedUpgradeCount()
        {
            _purchasedUpgradeCount = _buyedCities.Sum(city => city.CurrentAirportLevel);
        }
        
        private void LevelUp(CityPoint cityPoint)
        {
            _purchasedUpgradeCount = _buyedCities.Sum(city => city.CurrentAirportLevel);
            _buyedCities.Add(cityPoint);
            TryEnableCities();
        }
        
        private void TryEnableCities()
        {
            _openedCity = Mathf.Min((_purchasedUpgradeCount + 1) / 3 * 3 + 2, _gameCities.Count);

            for (int i = 0; i < _gameCities.Count; i++)
            {
                _gameCities[i].gameObject.SetActive(i < _openedCity);
            }
        }
    }
}