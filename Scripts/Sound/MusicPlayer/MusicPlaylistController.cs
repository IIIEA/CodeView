using System.Collections;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Sound.Music_Manager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.MusicPlayer
{
    public sealed class MusicPlaylistController : MonoBehaviour,
        IGameStartListener,
        IGameFinishListener
    {
        [SerializeField] private MusicPlayList _playList;

        [SerializeField] private float _pauseBetweenTracks = 1.5f;

        private int _trackPointer;

        public void OnStartGame()
        {
            if (_playList.TrackList.Length == 0)
                return;

            MusicManager.OnFinsihed += OnMusicFinished;

            var track = _playList.TrackList[0];


            MusicManager.Play(track);
        }

        public void OnFinishGame()
        {
            MusicManager.OnFinsihed -= OnMusicFinished;
            MusicManager.Stop();
        }

        private void OnMusicFinished()
        {
            _trackPointer++;

            if (_trackPointer >= _playList.TrackList.Length)
            {
                _trackPointer = 0;
            }

            StartCoroutine(PlayNextTrack());
        }

        private IEnumerator PlayNextTrack()
        {
            yield return new WaitForSeconds(_pauseBetweenTracks);
            var nextTrack = _playList.TrackList[_trackPointer];
            MusicManager.Play(nextTrack);
        }
    }
}