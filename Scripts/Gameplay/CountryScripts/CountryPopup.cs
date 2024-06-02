using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using _Fly_Connect.Scripts.Tutorial;
using MAXHelper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class CountryPopup : MonoWindow<ICountryPresenter>
    {
        [field: SerializeField] public TMP_Text Title { get; private set; }
        [field: SerializeField] public TMP_Text BuyButtonText { get; private set; }
        [field: SerializeField] public TMP_Text RewardPriceText { get; private set; }
        [field: SerializeField] public Button BuyButton { get; private set; }
        [field: SerializeField] public Button ExitButton { get; private set; }
        [field: SerializeField] public Button RewardButton { get; private set; }
        [field: SerializeField] public Image TicketIcon { get; private set; }
        [field: SerializeField] public Image RewardIcon { get; private set; }

        private bool _canShowReward = true;
        private ICountryPresenter _presenter;

        protected override void OnShow(ICountryPresenter args)
        {
            if (args is not ICountryPresenter presenter)
            {
                throw new Exception("Expected Country Presenter");
            }

            _presenter = presenter;

            presenter.OnBuyButtonStateChanged += UpdateButtonState;

            presenter.Enable();
            gameObject.SetActive(true);
            Title.SetText(presenter.Title);
            BuyButtonText.SetText(presenter.BuyButtonText);
            RewardPriceText.SetText(presenter.BuyButtonText);
            BuyButton.onClick.AddListener(OnBuyButtonClicked);
            RewardButton.onClick.AddListener(OnRewardButtonClicked);
            ExitButton.onClick.AddListener(OnExitButtonClicked);
            bool isBuyButtonInteractable = presenter.IsButtonInteractable;
            RewardButton.gameObject.SetActive(!presenter.IsButtonInteractable);
            BuyButton.interactable = isBuyButtonInteractable;
            ExitButton.gameObject.SetActive(true);

            SetRewardIcon();

            if (TutorialManager.Instance.CurrentStep == TutorialStep.BUY_COUNTRY ||
                TutorialManager.Instance.CurrentStep == TutorialStep.BUY_MORE_COUNTRY)
            {
                ExitButton.gameObject.SetActive(false);
            }

        }

        private void OnRewardButtonClicked()
        {
            _presenter.OnRewardButtonClicked();
            Hide();
            NotifyAboutClose();
        }

        protected override void OnHide()
        {
            _presenter.Disable();
            _presenter.OnBuyButtonStateChanged -= UpdateButtonState;
            BuyButton.onClick.RemoveListener(OnBuyButtonClicked);
            RewardButton.onClick.RemoveListener(OnRewardButtonClicked);
            ExitButton.onClick.RemoveListener(OnExitButtonClicked);
            gameObject.SetActive(false);
        }

        private void UpdateButtonState()
        {
            var interactable = _presenter.IsButtonInteractable;

            RewardButton.gameObject.SetActive(!interactable);

            BuyButton.interactable = interactable;

            SetRewardIcon();
        }

        private void SetRewardIcon()
        {
            if (_presenter.IsShowTicketIcon)
            {
                TicketIcon.gameObject.SetActive(true);
                RewardIcon.gameObject.SetActive(false);
            }
            else
            {
                TicketIcon.gameObject.SetActive(false);
                RewardIcon.gameObject.SetActive(true);
            }
        }

        private void OnBuyButtonClicked()
        {
            _presenter.OnBuyButtonClicked();
            Hide();
            NotifyAboutClose();
        }

        private void OnExitButtonClicked()
        {
            _presenter.OnExitButtonClicked();
            Hide();
            NotifyAboutClose();
        }
    }
}