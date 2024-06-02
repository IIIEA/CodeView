using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts.Gameplay;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using Lean.Touch;
using MadPixelAnalytics;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    [AddComponentMenu("Tutorial/Step «Buy Airport»")]
    public sealed class BuyAirportStepController : TutorialStepController
    {
        [SerializeField] private RectTransform _airportTransform;
        [SerializeField] private Transform _cityPoint1;

        private string Text1 = "Click on Berlin to open the city window and acquire the airport. Give it a try!";
        private string Title = "Purchase the airport";

        private const string TutorialCity = "Berlin";
        private PopupManager _popupManager;
        private CountryService _countryService;
        private Country _tutorialCountry;
        private Map _map;
        private List<CityPoint> _cityPoints = new();
        private CityPoint _tutorialCityPoint;
        private TutorialPopup _tutorialPopup;
        private WorldFinger _worldFinger;
        private Finger _finger;
        private InputSystem _inputSystem;
        private PopupInputController _popupInputController;
        private LeanPinchCamera _leanPinchCamera;
        private LeanDragCamera _leanDragCamera;
        private CameraController _cameraController;

        [Inject]                                                                          
        public override void Construct(GameContext context)
        {
            base.Construct(context);

            _popupManager = context.TryGetService<PopupManager>();
            _map = context.TryGetService<Map>();
            _cityPoints = _map.GetGameCities();
            _tutorialPopup = context.TryGetService<TutorialPopup>();
            _worldFinger = context.TryGetService<WorldFinger>();
            _finger = context.TryGetService<Finger>();
            _inputSystem = context.TryGetService<InputSystem>();
            _popupInputController = context.TryGetService<PopupInputController>();
            _leanPinchCamera = context.TryGetService<LeanPinchCamera>();
            _leanDragCamera = context.TryGetService<LeanDragCamera>();
            _cameraController = context.TryGetService<CameraController>();
        }

        protected override void OnStart()
        {
            _tutorialCityPoint = _cityPoints.First(city => city.name == TutorialCity);
            _tutorialCityPoint.OnCityUpgrade += OnCityUpgrade;
            _popupManager.OnPopupShown += OnShowPopup;
            _tutorialPopup.Init(Text1, Title);
            _tutorialPopup.SetHeightWindow(285f);

            _worldFinger.transform.position = _cityPoint1.position;
            _worldFinger.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            _worldFinger.gameObject.SetActive(true);
            _worldFinger.Animator.Play("Idle");
            _inputSystem.SwitchState(InputStateId.BASE);
            _leanPinchCamera.Zoom = _leanPinchCamera.GetComponent<Camera>().orthographicSize;
        }

        protected override void OnStop()
        {

        }

        private void OnCityUpgrade(CityPoint city)
        {
            _popupManager.HidePopup(PopupName.CITY_POPUP);
            _popupManager.OnPopupShown -= OnShowPopup;
            _tutorialCityPoint.OnCityUpgrade -= OnCityUpgrade;
            _worldFinger.gameObject.SetActive(false);
            AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
            {
                {"03_buyAirportStep", true},
            },true);
            NotifyAboutCompleteAndMoveNext();
        }

        private void OnShowPopup(PopupName popupName)
        {
            if (popupName == PopupName.CITY_POPUP)
            {
                _leanDragCamera.enabled = false;
                _leanPinchCamera.enabled = false;

                _worldFinger.RectTransform.position = _airportTransform.position;

                _worldFinger.transform.SetAsLastSibling();
            }
        }
    }
}
