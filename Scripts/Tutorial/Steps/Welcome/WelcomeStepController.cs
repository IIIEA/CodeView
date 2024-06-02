using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using Lean.Touch;
using MadPixelAnalytics;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    [AddComponentMenu("Tutorial/Step «Welcome»")]
    public sealed class WelcomeStepController : TutorialStepController
    {
        private string Title = "Welcome to Fly-Connect!";

        private string Text = "Are you ready to launch your airline? Let's begin!";

        private PopupManager _popupManager;
        private ITutorialPopupPresenter _tutorialPopupPresenter;
        private TutorialPopup _tutorialPopup;
        private CountryService _countryService;
        private LeanDragCamera _leanDragCamera;
        private LeanPinchCamera _leanPinchCamera;

        [Inject]
        public override void Construct(GameContext context)
        {
            base.Construct(context);

            _popupManager = context.TryGetService<PopupManager>();
            _tutorialPopup = context.TryGetService<TutorialPopup>();
            _countryService = context.TryGetService<CountryService>();
            _leanDragCamera = context.TryGetService<LeanDragCamera>();
            _leanPinchCamera = context.TryGetService<LeanPinchCamera>();
        }

        protected override void OnStart()
        {
            _tutorialPopupPresenter = new TutorialPopupPresenter();
            _popupManager.ShowPopup(PopupName.TUTORIAL_POPUP, _tutorialPopupPresenter);
            _tutorialPopup.Init(Text, Title, true);
            _tutorialPopup.SetHeightWindow(288.25f);
            _tutorialPopupPresenter.OnNextButtonClick += OnNextButtonClick;
            _leanDragCamera.enabled = false;
            _leanPinchCamera.enabled = false;

            foreach (var country in _countryService.Country)
            {
                if (country.MeshMarker != null)
                    country.MeshMarker.DisableCollider();
            }
        }

        protected override void OnStop()
        {
            _tutorialPopupPresenter.OnNextButtonClick -= OnNextButtonClick;
            _leanDragCamera.enabled = true;
            _leanPinchCamera.enabled = true;
        }

        private void OnNextButtonClick()
        {
            AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
            {
                {"01_welcomeStep", true},
            },true);
            
            NotifyAboutCompleteAndMoveNext();
        }
    }
}