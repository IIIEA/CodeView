using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Popup;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.Tutorial.Steps.Input;
using UnityEngine;

namespace _Fly_Connect.Scripts
{
    public sealed class PopupInstaller : GameInstaller
    {
        [Service(typeof(PopupManager))] 
        PopupManager _popupManager = new();

        [SerializeField]
        PopupSupplier _popupSupplier;

        [Service(typeof(CityPopupFactory))]
        private CityPopupFactory _cityPopupFactory = new();

        [SerializeField, Service(typeof(CityPopup)), Listener]
        private CityPopup _cityPopup;

        [SerializeField, Listener, Service(typeof(PopupInputController))]
        private PopupInputController _popupInputController = new();

        [Service(typeof(CountryPopupFactory))] private CountryPopupFactory _countryPopupFactory = new();

        [SerializeField, Service(typeof(CountryPopup))]
        private CountryPopup _countryPopup;

        [SerializeField, Service(typeof(ShopPopup))]
        private ShopPopup _shopPopup;

        [SerializeField, Service(typeof(ICoroutineRunner))]
        private CoroutineRunner _coroutineRunner;

        [SerializeField, Listener] 
        private SettingButtonAdapter _settingButtonAdapter;

        [SerializeField, Listener]
        private ShopButtonAdapter _shopButtonAdapter;

        [SerializeField, Listener, Service(typeof(WarningPopupManager))]
        private WarningPopupManager _warningPopupManager;

        [SerializeField, Listener, Service(typeof(StarterPackPopup))]
        StarterPackPopup _starterPackPopup;

        [SerializeField, Listener, Service(typeof(StarterPackView))]
        StarterPackView _starterPackView;

        [SerializeField, Listener, Service(typeof(BuyTicketView))]
        BuyTicketView _buyTicketView;

        [SerializeField, Listener, Service(typeof(RemoveAllAdsView))]
        RemoveAllAdsView _removeAllAdsView;

        [SerializeField, Listener, Service(typeof(RemoveAdManager))]
        RemoveAdManager _removeAdManager = new();

        [SerializeField, Listener, Service(typeof(NoAdsButtonAdapter))]
        NoAdsButtonAdapter _noAdsButtonAdapter = new();

        [SerializeField, Listener, Service(typeof(FreeTimeView))]
        FreeTimeView _freeTimeView = new();

        private void Awake()
        {
            _popupManager.SetSupplier(_popupSupplier);
        }
    }
}