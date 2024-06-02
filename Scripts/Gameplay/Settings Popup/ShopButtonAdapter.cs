using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using MAXHelper;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public class ShopButtonAdapter : MonoBehaviour,
        IGameStartListener,
        IGameFinishListener
    {
        private Button _button;
        private PopupManager _popupManager;

        [Inject]
        public void Construct(PopupManager popupManager)
        {
            _popupManager = popupManager;
        }

        public void OnStartGame()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
            _popupManager.OnPopupHidden += OnHiddenPopup;
        }

        public void OnFinishGame()
        {
            _popupManager.OnPopupHidden -= OnHiddenPopup;
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            IShopPresenter presenter = new ShopPresenter();
            _popupManager.ShowPopup(PopupName.SHOP_POPUP, presenter);
            AdsManager.ToggleBanner(false);
        }

        private void OnHiddenPopup(PopupName name)
        {
            if (name == PopupName.SHOP_POPUP)
            {
                AdsManager.ToggleBanner(true);
            }
        }
    }
}