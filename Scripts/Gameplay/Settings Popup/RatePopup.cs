using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using UnityEngine;
using UnityEngine.UI;

public sealed class RatePopup : MonoWindow<IRateUsPresenter>
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _notMuchButton;
    [SerializeField] private Button _loveItButton;

    private IRateUsPresenter _presenter;

    protected override void OnShow(IRateUsPresenter args)
    {
        if (args is not IRateUsPresenter presenter)
        {
            throw new Exception("Expected Setting Presenter");
        }

        _presenter = presenter;
        gameObject.SetActive(true);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
        _notMuchButton.onClick.AddListener(NotMuchButtonClicked);
        _loveItButton.onClick.AddListener(LoveItButtonClicked);
    }

    private void LoveItButtonClicked()
    {
         _presenter.OnRateButtonClicked();
    }

    private void NotMuchButtonClicked()
    {
        Hide();
        NotifyAboutClose();
    }

    private void OnCloseButtonClicked()
    {
        Hide();
        NotifyAboutClose();
    }

    protected override void OnHide()
    {
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        _notMuchButton.onClick.RemoveListener(NotMuchButtonClicked);
        _loveItButton.onClick.RemoveListener(LoveItButtonClicked);
        gameObject.SetActive(false);
    }
}