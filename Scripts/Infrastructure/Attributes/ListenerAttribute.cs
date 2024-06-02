using System;
using JetBrains.Annotations;

namespace _Fly_Connect.Scripts.Infrastructure.Attributes
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ListenerAttribute : Attribute
    {
    
    }
}