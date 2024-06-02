using System;
using _Fly_Connect.Scripts.PopupScripts.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    public class CityAirportView : MonoWindow<IAirportPresenter>
    {
        [field: SerializeField] public Button AirportBuyButton { get; private set; }
        [field: SerializeField] public Button RewardButton { get; private set; }
        [field: SerializeField] public TMP_Text AirportLevelText { get; private set; }
        [field: SerializeField] public TMP_Text AirlaneCount { get; private set; }
        [field: SerializeField] public TMP_Text RouteCount { get; private set; }
        [field: SerializeField] public TMP_Text BuyAirportButtonText { get; private set; }
        [field: SerializeField] public TMP_Text RewardPrice { get; private set; }
        [field: SerializeField] public TMP_Text AirportPrice { get; private set; }
        [field: SerializeField] public TMP_Text MaxText { get; private set; }
        [field: SerializeField] public Image TicketIcon { get; private set; }
        [field: SerializeField] public Image RewardIcon { get; private set; }

        [SerializeField] private Color _redTextColor;
        [SerializeField] private Color _defaultColor;

        private IAirportPresenter _presenter;

        protected override void OnShow(IAirportPresenter args)
        {
            if (args is not IAirportPresenter presenter)
            {
                throw new Exception("Expected Airport Presenter");
            }

            _presenter = presenter;

            RouteCount.SetText(_presenter.RouteText);

            RouteCount.color = _presenter.IsRouteRedTextRed ? _redTextColor : _defaultColor;

            BuyAirportButtonText.SetText(presenter.AirportButtonText);
            AirportBuyButton.interactable = presenter.IsAirportButtonInteractable;
            AirportLevelText.SetText(presenter.AirportLevelText);
            AirportBuyButton.onClick.AddListener(OnBuyAirportButtonClicked);
            RewardButton.onClick.AddListener(OnRewardButtonClicked);
            presenter.OnBuyButtonStateChanged += UpdateButtonState;
            presenter.HasMoney += OnHasMoney;
            AirportPrice.SetText(_presenter.AirportPrice);
            RewardPrice.SetText(_presenter.AirportPrice);
            AirportPrice.gameObject.SetActive(_presenter.IsAirportButtonInteractable);
            var transformPosition = presenter.PopupTransform.position + new Vector3(0, 0.15f, -2);
            transform.position = transformPosition;
            var popupTransformPosition = new Vector3(presenter.CityPosition.x, presenter.CityPosition.y, -250);
            _presenter.BuildingCameraService.Camera.transform.localPosition = popupTransformPosition;
            SetRewardIcon();
            _presenter.CameraController.MoveTo(presenter.PopupTransform.position, new Vector3(0,1f,0));
            transform.localScale = presenter.PopupScale;
            UpdateButtonState();

            AirlaneCount.SetText(presenter.AirplaneCount);
            presenter.Enable();
        }

        protected override void OnHide()
        {
            _presenter.HasMoney -= OnHasMoney;
            _presenter.OnBuyButtonStateChanged -= UpdateButtonState;
            _presenter.Disable();
            RewardButton.onClick.RemoveListener(OnRewardButtonClicked);
            AirportBuyButton.onClick.RemoveListener(OnBuyAirportButtonClicked);
        }

        private void OnHasMoney(bool state)
        {
            AirportBuyButton.interactable = true;
            MaxText.gameObject.SetActive(true);

            if (state)
                RewardButton.gameObject.SetActive(false);
            else if (state == false && _presenter.IsAirportButtonInteractable)
                RewardButton.gameObject.SetActive(true);
            else
                RewardButton.gameObject.SetActive(false);
        }

        private void OnRewardButtonClicked()
        {
            _presenter.OnRewardButtonClicked();
        }

        private void UpdateButtonState()
        {
            var interactable = _presenter.IsAirportButtonInteractable;
            AirportBuyButton.gameObject.SetActive(interactable);
            BuyAirportButtonText.SetText(_presenter.AirportButtonText);
            AirportPrice.SetText(_presenter.AirportPrice);
            RewardPrice.SetText(_presenter.AirportPrice);
            AirportLevelText.SetText(_presenter.AirportLevelText);
            AirportPrice.gameObject.SetActive(_presenter.IsAirportButtonInteractable);
            AirlaneCount.SetText(_presenter.AirplaneCount);
            RouteCount.SetText(_presenter.RouteText);
            RouteCount.color = _presenter.IsRouteRedTextRed ? _redTextColor : _defaultColor;
            SetRewardIcon();
        }

        private void SetRewardIcon()
        {
            if (_presenter.IsShowTicketIcon)
            {
                TicketIcon.gameObject.SetActive(true);
                RewardIcon.gameObject.SetActive(false);
            }
            else
            {
                TicketIcon.gameObject.SetActive(false);
                RewardIcon.gameObject.SetActive(true);
            }
        }

        private void OnBuyAirportButtonClicked()
        {
            _presenter.OnBuyAirportButtonClicked();
            AirportLevelText.SetText(_presenter.AirportLevelText);
            AirlaneCount.SetText(_presenter.AirplaneCount);
        }
    }
}