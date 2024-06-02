using System;
using System.Collections;
using System.Collections.Generic;
using _Fly_Connect.Scripts.ApplicationLoader;
using _Fly_Connect.Scripts.Gameplay;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Gameplay.GoldenAirplane;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Gameplay.UfoScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Popup;
using _Fly_Connect.Scripts.Sound.UI_Audio_Manager;
using _Fly_Connect.Scripts.Tutorial;
using Dreamteck.Splines;
using Lean.Touch;
using Lofelt.NiceVibrations;
using MadPixelAnalytics;
using UnityEngine;

namespace _Fly_Connect.Scripts.PopupScripts
{
    [Serializable]
    public class InputSystem : IGameStartListener, IGameFinishListener
    {
        [SerializeField] private Transform _lineTransform;
        [SerializeField] private float _minDistanceThreshold;

        private Camera _camera;
        private bool _canShowPopup = true;
        private CameraController _cameraController;
        private CityPopupFactory _cityPopupFactory;
        private CountryPopupFactory _countryPopupFactory;
        private PopupManager _popupManager;
        private InputStateId _inputStateId = InputStateId.BASE;
        private GoldenAirplanePopupFactory _ufoPopupFactory;
        private AirplaneRouteFactory _airplaneRouteFactory;
        private CityPoint _city1;
        private CityPointView _cityPointView1;
        private CityPointView _cityPointView2;
        private LeanDragCamera _leanDragCamera;
        private AirplaneSeller _airplaneSeller;
        private Map _map;
        private List<CityPoint> _gameCities = new();
        private CityPoint _closestCity = null;
        private WarningPopupManager _warningPopupManager;
        private ICoroutineRunner _coroutineRunner;
        private LeanPinchCamera _leanPinchCamera;
        private MoneyStorage _moneyStorage;
        private AirportSeller _airportSeller;
        private CityPointView _previousCityView;
        private bool _canVibration = false;
        private AirportUpgradeSeller _airportUpgradeSeller;
        private StarterPackManager _starterPackManager;

        public event Action<Vector3> OnPathTap;

        [Inject]
        public void Construct(Camera camera, CityPopupFactory cityPopupFactory, CountryPopupFactory countryPopupFactory,
            PopupManager popupManager, GoldenAirplanePopupFactory goldenAirplanePopupFactory,
            AirplaneRouteFactory airplaneRouteFactory, LeanDragCamera leanDragCamera, CameraController cameraController,
            Map map, WarningPopupManager warningPopupManager, ICoroutineRunner coroutineRunner,
            LeanPinchCamera leanPinchCamera, MoneyStorage moneyStorage, AirportSeller airportSeller,
            AirportUpgradeSeller airportUpgradeSeller, StarterPackManager starterPackManager)
        {
            _starterPackManager = starterPackManager;
            _airportUpgradeSeller = airportUpgradeSeller;
            _airportSeller = airportSeller;
            _moneyStorage = moneyStorage;
            _leanPinchCamera = leanPinchCamera;
            _cameraController = cameraController;
            _coroutineRunner = coroutineRunner;
            _warningPopupManager = warningPopupManager;
            _map = map;
            _gameCities = _map.GetGameCities();
            _leanDragCamera = leanDragCamera;
            _airplaneRouteFactory = airplaneRouteFactory;
            _ufoPopupFactory = goldenAirplanePopupFactory;
            _popupManager = popupManager;
            _camera = camera;
            _cityPopupFactory = cityPopupFactory;
            _countryPopupFactory = countryPopupFactory;
        }

        public void SetDistanceThreshold(float distance)
        {
            _minDistanceThreshold = distance;
        }

        public void OnStartGame()
        {
            LoadingScreen.OnHide += OnHandleFingerTap;
        }

        private void OnHandleFingerTap()
        {
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
            LeanTouch.OnFingerTap += HandleFingerTap;
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerUp += OnFingerUp;
        }

        public void OnFinishGame()
        {
            LeanTouch.OnFingerTap -= HandleFingerTap;
            LoadingScreen.OnHide -= OnHandleFingerTap;
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerUp -= OnFingerUp;
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        }

