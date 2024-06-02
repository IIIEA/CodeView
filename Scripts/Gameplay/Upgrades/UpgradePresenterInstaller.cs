using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Upgrades
{
    public class UpgradePresenterInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(UpgradePresenter))]
        private UpgradePresenter _upgradePresenter;
    }
}