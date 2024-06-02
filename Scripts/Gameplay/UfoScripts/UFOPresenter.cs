using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Gameplay.UfoScripts;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using MAXHelper;

namespace _Fly_Connect.Scripts.Gameplay.GoldenAirplane
{
    public class UFOPresenter : IGoldenAirplanePresenter
    {
        private MoneyStorage _moneyStorage;
        private AirplaneSeller _airplaneSeller;
        private UFOSeller _ufoSeller;
        private UFOAirplaneManager _ufoAirplaneManager;
        private bool _canShowReward = true;
        public bool IsShowTicketIcon => _moneyStorage.Ticket > 0;

        public string MoneyPrice { get; private set; }
        public string RewardPrice { get; private set; }

        public UFOPresenter(MoneyStorage moneyStorage, AirplaneSeller airplaneSeller,
            UFOSeller ufoSeller, UFOAirplaneManager ufoAirplaneManager)
        {
            _ufoAirplaneManager = ufoAirplaneManager;
            _ufoSeller = ufoSeller;
            _airplaneSeller = airplaneSeller;
            _moneyStorage = moneyStorage;
            BigNumber moneyBigNumber = new BigNumber(_ufoSeller.GetCurrentPrice());
            BigNumber rewardBigNumber = new BigNumber(CalculateRewardPrice());
            RewardPrice = rewardBigNumber.ToString();
            MoneyPrice = moneyBigNumber.ToString();
        }

        public void OnRewardButtonClicked()
        {
            if (_moneyStorage.Ticket > 0)
            {
                _moneyStorage.SpendTicket(1);
                OnFinishAds(true);
            }
            else
            {
                if (_canShowReward == false)
                    return;

                _canShowReward = false;

                AdsManager.EResultCode result =
                    AdsManager.ShowRewarded(_ufoAirplaneManager.CurrentUfo.gameObject, OnFinishAds, "UFO");
                MusicManager.Pause();

                if (result != AdsManager.EResultCode.OK)
                {
                    _canShowReward = true;
                    MusicManager.Resume();
                }
            }
        }

        private void OnFinishAds(bool isSuccess)
        {
            if (isSuccess)
            {
                long finalPrice = CalculateRewardPrice();
                BigNumber price = new BigNumber(finalPrice);
                _moneyStorage.EarnMoney(price);
                _ufoAirplaneManager.TryDestroyUFO();
            }

            _canShowReward = true;
            MusicManager.Resume();
        }

        private long CalculateRewardPrice()
        {
            var goldenAirplanePrice = _ufoSeller.GetCurrentPrice();
            long price = (long) (goldenAirplanePrice * _ufoSeller.AdMultiplier);
            return price;
        }

        public void OnMoneyButtonClicked()
        {
            long price = _ufoSeller.GetCurrentPrice();
            _ufoAirplaneManager.TryDestroyUFO();
            BigNumber priceBigNumber = new BigNumber(price);
            _moneyStorage.EarnMoney(priceBigNumber);
        }
    }
}