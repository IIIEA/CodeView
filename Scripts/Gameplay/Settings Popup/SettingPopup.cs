using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public sealed class SettingPopup : MonoWindow<ISettingPresenter>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _rateButtonUs;
        [SerializeField] private Image _airplaneImage;

        private ISettingPresenter _presenter;

        protected override void OnShow(ISettingPresenter args)
        {
            if (args is not ISettingPresenter presenter)
            {
                throw new Exception("Expected Setting Presenter");
            }

            _presenter = presenter;

            SetRateUsButtonState();

            gameObject.SetActive(true);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _rateButtonUs.onClick.AddListener(OnRateButtonUsClicked);
        }

        private void SetRateUsButtonState()
        {
            if (_presenter.IsShowRateUsButton)
            {
                _rateButtonUs.gameObject.SetActive(true);
                _airplaneImage.gameObject.SetActive(false);
            }
            else
            {
                _rateButtonUs.gameObject.SetActive(false);
                _airplaneImage.gameObject.SetActive(true);
            }
        }

        private void OnRateButtonUsClicked()
        {
            _presenter.OnRateButtonClicked();
            Hide();
            NotifyAboutClose();
        }

        private void OnCloseButtonClicked()
        {
            Hide();
            NotifyAboutClose();
        }

        protected override void OnHide()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            gameObject.SetActive(false);
        }
    }
}