using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    [Serializable]
    public struct CityData
    {
        [SerializeField] 
        public string Name;

        [SerializeField] 
        public int CurrentAirportLevel;

        [SerializeField] 
        public bool IsBuy;

        [SerializeField]
        public List<int> Routes;
    }
}