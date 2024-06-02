using UnityEngine;

namespace _Fly_Connect.Scripts
{
    public sealed class RunInBackgroundScript : MonoBehaviour
    {
        [SerializeField]
        private bool _runInBackground;

        private void Awake()
        {
            Application.runInBackground = _runInBackground;
        }
    }
}