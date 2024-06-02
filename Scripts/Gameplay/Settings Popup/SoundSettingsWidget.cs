namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public sealed class SoundSettingsWidget : AudioSettingsWidget
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            AudioSettingsManager.OnSoundVolumeChanged += UpdateSlider;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            AudioSettingsManager.OnSoundVolumeChanged -= UpdateSlider;
        }

        protected override void SetVolume(float volume)
        {
            AudioSettingsManager.SetSoundVolume(volume);
        }

        protected override float GetVolume()
        {
            return AudioSettingsManager.SoundVolume;
        }
    }
}