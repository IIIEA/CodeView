using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using _Fly_Connect.Scripts.Tutorial;
using MAXHelper;
using Sirenix.OdinInspector;
using UnityEngine;

public class InterstitialManager : MonoBehaviour, IGameUpdateListener
{
    [SerializeField] private float _delayBetweenShowInter;
    [SerializeField] private Countdown _timer;

    [ShowInInspector][ReadOnly] private float _elapsedTime;
    private bool _canShowAd = true;
    private PopupManager _popupManager;
    private RemoveAdManager _removeAdManager;

    [Inject]
    private void Construct(PopupManager popupManager, RemoveAdManager removeAdManager)
    {
        _removeAdManager = removeAdManager;
        _popupManager = popupManager;
    }

    public void OnUpdate(float deltaTime)
    {
        if(_removeAdManager.IsBuy)
            return;

        if (!TutorialManager.Instance.IsCompleted)
            return;
        
        if (!_canShowAd)
            return;

        _elapsedTime += deltaTime;

        if (_elapsedTime >= _delayBetweenShowInter)
        {
            _canShowAd = false;
            _elapsedTime = 0;

            var result = AdsManager.ShowInter(gameObject, OnInterDismissed, "inter_between_60_seconds");
            MusicManager.Pause();

            if (result != AdsManager.EResultCode.OK)
            {
                OnInterDismissed(false);
            }
        }

        if (_popupManager.IsPopupActive(PopupName.INTER_POPUP))
            return;
        
        if (_elapsedTime >= (_delayBetweenShowInter - 3))
        {
            IInterPresenter interPresenter = new InterPresenter();
            _popupManager.ShowPopup(PopupName.INTER_POPUP, interPresenter);
        }
    }

    private void OnInterDismissed(bool isResult)
    {
        _canShowAd = true;
        _popupManager.HidePopup(PopupName.INTER_POPUP);
        MusicManager.Resume();
    }
}