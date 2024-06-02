using _Fly_Connect.Scripts.Gameplay.Upgrades;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class UpgradeSaveLoader : SaveLoader<UpgradeData, UpgradePresenter>
    {
        protected override void SetupData(UpgradePresenter starterPackManager, UpgradeData startPackData)
        {
            starterPackManager.SetupUpgradeInfo(startPackData);
        }

        protected override UpgradeData ConvertToData(UpgradePresenter service)
        {
            return service.GetUpgradeInfo();
        }
    }
}