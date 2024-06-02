using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    [CreateAssetMenu(fileName = "CityData", menuName = "CityData", order = 1)]
    public class CityData : ScriptableObject
    {
        [field: SerializeField] public string CityName { get; private set; }
        [field: SerializeField] public string Province { get; private set; }
        [field: SerializeField] public int CountryIndex { get; private set; }
        [field: SerializeField] public int Population { get; private set; }
        [field: SerializeField] public Vector2 Unity2DLocation { get; private set; }
        [field: SerializeField] public CITY_CLASS CityClass { get; private set; }
    }
}