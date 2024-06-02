using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Infrastructure.Locator;
using _Fly_Connect.Scripts.SaveLoadSystem;
using Sirenix.Utilities;
using UniLinq;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class MapInstaller : GameInstaller
    {
        [SerializeField] private List<Material> _countryMaterials;

        [SerializeField, Service(typeof(CountryService))]
        private CountryService _countryService = new();

        [SerializeField, Service(typeof(CityPointService))]
        private CityPointService _cityPointService = new();

        [SerializeField, Listener, Service(typeof(Map))]
        private Map _map = new();

        public override void Inject(ServiceLocator serviceLocator)
        {
            _map.Construct(_countryService, serviceLocator.GetService<AirplaneRouteFactory>(),
                serviceLocator.GetService<AirplaneLaunchManager>(), serviceLocator.GetService<GameRepository>(),
                serviceLocator.GetService<CountrySeller>());

            SetId();

            _cityPointService.Setup(_countryService.Country.SelectMany(country => country.GameCities).ToArray());

            foreach (var country in _countryService.Country)
            {
                country.Construct(serviceLocator.GetService<AirplaneRouteFactory>(),
                    serviceLocator.GetService<AirplaneLaunchManager>(), _map,
                    serviceLocator.GetService<MoneyStorage>(), serviceLocator.GetService<CountrySeller>());
            }

            _countryService.Country.ForEach(country => country.SetMaterials(_countryMaterials));
        }

        private void SetId()
        {
            var countries = _countryService.Country;
            var id = 0;

            for (int i = 0; i < countries.Length; i++)
            {
                Country country = countries[i];

                if (country.GameCities.Count > 0)
                {
                    for (int j = 0; j < country.GameCities.Count; j++)
                    {
                        country.GameCities[j].Id = id;
                        id++;
                    }
                }
            }
        }
    }
}