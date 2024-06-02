using System;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public interface ICountryPresenter
    {
        public string Title { get; }
        bool IsButtonInteractable { get; }
        string BuyButtonText { get; }
        bool IsShowTicketIcon { get; }
        void OnBuyButtonClicked();
        void Enable();
        void Disable();
        
        event Action OnBuyButtonStateChanged;
        void OnExitButtonClicked();
        void OnRewardButtonClicked();
    }
}