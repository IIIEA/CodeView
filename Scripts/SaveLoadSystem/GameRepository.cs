using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using _Fly_Connect.Scripts.Tutorial;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    [Serializable]
    public sealed class GameRepository : IGameRepository
    {
        private string FILE_PATH = "GAME_STATE";

        [OdinSerialize] private Dictionary<string, string> _gameState = new();

        public void LoadState()
        {
            if (PlayerPrefs.HasKey(FILE_PATH))
            {
                string serializedState = PlayerPrefs.GetString(FILE_PATH);
                _gameState = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedState);
            }
            else
            {
                _gameState = new Dictionary<string, string>();
            }
        }

        public void SaveState()
        {
            if(TutorialManager.Instance.IsCompleted == false)
                return;

            string serializedState = JsonConvert.SerializeObject(_gameState);
            PlayerPrefs.SetString(FILE_PATH, serializedState);
            PlayerPrefs.Save();
        }

        public T GetData<T>()
        {
            var serializedData = _gameState[typeof(T).Name];
            return JsonConvert.DeserializeObject<T>(serializedData);
        }

        public bool TryGetData<T>(out T value)
        {
            if (_gameState.TryGetValue(typeof(T).Name, out var serializedData))
            {
                value = JsonConvert.DeserializeObject<T>(serializedData);
                return true;
            }

            value = default;
            return false;
        }

        public void SetData<T>(T value)
        {
            var serializedData = JsonConvert.SerializeObject(value);
            _gameState[typeof(T).Name] = serializedData;
        }
    }
}