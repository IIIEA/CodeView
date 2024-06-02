using System;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using _Fly_Connect.Scripts.Sound.MusicPlayer;
using MAXHelper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    [Serializable, InlineProperty]
    public class MoneyBoosterPresenter : IGameStartListener, IGameFinishListener
    {
        private MoneyBoosterView _moneyBoosterView;
        private MoneyBoosterModel _moneyBoosterModel;
        private MoneyStorage _moneyStorage;

        [Inject]
        public void Construct(MoneyBoosterModel moneyBoosterModel, MoneyBoosterView moneyBoosterView, MoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
            _moneyBoosterModel = moneyBoosterModel;
            _moneyBoosterView = moneyBoosterView;
        }

        public void OnStartGame()
        {
            _moneyBoosterView.Button.onClick.AddListener(OnButtonClicked);
            _moneyBoosterModel.OnFinish += OnTimerFinish;
            _moneyBoosterModel.OnTimerChanged += OnTimerChanged;
            _moneyStorage.OnTicketChanged += OnTicketChanged;
            OnTicketChanged(_moneyStorage.Ticket);
        }

        public void OnFinishGame()
        {
            _moneyBoosterView.Button.onClick.RemoveListener(OnButtonClicked);
            _moneyBoosterModel.OnFinish -= OnTimerFinish;
            _moneyBoosterModel.OnTimerChanged -= OnTimerChanged;
            _moneyStorage.OnTicketChanged -= OnTicketChanged;
        }

        private void OnTicketChanged(int ticket)
        {
            if (ticket > 0)
            {
                _moneyBoosterView.SetTicketIcon();
            }
            else
            {
                _moneyBoosterView.SetAdIcon();
            }
        }

        private void OnTimerFinish()
        {
            _moneyBoosterView.DisableCountDown();
        }

        private void OnTimerChanged(float time)
        {
            _moneyBoosterView.SetTime(time);
        }

        private void OnButtonClicked()
        {
            if (_moneyStorage.Ticket <= 0)
            {
                _moneyBoosterView.Button.interactable = false;

                AdsManager.EResultCode result =
                    AdsManager.ShowRewarded(_moneyBoosterView.gameObject, OnFinishAds, "money_booster");
                MusicManager.Pause();

                if (result != AdsManager.EResultCode.OK)
                {
                    _moneyBoosterView.Button.interactable = true;
                    MusicManager.Resume();
                }
            }
            else
            {
                _moneyStorage.SpendTicket(1);
                OnFinishAds(true);
            }
        }
        
        private void  OnFinishAds(bool isSuccess)
        {
            if (isSuccess) 
            {
                _moneyBoosterModel.StartTimer();
                _moneyBoosterView.EnableCountDown();
            } 
            else 
            {
                _moneyBoosterView.Button.interactable = true;
            }

            MusicManager.Resume();
        }
    }
}