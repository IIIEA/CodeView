using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class CitySaveLoader : SaveLoader<CityData[], CityPointService>
    {
        protected override void SetupData(CityPointService starterPackManager, CityData[] startPackData)
        {
            for (int i = 0; i < starterPackManager.GameCityPoints.Length; i++)
            {
                CityData data = startPackData[i];

                starterPackManager.GameCityPoints[i].Setup(data.IsBuy, data.CurrentAirportLevel, data.Routes);
            }
        }

        protected override CityData[] ConvertToData(CityPointService service)
        {
            CityData[] dates = new CityData[service.GameCityPoints.Length];

            var gameCities = service.GameCityPoints;

            for (var i = 0; i < gameCities.Length; i++)
            {
                CityData data = new()
                {
                    Name = gameCities[i].gameObject.name,
                    CurrentAirportLevel = gameCities[i].CurrentAirportLevel,
                    IsBuy = gameCities[i].IsBuy,
                    Routes = gameCities[i].GetRouteIndexes()
                };

                dates[i] = data;
            }

            return dates;
        }
    }
}