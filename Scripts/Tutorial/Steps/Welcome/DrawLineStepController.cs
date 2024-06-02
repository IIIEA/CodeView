using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts.Gameplay;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using DG.Tweening;
using MadPixelAnalytics;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    [AddComponentMenu("Tutorial/Step «Draw Line»")]
    public sealed class DrawLineStepController : TutorialStepController
    {
        [SerializeField] private Transform _cityPoint1;
        [SerializeField] private Transform _cityPoint2;
        [SerializeField] private Transform _cameraTargetPosition;

        private string Text1 =
            "Tap on Berlin and draw the line to Hamburg. Planes will start flying along the route! You earn money when the passenger reaches the final destination. Give it a try.";

        private string Title = "Build your first route";

        private const string TutorialCity = "Berlin";
        private CountryService _countryService;
        private Country _tutorialCountry;
        private CityPoint _tutorialCityPoint;
        private List<CityPoint> _cityPoints = new();
        private AirplaneRouteFactory _airplaneRouteFactory;
        private TutorialPopup _tutorialPopup;
        private WorldFinger _worldFinger;
        private Sequence _fingerAnimation;
        private InputSystem _inputSystem;
        private CameraController _cameraController;
        private PopupManager _popupManager;

        [Inject]
        public override void Construct(GameContext context)
        {
            base.Construct(context);

            _countryService = context.TryGetService<CountryService>();
            _airplaneRouteFactory = context.TryGetService<AirplaneRouteFactory>();
            _tutorialPopup = context.TryGetService<TutorialPopup>();
            _worldFinger = context.TryGetService<WorldFinger>();
            _inputSystem = context.TryGetService<InputSystem>();
            _cameraController = context.TryGetService<CameraController>();
            _popupManager = context.TryGetService<PopupManager>();

            var map = context.TryGetService<Map>();
            _cityPoints = map.GetGameCities();
        }

        protected override void OnStart()
        {
            _tutorialPopup.Init(Text1, Title);
            _tutorialPopup.SetHeightWindow(386);
            _tutorialCityPoint = _cityPoints.First(city => city.name == TutorialCity);
            _airplaneRouteFactory.OnRouteGenerated += OnRouteGenerated;
            _worldFinger.gameObject.SetActive(true);
            _worldFinger.Animator.Play("Click");
            _cameraController.MoveTo(_cameraTargetPosition.position, Vector3.zero, true);

            _fingerAnimation = DOTween.Sequence();

            _fingerAnimation
                .Append(_worldFinger.transform.DOMove(_cityPoint1.transform.position, 0.01f))
                .Append(_worldFinger.transform.DOMove(_cityPoint2.transform.position, 3f))
                .SetLoops(-1, LoopType.Restart);

            _inputSystem.SwitchState(InputStateId.BASE);
        }

        protected override void OnStop()
        {
            _airplaneRouteFactory.OnRouteGenerated -= OnRouteGenerated;
        }

        private void OnRouteGenerated()
        {
            foreach (var route in _tutorialCityPoint.Routes)
            {
                if (route.StartCityIndex == 262 && route.EndCityIndex == 263)
                {
                    _popupManager.HidePopup(PopupName.BUY_CITY_POPUP);
                    AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
                    {
                        {"04_drawLineStep", true},
                    },true);
                    NotifyAboutCompleteAndMoveNext();
                    _fingerAnimation.Kill();
                }
            }
        }
    }
}