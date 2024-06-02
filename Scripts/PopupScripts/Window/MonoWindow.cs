using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Fly_Connect.Scripts.PopupScripts.Window
{
    public class MonoWindow : MonoBehaviour, IWindow
    {
        [Space]
        [SerializeField]
        private UnityEvent<object> onShow;

        [SerializeField]
        private UnityEvent onHide;

        private IWindow.Callback _callback;

        public void Show(object args)
        {
            OnShow(args);
            onShow?.Invoke(args);
        }

        public void Show(object args, IWindow.Callback callback)
        {
            _callback = callback;
            OnShow(args);
            onShow?.Invoke(args);
        }

        public void Hide()
        {
            OnHide();
            onHide?.Invoke();
        }

        public void NotifyAboutClose()
        {
            if (_callback != null)
            {
                _callback.OnClose(this);
            }
        }

        public IEnumerator CloseAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            NotifyAboutClose();
        }

        protected virtual void OnShow(object args)
        {
        }

        protected virtual void OnHide()
        {
        }
    }

    public abstract class MonoWindow<TArgs> : MonoWindow
    {
        protected sealed override void OnShow(object args)
        {
            if (args is TArgs tArgs)
            {
                OnShow(tArgs);
            }
            else
            {
                throw new Exception($"Expected args of type {typeof(TArgs).Name}!");
            }
        }

        protected abstract void OnShow(TArgs args);
    }
}