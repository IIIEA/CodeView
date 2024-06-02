using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using DG.Tweening;
using Lean.Touch;
using MadPixelAnalytics;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    [AddComponentMenu("Tutorial/Step «Upgrade Airport»")]
    public sealed class UpgradeAirportStepController : TutorialStepController
    {
        [SerializeField] private RectTransform _airportTransform;
        [SerializeField] private Transform _cityPoint1;

        private string Text1 =
            "Click on the city and then on the 'Upgrade' button. The higher the city level the more money it will earn you";

        private string Title = "Airport Upgrade";

        private const string TutorialCity = "Berlin";
        private CountryService _countryService;
        private Country _tutorialCountry;
        private CityPoint _tutorialCityPoint;
        private List<CityPoint> _cityPoints = new();
        private TutorialPopup _tutorialPopup;
        private PopupManager _popupManager;
        private WorldFinger _worldFinger;
        private Finger _finger;
        private LeanPinchCamera _leanPinchCamera;
        private LeanDragCamera _leanDragCamera;

        [Inject]
        public override void Construct(GameContext context)
        {
            base.Construct(context);
            _tutorialPopup = context.TryGetService<TutorialPopup>();
            _countryService = context.TryGetService<CountryService>();
            _popupManager = context.TryGetService<PopupManager>();
            _worldFinger = context.TryGetService<WorldFinger>();
            _leanPinchCamera = context.TryGetService<LeanPinchCamera>();
            _leanDragCamera = context.TryGetService<LeanDragCamera>();
            _finger = context.TryGetService<Finger>();
            var map = context.TryGetService<Map>();
            _cityPoints = map.GetGameCities();
        }

        protected override void OnStart()
        {
            _tutorialCityPoint = _cityPoints.First(city => city.name == TutorialCity);
            _tutorialCityPoint.OnCityUpgrade += OnCityUpgrade;
            _popupManager.OnPopupShown += OnShowPopup;
            _tutorialPopup.Init(Text1, Title);
            _tutorialPopup.SetHeightWindow(336.7f);
            _worldFinger.transform.position = _cityPoint1.position;
            _worldFinger.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            _worldFinger.gameObject.SetActive(true);
            _worldFinger.Animator.Play("Idle");
            _leanPinchCamera.enabled = false;
            _leanDragCamera.enabled = false;
        }

        protected override void OnStop()
        {
            _popupManager.OnPopupShown -= OnShowPopup;
            _tutorialCityPoint.OnCityUpgrade -= OnCityUpgrade;
        }

        private void OnShowPopup(PopupName popupName)
        {
            if (popupName == PopupName.CITY_POPUP)
            {
                _worldFinger.RectTransform.position = _airportTransform.position;
                _worldFinger.transform.SetAsLastSibling();
            }
        }

        private void OnCityUpgrade(CityPoint city)
        {
            if (city.CurrentAirportLevel == 4)
            {
                AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
                {
                    {"05_upgradeAirportStep", true},
                },true);
                NotifyAboutCompleteAndMoveNext();
            }
        }
    }
}