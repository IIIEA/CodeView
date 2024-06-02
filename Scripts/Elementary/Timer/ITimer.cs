using System;

namespace _Fly_Connect.Scripts.Elementary.Timer
{
    public interface ITimer
    {
        event Action OnStarted;

        event Action OnTimeChanged;

        event Action OnCanceled;

        event Action OnFinished;

        event Action OnReset;

        bool IsPlaying { get; }

        float Progress { get; set; }

        float Duration { get; set; }

        float CurrentTime { get; set; }

        void Play();

        void Stop();

        void ResetTime();
    }
}