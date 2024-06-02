using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Reward;
using _Fly_Connect.Scripts.SaveLoadSystem;
using MadPixelAnalytics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts
{
    public class DevelopmentPanel : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        [SerializeField] private Button _add1KMoney;
        [SerializeField] private Button _add10KMoney;
        [SerializeField] private Button _add1MMoney;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _disableUIButton;
        [SerializeField] private TMP_InputField _basePriceCountrySeller;
        [SerializeField] private TMP_InputField _multiplierCountrySeller;
        [SerializeField] private TMP_InputField _exponentAirportUpgradeSeller;
        [SerializeField] private TMP_InputField _multiplierAirportUpgradeSeller;
        [SerializeField] private TMP_InputField _multiplierIncomeUpgradeSeller;
        [SerializeField] private TMP_InputField _exponentIncomeUpgradeSeller;
        [SerializeField] private TMP_InputField _multiplierUpgradeSeller;
        [SerializeField] private TMP_InputField _exponentUpgradeSeller;
        [SerializeField] private TMP_InputField _multiplierTapUpgradeSeller;
        [SerializeField] private TMP_InputField _exponentTapUpgradeSeller;
        [SerializeField] private TMP_InputField _multiplierUFOSeller;
        [SerializeField] private TMP_InputField _exponentUFOSeller;
        [SerializeField] private TMP_InputField _multiplierPlacementSeller;
        [SerializeField] private TMP_InputField _exponentPlacementSeller;
        [SerializeField] private TMP_InputField _airportBasePrice;
        [SerializeField] private TMP_InputField _airportMultiplier;
        [SerializeField] private TMP_InputField _totalMultiplier;
        [SerializeField] private GameObject _UI;

        private bool _uiState = true;
        private CountrySeller _countrySeller;
        private MoneyStorage _moneyStorage;
        private AirportUpgradeSeller _airportUpgradeSeller;
        private AirportCapacityUpgradeSeller _airportCapacityUpgradeSeller;
        private SpeedUpgradeSeller _speedUpgradeSeller;
        private UFOIncomeSeller _tapUpgradeSeller;
        private UFOSeller _ufoSeller;
        private ShowplaceRewardSeller _showplaceRewardSeller;
        private SaveLoadManager _saveLoadManager;
        private AirportSeller _airportSeller;
        private RewardManager _rewardManager;

        [Inject]
        private void Construct(CountrySeller countrySeller, AirportUpgradeSeller airportUpgradeSeller,
            AirportCapacityUpgradeSeller airportCapacityUpgradeSeller, MoneyStorage moneyStorage,
            SpeedUpgradeSeller speedUpgradeSeller, UFOIncomeSeller ufoIncomeSeller, UFOSeller ufoSeller,
            ShowplaceRewardSeller showplaceRewardSeller, SaveLoadManager saveLoadManager, AirportSeller airportSeller,
            RewardManager rewardManager)
        {
            _rewardManager = rewardManager;
            _airportSeller = airportSeller;
            _saveLoadManager = saveLoadManager;
            _showplaceRewardSeller = showplaceRewardSeller;
            _ufoSeller = ufoSeller;
            _tapUpgradeSeller = ufoIncomeSeller;
            _speedUpgradeSeller = speedUpgradeSeller;
            _airportCapacityUpgradeSeller = airportCapacityUpgradeSeller;
            _airportUpgradeSeller = airportUpgradeSeller;
            _moneyStorage = moneyStorage;
            _countrySeller = countrySeller;
        }

        public void OnStartGame()
        {
            _add1KMoney.onClick.AddListener(OnAdd1KMoney);
            _add10KMoney.onClick.AddListener(OnAdd10KMoney);
            _add1MMoney.onClick.AddListener(OnAdd1MMoney);
            _restartButton.onClick.AddListener(OnRestartButton);
            _disableUIButton.onClick.AddListener(OnUIDisable);

            _basePriceCountrySeller.onValueChanged.AddListener(SetCountryBasePrice);
            _multiplierCountrySeller.onValueChanged.AddListener(SetCountryMultiplierSeller);

            _exponentAirportUpgradeSeller.onValueChanged.AddListener(SetExponentAirportUpgradeSeller);
            _multiplierAirportUpgradeSeller.onValueChanged.AddListener(SetMultiplierAirportUpgradeSeller);

            _multiplierIncomeUpgradeSeller.onValueChanged.AddListener(SetMultiplierIncomeUpgradeSeller);
            _exponentIncomeUpgradeSeller.onValueChanged.AddListener(SetExponentIncomeUpgradeSeller);

            _multiplierUpgradeSeller.onValueChanged.AddListener(SetMultiplierSpeedUpgradeSeller);
            _exponentUpgradeSeller.onValueChanged.AddListener(SetExponentSpeedUpgradeSeller);

            _multiplierTapUpgradeSeller.onValueChanged.AddListener(SetMultiplierTapUpgradeSeller);
            _exponentTapUpgradeSeller.onValueChanged.AddListener(SetExponentTapUpgradeSeller);

            _multiplierUFOSeller.onValueChanged.AddListener(SetMultiplierUFOSeller);
            _exponentUFOSeller.onValueChanged.AddListener(SetExponentUFOSeller);

            _multiplierPlacementSeller.onValueChanged.AddListener(SetMultiplierPlacementSeller);
            _exponentPlacementSeller.onValueChanged.AddListener(SetExponentPlacementSeller);

            _airportBasePrice.onValueChanged.AddListener(SetBasePriceAirportSeller);
            _airportMultiplier.onValueChanged.AddListener(SetMultiplierAirportSeller);

            _totalMultiplier.onValueChanged.AddListener(SetTotalIncomeMultiplier);
        }

        public void OnFinishGame()
        {
            _add1KMoney.onClick.RemoveListener(OnAdd1KMoney);
            _add10KMoney.onClick.RemoveListener(OnAdd10KMoney);
            _add1MMoney.onClick.RemoveListener(OnAdd1KMoney);
            _restartButton.onClick.RemoveListener(OnRestartButton);
            _disableUIButton.onClick.RemoveListener(OnUIDisable);

            _basePriceCountrySeller.onValueChanged.RemoveListener(SetCountryBasePrice);
            _multiplierCountrySeller.onValueChanged.RemoveListener(SetCountryMultiplierSeller);

            _exponentAirportUpgradeSeller.onValueChanged.RemoveListener(SetExponentAirportUpgradeSeller);
            _multiplierAirportUpgradeSeller.onValueChanged.RemoveListener(SetMultiplierAirportUpgradeSeller);

            _multiplierIncomeUpgradeSeller.onValueChanged.RemoveListener(SetMultiplierIncomeUpgradeSeller);
            _exponentIncomeUpgradeSeller.onValueChanged.RemoveListener(SetExponentIncomeUpgradeSeller);

            _multiplierUpgradeSeller.onValueChanged.RemoveListener(SetMultiplierSpeedUpgradeSeller);
            _exponentUpgradeSeller.onValueChanged.RemoveListener(SetExponentSpeedUpgradeSeller);

            _multiplierTapUpgradeSeller.onValueChanged.RemoveListener(SetMultiplierTapUpgradeSeller);
            _exponentTapUpgradeSeller.onValueChanged.RemoveListener(SetExponentTapUpgradeSeller);

            _multiplierUFOSeller.onValueChanged.RemoveListener(SetMultiplierUFOSeller);
            _exponentUFOSeller.onValueChanged.RemoveListener(SetExponentUFOSeller);

            _multiplierPlacementSeller.onValueChanged.RemoveListener(SetMultiplierPlacementSeller);
            _exponentPlacementSeller.onValueChanged.RemoveListener(SetExponentPlacementSeller);

            _airportBasePrice.onValueChanged.RemoveListener(SetBasePriceAirportSeller);
            _airportMultiplier.onValueChanged.RemoveListener(SetMultiplierAirportSeller);

            _totalMultiplier.onValueChanged.RemoveListener(SetTotalIncomeMultiplier);
        }

        private void OnUIDisable()
        {
            if (_uiState)
            {
                _UI.gameObject.SetActive(false);
                _uiState = false;
            }
            else
            {
                _UI.gameObject.SetActive(true);
                _uiState = true;
            }
        }

        private void OnRestartButton()
        {
            _saveLoadManager.Clear();
            Application.Quit();
        }

        private void OnAdd1KMoney()
        {
            _moneyStorage.EarnMoney(1000);
        }

        private void OnAdd10KMoney()
        {
            _moneyStorage.EarnMoney(10000);
        }

        private void OnAdd1MMoney()
        {
            _moneyStorage.EarnMoney(1000000000);
        }

        private void SetCountryBasePrice(string basePrice)
        {
            if (float.TryParse(_basePriceCountrySeller.text.Replace(".", ","), out float value))
            {
                _countrySeller.SetBasePrice(value);
            }
        }

        private void SetCountryMultiplierSeller(string multiplier)
        {
            if (float.TryParse(_multiplierCountrySeller.text.Replace(".", ","), out float value))
            {
                _countrySeller.SetMultiplier(value);
            }
        }

        private void SetExponentAirportUpgradeSeller(string arg0)
        {
            if (float.TryParse(_exponentAirportUpgradeSeller.text.Replace(".", ","), out float value))
            {
                _airportUpgradeSeller.SetTime(value);
            }
        }

        private void SetMultiplierAirportUpgradeSeller(string arg0)
        {
            if (float.TryParse(_multiplierAirportUpgradeSeller.text.Replace(".", ","), out float value))
            {
                _airportUpgradeSeller.SetMultiplier(value);
            }
        }

        private void SetExponentIncomeUpgradeSeller(string arg0)
        {
            if (float.TryParse(_exponentIncomeUpgradeSeller.text.Replace(".", ","), out float value))
            {
                _airportCapacityUpgradeSeller.SetExponent(value);
            }
        }

        private void SetMultiplierIncomeUpgradeSeller(string arg0)
        {
            if (float.TryParse(_multiplierIncomeUpgradeSeller.text.Replace(".", ","), out float value))
            {
                _airportCapacityUpgradeSeller.SetMultiplier(value);
            }
        }

        private void SetExponentSpeedUpgradeSeller(string arg0)
        {
            if (float.TryParse(_exponentUpgradeSeller.text.Replace(".", ","), out float value))
            {
                _speedUpgradeSeller.SetExponent(value);
            }
        }

        private void SetMultiplierSpeedUpgradeSeller(string arg0)
        {
            if (float.TryParse(_multiplierUpgradeSeller.text.Replace(".", ","), out float value))
            {
                _speedUpgradeSeller.SetMultiplier(value);
            }
        }

        private void SetExponentTapUpgradeSeller(string arg0)
        {
            if (float.TryParse(_exponentTapUpgradeSeller.text.Replace(".", ","), out float value))
            {
                _tapUpgradeSeller.SetExponent(value);
            }
        }

        private void SetMultiplierTapUpgradeSeller(string arg0)
        {
            if (float.TryParse(_multiplierTapUpgradeSeller.text.Replace(".", ","), out float value))
            {
                _tapUpgradeSeller.SetMultiplier(value);
            }
        }

        private void SetExponentUFOSeller(string arg0)
        {
            if (float.TryParse(_exponentUFOSeller.text.Replace(".", ","), out float value))
            {
                _ufoSeller.SetTime(value);
            }
        }

        private void SetMultiplierUFOSeller(string arg0)
        {
            if (float.TryParse(_multiplierUFOSeller.text.Replace(".", ","), out float value))
            {
                _ufoSeller.SetMultiplier(value);
            }
        }

        private void SetExponentPlacementSeller(string arg0)
        {
            if (float.TryParse(_exponentPlacementSeller.text.Replace(".", ","), out float value))
            {
                _showplaceRewardSeller.SetTime(value);
            }
        }

        private void SetMultiplierPlacementSeller(string arg0)
        {
            if (float.TryParse(_multiplierPlacementSeller.text.Replace(".", ","), out float value))
            {
                _showplaceRewardSeller.SetMultiplier(value);
            }
        }

        private void SetBasePriceAirportSeller(string arg0)
        {
            if (float.TryParse(_airportBasePrice.text.Replace(".", ","), out float value))
            {
                _airportSeller.SetTime(value);
            }
        }

        private void SetMultiplierAirportSeller(string arg0)
        {
            if (float.TryParse(_airportMultiplier.text.Replace(".", ","), out float value))
            {
                _airportSeller.SetMultiplier(value);
            }
        }

        private void SetTotalIncomeMultiplier(string arg0)
        {
            if (float.TryParse(_totalMultiplier.text.Replace(".", ","), out float value))
            {
                _rewardManager.SetTotalMultiplier(value);
            }
        }
    }
}