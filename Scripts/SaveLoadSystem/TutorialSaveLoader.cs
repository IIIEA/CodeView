using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Tutorial;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class TutorialSaveLoader : SaveLoader<TutorialData, MoneyStorage>
    {
        protected override void SetupData(MoneyStorage starterPackManager, TutorialData startPackData)
        {
            TutorialManager.Instance.Initialize(startPackData.IsCompleted, 9);
        }

        protected override TutorialData ConvertToData(MoneyStorage service)
        {
            return new TutorialData
            {
                IsCompleted = TutorialManager.Instance.IsCompleted
            };
        }
    }
}