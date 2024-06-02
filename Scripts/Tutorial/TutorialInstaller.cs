using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.Tutorial.Steps.Input;
using _Fly_Connect.Scripts.Tutorial.Steps.Welcome;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial
{
    public class TutorialInstaller : GameInstaller
    {
        [SerializeField, Listener, Service(typeof(TutorialPopup))]
        private TutorialPopup _tutorialPopup;  
        
        [SerializeField, Service(typeof(Finger))]
        private Finger _finger;

        [SerializeField, Service(typeof(WorldFinger))]
        private WorldFinger _worldFinger;

        [SerializeField, Listener]
        private WelcomeStepController _welcomeStepController;

        [SerializeField, Listener]
        private InputStepController _inputStepController;

        [SerializeField, Listener]
        private BuyCountryStepController _buyCountryStepController;

        [SerializeField, Listener]
        private BuyAirportStepController _buyAirportStepController;

        [SerializeField, Listener]
        private DrawLineStepController _drawLineStepController; 

        [SerializeField, Listener]
        private UpgradeAirportStepController _upgradeAirportStepController;

        [SerializeField, Listener]
        private DrawMoreLineStepController _drawMoreLineStepController ;

        [SerializeField, Listener] 
        private BuyMoreCountryStepController _buyMoreCountryStepController;

        [SerializeField, Listener]
        private CompletedStepController _completedStepController;

    }
}
