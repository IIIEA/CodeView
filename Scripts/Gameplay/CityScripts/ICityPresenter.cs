using System;
using _Fly_Connect.Scripts.Gameplay.CamerasScripts;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    public interface ICityPresenter
    {
        public string Title { get; }
        IAirportPresenter AirportPresenter { get; }
        void Enable();
        void Disable();
    }

    public interface IAirportPresenter
    {
        bool IsAirportButtonInteractable { get; }
        string AirportLevelText { get; }
        string AirportButtonText { get; }
        string AirportPrice { get; }
        Transform PopupTransform { get; }
        bool IsRouteBuild { get;  }
        Vector3 PopupScale { get; }
        string AirplaneCount { get; }
        BuildingCameraService BuildingCameraService { get; }
        CameraController CameraController { get; }
        Vector3 CityPosition { get; }
        string RouteText { get; }
        bool IsRouteRedTextRed { get; }
        string AirportLevel { get; }
        bool IsShowTicketIcon { get; }
        void Enable();
        void Disable();

        event Action OnBuyButtonStateChanged;
        void OnBuyAirportButtonClicked();
        event Action<bool> HasMoney;
        void OnRewardButtonClicked();
    }
}