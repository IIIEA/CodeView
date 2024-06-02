using MadPixel.InApps;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class StarterPackPresenter : IStarterPackPresenter
    {
        public void OnBuyButtonClicked()
        {
            MobileInAppPurchaser.BuyProduct("idle.clicker.airline.manager_starter_pack");
        }
    }
}