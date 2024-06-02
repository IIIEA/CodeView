using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    [Serializable]
    public struct CitySellerData
    {
        [SerializeField]
        public int CurrentOpenedAirport;
    }
}