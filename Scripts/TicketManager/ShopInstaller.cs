using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.TicketManager
{
    public sealed class ShopInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(StarterPackManager)), Listener]
        private StarterPackManager _starterPackManager = new();

        [SerializeField, Listener, Service(typeof(TicketManager))]
        private TicketManager _ticketManager;

        [SerializeField, Listener, Service(typeof(PurchasesManager))]
        private PurchasesManager _purchasesManager;
    }
}