using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.Scene_Audio_Manager
{
    public class SceneAudioManager : MonoBehaviour
    {
        public static event Action OnInitialized;

        public static bool IsInitialized
        {
            get { return _instanceExists; }
        }

        private static SceneAudioManager _instance;

        private static bool _instanceExists;

        [SerializeField] private ChannelHolder[] _channels;

        public static void PlaySound(SceneAudioType channelType, AudioClip sound)
        {
            if (!_instanceExists)
                return;

            var channel = _instance.FindChannel(channelType);
            channel.PlaySound(sound);
        }

        public static bool IsEnable(SceneAudioType type)
        {
            if (!_instanceExists)
                return default;

            var channel = _instance.FindChannel(type);
            return channel.IsEnabled;
        }

        public static void SetEnable(SceneAudioType type, bool enabled)
        {
            if (!_instanceExists)
                return;

            var channel = _instance.FindChannel(type);
            channel.IsEnabled = enabled;
        }

        public static void SetEnableAll(bool enabled)
        {
            if (!_instanceExists)
                return;

            var holders = _instance._channels;
            for (int i = 0, count = holders.Length; i < count; i++)
            {
                var holder = holders[i];
                holder.channel.IsEnabled = enabled;
            }
        }

        public static float GetVolume(SceneAudioType type)
        {
            if (!_instanceExists)
                return default;

            var channel = _instance.FindChannel(type);
            return channel.Volume;
        }

        public static void SetVolume(SceneAudioType type, float volume)
        {
            if (!_instanceExists)
                return;

            var channel = _instance.FindChannel(type);
            channel.Volume = volume;
        }

        public static void SetVolumeAll(float volume)
        {
            if (!_instanceExists)
                return;

            var holders = _instance._channels;

            for (int i = 0, count = holders.Length; i < count; i++)
            {
                var holder = holders[i];
                holder.channel.Volume = volume;
            }
        }

        public static void AddListener(SceneAudioType type, ISceneAudioListener listener)
        {
            if (!_instanceExists)
                return;

            var channel = _instance.FindChannel(type);
            channel.AddListener(listener);
        }

        public static void RemoveListener(SceneAudioType type, ISceneAudioListener listener)
        {
            if (!_instanceExists)
                return;

            var channel = _instance.FindChannel(type);
            channel.RemoveListener(listener);
        }

        private SceneAudioChannel FindChannel(SceneAudioType channelType)
        {
            for (int i = 0, count = _channels.Length; i < count; i++)
            {
                var holder = _channels[i];
                if (holder.channelType == channelType)
                {
                    return holder.channel;
                }
            }

            throw new Exception($"Channel of type {channelType} is not found!");
        }

        private void OnEnable()
        {
            if (_instanceExists)
                throw new Exception("GameAudioSystem is already exists!");

            _instance = this;
            _instanceExists = true;
            OnInitialized?.Invoke();
        }

        private void OnDisable()
        {
            _instanceExists = false;
            _instance = null;
        }

        [Serializable]
        public sealed class ChannelHolder
        {
            [SerializeField] public SceneAudioType channelType;

            [SerializeField] public SceneAudioChannel channel;
        }
    }
}