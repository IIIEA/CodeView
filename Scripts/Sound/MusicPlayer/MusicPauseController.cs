using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.MusicPlayer
{
    public sealed class MusicPauseController : MonoBehaviour,
        IGamePauseListener,
        IGameResumeListener
    {
        public void OnPauseGame()
        {
            MusicManager.Pause();
        }

        public void OnResumeGame()
        {
            MusicManager.Resume();
        }
    }
}