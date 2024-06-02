using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    public class CityPointView : SerializedMonoBehaviour
    {
        [OdinSerialize] private List<Texture> _textures = new();
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private TMP_Text _titleText;

        [field: SerializeField] public SpriteRenderer Outline { get; private set; }
        [field: SerializeField] public SpriteRenderer PulsationOutline { get; private set; }

        private Sequence _scaleTween;
        private bool _canPlayTween = true;
        private float _scaleDuration = 0.3F;
        private Vector3 _targetScale;
        private Vector3 _startScale;

        public CityPoint CityPoint { get; private set; }

        private void Awake()
        {
            CityPoint = GetComponentInParent<CityPoint>();
            CityPoint.OnCityUpgrade += OnCityUpgrade;
            CityPoint.Routes.CountChanged += OnCountChanged;
            _startScale = transform.localScale;
            _targetScale = _startScale + new Vector3(0.5f, 0.5f, 0.5f);
        }

        private void OnDestroy()
        {
            CityPoint.OnCityUpgrade -= OnCityUpgrade;
            CityPoint.Routes.CountChanged -= OnCountChanged;
        }

        private void OnCountChanged(int count)
        {
            if (count != CityPoint.CurrentAirportLevel)
            {
                if (_canPlayTween)
                {
                    _canPlayTween = false;

                    _scaleTween = DOTween.Sequence();
                    PulsationOutline.gameObject.SetActive(true);

                    _scaleTween
                        .Append(PulsationOutline.transform.DOScale(0.2f, 0.75f))
                        .Append(PulsationOutline.transform.DOScale(0.17f, 0.75f)).SetLoops(-1).SetEase(Ease.Linear);
                }
            }
            else
            {
                if (_scaleTween != null)
                    _scaleTween.Kill();

                PulsationOutline.gameObject.SetActive(false);
                _canPlayTween = true;
            }
        }

        private void OnCityUpgrade(CityPoint cityPoint)
        {
            if (_textures == null)
                return;

            var cityPointCurrentAirportLevel = CityPoint.CurrentAirportLevel - 1;

            if (cityPointCurrentAirportLevel >= 0)
            {
                var texture = _textures[cityPointCurrentAirportLevel];
                SetTexture(texture);
            }

            OnCountChanged(cityPoint.Routes.Count);
        }

        public void SetTitle(string title)
        {
            _titleText.SetText(title);
        }

        private void SetTexture(Texture cityPointTexture)
        {
            _meshRenderer.material.SetTexture("_BaseMap", cityPointTexture);
        }

        public void EnableTitle()
        {
            if (_titleText.gameObject.activeSelf == false)
                _titleText.gameObject.SetActive(true);
        }

        public void DisableTitle()
        {
            if (_titleText.gameObject.activeSelf == true)
                _titleText.gameObject.SetActive(false);
        }

        public void SetSelectedScale()
        {
            if (transform.localScale.x <= _targetScale.x)
                transform.DOScale(_targetScale, _scaleDuration);
        }

        public void SetDefaultScale()
        {
            if (transform.localScale != _startScale)
                transform.DOScale(_startScale, _scaleDuration);
        }

        public void SetScale(float scale)
        {
            if (transform.localScale != new Vector3(scale, scale, scale))
            {
                transform.DOScale(new Vector3(scale, scale, scale), _scaleDuration);
            }
        }
    }
}