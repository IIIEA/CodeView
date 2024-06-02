using System;
using System.Collections.Generic;
using System.Reflection;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.Locator;

namespace _Fly_Connect.Scripts.Infrastructure.GameManager
{
    public static class DependencyInjector
    {
        private static Dictionary<Type, object> _cachedServices = new();

        public static void Inject(object target, ServiceLocator locator)
        {
            Type type = target.GetType();

            MethodInfo[] methods = type.GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.FlattenHierarchy);

            foreach (MethodInfo method in methods)
            {
                if (method.IsDefined(typeof(InjectAttribute)))
                {
                    InvokeConstruct(method, target, locator);
                }
            }
        }

        private static void InvokeConstruct(MethodInfo method, object target, ServiceLocator locator)
        {
            ParameterInfo[] parameters = method.GetParameters();
            int count = parameters.Length;
            object[] args = new object[count];

            for (int i = 0; i < count; i++)
            {
                ParameterInfo parameter = parameters[i];
                Type type = parameter.ParameterType;

                if (_cachedServices.TryGetValue(type, out object cachedService))
                {
                    args[i] = cachedService;
                }
                else
                {
                    object service = locator.GetService(type);
                    args[i] = service;
                    _cachedServices[type] = service;
                }
            }

            method.Invoke(target, args);
        }
    }
}