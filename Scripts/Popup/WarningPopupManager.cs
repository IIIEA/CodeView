using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Fly_Connect.Scripts.Popup
{
    [Serializable]
    public class WarningPopupManager : IGameStartListener
    {
        [SerializeField] private WarningPopup _warningPopupPrefab;
        [SerializeField] private Transform _container;

        private bool _canShow = true;
        private Queue<WarningPopup> _pool = new();
        private int _poolCount = 30;
        private Sequence _rotateTween;
        private LeanDragCamera _leanDragCamera;

        [Inject]
        private void Construct(LeanDragCamera leanDragCamera)
        {
            _leanDragCamera = leanDragCamera;
        }

        public void OnStartGame()
        {
            for (int i = 0; i < _poolCount; i++)
            {
                var warningPopup = Create();
                warningPopup.gameObject.SetActive(false);
                _pool.Enqueue(warningPopup);
            }
        }

        public WarningPopup Get()
        {
            if (!_canShow)
                return null;

            var warningPopup = _pool.TryDequeue(out var result) ? result : Create();
            warningPopup.gameObject.SetActive(true);
            warningPopup.DoScale(1, 0.7f);
            warningPopup.transform.localRotation = Quaternion.identity;
            _canShow = false;
            _rotateTween = DOTween.Sequence();
            _leanDragCamera.enabled = false;

            _rotateTween
                .Append(warningPopup.transform.DOLocalRotate(new Vector3(0,0,10), 0.25f))
                .Append(warningPopup.transform.DOLocalRotate(new Vector3(0,0,-10), 0.25f)).SetLoops(-1);

            warningPopup.DoAlpha(1, 0, () =>
            {
                warningPopup.DoAlpha(0.7f, 1, () =>
                {
                    _leanDragCamera.enabled = true;
                    _canShow = true;
                    _rotateTween.Kill();
                    warningPopup.gameObject.SetActive(false);
                }, 1);
                _pool.Enqueue(warningPopup);
            });

            return warningPopup;
        }

        public WarningPopup Create()
        {
            return Object.Instantiate(_warningPopupPrefab, _container);
        }
    }
}