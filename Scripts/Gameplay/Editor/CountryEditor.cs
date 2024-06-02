using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    [CustomEditor(typeof(Country))]
    [CanEditMultipleObjects]
    public class CountryEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Country country = (Country) target;

            if (GUILayout.Button("Edit Cities"))
            {
                country.ShowCities = !country.ShowCities;

                if (country.ShowCities)
                {
                    country.EnableCities();
                }
                else
                {
                    country.DisableCities();
                }
            }

            if (GUILayout.Button("Edit Game Cities"))
            {
                country.ShowGameCities = !country.ShowGameCities;

                if (country.ShowGameCities)
                {
                    country.EnableGameCities();
                }
                else
                {
                    country.DisableGameCities();
                }
            }

            if (GUILayout.Button("Select All Game Cities"))
            {
                List<Object> gameCityObjects = new List<Object>();
    
                foreach (var gameCity in country.GameCities)
                {
                    gameCityObjects.Add(gameCity.gameObject);
                }

                Selection.objects = gameCityObjects.ToArray();
            }

            if (GUILayout.Button("Copy Cities"))
            {
                var text = country.ArrayToString();
                country.CopyToClipboard(text);
            }

            if (EditorApplication.isPlaying)
            {
                country.ShowCities = false;
            }

            EditorUtility.SetDirty(target);
            SceneView.RepaintAll();
        }
    }
}