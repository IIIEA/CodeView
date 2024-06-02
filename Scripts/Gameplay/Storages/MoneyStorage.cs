using System;
using Sirenix.OdinInspector;

namespace _Fly_Connect.Scripts.Gameplay.Storages
{
    [Serializable]
    public class MoneyStorage
    {
        public BigNumber Money { get; private set; }
        public int Ticket { get; private set; }

        public event Action<BigNumber> OnMoneyChanged;
        public event Action<int> OnTicketChanged;

        public void SetupMoney(BigNumber bigNumber, int ticket)
        {
            Ticket = ticket;
            Money = bigNumber;
            OnMoneyChanged?.Invoke(Money);
            OnTicketChanged?.Invoke(Ticket);
        }

        public BigNumber GetMoney()
        {
            return Money;
        }
        
        [Button]
        public void EarnTicket(int range)
        {
            Ticket += range;
            OnTicketChanged?.Invoke(Ticket);
        }

        [Button]
        public void SpendTicket(int range)
        {
            Ticket -= range;
            OnTicketChanged?.Invoke(Ticket);
        }

        [Button]
        public void EarnMoney(int range)
        {
            Money += range;
            OnMoneyChanged?.Invoke(Money);
        }

        [Button]
        public void SpendMoney(int range)
        {
            Money -= range;
            OnMoneyChanged?.Invoke(Money);
        }

        [Button]
        public void SpendMoney(long range)
        {
            BigNumber money = new BigNumber(range);
            Money -= money;
            OnMoneyChanged?.Invoke(Money);
        }

        public void EarnMoney(BigNumber money)
        {
            Money += money;
        }

        public void SetupTicket(int ticket)
        {
            Ticket = ticket;
        }
    }
}