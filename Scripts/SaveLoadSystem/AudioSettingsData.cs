using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    [Serializable]
    public struct AudioSettingsData
    {
        [SerializeField]
        public float MusicVolume;

        [SerializeField]
        public float SoundVolume;

        [SerializeField]
        public bool IsVibrationActive;
    }
}