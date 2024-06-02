using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CamerasScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using Lean.Touch;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    public class CityPresenter : ICityPresenter
    {
        public IAirportPresenter AirportPresenter { get; private set; }

        private readonly CityPoint _city;

        public string Title => _city.gameObject.name;

        public CityPresenter(CityPoint city, MoneyStorage moneyStorage, AirplaneLaunchManager airplaneLaunchManager,
            AirportSeller airportSeller, AirportUpgradeSeller airportUpgradeSeller, AirplaneSeller airplaneSeller,
            LeanPinchCamera leanPinchCamera, BuildingCameraService buildingCameraService, CameraController cameraController, IncomeCounter incomeCounter)
        {
            _city = city;

            AirportPresenter = new AirportPresenter(_city, airportSeller, airportUpgradeSeller, moneyStorage,
                airplaneSeller, airplaneLaunchManager, leanPinchCamera, buildingCameraService,cameraController,incomeCounter);
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }
    }
}