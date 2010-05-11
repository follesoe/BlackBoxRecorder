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
using System.Reflection;

namespace Librarian.Framework
{
    /// <summary>
    /// Discovers and evaluates business rules.
    /// </summary>
    /// <remarks>
    /// Client code should not access business rules directly (it should not know their existence).
    /// They should go through the <see cref="BusinessRulesManager"/> instead.
    /// </remarks>
    public static class BusinessRulesManager
    {
        private static readonly Dictionary<string, List<BusinessRule>> businessRules =
            new Dictionary<string, List<BusinessRule>>( StringComparer.InvariantCultureIgnoreCase );

        public static void RegisterAssembly( Assembly assembly )
        {
            lock ( businessRules )
            {
                // Discover, instantiate and index business rules.
                foreach ( Type type in assembly.GetExportedTypes() )
                {
                    // Is the type derived from BusinessRules?
                    if ( typeof(BusinessRule).IsAssignableFrom( type ) )
                    {
                        // Index each situation to which the rule applies.
                        foreach ( BusinessRuleAppliesAttribute attribute in
                            type.GetCustomAttributes( typeof(BusinessRuleAppliesAttribute), true ) )
                        {
                            List<BusinessRule> businessRulesForThisSituation;
                            if ( !businessRules.TryGetValue( attribute.Situation, out businessRulesForThisSituation ) )
                            {
                                businessRulesForThisSituation = new List<BusinessRule>();
                                businessRules.Add( attribute.Situation, businessRulesForThisSituation );
                            }

                            businessRulesForThisSituation.Add( (BusinessRule) Activator.CreateInstance( type ) );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Throws an exception when fails at least one of the business rules applying to a
        /// given situation.
        /// </summary>
        /// <param name="situation">Situation to which business rules are applied.</param>
        /// <param name="item">Parameter of the situation (typically the entity that is
        /// subject to the business process).</param>
        /// <exception cref="BusinessRuleException">At least one of the business rules was
        /// not fulfilled.</exception>
        public static void Assert( string situation, object item )
        {
            BusinessRuleCollection failedRules = new BusinessRuleCollection();
            if ( !Evaluate( situation, item, failedRules ) )
            {
                throw new BusinessRuleException( failedRules );
            }
        }

        /// <summary>
        /// Evaluates the set of business rules applying to a given situation.
        /// </summary>
        /// <param name="situation">Situation to which business rules are applied.</param>
        /// <param name="item">Parameter of the situation (typically the entity that is
        /// subject to the business process).</param>
        /// <param name="failedRules">Collection to which should be added the business rules 
        /// that failed, or <b>null</b> if this information is not requested.</param>
        /// <returns><b>true</b> if all rules were successful, otherwise <b>false</b>.</returns>
        public static bool Evaluate( string situation, object item, BusinessRuleCollection failedRules )
        {
            if ( situation == null )
                throw new ArgumentNullException( "situation" );

            List<BusinessRule> businessRulesForThisSituation;
            if ( !businessRules.TryGetValue( situation, out businessRulesForThisSituation ) )
            {
                return true;
            }
            else
            {
                bool success = true;

                foreach ( BusinessRule rule in businessRulesForThisSituation )
                {
                    if ( rule.Evaluate( item ) ) continue;

                    success = false;
                    if ( failedRules != null )
                    {
                        failedRules.Add( rule );
                    }
                }

                return success;
            }
        }
    }
}