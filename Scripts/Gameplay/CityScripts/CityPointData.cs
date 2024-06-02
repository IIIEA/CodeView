using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    [Serializable]
    public struct CityPointData
    {
        public string Name;
        public string Province;
        public int CountryIndex;
        public int Population;
        public Vector2 Unity2DLocation;
        public CITY_CLASS CityClass;
    }

    public enum CITY_CLASS
    {
        CITY = 1,
        REGION_CAPITAL = 2,
        COUNTRY_CAPITAL = 4
    }
}