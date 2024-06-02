using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public sealed class StarterPackPopup : MonoWindow<IStarterPackPresenter>
    {
        [field: SerializeField] public Button BuyButton { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }

        private IStarterPackPresenter _presenter;

        protected override void OnShow(IStarterPackPresenter args)
        {
            if (args is not IStarterPackPresenter presenter)
            {
                throw new Exception("Expected Country Presenter");
            }

            _presenter = presenter;

            BuyButton.onClick.AddListener(OnBuyButtonClicked);
            CloseButton.onClick.AddListener(OnCloseButtonClicked);
        }

        protected override void OnHide()
        {
            BuyButton.onClick.RemoveListener(OnBuyButtonClicked);
            CloseButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void OnBuyButtonClicked()
        {
            _presenter.OnBuyButtonClicked();
        }

        private void OnCloseButtonClicked()
        {
            Hide();
            NotifyAboutClose();
        }
    }
}