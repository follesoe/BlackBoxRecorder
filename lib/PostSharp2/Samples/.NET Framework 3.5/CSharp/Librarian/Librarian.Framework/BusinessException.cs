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

using System;
using System.Runtime.Serialization;

namespace Librarian.Framework
{
    /// <summary>
    /// Base class for business exceptions.
    /// </summary>
    /// <remarks>
    /// A business exception is, by default, a previsible exception
    /// that results from user operations. Business exceptions are
    /// exceptions that would happen even if 'the technique was perfect'.
    /// </remarks>
    [Serializable]
    public abstract class BusinessException : Exception
    {
        protected BusinessException(
            SerializationInfo info,
            StreamingContext context )
            : base( info, context )
        {
        }

        protected BusinessException( string message )
            : base( message )
        {
        }
    }
}