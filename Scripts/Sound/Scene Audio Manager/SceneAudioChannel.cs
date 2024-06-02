using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.Scene_Audio_Manager
{
    public sealed class SceneAudioChannel : MonoBehaviour
    {
        private const float BLOCKED_AUDIO_DELAY = 0.1f;

        public bool IsEnabled
        {
            get { return _isEnable; }
            set => this.SetEnable(value);
        }

        public float Volume
        {
            get { return _volume; }
            set => this.SetVolume(value);
        }

        [SerializeField]
        private bool _isEnable;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float _volume;

        [Space]
        [SerializeField]
        private AudioSource _source;

        [SerializeField]
        private bool _controlClips;
        
        private readonly List<BlockedAudio> _blockedClips = new();
        private readonly List<BlockedAudio> _cache = new();

        private readonly List<ISceneAudioListener> _listeners = new();

        public void PlaySound(AudioClip clip)
        {
            if (!_isEnable)
            {
                return;
            }

            var clipName = clip.name;
            
            if (_controlClips)
            {
                if (IsBlocked(clipName))
                {
                    return;
                }

                _blockedClips.Add(new BlockedAudio(clipName, BLOCKED_AUDIO_DELAY));
            }

            _source.PlayOneShot(clip);
        }

        private void SetEnable(bool enable)
        {
            if (_isEnable == enable)
            {
                return;
            }

            _isEnable = enable;
            _source.enabled = enable;

            for (int i = 0, count = _listeners.Count; i < count; i++)
            {
                var observer = _listeners[i];
                observer.OnEnabled(enable);
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
            _source.volume = volume;

            for (int i = 0, count = _listeners.Count; i < count; i++)
            {
                var observer = _listeners[i];
                observer.OnVolumeChanged(volume);
            }
        }

        public void AddListener(ISceneAudioListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(ISceneAudioListener listener)
        {
            _listeners.Remove(listener);
        }

        private void Awake()
        {
            _source.enabled = _isEnable;
            _source.volume = _volume;
        }

        private void Update()
        {
            if (_isEnable)
            {
                ProcessBlockedClips(Time.deltaTime);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                Awake();
            }
            catch (Exception)
            {
            }
        }
#endif

        private bool IsBlocked(string soundName)
        {
            for (int i = 0, count = _blockedClips.Count; i < count; i++)
            {
                var clip = _blockedClips[i];
                if (clip.name == soundName)
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessBlockedClips(float deltaTime)
        {
            if (!_controlClips)
            {
                return;
            }
            
            _cache.Clear();
            _cache.AddRange(_blockedClips);

            for (int i = 0, count = _cache.Count; i < count; i++)
            {
                var clip = _cache[i];
                var remainingTime = clip.delay - deltaTime;

                if (remainingTime <= 0)
                {
                    _blockedClips.RemoveAt(i);
                }
                else
                {
                    _blockedClips[i] = new BlockedAudio(clip.name, remainingTime);
                }
            }
        }

        private readonly struct BlockedAudio
        {
            public readonly string name;
            public readonly float delay;

            public BlockedAudio(string name, float delay)
            {
                this.name = name;
                this.delay = delay;
            }
        }
    }
}