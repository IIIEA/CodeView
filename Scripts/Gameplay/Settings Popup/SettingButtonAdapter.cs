using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Popup;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.SaveLoadSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public class SettingButtonAdapter : MonoBehaviour,
        IGameStartListener,
        IGameFinishListener
    {
        private Button _button;

        private PopupManager _popupManager;
        private RateUsManager _rateUsManager;

        [Inject]
        public void Construct(PopupManager popupManager, RateUsManager rateUsManager)
        {
            _rateUsManager = rateUsManager;
            _popupManager = popupManager;
        }

        public void OnStartGame()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        public void OnFinishGame()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            ISettingPresenter settingPresenter = new SettingPresenter(_popupManager, _rateUsManager);
            _popupManager.ShowPopup(PopupName.SETTINGS_POPUP, settingPresenter);
        }
    }
}