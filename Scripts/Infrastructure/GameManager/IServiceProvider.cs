using System;
using System.Collections.Generic;

namespace _Fly_Connect.Scripts.Infrastructure.GameManager
{
    public interface IServiceProvider
    {
        IEnumerable<(Type, object)> ProvideServices();
    }
}