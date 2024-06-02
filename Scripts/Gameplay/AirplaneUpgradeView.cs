using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay
{
    public class AirplaneUpgradeView : MonoBehaviour
    {
        [SerializeField] public Image _icon;
        [SerializeField] public TMP_Text _levelText;
        [SerializeField] public TMP_Text _buyButtonText;
        [SerializeField] public TMP_Text _rewardPriceText;
        [field: SerializeField] public Button BuyButton { get; private set; }
        [field: SerializeField] public Button RewardButton { get; private set; }

        public event Action<AirplaneUpgradeView> OnButtonClicked;
        public event Action<AirplaneUpgradeView> OnRewardButtonClicked;

        private void OnEnable()
        {
            BuyButton.onClick.AddListener(ButtonClicked);
            RewardButton.onClick.AddListener(RewardButtonClicked);
        }

        private void OnDisable()
        {
            BuyButton.onClick.RemoveListener(ButtonClicked);
            RewardButton.onClick.RemoveListener(RewardButtonClicked);
        }

        public void SetLevel(string level)
        {
            _levelText.SetText(level);
        }

        public void SetBuyButtonText(string buyButtonText)
        {
            _buyButtonText.SetText(buyButtonText);
            _rewardPriceText.SetText(buyButtonText);
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        private void ButtonClicked()
        {
            OnButtonClicked?.Invoke(this);
        }

        private void RewardButtonClicked()
        {
            OnRewardButtonClicked?.Invoke(this);
        }

        public void ActivateAdState()
        {
            RewardButton.gameObject.SetActive(true);
        }

        public void DeactivateAdState()
        {
            RewardButton.gameObject.SetActive(false);
        }

        public void DeactivateBuyButton()
        {
            BuyButton.gameObject.SetActive(false);
            RewardButton.gameObject.SetActive(false);
        }

        public void ActivateBuyButton()
        {
            BuyButton.gameObject.SetActive(true);
        }
    }
}