using System;
using System.Collections;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Tutorial;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed = 1;
        [SerializeField] private float _moveDuration;
        [SerializeField] private Ease _ease;
        [SerializeField] private Rect _rect;

        private float _currentSpeed = 0f;
        private float _accelerationTime = 2f;
        private LeanDragCamera _leanDragCamera;
        private LeanPinchCamera _leanPinchCamera;
        private Camera _camera;
        private ICoroutineRunner _coroutineRunner;

        [Inject]
        public void Construct(Camera camera, LeanPinchCamera leanPinchCamera, LeanDragCamera leanDragCamera,
            ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _camera = camera;
            _leanPinchCamera = leanPinchCamera;
            _leanDragCamera = leanDragCamera;
        }

        public void MoveTo(Vector3 targetTransform, Vector3 offset = default, bool isDisableLean = false,
            float duration = 0, Action onComplete = null)
        {
            if (duration == 0)
                duration = _moveDuration;

            _leanDragCamera.enabled = false;
            _leanPinchCamera.enabled = false;

            Vector3 targetPosition =
                new Vector3(targetTransform.x, targetTransform.y, transform.position.z) + offset;

            transform.DOMove(targetPosition, duration).SetEase(_ease).OnComplete(() =>
            {
                if (isDisableLean == false)
                {
                    if (TutorialManager.Instance.CurrentStep != TutorialStep.BUY_AIRPORT &&
                        TutorialManager.Instance.CurrentStep != TutorialStep.AIRPORT_UPGRADE &&
                        TutorialManager.Instance.CurrentStep != TutorialStep.DRAW_LINE &&
                        TutorialManager.Instance.CurrentStep != TutorialStep.DRAW_MORE_LINE &&
                        TutorialManager.Instance.CurrentStep != TutorialStep.BUY_MORE_COUNTRY)
                    {
                        _leanDragCamera.enabled = true;
                        _leanPinchCamera.enabled = true;
                    }
                }

                onComplete?.Invoke();
            });
        }

        public void LerpToPosition(Vector3 targetPosition)
        {
            if (_rect.Contains(targetPosition) == false)
            {
                Vector3 screenCenter =
                    _camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));

                float distanceFromCenter = Vector3.Distance(targetPosition, screenCenter);
                float maxDistance = Mathf.Min(Screen.width * 0.5f, Screen.height * 0.5f);

                float normalizedDistance = Mathf.Clamp01(distanceFromCenter / maxDistance);
                float modifiedSpeed = Mathf.Lerp(0f, _maxSpeed, normalizedDistance);

                Vector3 mousePosition = _camera.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, 0));
                transform.position = Vector3.Lerp(transform.position, mousePosition, modifiedSpeed * Time.deltaTime);
            }
        }

        public void SetSpeed(int speed)
        {
            _currentSpeed = speed;
        }

        public void MoveWithZoomTo(Transform cityPointShowplaceTransform, float duration, float targetZoom,
            Action onComplete)
        {
            Vector3 targetPosition = new Vector3(cityPointShowplaceTransform.position.x,
                cityPointShowplaceTransform.position.y, transform.position.z);

            transform.DOMove(targetPosition, duration).SetEase(_ease).OnComplete((() =>
            {
                onComplete?.Invoke();
            }));

            DOTween.To(value => { _leanPinchCamera.Zoom = value; }, _leanPinchCamera.Zoom, targetZoom, duration);
        }
        // void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //
        //     Gizmos.DrawWireCube(new Vector3(_rect.x + _rect.width * 0.5f, _rect.y + _rect.height * 0.5f, 0),
        //         new Vector3(_rect.width, _rect.height, 0));
        // }
    }
}