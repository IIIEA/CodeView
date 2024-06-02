using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [Serializable]
    public class CountryService
    {
        [field: SerializeField] public Country[] Country { get; private set; }

        [SerializeField] public List<string> _names;

        [Button]
        public void Show()
        {
            _names = new List<string>();

            foreach (var country in Country)
            {
                foreach (var city in country.GameCities)
                {
                    if (city.CityUpLevelController != null)
                    {
                        var gameObject = city.CityUpLevelController.EnabledObjects[^1];
                        SpriteRenderer[] gameObjects = gameObject.GetComponentsInChildren<SpriteRenderer>(true);

                        if (gameObjects.Length == 0)
                            _names.Add(city.name);
                    }
                }
            }
        }
    }
}