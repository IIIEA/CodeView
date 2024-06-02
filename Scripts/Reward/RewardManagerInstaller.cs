using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Reward
{
    public class RewardManagerInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(RewardManager))]
        private RewardManager _rewardManager;
    }
}