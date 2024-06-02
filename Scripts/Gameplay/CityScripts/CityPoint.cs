using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    [ExecuteInEditMode]
    public class CityPoint : SerializedMonoBehaviour
    {
        [SerializeField] private CityPointView _cityPointView;
        [SerializeField] public CityPointData _data;
        [field: SerializeField] public Country Country { get; private set; }
        [field: SerializeField] public CityLevelUpController CityUpLevelController { get; private set; }
        [field: SerializeField] public Transform CityPosition { get; private set; }
        [field: SerializeField] public Transform ShowplaceTransform { get; private set; }
        [field: SerializeField] public bool IsNotHide { get; private set; }

        public Vector3 ShowPlaceMaxSize;
        [FormerlySerializedAs("StartSize")] public Vector3 StartShowplaceSize;
        public bool IsGameCity { get; private set; }
        public ObservableList<Route> Routes { get; private set; } = new();
        public int CurrentAirportLevel { get; private set; }
        public int SavedAirportLevel { get; private set; }
        public int MaxAirportLevel { get; private set; } = 4;
        public bool IsBuy { get; private set; }
        public bool ShowCitiesGizmo { get; set; } = true;
        public bool ShowActionPopup { get; set; }
        public int Id { get; set; }
        public CityPointView CityPointView => _cityPointView;
        public List<int> RouteIndexes { get; private set; } = new();

        public event Action<CityPoint> OnCityUpgrade;
        //
        // private void OnValidate()
        // {
        //     GameObject parent;
        //
        //     if (CityUpLevelController != null)
        //     {
        //         if (CityUpLevelController != null)
        //         {
        //             parent = CityUpLevelController.EnabledObjects[3];
        //
        //             SpriteRenderer[] renderers = parent.GetComponentsInChildren<SpriteRenderer>(true);
        //
        //             List<SpriteRenderer> sortRenderers = renderers.OrderByDescending(r => r.transform.localScale.x).ToList();
        //
        //             ShowplaceTransform = sortRenderers[0].transform;
        //         }
        //     }
        // }

        // private void OnValidate()
        //
        // {
        //
        //     foreach (Transform child in transform)
        //
        //     {
        //
        //         RecursiveChildSearch(child);
        //
        //     }
        //
        //
        //
        //     CityUpLevelController = GetComponent<CityLevelUpController>();
        //
        // }
        //
        //
        //
        // void RecursiveChildSearch(Transform parent)
        //
        // {
        //
        //     foreach (Transform child in parent)
        //
        //     {
        //
        //         if (child.name == "City_roads_1" || child.name == "City_roads_2" || child.name == "City_roads_3")
        //
        //         {
        //
        //             CityPosition = child;
        //
        //         }
        //
        //
        //
        //         RecursiveChildSearch(child);
        //
        //     }
        //
        // }


        private void Awake()
        {
            _cityPointView.SetTitle(gameObject.name);


            if (ShowplaceTransform != null)
            {
                StartShowplaceSize = ShowplaceTransform.localScale;

                var x = (StartShowplaceSize.x * 50) / 100;
                var y = (StartShowplaceSize.y * 50) / 100;
                var z = (StartShowplaceSize.z * 50) / 100;
                ShowPlaceMaxSize = StartShowplaceSize + new Vector3(x, y, z);
            }

            if (CityPosition == null)
                CityPosition = transform;
        }

        [Button("Sort")]
        public void Sort()
        {
            SpriteRenderer[] spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
            Array.Sort(spriteRenderers,
                (spriteRenderers1, spriteRenderers2) =>
                {
                    return spriteRenderers1.transform.position.y.CompareTo(spriteRenderers2.transform.position.y);
                });

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i].name == "City_roads_1" || spriteRenderers[i].name == "City_roads_2" ||
                    spriteRenderers[i].name == "City_roads_3")
                {
                    spriteRenderers[i].sortingLayerName = "Road";
                    continue;
                }

#if UNITY_EDITOR
                Undo.RecordObject(spriteRenderers[i], "Change Sorting Order");
#endif

                spriteRenderers[i].sortingOrder = spriteRenderers.Length - i;
                spriteRenderers[i].sortingLayerName = "City";
            }
        }

        public List<int> GetRouteIndexes()
        {
            var indexes = new List<int>();

            for (int i = 0; i < Routes.Count; i++)
            {
                int endIndex = Routes[i].EndCityIndex;

                indexes.Add(endIndex);
            }

            return indexes;
        }

        public void Setup(bool isBuy, int currentAirportLevel, List<int> dataRoutes)
        {
            IsBuy = isBuy;
            SavedAirportLevel = currentAirportLevel;
            RouteIndexes = dataRoutes;
        }

        public void SetIsBuy(bool isBusy)
        {
            IsBuy = isBusy;
        }

        public void AddRoute(Route route)
        {
            Routes.Add(route);
        }

        public void LevelUp()
        {
            if (CurrentAirportLevel != MaxAirportLevel)
            {
                CurrentAirportLevel++;
                IsBuy = true;

                if (Routes != null)
                {
                    Routes.ForEach(route =>
                    {
                        if (route.Airplane != null)
                            route.Airplane.SetLevel(CurrentAirportLevel);
                    });
                }

                OnCityUpgrade?.Invoke(this);
            }
        }

        public void CreateRoute(AirplaneRouteFactory airplaneRouteFactory, CityPoint city2)
        {
            city2.SetIsBuy(true);
            IsBuy = true;

            airplaneRouteFactory.GenerateRoute(this, city2);
        }

        public void SwitchIsGameCity(bool isGameCity)
        {
            IsGameCity = isGameCity;
        }

        public void SetCurrentLevel(int level)
        {
            CurrentAirportLevel = level;

            if (Routes != null)
            {
                Routes.ForEach(route =>
                {
                    if (route.Airplane != null)
                        route.Airplane.SetLevel(CurrentAirportLevel);
                });
            }

            OnCityUpgrade?.Invoke(this);
        }

        public void SetIcon()
        {
            OnCityUpgrade?.Invoke(this);
        }
    }
}