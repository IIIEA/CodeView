using _Fly_Connect.Scripts.Elementary.Timer;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Reward;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    public class BalanceModuleInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(CountrySeller))]
        private CountrySeller _countrySeller = new();

        [SerializeField, Service(typeof(AirportSeller))]
        private AirportSeller airportSeller = new();

        [SerializeField, Service(typeof(AirportUpgradeSeller))]
        private AirportUpgradeSeller _airportUpgradeSeller = new();

        [FormerlySerializedAs("tapSpeedUpgradeSeller")] [FormerlySerializedAs("_incomeMoneyUpgradeSeller")] [SerializeField, Service(typeof(UFOIncomeSeller))]
        private UFOIncomeSeller ufoIncomeSeller = new();

        [SerializeField, Service(typeof(AirportCapacityUpgradeSeller))]
        private AirportCapacityUpgradeSeller airportCapacityUpgradeSeller = new();

        [SerializeField, Service(typeof(SpeedUpgradeSeller))]
        private SpeedUpgradeSeller _speedUpgradeSeller = new();

        [FormerlySerializedAs("ufoSeller")]
        [FormerlySerializedAs("_airplaneSeller")]
        [SerializeField, Service(typeof(AirplaneSeller))]
        private AirplaneSeller airplaneSeller = new();

        [SerializeField, Service(typeof(AirplaneUpgradeSeller))]
        private AirplaneUpgradeSeller _airplaneUpgradeSeller = new();

        [FormerlySerializedAs("_goldenAirplaneSeller")] [SerializeField, Service(typeof(UFOSeller))]
        private UFOSeller ufoSeller = new();

        [SerializeField, Service(typeof(ShowplaceRewardSeller))]
        private ShowplaceRewardSeller _showplaceRewardSeller = new();

        [SerializeField, Listener, Service(typeof(RewardManager))]
        private RewardManager _rewardManager;
    }
}