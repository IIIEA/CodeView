using System;
using System.Collections;
using _Fly_Connect.Scripts.ApplicationLoader;
using _Fly_Connect.Scripts.Gameplay.GoldenAirplane;
using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.Tutorial;
using MAXHelper;
using Sirenix.OdinInspector;
using UnityEngine;
//using Debug = fbg.Debug;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [Serializable]
    public class StarterPackManager : IGameStartListener, IGameFinishListener
    {
        private const int Seconds = 300;
        private PopupManager _popupManager;
        private ICoroutineRunner _coroutineRunner;
        private bool _canStartCoroutine = true;
        private bool _isNeedShow;
        private GoldenAirplanePopupFactory _ufoPopupFactory;
        private RemoveAdManager _removeAdManager;

        [ShowInInspector] public bool IsBuy { get; private set; }
        [ShowInInspector] public bool CanShow { get; private set; }

        [Inject]
        private void Construct(PopupManager popupManager, ICoroutineRunner coroutineRunner,
            GoldenAirplanePopupFactory ufoPopupFactory, RemoveAdManager removeAdManager)
        {
            _removeAdManager = removeAdManager;
            _ufoPopupFactory = ufoPopupFactory;
            _coroutineRunner = coroutineRunner;
            _popupManager = popupManager;
        }

        public void Setup(bool isBuy)
        {
            IsBuy = isBuy;

            if(IsBuy)
                AdsManager.CancelAllAds();
        }

        public void OnStartGame()
        {
            LoadingScreen.OnHide += OnLoadingScreenHide;
            _popupManager.OnPopupHidden += OnPopupHidden;
        }

        public void OnFinishGame()
        {
            LoadingScreen.OnHide -= OnLoadingScreenHide;
            _popupManager.OnPopupHidden -= OnPopupHidden;
        }

        private void OnPopupHidden(PopupName popupName)
        {
            if (popupName == PopupName.STARTER_PACK_POPUP)
            {
                AdsManager.ToggleBanner(true);
            }

            if (_isNeedShow)
                return;

            if (popupName == PopupName.STARTER_PACK_POPUP)
            {
                var presenter = _ufoPopupFactory.Create();
                _popupManager.ShowPopup(PopupName.GOLDEN_AIRPLANE_POPUP, presenter);
                AdsManager.ToggleBanner(true);
            }
        }

        public void ShowPopup(bool isNeedShow = false)
        {
            _isNeedShow = isNeedShow;

            if (CanShow || isNeedShow)
            {
                IStarterPackPresenter presenter = new StarterPackPresenter();
                _popupManager.ShowPopup(PopupName.STARTER_PACK_POPUP, presenter);
                AdsManager.ToggleBanner(false);

                if (_canStartCoroutine)
                {
                    _canStartCoroutine = false;
                    _coroutineRunner.StartCoroutine(ShowPopupCoroutine());
                }

                CanShow = false;
            }
        }

        private void OnLoadingScreenHide()
        {
            if (TutorialManager.Instance.IsCompleted == false)
                return;

            if (IsBuy)
                return;

            ShowPopup(true);
        }

        private IEnumerator ShowPopupCoroutine()
        {
            yield return new WaitForSeconds(Seconds);
            CanShow = true;
            _canStartCoroutine = true;
        }
    }
}