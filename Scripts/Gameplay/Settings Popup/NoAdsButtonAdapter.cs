using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Tutorial;
using MadPixel.InApps;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public class NoAdsButtonAdapter : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        private Button _buyButton;
        private StarterPackManager _starterPackManager;
        private RemoveAdManager _removeAdManager;

        [Inject]
        public void Construct(RemoveAdManager removeAdManager, StarterPackManager starterPackManager)
        {
            _removeAdManager = removeAdManager;
            _starterPackManager = starterPackManager;
        }

        private void Awake()
        {
            _buyButton = GetComponent<Button>();
        }

        public void OnStartGame()
        {
            _buyButton.onClick.AddListener(OnButtonClicked);

            if (_removeAdManager.IsBuy || _starterPackManager.IsBuy || !TutorialManager.Instance.IsCompleted)
            {
                gameObject.SetActive(false);
            }

            TutorialManager.Instance.OnCompleted += OnTutorialCompleted;
        }

        public void OnFinishGame()
        {
            _buyButton.onClick.RemoveListener(OnButtonClicked);
            TutorialManager.Instance.OnCompleted -= OnTutorialCompleted;
        }

        private void OnTutorialCompleted()
        {
            if (_removeAdManager.IsBuy || _starterPackManager.IsBuy)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        private void OnButtonClicked()
        {
            MobileInAppPurchaser.BuyProduct("idle.clicker.airline.manager_no_ads");
        }
    }
}