        private void OnFingerUp(LeanFinger leanFinger)
        {
            _canShowPopup = false;
            _cameraController.SetSpeed(0);

            if (_cityPointView1 != null)
            {
                if (_leanPinchCamera.Zoom <= 15)
                {
                    _cityPointView1.SetDefaultScale();
                }

                _cityPointView1.Outline.gameObject.SetActive(false);
            }

            if (_popupManager.IsPopupActive(PopupName.BUY_CITY_POPUP))
                _popupManager.HidePopup(PopupName.BUY_CITY_POPUP);

            if (_inputStateId == InputStateId.LOCK)
                return;

            if (leanFinger.IsOverGui)
            {
                ClearRenderer();
                return;
            }

            if (_city1 == null)
                return;

            if (_cityPointView2 != null)
            {
                _cityPointView2.Outline.gameObject.SetActive(false);
                _cityPointView2 = null;
            }

            if (_closestCity != null)
            {
                if (!_airplaneRouteFactory.RouteExists(_city1, _closestCity))
                {
                    if (!_city1.IsBuy && _closestCity.IsBuy)
                    {
                        ClearRenderer();
                        return;
                    }

                    if (_city1 == _closestCity)
                    {
                        ClearRenderer();
                        return;
                    }

                    if (_closestCity.CurrentAirportLevel == 0)
                    {
                        _airportUpgradeSeller.IncreaseCityCount();

                        AnalyticsManager.CustomEvent("unlock_city", new Dictionary<string, object>()
                        {
                            {"name", _closestCity.name},
                            {"maxLevelCityCount", _airportUpgradeSeller.MaxLevelCityCount},
                            {"cityCount", _airportUpgradeSeller.CityCount},
                        });
                    }

                    _airplaneRouteFactory.GenerateRoute(_city1, _closestCity);
                    _closestCity = null;
                }
            }

            ClearRenderer();
        }

        private void OnFingerUpdate(LeanFinger leanFinger)
        {
            if (_city1 == null)
                return;

            var ray = _camera.ScreenPointToRay(leanFinger.ScreenPosition);

            if (Physics.Raycast(ray, out var hit))
            {
                _airplaneRouteFactory.DrawRoute(_city1, hit.point, _lineTransform);

                if (TutorialManager.Instance.IsCompleted)
                {
                    _cameraController.LerpToPosition(leanFinger.ScreenPosition);
                }

                if (_closestCity != null)
                    _closestCity.CityPointView.Outline.gameObject.SetActive(false);

                CityPoint city = FindNearestCity(hit);

                if (city != null)
                {
                    if (TutorialManager.Instance.CurrentStep == TutorialStep.DRAW_LINE && city.name == "Munich")
                        return;

                    _closestCity = city;

                    if (_airplaneRouteFactory.RouteExists(_city1, _closestCity) ||
                        _airplaneRouteFactory.RouteExists(_closestCity, _city1))
                        return;

                    if (city.CityPointView != _cityPointView1)
                    {
                        _cityPointView2 = city.CityPointView;
                        _cityPointView2.Outline.gameObject.SetActive(true);
                        _cityPointView2.Outline.color = Color.green;

                        if (_popupManager.IsPopupActive(PopupName.BUY_CITY_POPUP))
                            _popupManager.HidePopup(PopupName.BUY_CITY_POPUP);

                        if (!_popupManager.IsPopupActive(PopupName.BUY_CITY_POPUP) && !_closestCity.IsBuy)
                        {
                            IBuyCityPresenter buyPresenter = new BuyCityPopupPresenter(_moneyStorage,
                                _airportSeller,
                                _closestCity.CityPointView.transform, _leanPinchCamera);
                            _popupManager.ShowPopup(PopupName.BUY_CITY_POPUP, buyPresenter);
                        }

                        if (_previousCityView != city.CityPointView ||
                            _canVibration && _previousCityView == city.CityPointView)
                        {
                            UISoundManager.PlaySound(UISoundType.AIRPORT_CHOOSEN);
                            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                            _canVibration = false;
                            _previousCityView = city.CityPointView;
                        }
                    }

                    _airplaneRouteFactory.SetTempRouteGreen();
                }
                else
                {
                    if (_cityPointView2 != null)
                    {
                        _cityPointView2.Outline.gameObject.SetActive(false);
                        _cityPointView2.Outline.color = Color.white;

                        if (_closestCity != null)
                            _closestCity = null;
                    }

                    if (_popupManager.IsPopupActive(PopupName.BUY_CITY_POPUP))
                        _popupManager.HidePopup(PopupName.BUY_CITY_POPUP);

                    _airplaneRouteFactory.SetTempRouteWhite();
                    _canVibration = true;
                }
            }
        }

