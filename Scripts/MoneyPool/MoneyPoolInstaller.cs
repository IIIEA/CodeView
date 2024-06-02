using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Sound.MusicPlayer;
using UnityEngine;

namespace _Fly_Connect.Scripts.MoneyPool
{
    public sealed class MoneyPoolInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(AddMoneyEffectFactory))]
        private AddMoneyEffectFactory _addMoneyEffectFactory = new();

        [SerializeField, Service(typeof(MoneyPool))]
        private MoneyPool _moneyPool;

        [SerializeField, Listener] 
        private MusicPlaylistController _usicPlaylistController;

        [SerializeField, Listener]
        private MusicPauseController _musicPauseController;
    }
}