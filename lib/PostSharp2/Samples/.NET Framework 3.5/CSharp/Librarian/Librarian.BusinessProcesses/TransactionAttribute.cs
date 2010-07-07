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
using System.Transactions;
using PostSharp.Aspects;

namespace Librarian
{
    [Serializable]
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=false )]
    public sealed class TransactionAttribute : OnMethodBoundaryAspect
    {
        private readonly TransactionScopeOption transactionScopeOption;
        private readonly float timeout;

        public TransactionAttribute( TransactionScopeOption transactionScopeOption, float timeout )
        {
            this.transactionScopeOption = transactionScopeOption;
            this.timeout = timeout;
        }

        public TransactionAttribute( TransactionScopeOption transactionScopeOption )
            : this( transactionScopeOption, 60 )
        {
        }

        public TransactionAttribute()
            : this( TransactionScopeOption.Required, 60 )
        {
        }

        public TransactionScopeOption TransactionScopeOption { get { return this.transactionScopeOption; } }
        public float Timeout { get { return this.timeout; } }

        public override void OnEntry( MethodExecutionArgs eventArgs )
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.ReadCommitted;
            options.Timeout = TimeSpan.FromSeconds( this.timeout );
            eventArgs.MethodExecutionTag = new TransactionScope( this.transactionScopeOption, options );
        }

        public override void OnSuccess( MethodExecutionArgs eventArgs )
        {
            TransactionScope transactionScope = (TransactionScope) eventArgs.MethodExecutionTag;
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public override void OnException( MethodExecutionArgs eventArgs )
        {
            TransactionScope transactionScope = (TransactionScope) eventArgs.MethodExecutionTag;
            transactionScope.Dispose();
        }
    }
}