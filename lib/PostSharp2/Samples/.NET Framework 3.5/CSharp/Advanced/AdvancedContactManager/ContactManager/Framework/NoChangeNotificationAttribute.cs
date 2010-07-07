using System;

namespace ContactManager.Framework
{
    [AttributeUsage( AttributeTargets.Property )]
    public sealed class NoChangeNotificationAttribute : Attribute
    {
    }
}