using _Fly_Connect.Scripts.Infrastructure.GameManager;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public abstract class SaveLoader<TData, TService> : ISaveLoader
    {
        void ISaveLoader.LoadGame(IGameRepository repository, GameContext context)
        {
            var service = context.TryGetService<TService>();

            if (repository.TryGetData(out TData data))
            {
                SetupData(service, data);
            }
            else
            {
                SetupByDefault(service);      
            }
        }
    
        void ISaveLoader.SaveGame(IGameRepository repository, GameContext context)
        {
            var service = context.TryGetService<TService>();
            var data = ConvertToData(service);
            repository.SetData(data);
        }
    
        protected abstract void SetupData(TService starterPackManager, TData startPackData);
    
        protected abstract TData ConvertToData(TService service);
    
        protected virtual void SetupByDefault(TService moneyStorage)
        {
        }
    }
}