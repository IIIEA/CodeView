using UnityEngine;

namespace _Fly_Connect.Scripts
{
    public class TargetFrameRate : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = 60;
        
        void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _targetFrameRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
