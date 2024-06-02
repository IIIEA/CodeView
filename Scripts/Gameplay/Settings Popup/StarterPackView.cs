using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using MadPixel.InApps;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public class StarterPackView : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        [SerializeField] private Button _buyButton;

        private StarterPackManager _starterPackManager;

        [Inject]
        public void Construct(StarterPackManager starterPackManager)
        {
            _starterPackManager = starterPackManager;
        }

        public void OnStartGame()
        {
            _buyButton.onClick.AddListener(OnBuyButtonClicked);

            if (_starterPackManager.IsBuy)
            {
                gameObject.SetActive(false);
            }
        }

        public void OnFinishGame()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }

        private void OnBuyButtonClicked()
        {
            MobileInAppPurchaser.BuyProduct("idle.clicker.airline.manager_starter_pack");
        }
    }
}