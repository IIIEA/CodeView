using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Gameplay.UfoScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;

namespace _Fly_Connect.Scripts.Gameplay.GoldenAirplane
{
    public class GoldenAirplanePopupFactory
    {
        private MoneyStorage _moneyStorage;
        private AirplaneSeller _airplaneSeller;
        private UFOSeller _ufoAirplaneSeller;
        private UFOAirplaneManager _ufoAirplaneManager;

        [Inject]
        private void Construct(MoneyStorage moneyStorage, AirplaneSeller airplaneSeller, UFOSeller ufoAirplaneSeller, UFOAirplaneManager ufoAirplaneManager)
        {
            _ufoAirplaneManager = ufoAirplaneManager;
            _ufoAirplaneSeller = ufoAirplaneSeller;
            _airplaneSeller = airplaneSeller;
            _moneyStorage = moneyStorage;
        }

        public IGoldenAirplanePresenter Create()
        {
            return new UFOPresenter(_moneyStorage, _airplaneSeller, _ufoAirplaneSeller, _ufoAirplaneManager);
        }
    }
}