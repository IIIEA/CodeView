using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using _Fly_Connect.Scripts.Tutorial;
using Lean.Touch;
using TMPro;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    public class CityPopup : MonoWindow<ICityPresenter>
    {
        [SerializeField] private CityAirportView _cityAirportView;

        [field: SerializeField] public TMP_Text Title { get; private set; }

        private ICityPresenter _presenter;

        protected override void OnShow(ICityPresenter args)
        {
            if (args is not ICityPresenter presenter)
            {
                throw new Exception("Expected City Presenter");
            }

            _presenter = presenter;

            gameObject.SetActive(true);
            Title.SetText(presenter.Title);

            presenter.Enable();
            _cityAirportView.Show(_presenter.AirportPresenter);
            LeanTouch.OnFingerDown += OnFingerDown;
        }

        protected override void OnHide()
        {
            gameObject.SetActive(false);
        }

        private void OnFingerDown(LeanFinger leanFinger)
        {
            if (!TutorialManager.Instance.IsCompleted)
                return;

            if (!LeanTouch.PointOverGui(Input.mousePosition))
            {
                _presenter.Disable();
                _cityAirportView.Hide();
                NotifyAboutClose();
                Hide();
                LeanTouch.OnFingerDown -= OnFingerDown;
            }
        }
    }
}