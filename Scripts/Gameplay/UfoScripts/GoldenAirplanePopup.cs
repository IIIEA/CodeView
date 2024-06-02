using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.GoldenAirplane
{
    public class GoldenAirplanePopup : MonoWindow<IGoldenAirplanePresenter>
    {
        [SerializeField] public Button _rewardButton;
        [SerializeField] public Button _moneyButton;
        [SerializeField] public Button _closeButton;
        [SerializeField] public TMP_Text _moneyText;
        [SerializeField] public TMP_Text _rewardText;
        [field: SerializeField] public Image TicketIcon { get; private set; }
        [field: SerializeField] public Image RewardIcon { get; private set; }

        private IGoldenAirplanePresenter _presenter;

        protected override void OnShow(IGoldenAirplanePresenter args)
        {
            if (args is not IGoldenAirplanePresenter presenter)
            {
                throw new Exception("Expected Golden Airplane Presenter");
            }

            _presenter = presenter;
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _rewardButton.onClick.AddListener(OnRewardButtonClicked);
            _moneyButton.onClick.AddListener(OnMoneyButtonClicked);
            _moneyText.SetText(_presenter.MoneyPrice);
            _rewardText.SetText(_presenter.RewardPrice);
            SetRewardIcon();

        }

        private void OnRewardButtonClicked()
        {
            _presenter.OnRewardButtonClicked();
            NotifyAboutClose();
            Hide();
        }

        private void OnMoneyButtonClicked()
        {
            _presenter.OnMoneyButtonClicked();
            NotifyAboutClose();
            Hide();
        }

        private void OnCloseButtonClicked()
        {
            NotifyAboutClose();
            Hide();
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

        protected override void OnHide()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _rewardButton.onClick.RemoveListener(OnRewardButtonClicked);
            _moneyButton.onClick.RemoveListener(OnMoneyButtonClicked);
            gameObject.SetActive(false);
        }
    }
}