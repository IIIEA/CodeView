using _Fly_Connect.Scripts.Gameplay.Storages;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class RateSaveLoader : SaveLoader<RateData, RateUsManager>
    {
        protected override void SetupData(RateUsManager starterPackManager, RateData startPackData)
        {
               starterPackManager.Setup(startPackData.IsRated);
        }

        protected override RateData ConvertToData(RateUsManager service)
        {
            return new RateData
            {
                IsRated = service.IsRated
            };
        }
    }
}