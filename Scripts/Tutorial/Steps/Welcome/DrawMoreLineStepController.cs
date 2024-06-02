using System.Collections.Generic;
using System.Linq;
using _Fly_Connect.Scripts;
using _Fly_Connect.Scripts.Gameplay;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.Tutorial;
using _Fly_Connect.Scripts.Tutorial.Steps.Welcome;
using DG.Tweening;
using Lean.Touch;
using MadPixelAnalytics;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Tutorial/Step «Draw More Line»")]
public sealed class DrawMoreLineStepController : TutorialStepController
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Transform _cityPoint1;
    [SerializeField] private Transform _cityPoint2;
    [SerializeField] private ParticleSystem _pulseParticle;
    [SerializeField] private Transform _cameraTransformPosition;

    private string Text1 =
        "Higher city level also improves the airport, so you can draw more routes to other destinations!";

    private string Title = "Draw more routes";

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
    private LeanPinchCamera _leanPinchCamera;
    private PopupManager _popupManager;
    private Finger _finger;
    private LeanDragCamera _leanDragCamera;

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
        _leanPinchCamera = context.TryGetService<LeanPinchCamera>();
        _popupManager = context.TryGetService<PopupManager>();
        _finger = context.TryGetService<Finger>();
        _leanDragCamera = context.TryGetService<LeanDragCamera>();

        var map = context.TryGetService<Map>();
        _cityPoints = map.GetGameCities();
    }

    protected override void OnStart()
    {
        _nextButton.gameObject.SetActive(true);
        _pulseParticle.gameObject.SetActive(true);
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _tutorialPopup.Init(Text1, Title);
        _tutorialPopup.SetHeightWindow(339.4f);
        _finger.gameObject.SetActive(false);
        _worldFinger.gameObject.SetActive(false);
    }

    private void OnNextButtonClicked()
    {
        _nextButton.onClick.RemoveListener(OnNextButtonClicked);
        _nextButton.gameObject.SetActive(false);
        _tutorialCityPoint = _cityPoints.First(city => city.name == TutorialCity);
        _airplaneRouteFactory.OnRouteGenerated += OnRouteGenerated;
        _worldFinger.gameObject.SetActive(true);
        _pulseParticle.gameObject.SetActive(false);
        _worldFinger.Animator.Play("Click");
        _cameraController.MoveTo(_cameraTransformPosition.position);
        _popupManager.HidePopup(PopupName.CITY_POPUP);
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
            if (route.StartCityIndex == 262 && route.EndCityIndex == 264)
            {
                _fingerAnimation.Kill();
                AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
                {
                    {"06_drawTwentyLineStep", true},
                },true);
                NotifyAboutCompleteAndMoveNext();
            }
        }
    }
}