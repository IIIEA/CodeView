using UnityEngine;

namespace _Fly_Connect.Scripts.PopupScripts
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInParent<T>(this Component component, out T result) where T : Component
        {
            result = component.GetComponentInParent<T>();
            return result != null;
        }
    }
}