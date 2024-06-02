using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.UI
{
    public class MoneyStorageInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(MoneyWidget))]
        private MoneyWidget moneyWidget;

        [SerializeField, Service(typeof(TicketWidget))]
        private TicketWidget _ticketWidget;

        [SerializeField, Service(typeof(MoneyStorage))] 
        private MoneyStorage _moneyStorage = new();
        
        [Listener]
        private MoneyPanelPresenter moneyPanelPresenter = new();
    }
}