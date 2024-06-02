using _Fly_Connect.Scripts.Gameplay.Settings_Popup;
using _Fly_Connect.Scripts.Gameplay.Storages;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class RemoveAdManagerSaveLoader : SaveLoader<RemoveAdPackData, RemoveAdManager>
    {
        protected override void SetupData(RemoveAdManager starterPackManager, RemoveAdPackData startPackData)
        {
            starterPackManager.Setup(startPackData.IsBuy);
        }

        protected override RemoveAdPackData ConvertToData(RemoveAdManager service)
        {
            return new RemoveAdPackData
            {
                IsBuy = service.IsBuy
            };
        }
    }
}