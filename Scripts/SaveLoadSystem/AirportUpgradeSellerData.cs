using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    [Serializable]
    public struct AirportUpgradeSellerData
    {
        [SerializeField]
        public int OrderUpgradeAirport;
        
        [SerializeField]
        public int MaxLevelCityCount;

        [SerializeField]
        public int CityCount;
    }
}