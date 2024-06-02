using System.Collections.Generic;

namespace _Fly_Connect.Scripts.Infrastructure.GameManager
{
    public interface IGameListenerProvider
    {
        IEnumerable<IGameListener> ProvideListeners();
    }
}