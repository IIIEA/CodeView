using System;
using System.Collections;
using _Fly_Connect.Scripts.Gameplay;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Upgrades;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using Lean.Touch;
using UnityEngine;

namespace _Fly_Connect.Scripts.PopupScripts
{
    [Serializable]
    public class PathTapObserver : IGameStartListener, IGameFinishListener
    {
        private const float MINIMUM_ZOOM = 1.6f;

        private InputSystem _inputSystem;
        private AirplaneLaunchManager _airplaneLaunchManager;

        private Map _map;
        private UpgradePresenter _upgradePresenter;
        private bool isRoutineRunning = false;
        private ICoroutineRunner _coroutineRunner;
        private LeanPinchCamera _leanPinchCamera;

        [Inject]
        public void Construct(AirplaneLaunchManager airplaneLaunchManager, InputSystem inputSystem,
            Map map, UpgradePresenter upgradePresenter, ICoroutineRunner coroutineRunner,
            LeanPinchCamera leanPinchCamera)
        {
            _leanPinchCamera = leanPinchCamera;
            _coroutineRunner = coroutineRunner;
            _upgradePresenter = upgradePresenter;
            _map = map;
            _airplaneLaunchManager = airplaneLaunchManager;
            _inputSystem = inputSystem;
        }

        public void OnStartGame()
        {
            _inputSystem.OnPathTap += OnTap;
        }

        public void OnFinishGame()
        {
            _inputSystem.OnPathTap -= OnTap;
        }

        private void OnTap(Vector3 position)
        {
            if (_leanPinchCamera.Zoom <= MINIMUM_ZOOM)
                return;

            if (isRoutineRunning == false)
            {
                _coroutineRunner.StartCoroutine(ResetRoutineFlagAfterDelay(4));
                isRoutineRunning = true;
                var buyedCities = _map.GetBuyedCities();

                foreach (var city in buyedCities)
                {
                    if (city.Routes != null && city.Routes.Count > 0)
                    {
                        foreach (var route in city.Routes)
                        {
                            var airplane = _airplaneLaunchManager.GetAirplane(route);
                            route.SetGreenColorLine();

                            MoveController moveController = airplane.GetMoveController();
                            moveController.AddTemporarySpeed(_upgradePresenter.GetUFOIncome(), OnAddSpeedBoostComplete);
                        }
                    }
                }
            }
        }

        private void OnAddSpeedBoostComplete(Route route)
        {
            route.SetWhiteColorLine();
            isRoutineRunning = false;
        }

        private IEnumerator ResetRoutineFlagAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            isRoutineRunning = false;
        }
    }
}