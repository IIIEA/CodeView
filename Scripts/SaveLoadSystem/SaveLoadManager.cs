using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class SaveLoadManager : MonoBehaviour
    {
        private ISaveLoader[] _saveLoaders;
        private GameRepository _repository;

        [Inject]
        public void Construct(ISaveLoader[] saveLoaders, GameRepository repository)
        {
            _saveLoaders = saveLoaders;
            _repository = repository;
        }

        public void Clear()
        {
            PlayerPrefs.DeleteAll();
        }

        [Button]
        public void Save()
        {
            GameContext context = FindObjectOfType<GameContext>();

            if (_saveLoaders == null)
                return;

            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.SaveGame(_repository, context);
            }

            _repository.SaveState();
        }

        [Button]
        public void Load()
        {
            _repository.LoadState();

            GameContext context = FindObjectOfType<GameContext>();

            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.LoadGame(_repository, context);
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                // if (TutorialManager.Instance.IsCompleted)
                    Save();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // if (pauseStatus)
            // {
            //     Save();
            // }
        }

        private void OnApplicationQuit()
        {
            Save();
        }
    }
}