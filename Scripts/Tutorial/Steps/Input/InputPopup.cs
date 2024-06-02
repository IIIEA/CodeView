using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Input
{
    public class InputPopup : MonoWindow<IInputPopupPresenter>
    {
        IInputPopupPresenter _presenter;

        protected override void OnShow(IInputPopupPresenter args)
        {
            if (args is not IInputPopupPresenter presenter)
            {
                throw new Exception("Expected Input Presenter");
            }

            gameObject.SetActive(true);
            _presenter = presenter;
        }

        protected override void OnHide()
        {
        }
    }
}