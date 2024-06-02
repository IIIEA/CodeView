using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.SaveLoadSystem;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public sealed class SettingPresenter : ISettingPresenter
    {
        private PopupManager _popupManager;
        private RateUsManager _rateUsManager;

        public bool IsShowRateUsButton => _rateUsManager.IsRated == false;

        public SettingPresenter(PopupManager popupManager, RateUsManager rateUsManager)
        {
            _rateUsManager = rateUsManager;
            _popupManager = popupManager;
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }

        public void OnExitButtonClicked()
        {
        }

        public void OnRateButtonClicked()
        {
            IRateUsPresenter presenter = new RatePresenterPresenter(_rateUsManager, _popupManager);
            _popupManager.ShowPopup(PopupName.RATE_US_POPUP, presenter);
        }
    }
}