using System;
using System.Collections.Generic;
using System.Reflection;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.Locator;
using UnityEngine;

namespace _Fly_Connect.Scripts.Infrastructure.GameManager
{
    public abstract class GameInstaller : MonoBehaviour,
        IGameListenerProvider,
        IServiceProvider,
        IInjectProvider
    {
        public virtual IEnumerable<IGameListener> ProvideListeners()
        {
            FieldInfo[] fields = GetType().GetFields
            (
                BindingFlags.Instance |
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.DeclaredOnly
            );

            foreach (var field in fields)
            {
                if (field.IsDefined(typeof(ListenerAttribute)) && field.GetValue(this) is IGameListener gameListener)
                {
                    yield return gameListener;
                }
            }
        }

        public virtual IEnumerable<(Type, object)> ProvideServices()
        {
            FieldInfo[] fields = GetType().GetFields
            (
                BindingFlags.Instance |
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.DeclaredOnly
            );

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<ServiceAttribute>();

                if (attribute != null)
                {
                    var type = attribute.Contract;
                    var service = field.GetValue(this);
                    yield return (type, service);
                }
            }
        }

        public virtual void Inject(ServiceLocator serviceLocator)
        {
            FieldInfo[] fields = GetType().GetFields
            (
                BindingFlags.Instance |
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.DeclaredOnly
            );

            foreach (var field in fields)
            {
                var target = field.GetValue(this);
                DependencyInjector.Inject(target, serviceLocator);
            }
        }
    }
}