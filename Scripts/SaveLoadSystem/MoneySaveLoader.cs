using _Fly_Connect.Scripts.Gameplay.Storages;
//using fbg;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class MoneySaveLoader : SaveLoader<MoneyData, MoneyStorage>
    {
        private int _startMoney = 1500;

        protected override void SetupData(MoneyStorage starterPackManager, MoneyData startPackData)
        {
            starterPackManager.SetupMoney(new BigNumber(startPackData.Money), startPackData.Ticket);

            if (starterPackManager.Money == 0)
            {
                starterPackManager.SetupMoney(new BigNumber(_startMoney), startPackData.Ticket);
            }

            // Debug.Log($"<color=green>Money loaded: {data.Money}!</color>");
        }

        protected override MoneyData ConvertToData(MoneyStorage service)
        {
            // Debug.Log($"<color=green>Money saved: {moneyStorage.Money}!</color>");

            var longMoney = service.Money.ToLong();

            return new MoneyData
            {
                Money = longMoney,
                Ticket = service.Ticket
            };
        }

        protected override void SetupByDefault(MoneyStorage moneyStorage)
        {
            if (moneyStorage.Money == 0)
            {
                moneyStorage.SetupMoney(new BigNumber(_startMoney), 1);
            }
        }
    }
}