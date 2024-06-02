using _Fly_Connect.Scripts.Gameplay.GoldenAirplane;
using _Fly_Connect.Scripts.Gameplay.UfoScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public class AirplaneInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(AirplaneLaunchManager))]
        private AirplaneLaunchManager _airplaneLauncherManager = new();

        [FormerlySerializedAs("_airplaneTapObserver")] [SerializeField, Listener, Service(typeof(PathTapObserver))]
        private PathTapObserver pathTapObserver = new();

        [FormerlySerializedAs("goldenAirplaneManager")] [FormerlySerializedAs("_golderAirplaneManager")] [SerializeField, Listener, Service(typeof(UFOAirplaneManager))] 
        private UFOAirplaneManager ufoAirplaneManager = new();
    }
}