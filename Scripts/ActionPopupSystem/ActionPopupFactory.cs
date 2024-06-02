using System;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Fly_Connect.Scripts.ActionPopupSystem
{
    [Serializable]
    public class ActionPopupFactory
    {
        [SerializeField] private ActionPopupView _actionPopupViewPrefab;
        [SerializeField] private Transform _container; 

        private MoneyPool.MoneyPool _moneyPool;

        [Inject]
        public void Construct(MoneyPool.MoneyPool moneyPool)
        {
            _moneyPool = moneyPool;
        }

        public ActionPopupView Create()
        {
            var actionPopupView = Object.Instantiate(_actionPopupViewPrefab, _container);
            actionPopupView.Construct(_moneyPool);
            return actionPopupView;
        }
    }
}