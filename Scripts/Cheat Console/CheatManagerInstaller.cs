using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.Cheat_Console
{
    public class CheatManagerInstaller : GameInstaller
    {
        [SerializeField, Listener] 
        private DevelopmentPanel _developmentPanel;

        [SerializeField, Listener] 
        private AnalyticPanel _analyticPanel;

        [SerializeField, Listener, Service(typeof(IncomeCounter))] 
        private IncomeCounter _incomeCounter;
    }
}