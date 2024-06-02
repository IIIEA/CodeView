using System;
using System.Collections;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.Tutorial;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts
{
    [Serializable, InlineProperty]
    public sealed class FingerTapManager : IGameStartListener, IGameFinishListener
    {
        [SerializeField] private RectTransform _finger;
        [SerializeField] private float _timeBetweenShowFinger;

        private CanvasGroup _canvasGroup;
        private InputSystem _inputSystem;
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _fingerShowRoutine;
        private float _startAlpha;

        [Inject]
        private void Construct(InputSystem inputSystem, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _inputSystem = inputSystem;
            _canvasGroup = _finger.GetComponent<CanvasGroup>();
            _startAlpha = _canvasGroup.alpha;
        }

        public void OnStartGame()
        {
            _inputSystem.OnPathTap += OnTap;
        }

        public void OnFinishGame()
        {
            _inputSystem.OnPathTap -= OnTap;
        }

        private void OnTap(Vector3 _)
        {
            if (!TutorialManager.Instance.IsCompleted)
                return;

            if (_fingerShowRoutine != null)
                _coroutineRunner.StopCoroutine(_fingerShowRoutine);

            if (_canvasGroup.alpha == _startAlpha)
            {
                _canvasGroup.DOFade(0, 1).OnComplete((() => { _finger.gameObject.SetActive(false); }));
            }

            _fingerShowRoutine = _coroutineRunner.StartCoroutine(ShowFinger());
        }

        private IEnumerator ShowFinger()
        {
            yield return new WaitForSeconds(_timeBetweenShowFinger);
            _canvasGroup.alpha = _startAlpha;
            _finger.gameObject.SetActive(true);
        }
    }
}