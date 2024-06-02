using System;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using DG.Tweening;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public class AirplaneScaleController : MonoBehaviour, IGameFinishListener
    {
        [SerializeField] private Transform _model;
        [SerializeField] private MoveController _moveController;
        [SerializeField] private float _targetScale;

        private Tween _scaleTween;

        public void Awake()
        {
            _moveController.OnTChanged += OnTChanged;
        }

        private void OnDestroy()
        {
            _scaleTween.Kill();
        }

        public void OnFinishGame()
        {
            _moveController.OnTChanged -= OnTChanged;
        }

        private void OnTChanged(float t, bool forward)
        {
            if (forward && t > 0.5f)
            {
                IncreaseScale(t);
            }
            else if (!forward && t > 0.5f)
            {
                IncreaseScale(t);
            } 
            else if(forward && t < 0.5f)
            {
                DecreaseScale(t);
            }
            else if(!forward && t < 0.5f)
            {
                DecreaseScale(t);
            }
        }

        private void DecreaseScale(float t)
        {
            float scale = Remap.DoRemap(0, 1, _targetScale, 1, t);
            _model.transform.localScale = new Vector3(scale, scale, scale);
        }

        private void IncreaseScale(float t)
        {
            float scale = Remap.DoRemap(0, 1, 1, _targetScale, t);
            _model.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}