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

/*  Custom attribute that, when applied on a method, caches its return value.
 *  The cache key is computed from the method name, the object instance,
 *  the parameter values and the generic arguments.
 * 
 * 
 * **/
using System;
using System.Collections.Generic;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Aspects.Dependencies;

namespace Dependencies.Aspects
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.Caching)]
    public sealed class CacheAttribute : OnMethodBoundaryAspect
    {
        // Some formatting strings to compose the cache key.
        private MethodFormatStrings formatStrings;

        // A dictionary that serves as a trivial cache implementation.
        private static readonly Dictionary<string, Object> cache = new Dictionary<string, object>();


        // Validate the attribute usage.
        public override bool CompileTimeValidate( MethodBase method )
        {
            // Don't apply to constructors.
            if ( method is ConstructorInfo )
            {
                Message.Write( SeverityType.Error, "CX0001", "Cannot cache constructors." );
                return false;
            }

            MethodInfo methodInfo = (MethodInfo) method;

            // Don't apply to void methods.
            if ( methodInfo.ReturnType.Name == "Void" )
            {
                Message.Write( SeverityType.Error, "CX0002", "Cannot cache void methods." );
                return false;
            }

            // Does not support out parameters.
            ParameterInfo[] parameters = method.GetParameters();
            for ( int i = 0; i < parameters.Length; i++ )
            {
                if ( parameters[i].IsOut )
                {
                    Message.Write( SeverityType.Error, "CX0003", "Cannot cache methods with return values." );
                    return false;
                }
            }

            return true;
        }


        // At compile time, initialize the format string that will be
        // used to create the cache keys.
        public override void CompileTimeInitialize( MethodBase method, AspectInfo aspectInfo )
        {
            this.formatStrings = Formatter.GetMethodFormatStrings( method );
        }

        // Executed at runtime, before the method.
        public override void OnEntry( MethodExecutionArgs eventArgs )
        {
            // Compose the cache key.
            string key = this.formatStrings.Format(
                eventArgs.Instance, eventArgs.Method, eventArgs.Arguments.ToArray() );

            // Test whether the cache contains the current method call.
            lock ( cache )
            {
                object value;
                if ( !cache.TryGetValue( key, out value ) )
                {
                    // If not, we will continue the execution as normally.
                    // We store the key in a state variable to have it in the OnExit method.
                    eventArgs.MethodExecutionTag = key;
                }
                else
                {
                    // If it is in cache, we set the cached value as the return value
                    // and we force the method to return immediately.
                    eventArgs.ReturnValue = value;
                    eventArgs.FlowBehavior = FlowBehavior.Return;
                }
            }
        }

        // Executed at runtime, after the method.
        public override void OnSuccess( MethodExecutionArgs eventArgs )
        {
            // Retrieve the key that has been computed in OnEntry.
            string key = (string) eventArgs.MethodExecutionTag;

            // Put the return value in the cache.
            lock (cache)
            {
                cache[key] = eventArgs.ReturnValue;
            }
        }
    }
}