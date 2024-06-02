using System;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Tutorial;
using Dreamteck.Splines;
using Lean.Touch;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace _Fly_Connect.Scripts.Gameplay.UfoScripts
{
    [Serializable]
    public class UFOAirplaneManager : IGameUpdateListener
    {
        [SerializeField] private float _minimalZoom = 9.7f;
        [SerializeField] [ReadOnly] private float _elapsedTime;
        [SerializeField] private SplineComputer _spline;
        [SerializeField] private UFO _ufoPrefab;

        private UFOPathGenerator _ufoPathGenerator = new();
        private Camera _camera;
        private LeanPinchCamera _leanPinchCamera;
        private bool _canSpawn = true;
        public UFO CurrentUfo { get; private set; }
        private int _timeBetweenSpawnUFO = 60;
        private StarterPackManager _starterPackManager;

        [Inject]
        private void Construct(LeanPinchCamera leanPinchCamera, Camera camera, StarterPackManager starterPackManager)
        {
            _starterPackManager = starterPackManager;
            _camera = camera;
            _leanPinchCamera = leanPinchCamera;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!TutorialManager.Instance.IsCompleted)
                return;
            
            if (_canSpawn == false)
                return;

            _elapsedTime += deltaTime;

            if (_elapsedTime >= _timeBetweenSpawnUFO && _leanPinchCamera.Zoom < _minimalZoom)
            {
                _canSpawn = false;
                _elapsedTime = 0;
                UFO ufo = Object.Instantiate(_ufoPrefab, _spline.transform);
                CurrentUfo = ufo;

                GeneratePath();

                ufo.Construct(_spline);
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
                CurrentUfo.SplineFollower.onEndReached += OnPathEnd;
            }
        }

        private void GeneratePath()
        {
            var points = _ufoPathGenerator.GeneratePoints();

            SplinePoint[] allPoints = new SplinePoint[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                SplinePoint point = new();
                point.SetPosition(points[i]);
                point.normal = Vector3.up;
                point.size = 0.06f;
                point.color = Color.white;
                allPoints[i] = point;
            }

            _spline.SetPoints(allPoints);
        }

        private void OnPathEnd(double _)
        {
            TryDestroyUFO();
        }

        public void TryDestroyUFO()
        {
            if (CurrentUfo != null)
            {
                Object.Destroy(CurrentUfo.gameObject);
                CurrentUfo.SplineFollower.onEndReached -= OnPathEnd;
                _canSpawn = true;
                CurrentUfo = null;
            }
        }
    }
}