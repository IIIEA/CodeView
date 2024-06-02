using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CamerasScripts
{
    [Serializable]
    public class BuildingCameraService
    {
        [field:SerializeField] public Camera Camera { get; private set; }
    }
}