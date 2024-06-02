using _Fly_Connect.Scripts.PopupScripts.Window;

namespace _Fly_Connect.Scripts.PopupScripts.PopupManagerScripts
{
    public interface IWindowSupplier<in TKey, TWindow> where TWindow : IWindow
    {
        TWindow LoadWindow(TKey key);

        void UnloadWindow(TWindow window);
    }
}