using System;
using JetBrains.Annotations;

namespace _Fly_Connect.Scripts.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    public sealed class InjectAttribute : Attribute
    {
    
    }
}