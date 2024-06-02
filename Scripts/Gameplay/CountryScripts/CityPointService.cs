using System;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [Serializable]
    public class CityPointService
    {
        [field: SerializeField] public CityPoint[] GameCityPoints { get; private set; }

        public void Setup(CityPoint[] cityPoints)
        {
            GameCityPoints = cityPoints;
        }
    }
}