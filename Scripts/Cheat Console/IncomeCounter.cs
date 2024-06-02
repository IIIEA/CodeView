using System;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Reward;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Fly_Connect.Scripts.Cheat_Console
{
    [Serializable]
    public class IncomeCounter
    {
        [SerializeField] private TMP_Text _counter;
        [SerializeField] private long _incomePerMinute = 0;

        private RewardManager _rewardManager;
        private long _totalReward;
        private AirplaneRouteFactory _airplaneRouteFactory;

        public long IncomePerMinute => _incomePerMinute;

        public event Action OnIncomeChanged;

        [Inject]
        private void Construct(RewardManager rewardManager, AirplaneRouteFactory airplaneRouteFactory)
        {
            _airplaneRouteFactory = airplaneRouteFactory;
            _rewardManager = rewardManager;
        }

        public void Setup(long income)
        {
            _incomePerMinute = income;
            BigNumber number = new BigNumber(_incomePerMinute);
            _counter.text = $"{number} / per minute";
            OnIncomeChanged?.Invoke();
        }

        public void CalculateIncome()
        {
            var routes = _airplaneRouteFactory.Routes;

            if (routes.Count <= 0)
                return;

            _totalReward = 0;

            for (int i = 0; i < routes.Count; i++)
            {
                float distance = routes[i].Distance;
                float speed = Mathf.Abs(routes[i].Airplane.GetMoveController().TotalSpeed);
                float time = distance / speed;
                var cityLevels = routes[i].GetCitiesLevel();
                int flightCount = (int) (60 / time);
                int halfFlightCount = flightCount / 2;

                for (int j = 0; j < flightCount; j++)
                {
                    var reward = _rewardManager.CalculateReward(cityLevels, routes[i].Airplane, false);

                    _totalReward += j < halfFlightCount ? reward.Item1 : reward.Item2;
                }
            }

            _incomePerMinute = _totalReward;
            BigNumber number = new BigNumber(_incomePerMinute);
            _counter.text = $"{number} / per minute";
            OnIncomeChanged?.Invoke();
        }
    }
}