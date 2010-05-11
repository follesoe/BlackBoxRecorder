using System;

namespace ContactManager.Framework
{
    [AttributeUsage( AttributeTargets.Method )]
    public sealed class SafeWhenDisposedAttribute : Attribute
    {
    }
}