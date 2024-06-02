﻿using System.Collections;
using System.Linq;
using _Fly_Connect.Scripts.Gameplay;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    [AddComponentMenu("Tutorial/Step «Buy Country»")]
    public sealed class BuyCountryStepController : TutorialStepController
    {
        [SerializeField] private RectTransform _countryRectTransform;

        private string Text1 = "Let's unlock your first country!";
        private string Title = "Purchase Germany";

        private const string TutorialCountry = "Germany";
        private PopupManager _popupManager;
        private CountryService _countryService;
        private Country _tutorialCountry;
        private TutorialPopup _tutorialPopup;
        private WorldFinger _worldFinger;
        private InputSystem _inputSystem;
        private Finger _finger;
        private LeanPinchCamera _leanPinchCamera;
        private LeanDragCamera _leanDragCamera;
        private CameraController _cameraController;

        [Inject]
        public override void Construct(GameContext context)
        {
            base.Construct(context);

            _popupManager = context.TryGetService<PopupManager>();
            _countryService = context.TryGetService<CountryService>();
            _tutorialPopup = context.TryGetService<TutorialPopup>();
            _worldFinger = context.TryGetService<WorldFinger>();
            _finger = context.TryGetService<Finger>();
            _inputSystem = context.TryGetService<InputSystem>();
            _leanPinchCamera = context.TryGetService<LeanPinchCamera>();
            _leanDragCamera = context.TryGetService<LeanDragCamera>();
            _cameraController = context.TryGetService<CameraController>();
        }

        protected override void OnStart()
        {
            _tutorialCountry = _countryService.Country.First(country => country.name == TutorialCountry);
            _tutorialCountry.MeshMarker.EnableCollider();
            _tutorialCountry.OnCountryOpened += OnOpenCountry;
            _popupManager.OnPopupShown += OnShowPopup;
            _tutorialPopup.Init(Text1, Title);
            _tutorialPopup.SetHeightWindow(247.5f);
            _worldFinger.RectTransform.anchoredPosition = new Vector3(-534.42f, -931.88f, -6.5f);
            _worldFinger.RectTransform.localScale = new Vector3(0.6f, 0.6f, 1);
            _worldFinger.gameObject.SetActive(true);
            _worldFinger.Animator.Play("Idle");
            _inputSystem.SwitchState(InputStateId.BASE);
            _cameraController.MoveTo(_tutorialCountry.MeshMarker.GetBoundCenter(), Vector3.zero, true, 2f);
            _leanPinchCamera.enabled = false;
            _leanDragCamera.enabled = false;
            _cameraController.GetComponent<Camera>().DOOrthoSize(5, 1f);
        }

        protected override void OnStop()
        {
            _tutorialCountry.OnCountryOpened -= OnOpenCountry;
            _popupManager.OnPopupShown -= OnShowPopup;
            _finger.gameObject.SetActive(false);
        }

        private void OnShowPopup(PopupName popupName)
        {
            if (popupName == PopupName.COUNTRY_POPUP)
            {
                _worldFinger.gameObject.SetActive(false);
                _finger.Animator.Play("Idle");
                _tutorialPopup.SetHeightWindow(247.5f);

                _finger.RectTransform.position = _countryRectTransform.position;
                _finger.gameObject.SetActive(true);
            }
        }

        private void OnOpenCountry(int countryId)
        {
            _popupManager.HidePopup(PopupName.COUNTRY_POPUP);
            NotifyAboutCompleteAndMoveNext();
        }
    }
}