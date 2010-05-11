#region Released to Public Domain by Gael Fraiteur
/*----------------------------------------------------------------------------*
 *   This file is part of samples of PostSharp.                                *
 *                                                                             *
 *   This sample is free software: you have an unlimited right to              *
 *   redistribute it and/or modify it.                                         *
 *                                                                             *
 *   This sample is distributed in the hope that it will be useful,            *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of            *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                      *
 *                                                                             *
 *----------------------------------------------------------------------------*/
#endregion

using System.Resources;
using PostSharp.Extensibility;

namespace DbInvoke
{
    /// <summary>
    /// Provides a <see cref="MessageSource"/> from the current plug-in.
    /// </summary>
    internal class DbInvokeMessageSource
    {
        public static readonly MessageSource Instance = new MessageSource(
            "PostSharp.Samples.DbInvoke",
            new ResourceManager( "PostSharp.Samples.DbInvoke.Messages", typeof(DbInvokeMessageSource).Assembly ) );
    }
}