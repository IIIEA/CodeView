using _Fly_Connect.Scripts.Gameplay.Sellers;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class UpgradeSellerSaveLoader : SaveLoader<AirportUpgradeSellerData, AirportUpgradeSeller>
    {
        protected override void SetupData(AirportUpgradeSeller starterPackManager, AirportUpgradeSellerData startPackData)
        {
            starterPackManager.Setup(startPackData.OrderUpgradeAirport, startPackData.MaxLevelCityCount, startPackData.CityCount);
        }

        protected override AirportUpgradeSellerData ConvertToData(AirportUpgradeSeller service)
        {
            return new()
            {
                OrderUpgradeAirport = service.OrderUpgradeAirport,
                MaxLevelCityCount = service.MaxLevelCityCount,
                CityCount = service.CityCount
            };
        }
    }
}