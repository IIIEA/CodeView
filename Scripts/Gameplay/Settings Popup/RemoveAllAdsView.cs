using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using MadPixel.InApps;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public class RemoveAllAdsView : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        [SerializeField] private Button _buyButton;

        private RemoveAdManager _removeAdManager;

        [Inject]
        public void Construct(RemoveAdManager removeAdManager)
        {
            _removeAdManager = removeAdManager;
        }

        public void OnStartGame()
        {
            if (_removeAdManager.IsBuy)
            {
                gameObject.SetActive(false);
            }

            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        public void OnFinishGame()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }

        private void OnBuyButtonClicked()
        {
            MobileInAppPurchaser.BuyProduct("idle.clicker.airline.manager_no_ads");
        }
    }
}