using UnityEngine;

namespace _Fly_Connect.Scripts.MapRelief
{
    public sealed class MapRelief : MonoBehaviour
    {
        [SerializeField] private GameObject _mapRelief;

        private bool _isActive;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                _isActive = !_isActive;
                _mapRelief.gameObject.SetActive(_isActive);
            }
        }
    }
}                                                             