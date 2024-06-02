using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Fly_Connect.Scripts.Popup
{
    public class WarningPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _duration = 0.25f;

        public void DoAlpha(float alpha, float startAlpha, Action onComplete = null, float delay = 0)
        {
            _canvasGroup.alpha = startAlpha;
            _canvasGroup.DOFade(alpha, _duration).SetDelay(delay).OnComplete(() => onComplete?.Invoke());
        }

        public void DoScale(float scale, float startScale)
        {
            transform.localScale = new Vector3(startScale, startScale, 0);
            transform.DOScale(new Vector3(scale, scale, 0), _duration);
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}