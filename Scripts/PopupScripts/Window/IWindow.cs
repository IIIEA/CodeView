namespace _Fly_Connect.Scripts.PopupScripts.Window
{
    public interface IWindow
    {
        void Show(object args = null, Callback callback = null);

        void Hide();

        public interface Callback
        {
            void OnClose(IWindow window);
        }
    }
}