using System.Collections;
using System.Collections.Generic;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Visual;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Fly_Connect.Scripts.MoneyPool
{
    public sealed class MoneyPool : SerializedMonoBehaviour
    {
        [OdinSerialize] private Queue<AddMoneyEffect> _pool = new();
        [OdinSerialize] private int _poolCount = 30;

        private bool _canShowEffect = true;
        public bool _isShowEffect = true;

        private AddMoneyEffectFactory _addMoneyEffectFactory;

        [Inject]
        private void Constructor(AddMoneyEffectFactory addMoneyEffectFactory)
        {
            _addMoneyEffectFactory = addMoneyEffectFactory;

            for (int i = 0; i < _poolCount; i++)
            {
                var moneyEffect = _addMoneyEffectFactory.Create();
                _pool.Enqueue(moneyEffect);
            }
        }

        public void SetShowEffect(bool isShowEffect)
        {
            if (_isShowEffect != isShowEffect)
                _isShowEffect = isShowEffect;
        }

        public AddMoneyEffect Get()
        {
            var effect = _pool.TryDequeue(out var result) ? result : _addMoneyEffectFactory.Create();
            effect.gameObject.SetActive(_isShowEffect);
            return effect;
        }

        public void Release(AddMoneyEffect effect)
        {
            effect.gameObject.SetActive(false);
            _pool.Enqueue(effect);
        }

        public void ShowGlowEffects()
        {
            if (_canShowEffect)
                StartCoroutine(ShowGlowEffect());
        }

        private IEnumerator ShowGlowEffect()
        {
            _canShowEffect = false;

            foreach (var effect in _pool)
            {
                effect.EnableGlow();
            }

            yield return new WaitForSeconds(2f);

            foreach (var effect in _pool)
            {
                effect.DisableGlow();
            }

            _canShowEffect = true;
        }
    }
}