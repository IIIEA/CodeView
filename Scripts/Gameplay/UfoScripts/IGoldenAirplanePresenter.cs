namespace _Fly_Connect.Scripts.Gameplay.GoldenAirplane
{
    public interface IGoldenAirplanePresenter
    {
        void OnRewardButtonClicked();
        void OnMoneyButtonClicked();
        string MoneyPrice { get; }
        string RewardPrice { get; }
        bool IsShowTicketIcon { get; }
    }
}