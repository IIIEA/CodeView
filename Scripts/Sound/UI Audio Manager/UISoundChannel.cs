using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.UI_Audio_Manager
{
    public sealed class UISoundChannel : MonoBehaviour
    {
        [SerializeField]
        private bool _isEnable;

        [Range(0, 1)]
        [SerializeField]
        private float _volume;

        [Space]
        [SerializeField]
        private AudioSource source;

        public event Action<bool> OnEnabled;

        public event Action<float> OnVolumeChanged;

        public bool IsEnable
        {
            get { return _isEnable; }
            set => SetEnable(value);
        }

        public float Volume
        {
            get { return _volume; }
            set => SetVolume(value);
        }

        public void PlaySound(AudioClip clip)
        {
            if (_isEnable)
            {
                source.PlayOneShot(clip);
            }
        }

        private void Awake()
        {
            source.volume = _volume;
            source.enabled = _isEnable;
        }

        private void SetEnable(bool enable)
        {
            if (_isEnable == enable)
            {
                return;
            }

            _isEnable = enable;
            source.enabled = enable;
            OnEnabled?.Invoke(enable);
        }

        private void SetVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);

            if (Mathf.Approximately(volume, _volume))
            {
                return;
            }

            _volume = volume;
            source.volume = volume;
            OnVolumeChanged?.Invoke(volume);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                source.volume = _volume;
                source.enabled = _isEnable;
            }
            catch (Exception)
            {
            }
        }
#endif
    }
}