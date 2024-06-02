using System;
using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.PopupScripts;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class Country : SerializedMonoBehaviour
    {
        [field: SerializeField] public CountryData CountryData { get; private set; }
        [SerializeField] private Country[] _additionalyCountries;
        [SerializeField] private List<CityPoint> _gameCities = new();
        [field: SerializeField] public List<CityPoint> CityPoints { get; private set; } = new();
        [SerializeField] private MeshMarker _meshMarker;
        [SerializeField] private CountryTitle _countryTitle;

        [ShowInInspector] private HashSet<CityPoint> _buyedCities = new();
        private AirplaneRouteFactory _airplaneRouteFactory;
        private AirplaneLaunchManager _airplaneLaunchManager;
        private MoneyStorage _moneyStorage;
        private CountrySeller _countrySeller;
        private static bool _isRouteCreationDone = false;
        private List<Material> _countryMaterials = new();
        private Map _map;
        public bool IsFirstOpened { get; set; }
        public bool IsOpenPopup { get; set; }
        public bool IsBuy { get; private set; }
        public MeshMarker MeshMarker => _meshMarker;
        public IEnumerable<CityPoint> BuyedCities => _buyedCities;
        public IReadOnlyList<CityPoint> GameCities => _gameCities;

        public event Action<int> OnCountryOpened;

        public List<int> OrderUpgradedCities = new();

        // private void OnValidate()
        // {
        //     _meshMarker = GetComponentInChildren<MeshMarker>(true);
        //     _countryTitle = GetComponentInChildren<CountryTitle>(true);
        // }

        [Inject]
        public void Construct(AirplaneRouteFactory airplaneRouteFactory, AirplaneLaunchManager airplaneLaunchManager,
            Map map, MoneyStorage moneyStorage, CountrySeller countrySeller)
        {
            _countrySeller = countrySeller;
            _moneyStorage = moneyStorage;
            _map = map;
            _airplaneRouteFactory = airplaneRouteFactory;
            _airplaneLaunchManager = airplaneLaunchManager;

            _gameCities.ForEach(city => city.OnCityUpgrade += OnCreateRoute);
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
        }

        private void OnDestroy()
        {
            _gameCities.ForEach(city => city.OnCityUpgrade -= OnCreateRoute);
        }

        private void OnMoneyChanged(BigNumber money)
        {
            if (_countrySeller.CanBuy(money.ToLong(), _gameCities.Count))
            {
                if (!IsOpenPopup)
                    ChangeMaterial(COUNTRY_MATERIAL.CAN_BUY_COUNTRY);
            }
            else
            {
                ChangeMaterial(COUNTRY_MATERIAL.DEFAULT);
            }

            if (IsBuy)
                _moneyStorage.OnMoneyChanged -= OnMoneyChanged;
        }

        private void OnCreateRoute(CityPoint cityPoint)
        {
            if (cityPoint.CurrentAirportLevel >= 1)
            {
                _buyedCities.Add(cityPoint);
            }
        }

        public void ChangeMaterial(COUNTRY_MATERIAL countryMaterial)
        {
            Material material = GetMaterial(countryMaterial);

            if (_meshMarker != null)
                _meshMarker.SetMaterial(material);
        }

        public void OpenCountry()
        {
            MeshMarker meshRenderer = gameObject.GetComponentInChildren<MeshMarker>();

            if (meshRenderer != null)
                meshRenderer.gameObject.layer = (int) PHYSICS_LAYER.ACTIVE_COUNTRY;

            _gameCities.ForEach(city => city.gameObject.SetActive(true));

            OnCountryOpened?.Invoke(CountryData.Index);

            if (_countryTitle != null)
                _countryTitle.SetWhiteTitle();

            IsBuy = true;

            if (_additionalyCountries.Length > 0)
            {
                _additionalyCountries.ForEach(country => country.OpenCountry());
            }
        }

        public Material GetMaterial(COUNTRY_MATERIAL countryMaterial)
        {
            return countryMaterial switch
            {
                COUNTRY_MATERIAL.DEFAULT => _countryMaterials[0],
                COUNTRY_MATERIAL.SELECTED => _countryMaterials[1],
                COUNTRY_MATERIAL.CAN_BUY_COUNTRY => _countryMaterials[2],
                _ => null
            };
        }

        public Transform GetFirstCity()
        {
            return _gameCities[0].transform;
        }


        public CityPoint FindClosestCity(CityPoint cityPoint)
        {
            OrderUpgradedCities.Add(cityPoint.Id);

            return _map.FindClosestCity(cityPoint);
        }

#if UNITY_EDITOR

        public bool ShowCities { get; set; }

        public bool ShowGameCities { get; set; }

        [HideInInspector] public bool areCitiesEnabled = false;

        [HideInInspector] public bool areGameCitiesEnabled = false;

        public string ArrayToString()
        {
            return string.Join("\n", _gameCities.Select(city => city.gameObject.name));
        }


        public void CopyToClipboard(string text)
        {
            EditorGUIUtility.systemCopyBuffer = text;
        }

        public void AddGameCity(CityPoint city)
        {
            _gameCities.Add(city);
        }

        public void RemoveGameCity(CityPoint city)
        {
            _gameCities.Remove(city);
        }

        private void OnMouseDown()
        {
            SceneView.RepaintAll();
        }

        public void EnableCities()
        {
            if (CityPoints != null)
            {
                foreach (var city in CityPoints)
                {
                    if (city != null)
                    {
                        city.gameObject.SetActive(true);
                    }
                }

                areCitiesEnabled = true;
                areGameCitiesEnabled = false;
            }
        }

        public void EnableGameCities()
        {
            if (_gameCities != null)
            {
                foreach (var city in _gameCities)
                {
                    if (city != null)
                    {
                        city.gameObject.SetActive(true);
                    }
                }

                areGameCitiesEnabled = true;
                areCitiesEnabled = false;
            }
        }

        public void DisableCities()
        {
            if (CityPoints != null && areCitiesEnabled)
            {
                foreach (var city in CityPoints)
                {
                    if (city != null)
                    {
                        city.gameObject.SetActive(false);
                    }
                }

                areCitiesEnabled = false;
            }
        }

        public void DisableGameCities()
        {
            if (_gameCities != null && areGameCitiesEnabled)
            {
                foreach (var city in _gameCities)
                {
                    if (city != null)
                    {
                        city.gameObject.SetActive(false);
                    }
                }

                areGameCitiesEnabled = false;
            }
        }
#endif

        public void AddBuyedCity(CityPoint city)
        {
            _buyedCities.Add(city);
        }

        public void SetMaterials(List<Material> countryMaterials)
        {
            _countryMaterials = countryMaterials;
        }

        public void DisableTitle()
        {
            _countryTitle.DisableText();
        }

        public void EnableTitle()
        {
            _countryTitle.EnableText();
        }

        public void DisableCollider()
        {
            if (_meshMarker != null)
                _meshMarker.DisableCollider();
        }

        public void EnableCollider()
        {
            if (_meshMarker != null)
                _meshMarker.EnableCollider();
        }
    }
}