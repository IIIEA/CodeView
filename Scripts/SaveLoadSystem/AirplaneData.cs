using System.Collections.Generic;
using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public class AirplaneData
    {
        public Dictionary<int, AirplaneInfo> AirplaneInfos;

        public AirplaneData(Dictionary<int, AirplaneInfo> airplaneInfos)
        {
            AirplaneInfos = airplaneInfos;
        } 
    }
}