using _Fly_Connect.Scripts.Infrastructure.GameManager;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public interface ISaveLoader
    {
        void LoadGame(IGameRepository repository, GameContext context);

        void SaveGame(IGameRepository repository, GameContext context);
    }
}