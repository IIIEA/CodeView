using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class CountryPopupFactory
    {
        private MoneyStorage _moneyStorage;
        private CountrySeller _countrySeller;

        private ICountryPresenter _presenter;
        private CameraController _cameraController;

        [Inject]
        public void Construct(MoneyStorage moneyStorage, CountrySeller countrySeller,
            CameraController cameraController)
        {
            _cameraController = cameraController;
            _moneyStorage = moneyStorage;
            _countrySeller = countrySeller;
        }

        public CountryPresenter Create(Country country)
        {
            return new CountryPresenter(_moneyStorage, _countrySeller, country, _cameraController);
        }
    }
}