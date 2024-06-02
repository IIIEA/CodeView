using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;

namespace _Fly_Connect.Scripts.Gameplay.UI
{
    public class MoneyPanelPresenter : IGameStartListener, IGameFinishListener
    {
        private MoneyWidget _moneyWidget;
        private TicketWidget _ticketWidget;
        private MoneyStorage _moneyStorage;

        [Inject]
        private void Construct(MoneyWidget moneyWidget, MoneyStorage moneyStorage, TicketWidget ticketWidget)
        {
            _ticketWidget = ticketWidget;
            _moneyWidget = moneyWidget;
            _moneyStorage = moneyStorage;
        }
        
        public void OnStartGame()
        {
            _moneyWidget.SetupMoney(_moneyStorage.GetMoney().ToString());
            _ticketWidget.SetupTicket(_moneyStorage.Ticket.ToString());
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
            _moneyStorage.OnTicketChanged += OnTicketChanged;
        }

        public void OnFinishGame()
        {
            _moneyStorage.OnMoneyChanged -= OnMoneyChanged;
            _moneyStorage.OnTicketChanged -= OnTicketChanged;
        }

        private void OnTicketChanged(int ticket)
        {
            _ticketWidget.UpdateTicket(ticket.ToString());
        }

        private void OnMoneyChanged(BigNumber money)
        {
            _moneyWidget.UpdateMoney(money.ToString());
        }
    }
}