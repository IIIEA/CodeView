using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Tutorial;
using Lean.Touch;
using Sirenix.Utilities;
using UnityEngine;

namespace _Fly_Connect.Scripts.LineInvisibilityManager
{
    [Serializable]
    public class LineInvisibilityManager : IGameStartListener, IGameFinishListener, IGameUpdateListener
    {
        private const float MAXIMUM_SIZE = 1f;
        private const float SIZE = 1.3f;
        private const float MINIMUM_SIZE = 0;
        private const float MINIMUM_ZOOM = 1.6f;
        private const float MIDDLE_ZOOM = 5.5f;
        private const int ZOOM = 10;

        public IReadOnlyList<Route> Routes;
        private LeanPinchCamera _leanPinchCamera;
        private float _previousZoom;
        private List<Route> _longPaths = new();
        private List<Route> _otherPaths = new();
        private AirplaneLaunchManager _airplaneLaunchManager;
        private Map _map;

        private MoneyPool.MoneyPool _moneyPool;
        private AirplaneRouteFactory _airplaneRouteFactory;

        [Inject]
        public void Construct(AirplaneRouteFactory airplaneRouteFactory, LeanPinchCamera leanPinchCamera,
            AirplaneLaunchManager airplaneLaunchManager, Map map, MoneyPool.MoneyPool moneyPool)
        {
            _airplaneRouteFactory = airplaneRouteFactory;
            _moneyPool = moneyPool;
            _map = map;
            _airplaneLaunchManager = airplaneLaunchManager;
            _leanPinchCamera = leanPinchCamera;
            Routes = airplaneRouteFactory.Routes;
        }

        public void OnStartGame()
        {
            OnUpdate(0);
            _airplaneLaunchManager.OnAirplaneLaunched += OnAirplaneLaunched;
            _previousZoom = _leanPinchCamera.Zoom;
        }

        public void OnFinishGame()
        {
            _airplaneLaunchManager.OnAirplaneLaunched -= OnAirplaneLaunched;
        }

        private void OnAirplaneLaunched(Airplane airplane)
        {
            if (_leanPinchCamera.Zoom is < MIDDLE_ZOOM and >= MINIMUM_ZOOM)
            {
                _airplaneLaunchManager.SetAirplaneScale(0.3f, true);
            }
            else if (_leanPinchCamera.Zoom is < ZOOM and >= MIDDLE_ZOOM)
            {
                _airplaneLaunchManager.SetAirplaneScale(0.5f, true);
            }
            else
            {
                _airplaneLaunchManager.SetAirplaneScale(1f, true);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (!TutorialManager.Instance.IsCompleted)
                return;

            if (_previousZoom != _leanPinchCamera.Zoom)
            {
                ClearPaths();
                SortRoute();

                float otherCityAlpha = Remap.DoRemap(5, 17, 200, 100, _leanPinchCamera.Zoom);
                float longCityAlpha = Remap.DoRemap(5, 17, 100, 255, _leanPinchCamera.Zoom);

                if (_leanPinchCamera.Zoom < MINIMUM_ZOOM)
                {
                    _moneyPool.SetShowEffect(false);
                    _longPaths.ForEach(x => x.SetWhiteColorLine());
                    _otherPaths.ForEach(x => x.SetWhiteColorLine());
                    _map.EnableCity();
                    _airplaneLaunchManager.DisableAirplane();
                    ChangeRoutesAlpha(_longPaths, MINIMUM_SIZE, 0.1f, true);
                    ChangeRoutesAlpha(_otherPaths, MINIMUM_SIZE, 0.1f, true);
                }
                else if (_leanPinchCamera.Zoom < MIDDLE_ZOOM && _leanPinchCamera.Zoom >= MINIMUM_ZOOM)
                {
                    _moneyPool.SetShowEffect(true);
                    _map.DisableCity();
                    _airplaneLaunchManager.EnableAirplane();
                    _airplaneLaunchManager.SetAirplaneScale(0.3f);
                    ChangeRoutesAlpha(_otherPaths, otherCityAlpha, 0.4f);
                    ChangeRoutesAlpha(_longPaths, longCityAlpha, 0.4f);
                }
                else if (_leanPinchCamera.Zoom < ZOOM && _leanPinchCamera.Zoom >= MIDDLE_ZOOM)
                {
                    _moneyPool.SetShowEffect(true);
                    _map.DisableCity();
                    _airplaneLaunchManager.EnableAirplane();
                    _airplaneLaunchManager.SetAirplaneScale(0.5f);
                    ChangeRoutesAlpha(_otherPaths, otherCityAlpha, 0.7f);
                    ChangeRoutesAlpha(_longPaths, longCityAlpha, 0.7f);
                    _airplaneLaunchManager.CachedAirplanes.ForEach(x => x.DisableTriangleRegime());
                }
                else
                {
                    _airplaneLaunchManager.CachedAirplanes.ForEach(x => x.EnableTriangleRegime());
                    _map.DisableCity();
                    _airplaneLaunchManager.SetAirplaneScale(1f);
                    _airplaneLaunchManager.EnableAirplane();
                    ChangeRoutesAlpha(_otherPaths, otherCityAlpha, SIZE);
                    ChangeRoutesAlpha(_longPaths, longCityAlpha, MAXIMUM_SIZE);
                }
            }

            _previousZoom = _leanPinchCamera.Zoom;
        }

        private void ChangeRoutesAlpha(List<Route> path, float alpha, float size, bool isChangePath = false)
        {
            foreach (var route in path)
            {
                route.SplineRenderer.size = size;

                if (!route.IsTapBoosted || isChangePath)
                {
                    var material = route.MeshRenderer.material;
                    SetColor(alpha, material);
                }

                route.SplineRenderer.Rebuild();
                route.SplineRenderer.RenderWithCamera(Camera.main);
            }
        }

        private static void SetColor(float alpha, Material material)
        {
            Color currentColor = Color.white;
            currentColor.a = alpha / 255;
            material.color = currentColor;
        }

        private void ClearPaths()
        {
            _longPaths.Clear();
            _otherPaths.Clear();
        }

        private void SortRoute()
        {
            foreach (var route in Routes)
            {
                float routeDistance = route.Distance;

                if (routeDistance >= 6.5f)
                {
                    _longPaths.Add(route);
                }
                else
                {
                    _otherPaths.Add(route);
                }
            }
        }
    }
}