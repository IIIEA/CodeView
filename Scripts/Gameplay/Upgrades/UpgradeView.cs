using System;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Upgrades
{
    public class UpgradeView : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public Button RewardButton { get; private set; }
        [field: SerializeField] public UpgradeType UpgradeType { get; private set; }
        [field: SerializeField] public TMP_Text PriceText { get; private set; }
        [field: SerializeField] public TMP_Text RewardPriceText { get; private set; }
        [field: SerializeField] public TMP_Text AddedValueText { get; private set; }
        [field: SerializeField] public Image AdImage { get; private set; }
        [field: SerializeField] public Sprite AdIcon { get; private set; }
        [field: SerializeField] public Sprite TicketIcon { get; private set; }

        public event Action<UpgradeView, bool> OnUpgradeButtonClicked;
        public event Action<UpgradeView> OnRewardButtonClicked;

        public void OnStartGame()
        {
            Button.onClick.AddListener(OnButtonClicked);
            RewardButton.onClick.AddListener(OnRewardClicked);
        }

        public void OnFinishGame()
        {
            Button.onClick.RemoveListener(OnButtonClicked);
            RewardButton.onClick.RemoveListener(OnRewardClicked);
        }

        public void SetAdIcon()
        {
            AdImage.sprite = AdIcon;
        }

        public void SetTicketIcon()
        {
            AdImage.sprite = TicketIcon;
        }

        public void SetPrice(string text)
        {
            PriceText.SetText(text);
            RewardPriceText.SetText(text);
        }

        public void SetAddedValue(string text)
        {
            AddedValueText.SetText(text);
        }

        public void SetButtonInteractable(bool interactable)
        {
            Button.interactable = interactable;
        }

        private void OnButtonClicked()
        {
            OnUpgradeButtonClicked?.Invoke(this, true);
        }

        private void OnRewardClicked()
        {
            OnRewardButtonClicked?.Invoke(this);
        }
    }
}