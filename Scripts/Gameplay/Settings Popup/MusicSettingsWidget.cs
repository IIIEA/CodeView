namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public sealed class MusicSettingsWidget : AudioSettingsWidget
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            AudioSettingsManager.OnMusicVolumeChanged += UpdateSlider;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            AudioSettingsManager.OnMusicVolumeChanged -= UpdateSlider;
        }

        protected override void SetVolume(float volume)
        {
            AudioSettingsManager.SetMusicVolume(volume);
        }

        protected override float GetVolume()
        {
            var musicVolume = AudioSettingsManager.MusicVolume;
            return musicVolume;
        }
    }
}