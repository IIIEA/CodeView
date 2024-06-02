using System;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Sound.Scene_Audio_Manager;
using Lofelt.NiceVibrations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Fly_Connect.Scripts.ActionPopupSystem
{
    public sealed class ActionPopupView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private AudioClip _closeSound;
        [SerializeField] private TMP_Text _priceText;
        [field: SerializeField] public RectTransform View;

        private CityPoint _cityPoint;
        private MoneyPool.MoneyPool _moneyPool;
        public bool IsActive { get; private set; }

        public event Action<ActionPopupView> OnButtonClick;

        public void Construct(MoneyPool.MoneyPool moneyPool)
        {
            _moneyPool = moneyPool;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, _closeSound);
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            OnButtonClick?.Invoke(this);
        }

        public void Enable(CityPoint cityPoint)
        {
            _cityPoint = cityPoint;
            _cityPoint.ShowActionPopup = true;
            IsActive = true;
        }

        public void Disable(string money, bool isAutoclose = false)
        {
            if (!isAutoclose)
            {
                var moneyEffect = _moneyPool.Get();
                moneyEffect.transform.position = _cityPoint.transform.position;
                moneyEffect.Show("+" + money, 2f, Color.green);
            }

            _cityPoint.ShowActionPopup = false;
            _cityPoint = null;
            IsActive = false;
        }

        public void SetPrice(string price)
        {
            _priceText.SetText(price);
        }
    }
}