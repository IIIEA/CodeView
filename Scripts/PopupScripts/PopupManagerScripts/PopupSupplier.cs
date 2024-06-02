using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.PopupScripts.PopupManagerScripts;
using _Fly_Connect.Scripts.PopupScripts.Window;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace _Fly_Connect.Scripts.PopupScripts
{
    [Serializable]
    public sealed class PopupSupplier : SerializedMonoBehaviour, IWindowSupplier<PopupName, MonoWindow>
    {
        [OdinSerialize] private readonly Dictionary<PopupName, MonoWindow> _cashedFrames = new();

        public MonoWindow LoadWindow(PopupName key)
        {
            if (_cashedFrames.TryGetValue(key, out var popup))
            {
                popup.gameObject.SetActive(true);
            }

            popup.transform.SetAsLastSibling();
            return popup;
        }

        public void UnloadWindow(MonoWindow window)
        {
            window.gameObject.SetActive(false);
        }
    }
}