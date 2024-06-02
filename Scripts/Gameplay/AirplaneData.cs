using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Storages
{
    [Serializable]
    public struct AirplaneData
    {
        [SerializeField]
        public int AirplaneCount;

        [SerializeField]
        public List<int> AirplaneLevels;
    }
}