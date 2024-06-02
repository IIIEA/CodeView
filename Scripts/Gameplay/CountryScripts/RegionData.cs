using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [CreateAssetMenu(fileName = "RegionData", menuName = "Custom/RegionData", order = 1)]
    public class RegionData : SerializedScriptableObject
    {
        [OdinSerialize] public List<CountryRegionData> AllCountryRegionData = new();

        public void SaveData(string countryName, List<List<Vector3>> regionPointsList)
        {
            CountryRegionData countryRegionData = AllCountryRegionData.Find(data => data.CountryName == countryName);

            if (countryRegionData == null)
            {
                countryRegionData = new CountryRegionData
                {
                    CountryName = countryName,
                    RegionPointsList = regionPointsList
                };
                AllCountryRegionData.Add(countryRegionData);
            }
            else
            {
                countryRegionData.RegionPointsList = regionPointsList;
            }

            countryRegionData.RegionCount = regionPointsList.Count;
        }

        public List<List<Vector3>> LoadData(string countryName)
        {
            CountryRegionData countryRegionData = AllCountryRegionData.Find(data => data.CountryName == countryName);
            return countryRegionData?.RegionPointsList;
        }

        public int GetRegionCount(string countryName)
        {
            CountryRegionData countryRegionData = AllCountryRegionData.Find(data => data.CountryName == countryName);
            return countryRegionData?.RegionCount ?? 0;
        }
    }

    [System.Serializable]
    public class CountryRegionData
    {
        public string CountryName;
        public int RegionCount;
        [ShowInInspector] public List<List<Vector3>> RegionPointsList = new List<List<Vector3>>();
    }
}