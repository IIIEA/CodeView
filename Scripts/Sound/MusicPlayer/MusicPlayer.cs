using System;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Fly_Connect.Scripts.Sound.MusicPlayer
{
    public sealed class MusicPlayer : MonoBehaviour
    {
        public event Action<bool> OnMuted;
        public event Action<float> OnVolumeChanged;
        public event Action OnStarted;
        public event Action OnPaused;
        public event Action OnResumed;
        public event Action OnStopped;
        public event Action OnFinsihed;

        public bool IsMute
        {
            get { return _isMute; }
            set => SetMute(value);
        }

        public float Volume
        {
            get { return _audioSource.volume; }
            set => this.SetVolume(value);
        }

        [PropertySpace(8.0f)]
        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-8)]
        public MusicState State
        {
            get { return _state; }
        }

        [PropertyOrder(-7)]
        [ReadOnly]
        [ShowInInspector]
        public AudioClip CurrentMusic
        {
            get { return GetCurrentMusic(); }
        }

        [PropertyOrder(-6)]
        [ReadOnly]
        [ShowInInspector]
        [ProgressBar(min: 0, max: 1, r: 1f, g: 0.83f, b: 0f)]
        public float PlayingProgress
        {
            get { return GetPlayingProgress(); }
        }

        [PropertySpace(8.0f)]
        [PropertyOrder(-10)]
        [SerializeField]
        private bool _isMute;

        [PropertyOrder(-9)]
        [Range(0, 1.0f)]
        [SerializeField]
        private float _volume;
        
        private MusicState _state;

        [PropertySpace(8.0f)]
        [PropertyOrder(-2)]
        [SerializeField]
        private AudioSource _audioSource;
        
        [PropertySpace(8.0f)]
        [SerializeField]
        private bool _randomizePitch = true;

        [SerializeField, ShowIf(nameof(_randomizePitch))]
        private float pitchOffset = 0.2f;

        [Title("Methods")]
        [GUIColor(1f, 0.83f, 0f)]
        [Button]
        public void Play(AudioClip music)
        {
            if (_state != MusicState.IDLE)
            {
                Debug.LogWarning("Music is already started!");
                return;
            }

            _state = MusicState.PLAYING;
            
            if (_randomizePitch)
            {
                _audioSource.pitch = Random.Range(1 - pitchOffset, 1 + pitchOffset);
            }
            
            _audioSource.clip = music;
            _audioSource.Play();
            OnStarted?.Invoke();
        }

        [GUIColor(1f, 0.83f, 0f)]
        [Button]
        public void Pause()
        {
            if (_state != MusicState.PLAYING)
            {
                Debug.LogWarning("Music is not playing!");
                return;
            }

            _state = MusicState.PAUSED;
            _audioSource.Pause();
            OnPaused?.Invoke();
        }

        [GUIColor(1f, 0.83f, 0f)]
        [Button]
        public void Resume()
        {
            if (_state != MusicState.PAUSED)
            {
                Debug.LogWarning("Music is not paused!");
                return;
            }

            _state = MusicState.PLAYING;
            _audioSource.UnPause();
            OnResumed?.Invoke();
        }

        [GUIColor(1f, 0.83f, 0f)]
        [Button]
        public void Stop()
        {
            if (_state == MusicState.IDLE)
            {
                Debug.LogWarning("Music is not playing!");
                return;
            }

            _state = MusicState.IDLE;
            _audioSource.Stop();
            _audioSource.clip = null;
            OnStopped?.Invoke();
        }

        private void Finish()
        {
            _state = MusicState.IDLE;
            _audioSource.Stop();
            _audioSource.clip = null;
            OnFinsihed?.Invoke();
        }

        private void Awake()
        {
            _audioSource.volume = _volume;
            _audioSource.mute = _isMute;
            _state = MusicState.IDLE;
        }

        private void Update()
        {
            if (_state == MusicState.PLAYING && _audioSource.time >= _audioSource.clip.length)
            {
                Finish();
            }
        }

        private void SetVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);

            if (Mathf.Approximately(volume, _volume))
            {
                return;
            }

            _volume = volume;
            _audioSource.volume = volume;
            OnVolumeChanged?.Invoke(volume);
        }

        private void SetMute(bool mute)
        {
            if (_isMute == mute)
            {
                return;
            }

            _isMute = mute;
            _audioSource.mute = mute;
            OnMuted?.Invoke(mute);
        }

        private float GetPlayingProgress()
        {
            if (_state == MusicState.IDLE)
            {
                return 0.0f;
            }

            if (_audioSource == null || _audioSource.clip == null)
            {
                return 0.0f;
            }

            return _audioSource.time / _audioSource.clip.length;
        }
        
        private AudioClip GetCurrentMusic()
        {
            if (_audioSource != null)
            {
                return _audioSource.clip;
            }

            return null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                _audioSource.volume = _volume;
                _audioSource.mute = _isMute;
            }
            catch (Exception)
            {
            }
        }
#endif
    }
}