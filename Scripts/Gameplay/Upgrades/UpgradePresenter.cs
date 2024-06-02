using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Reward;
using _Fly_Connect.Scripts.SaveLoadSystem;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using _Fly_Connect.Scripts.TicketManager;
using MAXHelper;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Upgrades
{
    public class UpgradePresenter : SerializedMonoBehaviour, IGameStartListener, IGameFinishListener,
        IGameUpdateListener
    {
        [OdinSerialize] private List<UpgradeView> _upgradeViews = new();
        [OdinSerialize] private List<UpgradeModel> _upgradeModels = new();
        [OdinSerialize] private HorizontalLayoutGroup _layoutGroup;

        private GameManager _gameManager;
        private AirplaneLaunchManager _airplaneLaunchManager;
        private MoneyStorage _moneyStorage;
        private SpeedUpgradeSeller _speedUpgradeSeller;
        private AirportCapacityUpgradeSeller _airportCapacityUpgradeSeller;
        private UFOIncomeSeller _ufoIncomeSeller;
        private RewardManager _rewardManager;
        private MoneyPool.MoneyPool _moneyPool;
        private IncomeCounter _incomeCounter;
        private UpgradeView _rewardClickedView;
        private bool _canShowReward = true;
        private RemoveAdManager _removeAdManager;
        private PurchasesManager _purchasesManager;

        public void SetupUpgradeInfo(UpgradeData upgradeDate)
        {
            foreach (var model in _upgradeModels)
            {
                if (model.UpgradeType == UpgradeType.AIRPLANE_SPEED)
                {
                    model.SetLevel(upgradeDate.SpeedLevel);
                    var currentPrice = _speedUpgradeSeller.GetCurrentPrice(model.Level);
                    BigNumber price = new BigNumber(currentPrice);
                    float addedValue = _speedUpgradeSeller.AddedValue * model.Level;
                    model.UpdateModel(addedValue, price);
                }
                else if (model.UpgradeType == UpgradeType.UFO_INCOME)
                {
                    model.SetLevel(upgradeDate.UfoSeller);
                    int currentPrice = _ufoIncomeSeller.GetCurrentPrice(model.Level);
                    BigNumber price = new BigNumber(currentPrice);
                    float addedValue = _ufoIncomeSeller.AddedValue * model.Level;
                    model.UpdateModel(addedValue, price);
                }
                else if (model.UpgradeType == UpgradeType.INCOME)
                {
                    model.SetLevel(upgradeDate.AirportCapacityLevel);
                    var currentPrice = _airportCapacityUpgradeSeller.GetCurrentPrice(model.Level);
                    BigNumber price = new BigNumber(currentPrice);
                    float addedValue = _airportCapacityUpgradeSeller.AddedValue * model.Level;
                    model.UpdateModel(addedValue, price);
                    _rewardManager.SetAddedMoneyOnAirplaneCapacity(addedValue);
                }
            }
        }

        [Inject]
        private void Construct(GameManager gameManager, AirplaneLaunchManager airplaneLaunchManager,
            MoneyStorage moneyStorage, UFOIncomeSeller ufoIncomeSeller,
            AirportCapacityUpgradeSeller airportCapacityUpgrade, SpeedUpgradeSeller speedUpgradeFormula,
            RewardManager rewardManager, MoneyPool.MoneyPool moneyPool, IncomeCounter incomeCounter,
            RemoveAdManager removeAdManager, PurchasesManager purchasesManager)
        {
            _purchasesManager = purchasesManager;
            _removeAdManager = removeAdManager;
            _incomeCounter = incomeCounter;
            _moneyPool = moneyPool;
            _rewardManager = rewardManager;
            _airportCapacityUpgradeSeller = airportCapacityUpgrade;
            _speedUpgradeSeller = speedUpgradeFormula;
            _ufoIncomeSeller = ufoIncomeSeller;
            _gameManager = gameManager;
            _airplaneLaunchManager = airplaneLaunchManager;
            _moneyStorage = moneyStorage;

            var listeners = GetComponentsInChildren<IGameListener>();

            _gameManager.AddListeners(listeners);
        }

        public void OnStartGame()
        {
            UpdateModelsState();

            for (int i = 0; i < _upgradeViews.Count; i++)
            {
                UpdateView(_upgradeViews[i], _upgradeModels[i]);
            }

            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
            _upgradeViews.ForEach(upgrade => upgrade.OnUpgradeButtonClicked += OnUpgradeButtonClicked);
            _upgradeViews.ForEach(upgrade => upgrade.OnRewardButtonClicked += OnRewardButtonClicked);

            _purchasesManager.OnBuyNoAds += OnBuyNoAds;
            OnBuyNoAds();
            SetAdIcon();
            UpdateRatio();
        }

        public void OnFinishGame()
        {
            _upgradeViews.ForEach(upgrade => upgrade.OnUpgradeButtonClicked -= OnUpgradeButtonClicked);
            _upgradeViews.ForEach(upgrade => upgrade.OnRewardButtonClicked -= OnRewardButtonClicked);
            _purchasesManager.OnBuyNoAds -= OnBuyNoAds;
            _moneyStorage.OnMoneyChanged -= OnMoneyChanged;
        }

        public UpgradeData GetUpgradeInfo()
        {
            var upgradeData = GetUpgradeData();
            return upgradeData;
        }

        private UpgradeData GetUpgradeData()
        {
            return new UpgradeData
            {
                SpeedLevel = _upgradeModels.FirstOrDefault(model => model.UpgradeType == UpgradeType.AIRPLANE_SPEED)
                    .Level,
                UfoSeller =
                    _upgradeModels.FirstOrDefault(model => model.UpgradeType == UpgradeType.UFO_INCOME).Level,
                AirportCapacityLevel = _upgradeModels
                    .FirstOrDefault(model => model.UpgradeType == UpgradeType.INCOME).Level
            };
        }

        private void OnRewardButtonClicked(UpgradeView view)
        {
            if (_moneyStorage.Ticket <= 0)
            {
                if (_canShowReward == false)
                    return;

                _canShowReward = false;

                _rewardClickedView = view;

                AdsManager.EResultCode result =
                    AdsManager.ShowRewarded(view.gameObject, OnFinishAds, "Upgrade");
                MusicManager.Pause();

                if (result != AdsManager.EResultCode.OK)
                {
                    _canShowReward = true;
                    MusicManager.Resume();
                }
            }
            else
            {
                _rewardClickedView = view;
                _moneyStorage.SpendTicket(1);
                OnFinishAds(true);
            }
        }

        private void OnFinishAds(bool isSuccess)
        {
            if (isSuccess)
            {
                OnUpgradeButtonClicked(_rewardClickedView, true);
                _rewardClickedView = null;
            }

            MusicManager.Resume();
            _canShowReward = true;
        }

        private void OnUpgradeButtonClicked(UpgradeView upgradeView, bool isNeedTrackAnalytic)
        {
            var upgradeModel = _upgradeModels.FirstOrDefault(model => model.UpgradeType == upgradeView.UpgradeType);
            var upgradeData = GetUpgradeData();

            if (upgradeView.UpgradeType == UpgradeType.INCOME)
            {
                if (!upgradeModel.AdState)
                    _moneyStorage.SpendMoney(
                        _airportCapacityUpgradeSeller.GetCurrentPrice(upgradeData.AirportCapacityLevel));

                upgradeModel.LevelUp();
                var currentPrice = _airportCapacityUpgradeSeller.GetCurrentPrice(upgradeModel.Level);
                BigNumber price = new BigNumber(currentPrice);
                var addedValue = _airportCapacityUpgradeSeller.AddedValue * upgradeModel.Level;
                upgradeModel.UpdateModel(addedValue, price);
                _rewardManager.SetAddedMoneyOnAirplaneCapacity(addedValue);
                _moneyPool.ShowGlowEffects();

                if (isNeedTrackAnalytic)
                {
                    MadPixelAnalytics.AnalyticsManager.CustomEvent("income_upgrade", new Dictionary<string, object>()
                    {
                        {"level", upgradeModel.Level},
                    });
                }
            }

            if (upgradeView.UpgradeType == UpgradeType.AIRPLANE_SPEED)
            {
                if (!upgradeModel.AdState)
                    _moneyStorage.SpendMoney(_speedUpgradeSeller.GetCurrentPrice(upgradeData.SpeedLevel));

                upgradeModel.LevelUp();
                var currentPrice = _speedUpgradeSeller.GetCurrentPrice(upgradeModel.Level);
                BigNumber price = new BigNumber(currentPrice);
                var addedValue = _speedUpgradeSeller.AddedValue * upgradeModel.Level;
                upgradeModel.UpdateModel(addedValue, price);
                _airplaneLaunchManager.IncreaseSpeed(addedValue);
                _airplaneLaunchManager.AddTemporarySpeed();

                if (isNeedTrackAnalytic)
                {
                    MadPixelAnalytics.AnalyticsManager.CustomEvent("speed_upgrade", new Dictionary<string, object>()
                    {
                        {"level", upgradeModel.Level},
                    });
                }

                _incomeCounter.CalculateIncome();
            }

            if (upgradeView.UpgradeType == UpgradeType.UFO_INCOME)
            {
                if (!upgradeModel.AdState)
                    _moneyStorage.SpendMoney(_ufoIncomeSeller.GetCurrentPrice(upgradeData.UfoSeller));

                upgradeModel.LevelUp();
                var currentPrice = _ufoIncomeSeller.GetCurrentPrice(upgradeModel.Level);
                BigNumber price = new BigNumber(currentPrice);
                var addedValue = _ufoIncomeSeller.AddedValue * upgradeModel.Level;
                upgradeModel.UpdateModel(addedValue, price);

                if (isNeedTrackAnalytic)
                {
                    MadPixelAnalytics.AnalyticsManager.CustomEvent("ufoIncome_upgrade", new Dictionary<string, object>()
                    {
                        {"level", upgradeModel.Level},
                    });
                }
            }

            UpdateModelsState();

            for (int i = 0; i < _upgradeViews.Count; i++)
            {
                UpdateView(_upgradeViews[i], _upgradeModels[i]);
            }
        }

        private void OnMoneyChanged(BigNumber money)
        {
            UpdateModelsState();

            SetAdIcon();

            for (int i = 0; i < _upgradeViews.Count; i++)
            {
                UpdateView(_upgradeViews[i], _upgradeModels[i]);
            }
        }

        private void SetAdIcon()
        {
            if (_moneyStorage.Ticket > 0)
            {
                for (int i = 0; i < _upgradeViews.Count; i++)
                {
                    _upgradeViews[i].SetTicketIcon();
                }
            }
            else
            {
                for (int i = 0; i < _upgradeViews.Count; i++)
                {
                    _upgradeViews[i].SetAdIcon();
                }
            }
        }

        private void UpdateModelsState()
        {
            foreach (var upgradeModel in _upgradeModels)
            {
                if (upgradeModel.UpgradeType == UpgradeType.UFO_INCOME)
                {
                    bool canBuy = _ufoIncomeSeller.CanBuy(_moneyStorage.Money, upgradeModel.Level);
                    upgradeModel.SetAdState(!canBuy);
                }
                else if (upgradeModel.UpgradeType == UpgradeType.INCOME)
                {
                    bool canBuy = _airportCapacityUpgradeSeller.CanBuy(_moneyStorage.Money, upgradeModel.Level);
                    upgradeModel.SetAdState(!canBuy);
                }
                else if (upgradeModel.UpgradeType == UpgradeType.AIRPLANE_SPEED)
                {
                    bool canBuy = _speedUpgradeSeller.CanBuy(_moneyStorage.Money, upgradeModel.Level);
                    upgradeModel.SetAdState(!canBuy);
                }
            }
        }

        private void UpdateView(UpgradeView upgradeView, UpgradeModel upgradeModel)
        {
            if (!upgradeModel.AdState)
            {
                upgradeView.RewardButton.gameObject.SetActive(false);
            }
            else
            {
                upgradeView.RewardButton.gameObject.SetActive(true);
            }

            upgradeView.SetPrice($"{upgradeModel.NextPrice}");
            upgradeView.SetAddedValue(($"{upgradeModel.NextAddedValue}%").ToString());
        }

        public float GetSpeedAddedValue()
        {
            var upgradeModel = _upgradeModels.FirstOrDefault(model => model.UpgradeType == UpgradeType.AIRPLANE_SPEED);

            return upgradeModel.NextAddedValue - 10;
        }

        public float GetUFOIncome()
        {
            var upgradeModel = _upgradeModels.FirstOrDefault(model => model.UpgradeType == UpgradeType.UFO_INCOME);

            return upgradeModel.NextAddedValue - 10;
        }

        public int GetLevelIncomeUpgrade()
        {
            var upgradeModel =
                _upgradeModels.FirstOrDefault(model => model.UpgradeType == UpgradeType.INCOME);

            return upgradeModel.Level;
        }

        public string GetTapIncomeLevel()
        {
            var upgradeModel = _upgradeModels.FirstOrDefault(model => model.UpgradeType == UpgradeType.UFO_INCOME);

            return upgradeModel.Level.ToString();
        }

        public string GetSpeedUpgradeLevel()
        {
            var upgradeModel = _upgradeModels.FirstOrDefault(model => model.UpgradeType == UpgradeType.AIRPLANE_SPEED);

            return upgradeModel.Level.ToString();
        }

        public string GetIncomeUpgradeLevel()
        {
            var upgradeModel =
                _upgradeModels.FirstOrDefault(model => model.UpgradeType == UpgradeType.INCOME);

            return upgradeModel.Level.ToString();
        }

        private void OnBuyNoAds()
        {
            float aspectRatio = (float) Screen.width / Screen.height;

            if (aspectRatio > 1.0f)
            {
                _layoutGroup.childAlignment = TextAnchor.LowerRight;
                _layoutGroup.padding.right = 274;

                if (!_removeAdManager.IsBuy)
                    _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 295);
                else
                    _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
            }
            else
            {
                _layoutGroup.childAlignment = TextAnchor.LowerCenter;
                _layoutGroup.padding.right = 0;

                if (!_removeAdManager.IsBuy)
                    _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 180);
                else
                    _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
            }
        }


        public void OnUpdate(float deltaTime)
        {
            float aspectRatio = (float) Screen.width / Screen.height;

            if (aspectRatio > 1.0f)
            {
                if (_layoutGroup.childAlignment != TextAnchor.LowerRight)
                {
                    _layoutGroup.childAlignment = TextAnchor.LowerRight;
                    _layoutGroup.padding.right = 274;

                    if (!_removeAdManager.IsBuy)
                        _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 400);
                    else
                        _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
                }
            }
            else
            {
                if (_layoutGroup.childAlignment != TextAnchor.LowerCenter)
                {
                    _layoutGroup.childAlignment = TextAnchor.LowerCenter;
                    _layoutGroup.padding.right = 0;

                    if (!_removeAdManager.IsBuy)
                        _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);
                    else
                        _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                }
            }
        }

        private void UpdateRatio()
        {
            float aspectRatio = (float) Screen.width / Screen.height;

            if (aspectRatio > 1.0f)
            {
                _layoutGroup.childAlignment = TextAnchor.LowerRight;
                _layoutGroup.padding.right = 274;

                if (!_removeAdManager.IsBuy)
                    _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 400);
                else
                    _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
            }
            else
            {
                _layoutGroup.childAlignment = TextAnchor.LowerCenter;
                _layoutGroup.padding.right = 0;

                if (!_removeAdManager.IsBuy)
                    _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);
                else
                    _layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
            }
        }
    }
}