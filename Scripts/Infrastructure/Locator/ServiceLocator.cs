using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Infrastructure.Locator
{
    public class ServiceLocator : MonoBehaviour
    {
        [ShowInInspector] [ReadOnly]private readonly Dictionary<Type, object> _services = new();

        public object GetService(Type type)
        {
            return _services[type];
        }

        public T GetService<T>() where T : class
        {
            return _services[typeof(T)] as T;
        }

        public void BindService(Type type, object service)
        {
            _services.Add(type, service);
        }

        public bool TryGetService<T>(out T service)
        {
          object findedService = default(object);

          if(_services.TryGetValue(typeof(T), out findedService))
          {
              service = (T) findedService;
              return true;
          }

          service = default;
          return false;
        }
    }
}