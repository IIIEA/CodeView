using System;
using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.PopupScripts.Window;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class ShopPopup : MonoWindow<IShopPresenter>
    {
        [SerializeField] private Button _closeButton;

        private IShopPresenter _presenter;

        protected override void OnShow(IShopPresenter args)
        {
            if (args is not IShopPresenter presenter)
            {
                throw new Exception("Expected Country Presenter");
            }

            _presenter = presenter;
            _closeButton.onClick.AddListener(OnExitButtonClicked);
        }

        protected override void OnHide()
        {
            _closeButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            _presenter.OnExitButtonClicked();
            Hide();
            NotifyAboutClose();
        }
    }
}