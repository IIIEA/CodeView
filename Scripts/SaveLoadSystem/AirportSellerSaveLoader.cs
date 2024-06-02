using _Fly_Connect.Scripts.Gameplay.Sellers;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class AirportSellerSaveLoader : SaveLoader<CitySellerData, AirportSeller>
    {
        protected override void SetupData(AirportSeller starterPackManager, CitySellerData startPackData)
        {
            starterPackManager.Setup(startPackData.CurrentOpenedAirport);
        }

        protected override CitySellerData ConvertToData(AirportSeller service)
        {
            return new()
            {
                CurrentOpenedAirport = service.CurrentOpenedCity
            };
        }
    }
}