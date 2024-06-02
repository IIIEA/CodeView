using System.Collections.Generic;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.Tutorial.Steps.Welcome;
using Lean.Touch;
using MadPixelAnalytics;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Input
{
    [AddComponentMenu("Tutorial/Step «Input»")]
    public sealed class InputStepController : TutorialStepController, IGameUpdateListener
    {
        [SerializeField] private GameObject _arrow;
        [SerializeField] private Finger _twentyFinger;

        private string Text1 = "To navigate the map, drag in any direction";

        private string Text2 = "Use 2 fingers to zoom in and out";

        private string Title = "Game Controls";

        private bool _isClicked = false;

        private Vector2 _lastPosition;
        private float distanceThreshold = 50;
        private LeanPinchCamera _leanPinchCamera;
        private bool _isFirstStepComplete;
        private bool _isTwentyComplete;
        private float _startZoom;
        private float _previousZoom;
        private float _zoomDifference;
        private float _totalZoom;
        private ITutorialPopupPresenter _tutorialPopupPresenter;
        private TutorialPopup _tutorialPopup;
        private Finger _finger;
        private bool _isShowTyping = true;

        [Inject]
        public override void Construct(GameContext context)
        {
            base.Construct(context);

            _leanPinchCamera = context.TryGetService<LeanPinchCamera>();
            _tutorialPopup = context.TryGetService<TutorialPopup>();
            _finger = context.TryGetService<Finger>();
        }

        protected override void OnStart()
        {
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
            LeanTouch.OnFingerUp += OnFingerUp;
            LeanTouch.OnFingerDown += OnFingerDown;

            _tutorialPopup.Init(Text1, Title);
            _tutorialPopup.SetHeightWindow(233);
            _finger.gameObject.SetActive(true);
            _arrow.gameObject.SetActive(true);
            _finger.Animator.Play("LeftRight");
        }

        protected override void OnStop()
        {
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
            LeanTouch.OnFingerUp -= OnFingerUp;
            LeanTouch.OnFingerDown -= OnFingerDown;
            _finger.gameObject.SetActive(false);
            _twentyFinger.gameObject.SetActive(false);
        }

        private void OnFingerUpdate(LeanFinger finger)
        {
            if (_isFirstStepComplete)
                return;

            if (_isClicked)
            {
                float distance = Vector2.Distance(_lastPosition, finger.ScreenPosition);

                if (distance > distanceThreshold)
                {
                    _finger.Animator.Play("Zoom");
                    _twentyFinger.gameObject.SetActive(true);
                    _twentyFinger.Animator.Play("Zoom2");
                    _isFirstStepComplete = true;
                    _startZoom = _leanPinchCamera.Zoom;
                    _previousZoom = _leanPinchCamera.Zoom;
                }
            }

            _lastPosition = finger.ScreenPosition;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isTwentyComplete)
                return;

            if (_isFirstStepComplete)
            {
                if (_isShowTyping)
                {
                    _isShowTyping = false;
                    _tutorialPopup.Init(Text2, Title);
                }

                _arrow.gameObject.SetActive(false);
                float currentZoom = _leanPinchCamera.Zoom;
                _zoomDifference = currentZoom - _previousZoom;
                _totalZoom += Mathf.Abs(_zoomDifference);

                if (_totalZoom >= 7f && _isTwentyComplete == false)
                {
                    _isTwentyComplete = true;
                    AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
                    {
                        {"02_inputStep", true},
                    },true);
                    
                    NotifyAboutCompleteAndMoveNext();
                }

                _previousZoom = currentZoom;
            }
        }

        private void OnFingerUp(LeanFinger _)
        {
            _isClicked = false;
        }

        private void OnFingerDown(LeanFinger finger)
        {
            _isClicked = true;
            _lastPosition = finger.ScreenPosition;
        }
    }
}