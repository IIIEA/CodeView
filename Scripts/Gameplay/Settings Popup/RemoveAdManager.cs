using MAXHelper;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public class RemoveAdManager
    {
        public bool IsBuy { get; private set; }

        public void Setup(bool isBuy)
        {
            IsBuy = isBuy;

            if (IsBuy)
                AdsManager.CancelAllAds();
        }
    }
}