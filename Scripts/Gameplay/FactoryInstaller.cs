using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.GoldenAirplane;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay
{
    public class FactoryInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(AirplaneRouteFactory))]
        private AirplaneRouteFactory _airplaneRouteFactory = new();
        
        [SerializeField, Service(typeof(GoldenAirplanePopupFactory))]
        private GoldenAirplanePopupFactory _goldenAirplanePopupFactory = new();

        [SerializeField, Service(typeof(Transform))]
        private Transform _mapTransform;
    }
}