using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.VibroSlider;
using UnityEngine;

namespace _Fly_Connect.Scripts.ActionPopupSystem
{
    public sealed class ActionPopupInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(ActionPopupFactory))]
        private ActionPopupFactory _actionPopupFactory = new();

        [SerializeField, Service(typeof(ActionPopupPool))]
        private ActionPopupPool _actionPopupPool = new();

        [SerializeField, Service(typeof(ActionPopupManager)), Listener]
        private ActionPopupManager _actionPopupManager = new();
    }
}