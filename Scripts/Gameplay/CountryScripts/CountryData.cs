using System;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [Serializable]
    public struct CountryData
    {
        public string Name;
        public int Index;
        public int TotalPopulation;
        public int MinPopulation;
        public int MaxPopulation;
    }
}