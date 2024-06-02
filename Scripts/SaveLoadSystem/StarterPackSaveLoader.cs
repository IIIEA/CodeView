using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Gameplay.Storages;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class StarterPackSaveLoader : SaveLoader<StartPackData, StarterPackManager>
    {
        protected override void SetupData(StarterPackManager starterPackManager, StartPackData startPackData)
        {
            starterPackManager.Setup(startPackData.IsBuy);
        }

        protected override StartPackData ConvertToData(StarterPackManager service)
        {
            return new StartPackData
            {
                IsBuy = service.IsBuy
            };
        }
    }
}