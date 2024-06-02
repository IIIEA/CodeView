using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    [Serializable]
    public struct MapData
    {
        [SerializeField]
        public List<int> OrderBuyedCountry;
    }
}