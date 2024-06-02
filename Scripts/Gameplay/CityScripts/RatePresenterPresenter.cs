using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.SaveLoadSystem;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    public class RatePresenterPresenter : IRateUsPresenter
    {
        private RateUsManager _rateUsManager;
        private PopupManager _popupManager;

        public RatePresenterPresenter(RateUsManager rateUsManager, PopupManager popupManager)
        {
            _popupManager = popupManager;
            _rateUsManager = rateUsManager;
        }

        public void OnRateButtonClicked()
        {
            _rateUsManager.OnRateUs += OnRateUs;
            _rateUsManager.OnErrorRateUs += OnErrorRateUS;
            _rateUsManager.RateUs();
        }

        private void OnRateUs()
        {
            _rateUsManager.OnRateUs -= OnRateUs;
            _rateUsManager.OnErrorRateUs -= OnErrorRateUS;
            _popupManager.HidePopup(PopupName.RATE_US_POPUP);
        }

        private void OnErrorRateUS()
        {
            _rateUsManager.OnRateUs -= OnRateUs;
            _rateUsManager.OnErrorRateUs -= OnErrorRateUS;
            _popupManager.HidePopup(PopupName.RATE_US_POPUP);
        }
    }
}