using System;
using System.Collections;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.BezierCurve;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using Dreamteck.Splines;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    [Serializable]
    public class AirplaneRouteFactory
    {
        [SerializeField] private Material _lineMaterial;
        [SerializeField] private SplineComputer _splinePrefab;

        [SerializeField] private float _distanceBetweenPoints = 0.3f;
        [SerializeField] private float _height = 0.1f;

        private Transform _mapTransform;
        private LineRenderer _tempLineRenderer;
        private Transform _routeTransform;
        private List<Route> _routes = new();
        private AirplaneLaunchManager _airplaneLaunchManager;
        private AirplaneSeller _airplaneSeller;
        private ICoroutineRunner _coroutineRunner;
        private MoneyStorage _moneyStorage;
        private AirportSeller _airportSeller;
        private IncomeCounter _incomeCounter;

        public event Action OnRouteGenerated;

        public IReadOnlyList<Route> Routes => _routes;

        [Inject]
        public void Construct(Transform mapTransform, AirplaneLaunchManager airplaneLaunchManager,
            ICoroutineRunner coroutineRunner, AirplaneSeller airplaneSeller, MoneyStorage moneyStorage,
            AirportSeller airportSeller, IncomeCounter incomeCounter)
        {
            _incomeCounter = incomeCounter;
            _airportSeller = airportSeller;
            _moneyStorage = moneyStorage;
            _airplaneSeller = airplaneSeller;
            _coroutineRunner = coroutineRunner;
            _airplaneLaunchManager = airplaneLaunchManager;
            _mapTransform = mapTransform;
        }

        public void GenerateRoute(CityPoint city1, CityPoint city2)
        {
            if (RouteExists(city1, city2) || RouteExists(city2, city1))
            {
                return;
            }

            if (city2.CurrentAirportLevel == 0)
            {
                if (!_airportSeller.CanBuy(_moneyStorage.Money.ToLong()))
                    return;

                _moneyStorage.SpendMoney(_airportSeller.GetCurrentPrice());
                city2.LevelUp();
            }

            float distance = Vector3.Distance(city1.transform.position, city2.CityPointView.transform.position);
            int resolution = Mathf.Max(2, (int) (distance / _distanceBetweenPoints));

            if (city1.CityPointView == null || _airplaneSeller == null)
                return;

            var p1 = GetP1(city1, city2.CityPointView.transform.position);
            _airplaneSeller.IncreaseOrderAirplaneBuyed();
            Route route = CreateRoute(city1, city2, p1, resolution);

            route.SetDistance(distance);

            OnRouteGenerated?.Invoke();

            List<Vector3> points = new();

            for (int i = 0; i < resolution; i++)
            {
                float t = i / (float) (resolution - 1);
                Vector3 position = Bezier.GetPoint(city1.CityPointView.transform.position, p1,
                    city2.CityPointView.transform.position, t);
                position.z = -0.1f;
                points.Add(position);
            }

            SplineComputer spline =
                Object.Instantiate(_splinePrefab, Vector3.zero, Quaternion.identity, _routeTransform);

            SplinePoint[] allPoints = new SplinePoint[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                SplinePoint point = new();
                point.SetPosition(points[i]);
                point.normal = Vector3.up;
                point.size = 0.08f;
                point.color = Color.white;
                allPoints[i] = point;
            }

            spline.SetPoints(allPoints);
            SplineRenderer splineRenderer = spline.GetComponent<SplineRenderer>();
            MeshRenderer meshRenderer = spline.GetComponent<MeshRenderer>();
            spline.RebuildImmediate();
            _coroutineRunner.StartCoroutine(StartDisableAutoOrient(splineRenderer));
            route.SetMeshRenderer(meshRenderer, splineRenderer);
            _airplaneLaunchManager.LaunchAirplane(route, city1.CurrentAirportLevel);
            _incomeCounter.CalculateIncome();
        }

        private IEnumerator StartDisableAutoOrient(SplineRenderer splineRenderer)
        {
            splineRenderer.autoOrient = true;
            yield return new WaitForSeconds(1f);
            splineRenderer.autoOrient = false;
            MeshCollider meshCollider = splineRenderer.GetComponent<MeshCollider>();
            meshCollider.enabled = true;
        }

        public void DrawRoute(CityPoint city1, Vector3 targetPosition, Transform lineTransform)
        {
            float distance = Vector3.Distance(city1.CityPointView.transform.position, targetPosition);
            int resolution = Mathf.Max(2, (int) (distance / _distanceBetweenPoints));

            var p1 = GetP1(city1, targetPosition);

            if (_tempLineRenderer == null)
            {
                _tempLineRenderer = lineTransform.gameObject.AddComponent<LineRenderer>();
            }

            InitLineRenderer(_tempLineRenderer, resolution, _lineMaterial, true);

            _tempLineRenderer.startWidth = 0.085f;
            _tempLineRenderer.endWidth = 0.085f;

            for (int i = 0; i < resolution; i++)
            {
                float t = i / (float) (resolution - 1);
                Vector3 position = Bezier.GetPoint(city1.CityPointView.transform.position, p1, targetPosition, t);
                position.z = -0.05f;
                _tempLineRenderer.SetPosition(i, position);
            }
        }

        private void InitLineRenderer(LineRenderer lineRenderer, int resolution, Material material,
            bool isRepeat = false)
        {
            lineRenderer.material = material;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = resolution;
            lineRenderer.useWorldSpace = true;

            if (isRepeat)
                lineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
        }

        private Vector3 GetP1(CityPoint city1, Vector3 city2)
        {
            Vector3 middlePoint = (city1.CityPointView.transform.position + city2) / 2f;
            Vector3 lineDirection = city2 - city1.CityPointView.transform.position;
            float distance = lineDirection.magnitude;
            Vector3 normal = Vector3.Cross(lineDirection, Vector3.forward).normalized;
            if (normal.y < 0)
            {
                normal = -normal;
            }

            float dot = Vector3.Dot(normal, Vector3.up);
            Vector3 offset = normal * distance * _height * dot;
            Vector3 p1 = middlePoint + offset;
            return p1;
        }

        private Route CreateRoute(CityPoint city1, CityPoint city2, Vector3 p2, int resolution)
        {
            GameObject route = new GameObject(city1.name + " - " + city2.name);
            route.transform.parent = _mapTransform;
            Route routeComponent = route.AddComponent<Route>();
            routeComponent.Construct(city1, city2, city1.CityPointView.transform.position, p2,
                city2.CityPointView.transform.position,
                resolution);
            _routes.Add(routeComponent);
            _routeTransform = route.transform;

            return routeComponent;
        }

        public void SetTempRouteWhite()
        {
            if (_tempLineRenderer.material.color != Color.white)
                _tempLineRenderer.material.color = Color.white;
        }

        public void SetTempRouteGreen()
        {
            if (_tempLineRenderer.material.color != Color.green)
                _tempLineRenderer.material.color = Color.green;
        }

        public void ClearLineRenderer()
        {
            if (_tempLineRenderer == null)
                return;

            _tempLineRenderer.positionCount = 0;
            Vector3[] position = {Vector3.zero};
            _tempLineRenderer.SetPositions(position);
        }

        public bool RouteExists(CityPoint city1, CityPoint city2)
        {
            foreach (Route route in _routes)
            {
                if (route.StartCityIndex == city1.Id && route.EndCityIndex == city2.Id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}