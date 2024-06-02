namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public interface ISettingPresenter
    {
        void Enable();
        void Disable();

        void OnExitButtonClicked();
        void OnRateButtonClicked();
        bool IsShowRateUsButton { get; }
    }
}