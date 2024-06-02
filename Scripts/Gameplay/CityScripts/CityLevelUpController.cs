using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.PopupScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    [Serializable]
    public class CityLevelUpController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _enabledObjects = new();
        [SerializeField] private CityPoint _cityPoint;
        [SerializeField] private CityPointView _cityPointView;
        [SerializeField] private Country _country;

        [ProgressBar(0, 4, 0, 1, 0, Segmented = true, DrawValueLabel = true)]
        public int _currentAirportLevel;

        public IReadOnlyList<GameObject> EnabledObjects => _enabledObjects;

        public void OnValidate()
        {
            // EnableObjects();

            // _currentAirportLevel = 0;
            // gameObject.SetActive(false);
        }

        private void Start()
        {
            UpdateInfo();
        }

        private void OnEnable()
        {
            _cityPoint.OnCityUpgrade += OnCityUpgrade;
        }

        private void OnDisable()
        {
            _cityPoint.OnCityUpgrade -= OnCityUpgrade;
        }

        public void Enable()
        {
            SetObjectState(true);
        }

        public void Disable()
        {
            SetObjectState(false);
        }

        public bool HasLayerWithoutCityLayer()
        {
            return _enabledObjects[0].layer == (int)PHYSICS_LAYER.BUILDING_WITHOUT_CITY;
        }

        private void SetObjectState(bool enable)
        {
            if (_cityPoint.CurrentAirportLevel == _cityPoint.MaxAirportLevel)
                return;

            GameObject parent = _enabledObjects[_cityPoint.CurrentAirportLevel];

            parent.gameObject.SetActive(enable);

            var renderers = parent.GetComponentsInChildren<SpriteRenderer>();

            foreach (var renderer in renderers)
            {
                renderer.color = enable ? new Color32(133, 255, 59, 180) : Color.white;
            }
        }

        private void UpdateInfo()
        {
            _currentAirportLevel = _cityPoint.CurrentAirportLevel;

            EnableObjects();
        }

        public void EnableObjects()
        {
            for (int i = 0; i < _enabledObjects.Count; i++)
            {
                if (_enabledObjects[i] != null)
                {
                    _enabledObjects[i].SetActive(i <= _currentAirportLevel - 1);
                }
            }
        }

        private void OnCityUpgrade(CityPoint cityPoint)
        {
            if (_enabledObjects == null)
                return;

            if (cityPoint.CurrentAirportLevel == 1)
                cityPoint.SetIsBuy(true);

            var cityPointCurrentAirportLevel = _cityPoint.CurrentAirportLevel - 1;

            if (cityPointCurrentAirportLevel < 0)
                return;

            GameObject parent = _enabledObjects[cityPointCurrentAirportLevel];

            var renderers = parent.GetComponentsInChildren<SpriteRenderer>();

            foreach (var renderer in renderers)
            {
                renderer.color = Color.white;
            }

            UpdateInfo();

            if (_cityPoint.CurrentAirportLevel != 1)
                Enable();
        }
    }
}