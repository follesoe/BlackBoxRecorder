using System;
using PostSharp.Extensibility;

namespace ContactManager.Framework
{
    [MulticastAttributeUsage( MulticastTargets.Property )]
    public sealed class EntityPropertyAttribute : Attribute
    {
    }
}