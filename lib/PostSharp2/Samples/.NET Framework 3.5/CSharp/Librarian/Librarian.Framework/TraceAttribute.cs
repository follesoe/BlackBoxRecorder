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
using System.Diagnostics;
using System.Reflection;
using PostSharp.Aspects;

namespace Librarian.Framework
{
    /// <summary>
    /// Custom attribute that, when applied to a method (eventually using multicasting),
    /// traces its execution.
    /// </summary>
    [Serializable]
    [AttributeUsage( AttributeTargets.All, AllowMultiple=true )]
    public class TraceAttribute : OnMethodBoundaryAspect
    {
        private MethodFormatStrings strings;

        private bool skipOnExit;

        /// <summary>
        /// Indicates that the method exit should not be traced.
        /// </summary>
        public bool DontTraceOnExit { get { return skipOnExit; } set { skipOnExit = value; } }


        /// <summary>
        /// Called at compile-time to initialize the current custom attribute instance.
        /// We compute the formatting strings.
        /// </summary>
        /// <param name="method">Method to which the current custom attribute is
        /// applied.</param>
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            this.strings = TraceFormatter.GetMethodFormatStrings( method );
        }


        /// <summary>
        /// Called at runtime before the method is executed.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        public override void OnEntry( MethodExecutionArgs eventArgs )
        {
            Trace.TraceInformation(
                "Entering " + this.strings.Format( eventArgs.Instance, eventArgs.Method, eventArgs.Arguments.ToArray() )
                );
            Trace.Indent();
        }

        /// <summary>
        /// Called at runtime after the method is executed, in case of success (no exception).
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        public override void OnSuccess( MethodExecutionArgs eventArgs )
        {
            Trace.Unindent();
            if ( !this.skipOnExit )
            {
                Trace.TraceInformation( "Exiting " +
                                        this.strings.Format( eventArgs.Instance, eventArgs.Method,
                                                             eventArgs.Arguments.ToArray()));
            }
        }

        /// <summary>
        /// Called at runtime after the method is executed, in case of exception.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        public override void OnException( MethodExecutionArgs eventArgs )
        {
            Trace.Unindent();
            Trace.TraceWarning( "Exiting " +
                                this.strings.Format(eventArgs.Instance, eventArgs.Method, eventArgs.Arguments.ToArray()) +
                                " with error: " + eventArgs.Exception.Message );
        }
    }
}