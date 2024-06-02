using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using UniLinq;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class MapSaveLoader : SaveLoader<MapData, Map>
    {
        protected override void SetupData(Map starterPackManager, MapData startPackData)
        {
            starterPackManager.Setup(startPackData.OrderBuyedCountry);
        }

        protected override MapData ConvertToData(Map service)
        {
            return new()
            {
                OrderBuyedCountry = service.OrderBuyedCountry,
            };
        }
    }
}