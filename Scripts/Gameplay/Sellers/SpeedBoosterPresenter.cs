using System;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using MAXHelper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable, InlineProperty]
    public class SpeedBoosterPresenter : IGameStartListener, IGameFinishListener
    {
        private SpeedBoosterView _speedBoosterView;
        private SpeedBoosterModel _speedBoosterModel;
        private MoneyStorage _moneyStorage;

        [Inject]
        public void Construct(SpeedBoosterModel speedBoosterModel, SpeedBoosterView speedBoosterView,
            MoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
            _speedBoosterModel = speedBoosterModel;
            _speedBoosterView = speedBoosterView;
        }

        public void OnStartGame()
        {
            _speedBoosterView.Button.onClick.AddListener(OnButtonClicked);
            _speedBoosterModel.OnFinish += OnTimerFinish;
            _speedBoosterModel.OnTimerChanged += OnTimerChanged;
            _moneyStorage.OnTicketChanged += OnTicketChanged;
            OnTicketChanged(_moneyStorage.Ticket);
        }

        public void OnFinishGame()
        {
            _speedBoosterView.Button.onClick.RemoveListener(OnButtonClicked);
            _speedBoosterModel.OnFinish -= OnTimerFinish;
            _speedBoosterModel.OnTimerChanged -= OnTimerChanged;
            _moneyStorage.OnTicketChanged -= OnTicketChanged;
        }

        private void OnTimerFinish()
        {
            _speedBoosterView.DisableCountDown();
        }

        private void OnTicketChanged(int ticket)
        {
            if (ticket > 0)
            {
                _speedBoosterView.SetTicketIcon();
            }
            else
            {
                _speedBoosterView.SetAdIcon();
            }
        }

        private void OnTimerChanged(float time)
        {
            _speedBoosterView.SetTime(time);
        }

        private void OnButtonClicked()
        {
            if (_moneyStorage.Ticket <= 0)
            {
                _speedBoosterView.Button.interactable = false;

                AdsManager.EResultCode result =
                    AdsManager.ShowRewarded(_speedBoosterView.gameObject, OnFinishAds, "speed_booster");
                MusicManager.Pause();

                if (result != AdsManager.EResultCode.OK)
                {
                    MusicManager.Resume();
                    _speedBoosterView.Button.interactable = true;
                }
            }
            else
            {
                _moneyStorage.SpendTicket(1);
                OnFinishAds(true);
            }
        }

        private void OnFinishAds(bool isSuccess)
        {
            if (isSuccess)
            {
                _speedBoosterModel.StartTimer();
                _speedBoosterView.EnableCountDown();
            }
            else
            {
                _speedBoosterView.Button.interactable = true;
            }

            MusicManager.Resume();
        }
    }
}