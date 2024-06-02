using _Fly_Connect.Scripts.Gameplay.Storages;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Infrastructure.Locator;
using _Fly_Connect.Scripts.VibroSlider;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class SaveLoadModuleInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(SaveLoadManager))]
        private SaveLoadManager _saveLoadManager;

        [ShowInInspector, Service(typeof(GameRepository))]
        private GameRepository _gameRepository = new();

        public override void Inject(ServiceLocator serviceLocator)
        {
            var saveLoaders = GetSaveLoaderList();

            _saveLoadManager.Construct(saveLoaders, _gameRepository);
        }

        private ISaveLoader[] GetSaveLoaderList()
        {
            ISaveLoader[] saveLoaders =
            {
                new UpgradeSaveLoader(),
                new MoneySaveLoader(),
                new CitySaveLoader(),
                new MapSaveLoader(),
                new AirportSellerSaveLoader(),
                new CountrySellerSaveLoader(),
                new UpgradeSellerSaveLoader(),
                new AirplaneUpgradeSaveLoader(),
                new AirplaneSellerSaveLoader(),
                new SettingsSaveLoader(),
                new TutorialSaveLoader(),
                new IncomePerMinuteSaveLoader(),
                new RateSaveLoader(),
                new TicketSaveLoader(),
                new StarterPackSaveLoader(),
                new RemoveAdManagerSaveLoader(),
            };

            return saveLoaders;
        }
    }
}