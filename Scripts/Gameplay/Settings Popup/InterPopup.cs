using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using UnityEngine;
using UnityEngine.UI;

public sealed class InterPopup : MonoWindow<IInterPresenter>
{
    private IInterPresenter _presenter;

    protected override void OnShow(IInterPresenter args)
    {
        if (args is not IInterPresenter presenter)
        {
            throw new Exception("Expected Setting Presenter");
        }

        _presenter = presenter;
        gameObject.SetActive(true);
    }

    private void OnCloseButtonClicked()
    {
        Hide();
        NotifyAboutClose();
    }

    protected override void OnHide()
    {
        gameObject.SetActive(false);
    }
}