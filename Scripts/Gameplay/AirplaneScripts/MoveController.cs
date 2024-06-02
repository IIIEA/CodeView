using System;
using System.Collections;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public class MoveController : MonoBehaviour, IGameUpdateListener
    {
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private RotateComponent _rotateComponent;
        [SerializeField] private float _speedMultiplier;
        [SerializeField] private GameObject _models;

        private float _t;
        private float _addedUpgradeSpeed;
        private Vector3 _p0;
        private Vector3 _p1;
        private Vector3 _p2;
        private bool _forward = true;
        private bool _paused;
        private float _pauseTimer;
        private float _pauseDuration;
        private float _temporarySpeed;
        private float _baseTapSpeed = 1f;
        private float _boostSpeed = 0;
        private float _delayBetweenSpeedBoost = 3f;
        private bool _canSpeedBoost = true;
        private Route _route;
        private float _distance;
        private Airplane _airplane;

        public float TotalSpeed { get; private set; }

        public event Action<Direction, Vector3, Route, Airplane> OnFinish;
        public event Action<float, bool> OnTChanged;

        private void Awake()
        {
            _airplane = GetComponent<Airplane>();
        }

        public void Construct(Vector3 p0, Vector3 p1, Vector3 p2, Route route)
        {
            _route = route;
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;

            AddZOffset();
            _distance = Vector3.Distance(_p0, _p1) + Vector3.Distance(_p1, _p2);
        }

        public void OnUpdate(float deltaTime)
        {
            _moveComponent.Move(_p0, _p1, _p2, _t);
            _rotateComponent.Rotate(_p0, _p1, _p2, _t, _forward);

            if (!_paused)
            {
                SetTValue();
            }
            else
            {
                _pauseTimer += Time.deltaTime;

                if (_pauseTimer >= _pauseDuration)
                {
                    _paused = false;
                    _models.gameObject.SetActive(true);
                    _pauseTimer = 0f;
                }
            }
        }

        public void SetAddedSpeed(float addedSpeed)
        {
            float increaseAmount = _speedMultiplier * (addedSpeed / 100f);
            _addedUpgradeSpeed = increaseAmount;
        }

        public void AddTemporarySpeed(float tapSpeed, Action<Route> onComplete = null)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(IncreaseSpeedCoroutine(tapSpeed, onComplete));
            }
        }

        private void SetTValue()
        {
            float directionMultiplier = _forward ? 1 : -1;
            float normalizedDistance = _t * _distance;

            var speed = directionMultiplier * (_speedMultiplier + _addedUpgradeSpeed + _temporarySpeed + _boostSpeed);
            TotalSpeed = directionMultiplier * (_speedMultiplier + _addedUpgradeSpeed);
            float distanceDelta = speed * Time.deltaTime;
            _t += distanceDelta / _distance;
            _t = Mathf.Clamp01(_t);


            if ((_forward && normalizedDistance >= _distance) || (!_forward && normalizedDistance <= 0))
            {
                if (_forward)
                {
                    _pauseDuration = 0;
                }
                else
                {
                    _pauseDuration = 0;
                }

                _forward = !_forward;
                OnFinish?.Invoke(_forward ? Direction.Forward : Direction.Backward, transform.position, _route,
                    _airplane);
                _paused = true;
                _models.gameObject.SetActive(false);
            }

            OnTChanged?.Invoke(_t, _forward);
            transform.Translate(Vector3.forward * distanceDelta);
        }

        private void AddZOffset()
        {
            _p0 += new Vector3(0, 0, -1);
            _p1 += new Vector3(0, 0, -1);
            _p2 += new Vector3(0, 0, -1);
        }

        private IEnumerator IncreaseSpeedCoroutine(float speed, Action<Route> onComplete = null)
        {
            var percent = _baseTapSpeed * (speed / 100f);
            _temporarySpeed = 0;
            _temporarySpeed = percent;

            _temporarySpeed += _baseTapSpeed;

            yield return new WaitForSeconds(_delayBetweenSpeedBoost);

            while (_temporarySpeed >= 0)
            {
                _temporarySpeed -= 0.5f * Time.deltaTime;
                yield return null;
            }

            _temporarySpeed = 0;

            onComplete?.Invoke(_route);
        }

        public void EnableX2Speed()
        {
            _boostSpeed += (_speedMultiplier + _addedUpgradeSpeed) * 2;
        }


        public void DisableX2Speed()
        {
            _boostSpeed = 0;
        }

        public bool IsForwardDirection()
        {
            return _forward;
        }
    }
}