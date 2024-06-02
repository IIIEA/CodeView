using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.Tutorial;
using Lean.Touch;
using Sirenix.Utilities;

namespace _Fly_Connect.Scripts.LineInvisibilityManager
{
    public class CityViewScaler : IGameStartListener, IGameFinishListener, IGameUpdateListener
    {
        private List<CityPoint> _city = new();
        private Map _map;
        private LeanPinchCamera _leanPinchCamera;
        private float _previousZoom;
        private float _targetScale = 4;
        private CountryService _countryService;
        private bool _isOpenCountry = false;
        private InputSystem _inputSystem;

        [Inject]
        private void Construct(LeanPinchCamera leanPinchCamera, Map map, CountryService countryService,
            InputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            _countryService = countryService;
            _leanPinchCamera = leanPinchCamera;
            _map = map;
            _countryService.Country.ForEach(country => country.OnCountryOpened += OnCountryOpened);
        }

        public void OnFinishGame()
        {
            _countryService.Country.ForEach(country => country.OnCountryOpened -= OnCountryOpened);
        }

        public void OnStartGame()
        {
            _city = _map.GetGameCities();
            _previousZoom = _leanPinchCamera.Zoom;
        }

        private void OnCountryOpened(int country)
        {
            OnUpdate(0f);
            _isOpenCountry = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!TutorialManager.Instance.IsCompleted)
                return;

            if (_previousZoom != _leanPinchCamera.Zoom &&
                _leanPinchCamera.Zoom > 15 || _isOpenCountry && _leanPinchCamera.Zoom > 15)
            {
                _inputSystem.SetDistanceThreshold(1.3f);
                _isOpenCountry = false;

                foreach (var city in _city)
                {
                    if (city.gameObject.activeSelf)
                    {
                        if (city.CityPointView != null)
                            city.CityPointView.SetScale(_targetScale);
                    }

                    if (city.CityPointView != null)
                        city.CityPointView.DisableTitle();
                }
            }
            else if (_previousZoom != _leanPinchCamera.Zoom)
            {
                _inputSystem.SetDistanceThreshold(0.4f);

                foreach (var city in _city)
                {
                    if (city.gameObject.activeSelf)
                    {
                        if (city.CityPointView != null)
                        {
                            city.CityPointView.SetDefaultScale();
                        }
                    }

                    if (city.CityPointView != null)
                        city.CityPointView.EnableTitle();
                }
            }

            _previousZoom = _leanPinchCamera.Zoom;
        }
    }
}