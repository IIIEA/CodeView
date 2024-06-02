using _Fly_Connect.Scripts.Infrastructure.Locator;

namespace _Fly_Connect.Scripts.Infrastructure.GameManager
{
    public interface IInjectProvider
    {
        void Inject(ServiceLocator serviceLocator);
    }
}