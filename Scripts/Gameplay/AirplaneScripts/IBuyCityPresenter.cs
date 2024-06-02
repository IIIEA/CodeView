using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public interface IBuyCityPresenter
    {
        Vector3 GetPopupScale();
        
        event Action OnBuyButtonStateChanged;
        Transform CityTransform { get; }
        string Price { get; }
        bool HasMoney { get; }
    }
}