using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.Gameplay.Storages;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class SettingsSaveLoader : SaveLoader<AudioSettingsData, MoneyStorage>
    {
        protected override void SetupData(MoneyStorage starterPackManager, AudioSettingsData startPackData)
        {
            AudioSettingsManager.SetMusicVolume(startPackData.MusicVolume);
            AudioSettingsManager.SetSoundVolume(startPackData.SoundVolume);
            AudioSettingsManager.SetSetVibration(startPackData.IsVibrationActive);
        }

        protected override AudioSettingsData ConvertToData(MoneyStorage service)
        {
            return new AudioSettingsData
            {
                MusicVolume = AudioSettingsManager.MusicVolume,
                SoundVolume = AudioSettingsManager.SoundVolume,
                IsVibrationActive = AudioSettingsManager.IsVibrationActive

            };
        }

        protected override void SetupByDefault(MoneyStorage moneyStorage)
        {
            base.SetupByDefault(moneyStorage);

            AudioSettingsManager.SetMusicVolumeDefault();
            AudioSettingsManager.SetSoundVolumeDefault();
            AudioSettingsManager.SetVibrationDefault();
        }
    }
}