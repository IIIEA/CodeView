using UnityEngine.Serialization;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public struct UpgradeData
    {
        public int SpeedLevel;
       [FormerlySerializedAs("SpeedOnTap")] public int UfoSeller;
       public int AirportCapacityLevel;
    }
}