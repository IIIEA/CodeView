using System;
using System.Collections.Generic;
using System.Numerics;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Gameplay.Upgrades;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Reward;
using _Fly_Connect.Scripts.SaveLoadSystem;
using _Fly_Connect.Scripts.Sound.Scene_Audio_Manager;
using _Fly_Connect.Visual;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    [Serializable]
    public class AirplaneLaunchManager
    {
        [SerializeField] private Airplane _airplanePrefab;
        [SerializeField] private GameManager _gameManager;

        private MoneyStorage _moneyStorage;
        private MoneyPool.MoneyPool _moneyPool;
        private RewardManager _rewardManager;
        private List<Airplane> _cachedAirplanes = new();
        private UpgradePresenter _upgradePresenter;
        private SpeedBoosterModel _speedBoosterModel;
        private bool _canChangeScale = true;

        public Action<Airplane> OnAirplaneLaunched;
        public IReadOnlyList<Airplane> CachedAirplanes => _cachedAirplanes;

        [Inject]
        private void Construct(ICoroutineRunner coroutineRunner, GameRepository gameRepository,
            MoneyStorage moneyStorage, MoneyPool.MoneyPool moneyPool, RewardManager rewardManager,
            UpgradePresenter upgradePresenter, SpeedBoosterModel speedBoosterModel)
        {
            _speedBoosterModel = speedBoosterModel;
            _upgradePresenter = upgradePresenter;
            _rewardManager = rewardManager;
            _moneyPool = moneyPool;
            _moneyStorage = moneyStorage;
        }

        public void EnableAirplane()
        {
            if (_cachedAirplanes.Count == 0)
                return;

            bool active = _cachedAirplanes[0].gameObject.activeSelf;

            if (active == false)
                _cachedAirplanes.ForEach(airplane => airplane.gameObject.SetActive(true));
        }

        public void DisableAirplane()
        {
            if (_cachedAirplanes.Count == 0)
                return;

            bool active = _cachedAirplanes[0].gameObject.activeSelf;

            if (active)
                _cachedAirplanes.ForEach(airplane => airplane.gameObject.SetActive(false));
        }

        private void OnFinish(Direction direction, Vector3 position, Route route, Airplane airplane)
        {
            var cityLevels = route.GetCitiesLevel();
            var reward = _rewardManager.CalculateReward(cityLevels, airplane);

            ShowMoneyEffect(direction, reward, position);

            _moneyStorage.EarnMoney(direction == Direction.Forward ? reward.Item1 : reward.Item2);
        }

        private void ShowMoneyEffect(Direction direction, (int, int) reward, Vector3 position)
        {
            var moneyEffect = _moneyPool.Get();

            if (direction == Direction.Forward)
            {
                moneyEffect.transform.position = position;
                moneyEffect.Show("+" + reward.Item1);
            }
            else
            {
                moneyEffect.transform.position = position;
                moneyEffect.Show("+" + reward.Item2);
            }

            moneyEffect.OnAnimationCompleted += OnAnimationCompleted;
        }

        private void OnAnimationCompleted(AddMoneyEffect moneyEffect)
        {
            moneyEffect.OnAnimationCompleted -= OnAnimationCompleted;
            _moneyPool.Release(moneyEffect);
        }

        private void SetAirplaneSpeed(Airplane airplane)
        {
            var speed = _upgradePresenter.GetSpeedAddedValue();
            airplane.SetAddedSpeed(speed);
        }

        public void AddTemporarySpeed()
        {
            foreach (var airplane in _cachedAirplanes)
            {
                MoveController moveController = airplane.GetMoveController();
                moveController.AddTemporarySpeed(_upgradePresenter.GetUFOIncome() * 2);
            }
        }

        public void EnableBoosterSpeed()
        {
            foreach (var airplane in _cachedAirplanes)
            {
                MoveController moveController = airplane.GetMoveController();
                moveController.EnableX2Speed();
            }
        }

        public void DisableBoosterSpeed()
        {
            foreach (var airplane in _cachedAirplanes)
            {
                MoveController moveController = airplane.GetMoveController();
                moveController.DisableX2Speed();
            }
        }

        public Airplane GetAirplane(Route route)
        {
            return route.Airplane;
        }

        public void LaunchAirplane(Route route, int level)
        {
            Airplane airplane = CreateAirplane(route.transform);
            airplane.GetMoveController().OnFinish += OnFinish;
            _cachedAirplanes.Add(airplane);

            airplane.SetLevel(level);
            SetAirplaneSpeed(airplane);
            route.SetAirplane(airplane);

            if (_speedBoosterModel.IsPlaying)
                airplane.GetMoveController().EnableX2Speed();

            OnAirplaneLaunched?.Invoke(airplane);

            if (_cachedAirplanes[0].HasTriangleRegime)
            {
                airplane.EnableTriangleRegime();
            }

            var gameListeners = airplane.GetComponents<IGameListener>();
            _gameManager.AddListeners(gameListeners);
        }

        public void IncreaseSpeed(float addedSpeed)
        {
            foreach (var airplane in _cachedAirplanes)
            {
                airplane.SetAddedSpeed(addedSpeed);
            }
        }

        private Airplane CreateAirplane(Transform transform)
        {
            Airplane airplane = Object.Instantiate(_airplanePrefab, transform);
            return airplane;
        }

        public void SetAirplaneScale(float scale, bool isNeedChangeScale = false)
        {
            if (_cachedAirplanes.Count == 0)
                return;

            if (_cachedAirplanes[0].transform.localScale.x == scale && !isNeedChangeScale)
                return;

            if (!_canChangeScale)
                return;

            _canChangeScale = false;

            foreach (var airplane in _cachedAirplanes)
            {
                airplane.transform.DOScale(scale, 0.5f).OnComplete((() => _canChangeScale = true));
            }
        }
    }
}