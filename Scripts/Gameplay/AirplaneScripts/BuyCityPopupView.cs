using System;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.PopupScripts.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public sealed class BuyCityPopupView : MonoWindow<IBuyCityPresenter>
    {
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _closeSprite;
        [SerializeField] private Sprite _openSprite;

        private CityPoint _cityPoint;
        private IBuyCityPresenter _presenter;

        protected override void OnShow(IBuyCityPresenter args)
        {
            if (args is not IBuyCityPresenter presenter)
            {
                throw new Exception("Expected Buy City Presenter");
            }

            _presenter = presenter;

            // _presenter.OnBuyButtonStateChanged += OnBuyButtonStateChanged;
            _image.sprite = _presenter.HasMoney ? _openSprite : _closeSprite;
            gameObject.SetActive(true);
            transform.position = _presenter.CityTransform.transform.position + new Vector3(0,0.16f,0);
            transform.localScale = _presenter.GetPopupScale();
            _priceText.SetText(_presenter.Price);

            _priceText.color = _presenter.HasMoney ? Color.white : Color.red;
        }

        protected override void OnHide()
        {
            // _presenter.OnBuyButtonStateChanged -= OnBuyButtonStateChanged;
            gameObject.SetActive(false);
        }

        private void OnBuyButtonStateChanged()
        {
            _image.sprite = _presenter.HasMoney ? _openSprite : _closeSprite;
            transform.localScale = _presenter.GetPopupScale();
            _priceText.SetText(_presenter.Price);
            _priceText.color = _presenter.HasMoney ? Color.white : Color.red;
        }
    }
}