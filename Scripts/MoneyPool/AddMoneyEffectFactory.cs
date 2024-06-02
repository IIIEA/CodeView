using System;
using _Fly_Connect.Visual;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Fly_Connect.Scripts.MoneyPool
{
    [Serializable]
    public sealed class AddMoneyEffectFactory
    {
        [SerializeField] private AddMoneyEffect _addMoneyEffectPrefab;
        [SerializeField] private Transform _container;

        public AddMoneyEffect Create()
        {
           var effect =  Object.Instantiate(_addMoneyEffectPrefab, _container.transform);
           effect.gameObject.SetActive(false);
           return effect;
        }
    }
}