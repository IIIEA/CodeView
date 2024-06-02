using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay
{
    public class Route : MonoBehaviour
    {
        private static int _id = 1;

        [SerializeReference] [ReadOnly] private CityPoint _city1;
        [SerializeReference] [ReadOnly] private CityPoint _city2;

        [field: SerializeField] public int Id { get; private set; } = _id;
        private float _distance;
        public MeshRenderer MeshRenderer { get; private set; }
        private MeshCollider _meshCollider;
        public SplineRenderer SplineRenderer { get; private set; }

        public Vector3 P0 { get; private set; }
        public Vector3 P1 { get; private set; }
        public Vector3 P2 { get; private set; }
        public int Resolution { get; private set; }
        public int StartCityIndex { get; set; }
        public int EndCityIndex { get; set; }
        public bool IsTapBoosted { get; private set; } 

        public Airplane Airplane { get; private set; }
        public float Distance => _distance;

        public void Construct(CityPoint city1, CityPoint city2, Vector3 p0, Vector3 p1, Vector3 p2, int resolution)
        {
            ++_id;
            _city1 = city1;
            _city2 = city2;
            StartCityIndex = _city1.Id;
            EndCityIndex = _city2.Id;

            _city1.AddRoute(this);

            P0 = p0;
            P1 = p1;
            P2 = p2;
            Resolution = resolution;
        }

        public void SetMeshRenderer(MeshRenderer meshRenderer, SplineRenderer splineRenderer)
        {
            SplineRenderer = splineRenderer;
            MeshRenderer = meshRenderer;
        }

        public (int, int) GetCitiesLevel()
        {
            return (_city1.CurrentAirportLevel, _city2.CurrentAirportLevel);
        }

        public void SetAirplane(Airplane airplane)
        {
            Airplane = airplane;
        }

        public void SetGreenColorLine()
        {
            MeshRenderer.sharedMaterial.color = new Color32(133, 255, 59, 255);
            IsTapBoosted = true;
        }

        public void SetWhiteColorLine()
        {
            MeshRenderer.sharedMaterial.color = new Color32(255, 255, 255, 100);
            IsTapBoosted = false;
        }

        public void SetDistance(float distance)
        {
            _distance = distance;
        }
    }
}