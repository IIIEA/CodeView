using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Storages
{
    [Serializable]
    public struct MoneyData
    {
        [SerializeField]
        public long Money;

        [SerializeField]
        public int Ticket;
    }
}                                                                            