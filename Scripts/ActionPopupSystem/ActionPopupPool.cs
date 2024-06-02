using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace _Fly_Connect.Scripts.ActionPopupSystem
{
    [Serializable]
    public sealed class ActionPopupPool
    {
        [OdinSerialize] [ReadOnly] private Queue<ActionPopupView> _pool = new();
        [OdinSerialize] private int _poolCount = 30;

        private ActionPopupFactory _actionPopupFactory;
       public Queue<ActionPopupView> Pool => _pool;

        [Inject]
        private void Constructor(ActionPopupFactory actionPopupFactory)
        {
            _actionPopupFactory = actionPopupFactory;

            for (int i = 0; i < _poolCount; i++)
            {
                var actionPopup = _actionPopupFactory.Create();
                actionPopup.gameObject.gameObject.SetActive(false);
                _pool.Enqueue(actionPopup);
            }
        }

        public ActionPopupView Get()
        {
            var effectActionPopupView = _pool.TryDequeue(out var result) ? result : _actionPopupFactory.Create();
            effectActionPopupView.gameObject.SetActive(true);
            return effectActionPopupView;
        }

        public void Release(ActionPopupView effect)
        {
            effect.gameObject.SetActive(false);
            _pool.Enqueue(effect);
        }
    }
}