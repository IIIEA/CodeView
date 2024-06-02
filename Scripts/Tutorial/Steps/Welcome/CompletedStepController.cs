using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.SaveLoadSystem;
using _Fly_Connect.Scripts.TicketManager;
using MadPixel.InApps;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    [AddComponentMenu("Tutorial/Step «Completed Step»")]
    public sealed class CompletedStepController : TutorialStepController
    {
        private GameContext _context;

        [Inject]
        public override void Construct(GameContext context)
        {
            _context = context;
            base.Construct(context);
        }

        protected override void OnStart()
        {
            var rateUsManager = _context.TryGetService<RateUsManager>();
            var popupManager = _context.TryGetService<PopupManager>();
            IRateUsPresenter presenter = new RatePresenterPresenter(rateUsManager, popupManager);

            MobileInAppPurchaser.Instance.RestorePurchases();

            popupManager.ShowPopup(PopupName.RATE_US_POPUP, presenter);
            NotifyAboutMoveNext();
        }

        protected override void OnStop()
        {
        }
    }
}