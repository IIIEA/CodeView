namespace _Fly_Connect.Scripts.Sound.Scene_Audio_Manager
{
    public interface ISceneAudioListener
    {
        void OnEnabled(bool enabled);

        void OnVolumeChanged(float volume);
    }
}