using System;
using System.Collections;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using MadPixel.InApps;
using MAXHelper;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;
using Product = UnityEngine.Purchasing.Product;

namespace _Fly_Connect.Scripts.TicketManager
{
    public sealed class PurchasesManager : MonoBehaviour, IGameStartListener, IGameFinishListener, IGameUpdateListener
    {
        private MoneyStorage _moneyStorage;
        private RemoveAdManager _removeAdManager;
        private StarterPackManager _starterPackManager;
        private StarterPackView _starterPackView;
        private RemoveAllAdsView _removeAllAdsView;
        private NoAdsButtonAdapter _noAdsButtonAdapter;
        private PopupManager _popupManager;
        private IncomeCounter _incomeCounter;
        private bool _isInit;

        public event Action OnBuyNoAds;

        [Inject]
        private void Construct(MoneyStorage moneyStorage, RemoveAdManager removeAdManager,
            StarterPackManager starterPackManager, RemoveAllAdsView removeAllAdsView, StarterPackView starterPackView,
            NoAdsButtonAdapter noAdsButtonAdapter, PopupManager popupManager)
        {
            _popupManager = popupManager;
            _noAdsButtonAdapter = noAdsButtonAdapter;
            _removeAllAdsView = removeAllAdsView;
            _starterPackView = starterPackView;
            _starterPackManager = starterPackManager;
            _removeAdManager = removeAdManager;
            _moneyStorage = moneyStorage;
        }

        public void OnUpdate(float deltaTime)
        {
            if (MobileInAppPurchaser.Instance.IsInitialized() && _isInit == false)
            {
                _isInit = true;
                MobileInAppPurchaser.Instance.OnPurchaseResult += OnPurchaseResult;
            }
        }
        
        public void RestorePurchases()
        {
            MobileInAppPurchaser.Instance.RestorePurchases();
        }

        public void OnStartGame()
        {
            // MobileInAppPurchaser.Instance.OnPurchaseResult += OnPurchaseResult;
        }


        public void OnFinishGame()
        {
            MobileInAppPurchaser.Instance.OnPurchaseResult -= OnPurchaseResult;
        }

        private void OnPurchaseResult(Product product)
        {
            if (product != null)
            {
                if (product.definition.id == "idle.clicker.airline.manager_10_tickets")
                {
                    _moneyStorage.EarnTicket(10);
                }
                else if (product.definition.id == "idle.clicker.airline.manager_20_tickets")
                {
                    _moneyStorage.EarnTicket(20);
                }
                else if (product.definition.id == "idle.clicker.airline.manager_30_tickets")
                {
                    _moneyStorage.EarnTicket(30);
                }
                else if (product.definition.id == "idle.clicker.airline.manager_starter_pack")
                {
                    _removeAdManager.Setup(true);
                    _starterPackManager.Setup(true);
                    _moneyStorage.EarnTicket(5);
                    _moneyStorage.EarnMoney(200000);
                    _starterPackView.gameObject.SetActive(false);
                    _removeAllAdsView.gameObject.SetActive(false);
                    _noAdsButtonAdapter.gameObject.SetActive(false);
            
                    if (_popupManager.IsPopupActive(PopupName.STARTER_PACK_POPUP))
                        _popupManager.HidePopup(PopupName.STARTER_PACK_POPUP);
            
                    AdsManager.CancelAllAds();
                    OnBuyNoAds?.Invoke();
                }
                else if (product.definition.id == "idle.clicker.airline.manager_no_ads")
                {
                    _removeAllAdsView.gameObject.SetActive(false);
                    _noAdsButtonAdapter.gameObject.SetActive(false);
                    _removeAdManager.Setup(true);
                    AdsManager.CancelAllAds();
                    OnBuyNoAds?.Invoke();
                }
            
                Debug.LogWarning($"Purchase complete! Implement logic for {product.definition.id}");
            }
            else
            {
                Debug.LogError($"Purchase {product.definition.id} went wrong!");
            }
        }
    }
}