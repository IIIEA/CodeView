using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.CamerasScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using Lean.Touch;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public class CityPopupFactory
    {
        private MoneyStorage _moneyStorage;
        private AirplaneLaunchManager _airplaneLaunchManager;

        private ICityPresenter _presenter;
        private AirportSeller _airportSeller;
        private AirplaneSeller _airplaneSeller;
        private AirportUpgradeSeller _airportUpgradeSeller;
        private LeanPinchCamera _leanPinchCamera;
        private BuildingCameraService _buildingCameraService;
        private CameraController _cameraController;
        private float _previousZoom;
        private IncomeCounter _incomeCounter;

        [Inject]
        public void Construct(CityPopup cityPopup, MoneyStorage moneyStorage, AirplaneRouteFactory airplaneRouteFactory,
            AirplaneLaunchManager airplaneLaunchManager,
            AirportSeller airportSeller, AirplaneSeller airplaneSeller, AirportUpgradeSeller airportUpgradeSeller,
            AirplaneUpgradeSeller airplaneUpgradeSeller, LeanPinchCamera leanPinchCamera, BuildingCameraService buildingCameraService,
            CameraController cameraController, IncomeCounter incomeCounter)
        {
            _incomeCounter = incomeCounter;
            _cameraController = cameraController;
            _buildingCameraService = buildingCameraService;
            _leanPinchCamera = leanPinchCamera;
            _airportUpgradeSeller = airportUpgradeSeller;
            _airplaneSeller = airplaneSeller;
            _airportSeller = airportSeller;
            _moneyStorage = moneyStorage;
            _airplaneLaunchManager = airplaneLaunchManager;
        }

        public ICityPresenter Create(CityPoint city)
        {
            return new CityPresenter(city, _moneyStorage, _airplaneLaunchManager, _airportSeller,
                _airportUpgradeSeller, _airplaneSeller, _leanPinchCamera, _buildingCameraService, _cameraController, _incomeCounter);
        }
    }
}