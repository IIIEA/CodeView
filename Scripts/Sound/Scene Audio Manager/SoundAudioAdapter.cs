using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.Scene_Audio_Manager
{
    public class SoundAudioAdapter : MonoBehaviour
    {
        public sealed class SceneAudioAdapter : MonoBehaviour, ISceneAudioListener
        {
            [SerializeField]
            private SceneAudioType _audioType;

            [Space]
            [SerializeField]
            private AudioSource[] _audioSources;

            private void OnEnable()
            {
                if (SceneAudioManager.IsInitialized)
                {
                    Initialize();
                }
                else
                {
                    SceneAudioManager.OnInitialized += Initialize;
                }
            }

            private void OnDisable()
            {
                SceneAudioManager.RemoveListener(_audioType, this);
            }

            private void Initialize()
            {
                SceneAudioManager.OnInitialized -= Initialize;
                SceneAudioManager.AddListener(_audioType, this);
                this.SetEnable(SceneAudioManager.IsEnable(_audioType));
                this.SetVolume(SceneAudioManager.GetVolume(_audioType));
            }

            void ISceneAudioListener.OnEnabled(bool enabled)
            {
                SetEnable(enabled);
            }

            void ISceneAudioListener.OnVolumeChanged(float volume)
            {
                SetVolume(volume);
            }
        
            private void SetEnable(bool enabled)
            {
                for (int i = 0, count = _audioSources.Length; i < count; i++)
                {
                    var source = _audioSources[i];
                    source.enabled = enabled;
                }
            }

            private void SetVolume(float volume)
            {
                for (int i = 0, count = this._audioSources.Length; i < count; i++)
                {
                    var source = this._audioSources[i];
                    source.volume = volume;
                }
            }
        }
    }
}
