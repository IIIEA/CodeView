using _Fly_Connect.Scripts.Gameplay.Storages;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class TicketSaveLoader : SaveLoader<TicketData, TicketManager.TicketManager>
    {
        protected override void SetupData(TicketManager.TicketManager starterPackManager, TicketData startPackData)
        {
            starterPackManager.Setup(startPackData.CollectedRewards, startPackData.CanGetReward);
        }

        protected override TicketData ConvertToData(TicketManager.TicketManager service)
        {
            return new TicketData
            {
                CollectedRewards = service.CurrentCollectedReward,
                CanGetReward = service.CanGetReward
            };
        }
    }
}