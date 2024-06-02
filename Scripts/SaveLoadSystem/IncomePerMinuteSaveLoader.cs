using _Fly_Connect.Scripts.Cheat_Console;
using _Fly_Connect.Scripts.Gameplay.Sellers;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class IncomePerMinuteSaveLoader : SaveLoader<IncomeCountSellerData, IncomeCounter>
    {
        protected override void SetupData(IncomeCounter starterPackManager, IncomeCountSellerData startPackData)
        {
            starterPackManager.Setup(startPackData.incomePerMinute);
        }

        protected override IncomeCountSellerData ConvertToData(IncomeCounter service)
        {
            return new()
            {
                incomePerMinute = service.IncomePerMinute
            };
        }
    }
}