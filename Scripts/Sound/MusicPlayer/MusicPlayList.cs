using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.MusicPlayer
{
     [CreateAssetMenu(fileName = "MusicConfig", menuName = "Gameplay/Audio/New MusicConfig")]
     public class MusicPlayList : ScriptableObject
     {
          [SerializeField] public AudioClip[] TrackList;
     }
}