        private void OnFingerDown(LeanFinger leanFinger)
        {
            if (_inputStateId == InputStateId.LOCK)
                return;

            if (leanFinger.IsOverGui)
                return;

            var ray = _camera.ScreenPointToRay(leanFinger.ScreenPosition);

            if (Physics.Raycast(ray, out var cityHit))
            {
                if (cityHit.collider.TryGetComponentInParent(out CityPoint cityPoint))
                {
                    if (TutorialManager.Instance.CurrentStep == TutorialStep.BUY_AIRPORT ||
                        TutorialManager.Instance.CurrentStep == TutorialStep.DRAW_LINE ||
                        TutorialManager.Instance.CurrentStep == TutorialStep.DRAW_MORE_LINE)
                    {
                        if (cityPoint.name != "Berlin")
                        {
                            return;
                        }
                    }

                    _canShowPopup = true;

                    if (cityPoint.IsBuy && cityPoint.Routes != null &&
                        cityPoint.Routes.Count != cityPoint.CurrentAirportLevel &&
                        cityPoint.CurrentAirportLevel > 0)
                    {
                        _city1 = cityPoint;
                        _cityPointView1 = _city1.CityPointView;
                        _cityPointView1.Outline.gameObject.SetActive(true);
                        _leanDragCamera.enabled = false;
                        _cityPointView1.SetSelectedScale();
                    }
                    else
                    {
                        _coroutineRunner.StartCoroutine(cityPoint.Routes.Count == cityPoint.MaxAirportLevel
                            ? ShowPopupRoutine("maximum number of lines")
                            : ShowPopupRoutine("Increase City Level"));
                    }

                    return;
                }

                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.TryGetComponent(out UFO ufo))
                    {
                        if (_starterPackManager.CanShow && !_starterPackManager.IsBuy)
                        {
                            _starterPackManager.ShowPopup();
                        }
                        else
                        {
                            var presenter = _ufoPopupFactory.Create();
                            _popupManager.ShowPopup(PopupName.GOLDEN_AIRPLANE_POPUP, presenter);
                        }
                    }
                }
            }
        }

        private void HandleFingerTap(LeanFinger leanFinger)
        {
            if (_inputStateId == InputStateId.LOCK)
                return;

            if (leanFinger.IsOverGui)
                return;

            var ray = _camera.ScreenPointToRay(leanFinger.ScreenPosition);
            int interactableLayers = LayerMask.GetMask("Country");

            if (Physics.Raycast(ray, out var countryHit, float.MaxValue, interactableLayers))
            {
                if (countryHit.collider.TryGetComponentInParent(out Country country))
                {
                    if (!country.IsBuy)
                    {
                        var countryPresenter = _countryPopupFactory.Create(country);
                        _popupManager.ShowPopup(PopupName.COUNTRY_POPUP, countryPresenter);
                        country.ChangeMaterial(COUNTRY_MATERIAL.SELECTED);
                    }
                }
            }

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponentInParent(out CityPoint city))
                {
                    if (TutorialManager.Instance.CurrentStep == TutorialStep.BUY_AIRPORT ||
                        TutorialManager.Instance.CurrentStep == TutorialStep.AIRPORT_UPGRADE ||
                        TutorialManager.Instance.CurrentStep == TutorialStep.DRAW_MORE_LINE ||
                        TutorialManager.Instance.CurrentStep == TutorialStep.BUY_MORE_COUNTRY)
                    {
                        if (city.name != "Berlin")
                        {
                            return;
                        }
                    }

                    if (TutorialManager.Instance.CurrentStep == TutorialStep.DRAW_LINE)
                        return;

                    var presenter = _cityPopupFactory.Create(city);
                    _popupManager.ShowPopup(PopupName.CITY_POPUP, presenter);

                    if (TutorialManager.Instance.IsCompleted)
                    {
                        _leanDragCamera.enabled = true;
                    }

                    city.CityPointView.Outline.gameObject.SetActive(false);
                    _canShowPopup = false;

                    if (_cityPointView1 != null && _leanPinchCamera.Zoom < 15)
                    {
                        _cityPointView1.SetDefaultScale();
                    }

                    ClearRenderer();
                }
            }
        }

        private IEnumerator ShowPopupRoutine(string text)
        {
            yield return new WaitForSeconds(0.2f);

            if (_canShowPopup)
            {
                var warningPopup = _warningPopupManager.Get();

                if (warningPopup != null)
                    warningPopup.SetText(text);
            }
        }

        private CityPoint FindNearestCity(RaycastHit hit)
        {
            float closestDistance = float.MaxValue;
            CityPoint cityPoint = null;

            foreach (var city in _gameCities)
            {
                if (city.gameObject.activeSelf && city != _city1 && city.CityPointView != null)
                {
                    float distance = Vector3.Distance(hit.point, city.CityPointView.transform.position);

                    if (distance <= _minDistanceThreshold && distance < closestDistance)
                    {
                        closestDistance = distance;
                        cityPoint = city;
                    }
                }
            }

            return cityPoint;
        }

        private void ClearRenderer()
        {
            _airplaneRouteFactory.ClearLineRenderer();

            if (_city1 == null)
                return;

            _city1 = null;

            if (TutorialManager.Instance.CurrentStep != TutorialStep.DRAW_LINE &&
                TutorialManager.Instance.CurrentStep != TutorialStep.AIRPORT_UPGRADE)
            {
                _leanDragCamera.enabled = true;
            }
        }

        public void SwitchState(InputStateId stateId)
        {
            _inputStateId = stateId;
        }
    }
}