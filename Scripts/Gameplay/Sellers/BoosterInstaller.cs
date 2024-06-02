using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    public class BoosterInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(MoneyBoosterView))]
        private MoneyBoosterView _moneyBoosterView;

        [SerializeField, Service(typeof(MoneyBoosterModel))]
        private MoneyBoosterModel _moneyBoosterModel;

        [SerializeField, Service(typeof(MoneyBoosterPresenter)), Listener]
        private MoneyBoosterPresenter moneyBoosterPresenter;

        [SerializeField, Service(typeof(SpeedBoosterView))]
        private SpeedBoosterView _speedBoosterView;

        [SerializeField, Service(typeof(SpeedBoosterModel))]
        private SpeedBoosterModel _speedBoosterModel;

        [SerializeField, Service(typeof(SpeedBoosterPresenter)), Listener]
        private SpeedBoosterPresenter _speedBoosterPresenter;
        
        [SerializeField, Service(typeof(InterstitialManager)), Listener]
        private InterstitialManager _interstitialManager;

    }
}