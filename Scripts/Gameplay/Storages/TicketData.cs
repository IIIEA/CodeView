using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Storages
{
    [Serializable]
    public struct TicketData
    {
        [SerializeField] 
        public int CollectedRewards;
        
        [SerializeField] 
        public bool CanGetReward;
    }
}