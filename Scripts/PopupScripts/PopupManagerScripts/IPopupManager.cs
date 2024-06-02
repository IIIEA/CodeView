using System;

namespace _Fly_Connect.Scripts.PopupScripts.PopupManagerScripts
{
    public interface IPopupManager<TKey>
    {
        event Action<TKey> OnPopupShown;

        event Action<TKey> OnPopupHidden;

        void ShowPopup(TKey key, object args = default);

        bool IsPopupActive(TKey key);

        void HidePopup(TKey key);

        void HideAllPopups();
    }
}