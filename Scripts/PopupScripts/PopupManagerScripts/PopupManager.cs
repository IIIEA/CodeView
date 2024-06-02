using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts.PopupScripts.Window;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.PopupScripts.PopupManagerScripts
{
    [Serializable]
    public class PopupManager<TKey, TPopup> : IPopupManager<TKey>, IWindow.Callback where TPopup : IWindow
    {
        private Dictionary<TKey, TPopup> _activePopups = new ();
        private List<TKey> _cache = new();
        private IWindowSupplier<TKey, TPopup> _supplier;

        public bool HasActivePopups => _activePopups.Count > 0;

        public event Action<TKey> OnPopupShown;
        public event Action<TKey> OnPopupHidden;

        [Button]
        public void ShowPopup(TKey key, object args = default)
        {
            if (!IsPopupActive(key))
            {
                ShowPopupInternal(key, args);
            }
        }

        [Button]
        public void HidePopup(TKey key)
        {
            if (IsPopupActive(key))
            {
                HidePopupInternal(key);
            }
        }

        [Button]
        public void HideAllPopups()
        {
            _cache.Clear();
            _cache.AddRange(_activePopups.Keys);

            for (int i = 0, count = _cache.Count; i < count; i++)
            {
                var popupName = _cache[i];
                HidePopupInternal(popupName);
            }
        }

        public bool IsPopupActive(TKey key)
        {
            return _activePopups.ContainsKey(key);
        }

        void IWindow.Callback.OnClose(IWindow window)
        {
            var popup = (TPopup) window;

            if (TryFindName(popup, out var popupName))
            {
                HidePopup(popupName);
            }
        }

        private void ShowPopupInternal(TKey name, object args)
        {
            var popup = _supplier.LoadWindow(name);
            popup.Show(args, callback: this);

            _activePopups.Add(name, popup);
            OnPopupShown?.Invoke(name);
        }

        private void HidePopupInternal(TKey name)
        {
            var popup = _activePopups[name];
            popup.Hide();

            _activePopups.Remove(name);
            _supplier.UnloadWindow(popup);
            OnPopupHidden?.Invoke(name);
        }

        private bool TryFindName(TPopup popup, out TKey name)
        {
            foreach (var (key, otherPopup) in _activePopups)
            {
                if (ReferenceEquals(popup, otherPopup))
                {
                    name = key;
                    return true;
                }
            }

            name = default;
            return false;
        }

        public void SetSupplier(IWindowSupplier<TKey, TPopup> supplier)
        {
            _supplier = supplier;
        }
    }
}