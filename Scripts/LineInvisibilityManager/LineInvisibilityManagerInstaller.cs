using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.SaveLoadSystem;
using _Fly_Connect.Scripts.VibroSlider;
using UnityEngine;

namespace _Fly_Connect.Scripts.LineInvisibilityManager
{
    public class LineInvisibilityManagerInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(VibrationSlider))]
        private VibrationSlider _vibrationSlider;

        [SerializeField, Listener] 
        private LineInvisibilityManager _lineInvisibilityManager = new();

        [SerializeField, Listener] 
        private CityViewScaler _cityViewScaler = new();

        [SerializeField, Listener] 
        private CityScaler _cityScaler = new();

        [SerializeField, Listener, Service(typeof(CityShower))] 
        private CityShower _cityShower = new();

        [SerializeField, Listener, Service(typeof(RateUsManager))] 
        private RateUsManager _rateUsManager = new();

        // [SerializeField, Listener] 
        // private FingerTapManager _fingerTapManager = new();
    }                                                                                    
}