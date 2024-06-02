using System;
using System.Collections;
using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.SaveLoadSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.VibroSlider
{
    public class VibrationSlider : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private RectTransform _fill;
        private GameRepository _gameRepository;

        public bool IsActive { get; private set; } = true;

        [Inject]
        private void Construct(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);

            if (IsActive)
                Enable(0);
            else
                Disable(0);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (!IsActive)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        private void Enable(float duration = 0.2f)
        {
            IsActive = true;
            AudioSettingsManager.SetSetVibration(IsActive);
            Increase(duration);
            _gameRepository.SaveState();
        }

        private void Disable(float duration = 0.2f)
        {
            IsActive = false;
            AudioSettingsManager.SetSetVibration(IsActive);
            Decrease(duration);
            _gameRepository.SaveState();
        }

        private void Increase(float duration)
        {
            _handle.DOAnchorPosX(55.6f, duration);
            _fill.DOAnchorPosX(32.8f, duration);
            _fill.DOSizeDelta(new Vector2(74.9663f, 54.2388f), duration);
        }

        private void Decrease(float duration)
        {
            _handle.DOAnchorPosX(0.2f, duration);
            _fill.DOAnchorPosX(14f, duration);
            _fill.DOSizeDelta(new Vector2(42, 54.2388f), duration);
        }
    }
}