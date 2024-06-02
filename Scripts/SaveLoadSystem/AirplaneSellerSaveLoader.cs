using _Fly_Connect.Scripts.Gameplay.Sellers;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class AirplaneSellerSaveLoader : SaveLoader<AirplaneSellerData, AirplaneSeller>
    {
        protected override void SetupData(AirplaneSeller starterPackManager, AirplaneSellerData startPackData)
        {
            starterPackManager.Setup(startPackData.OrderUpgradeAirport);
        }

        protected override AirplaneSellerData ConvertToData(AirplaneSeller service)
        {
            return new()
            {
                OrderUpgradeAirport = service.OrderAirplaneBuyed
            };
        }
    }
}