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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Librarian.Framework
{
    /// <summary>
    /// Exception (thrown by <see cref="BusinessRulesManager"/>.<see cref="BusinessRulesManager.Assert"/>) when a set of business rules was
    /// asserted, but at least one of these rules was not successful.
    /// </summary>
    [Serializable]
    public class BusinessRuleException : BusinessException
    {
        private readonly ICollection<BusinessRule> failedRules;

        /// <summary>
        /// Initializes a new <see cref="BusinessRuleException"/>.
        /// </summary>
        /// <param name="failedRules">Collection of business rules that have failed.</param>
        internal BusinessRuleException( IList<BusinessRule> failedRules )
            : base( GetMessage( failedRules ) )
        {
            this.failedRules = new ReadOnlyCollection<BusinessRule>( failedRules );
        }

        /// <summary>
        /// Composes the exception message as a function of the set of business rules
        /// that have failed.
        /// </summary>
        /// <param name="failedRules">Set of exceptions that have failed.</param>
        /// <returns>A human-readable exception message.</returns>
        private static string GetMessage( ICollection<BusinessRule> failedRules )
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append( "The following business rules have failed: " );
            foreach ( BusinessRule businessRule in failedRules )
            {
                messageBuilder.Append( Environment.NewLine );
                messageBuilder.Append( '\t' );
                messageBuilder.Append( businessRule.Description );
            }

            return messageBuilder.ToString();
        }

        /// <summary>
        /// Gets the collection of failed business rules.
        /// </summary>
        public ICollection<BusinessRule> FailedRules { get { return this.failedRules; } }
    }
}