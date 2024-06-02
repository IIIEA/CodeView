using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    [Serializable]
    public class AirplaneInfo
    {
        [SerializeField]
        public int AirplaneCount;

        [SerializeField]
        public List<int> AirplaneLevels = new();
    }
}