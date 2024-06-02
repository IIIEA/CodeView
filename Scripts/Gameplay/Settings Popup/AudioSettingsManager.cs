using System;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using _Fly_Connect.Scripts.Sound.Scene_Audio_Manager;
using _Fly_Connect.Scripts.Sound.UI_Audio_Manager;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.Audio;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public static class AudioSettingsManager
    {
        private const string MIXER_PATH = "AudioMixer";

        private const float DEFAULT_MUSIC_VOLUME = 0.5f;
        private const float DEFAULT_SOUND_VOLUME = 0.5f;

        public static float MusicVolume { get; private set; }
        public static float SoundVolume { get; private set; }
        public static bool IsVibrationActive { get; private set; }

        public static event Action<float> OnMusicVolumeChanged;
        public static event Action<float> OnSoundVolumeChanged;

        private static AudioMixer _mixer;

        public static AudioMixer Mixer
        {
            get
            {
                if (_mixer == null)
                    _mixer = Resources.Load<AudioMixer>(MIXER_PATH);
                return _mixer;
            }
        }

        public static void SetMusicVolumeDefault()
        {
            SetMusicVolume(DEFAULT_MUSIC_VOLUME);
        }

        public static void SetSoundVolumeDefault()
        {
            SetSoundVolume(DEFAULT_SOUND_VOLUME);
        }

        public static void SetMusicVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            MusicVolume = volume;

            MusicManager.Volume = volume;
            OnMusicVolumeChanged?.Invoke(volume);
        }

        public static void SetSoundVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            SoundVolume = volume;

            UISoundManager.Volume = volume;
            SceneAudioManager.SetVolumeAll(volume);

            OnSoundVolumeChanged?.Invoke(volume);
        }

        public static void SetVibrationDefault()
        {
            HapticController.hapticsEnabled = true;
        }

        public static void SetSetVibration(bool isVibrationActive)
        {
            IsVibrationActive = isVibrationActive;
        }
    }
}