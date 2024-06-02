using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.UI_Audio_Manager
{
    [CreateAssetMenu(
        fileName = "UISoundCatalog",
        menuName = "Audio/New UISoundCatalog"
    )]
    public sealed class UISoundCatalog : ScriptableObject
    {
        [SerializeField] private Sounds[] _sounds;

        public bool TryFindClip(UISoundType type, out AudioClip clip)
        {
            for (int i = 0, count = _sounds.Length; i < count; i++)
            {
                var sound = _sounds[i];
                if (sound.type == type)
                {
                    clip = sound.clip;
                    return true;
                }
            }

            clip = null;
            return false;
        }

        public AudioClip FindClip(UISoundType type)
        {
            for (int i = 0, count = _sounds.Length; i < count; i++)
            {
                var sound = _sounds[i];
                if (sound.type == type)
                {
                    return sound.clip;
                }
            }

            throw new Exception($"Sound {type} is not found!");
        }

        [Serializable]
        private sealed class Sounds
        {
            [SerializeField] public UISoundType type;

            [SerializeField] public AudioClip clip;
        }
    }

    [Serializable]
    public enum UISoundType
    {
        CLICK = 0,
        ERROR = 1,
        ACCEPT = 2,
        CLOSE = 3,
        BUY = 4,
        SHOW_POPUP = 5,
        AIRPORT_BUY = 6,
        BOOSTER = 7,
        AIRPORT_MAX_LEVEL = 8,
        AIRPORT_CHOOSEN = 9,
    }
}