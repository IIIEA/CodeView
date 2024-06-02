using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Settings_Popup
{
    public abstract class AudioSettingsWidget : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        protected virtual void OnEnable()
        {
            _slider.onValueChanged.AddListener(OnVolumeChanged);
            _slider.value = GetVolume();
        }

        protected virtual void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnVolumeChanged);
        }

        private void OnVolumeChanged(float volume)
        {
            SetVolume(volume);
        }

        protected abstract void SetVolume(float volume);
        
        protected abstract float GetVolume();
        
        protected void UpdateSlider(float volume)
        {
            _slider.value = volume;
        }
    }
}