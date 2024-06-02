using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts
{
    public class GameManagerInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(GameManager))]
        private GameManager _gameManage;

        [SerializeField, Service(typeof(GameContext))]
        private GameContext _gameContext;
    }
}