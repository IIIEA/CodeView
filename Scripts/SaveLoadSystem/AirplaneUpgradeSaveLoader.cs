using _Fly_Connect.Scripts.Gameplay.Sellers;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class AirplaneUpgradeSaveLoader : SaveLoader<AirplaneUpgradeSellerData, AirplaneUpgradeSeller>
    {
        protected override void SetupData(AirplaneUpgradeSeller starterPackManager, AirplaneUpgradeSellerData startPackData)
        {
            starterPackManager.Setup(startPackData.OrderAirplaneUpgrade);
        }

        protected override AirplaneUpgradeSellerData ConvertToData(AirplaneUpgradeSeller service)
        {
            return new()
            {
                OrderAirplaneUpgrade = service.OrderAirplaneUpgrade
            };
        }
    }
}