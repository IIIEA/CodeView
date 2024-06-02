using _Fly_Connect.Scripts.Gameplay.CountryScripts;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class CountrySellerSaveLoader : SaveLoader<CountrySellerData, CountrySeller>
    {
        protected override void SetupData(CountrySeller starterPackManager, CountrySellerData startPackData)
        {
            starterPackManager.Setup(startPackData.CurrentOpenedCountry);
        }

        protected override CountrySellerData ConvertToData(CountrySeller service)
        {
            return new CountrySellerData()
            {
                CurrentOpenedCountry = service.CurrentOpenedCountry
            };
        }
    }
}