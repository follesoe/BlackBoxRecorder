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

using System.Collections.ObjectModel;

namespace Librarian.Framework
{
    /// <summary>
    /// Base class for validation busines rules.
    /// </summary>
    /// <remarks>
    /// Classed derived from <see cref="BusinessRule"/> should
    /// be decorated with the <see cref="BusinessRuleAppliesAttribute"/> custom
    /// attribute.
    /// </remarks>
    public abstract class BusinessRule
    {
        /// <summary>
        /// Gets the human-readable description of the business rule.
        /// </summary>
        public virtual string Description { get { return this.Name; } }

        /// <summary>
        /// Gets the code name of the business rule.
        /// </summary>
        public virtual string Name { get { return this.GetType().Name; } }

        /// <summary>
        /// Evaluates the business rule.
        /// </summary>
        /// <param name="item">Argument of the business rule (typically the entity affected by the business process).</param>
        /// <returns><b>true</b> if the business rule is respected, <b>false</b> otherwise (in case of error).</returns>
        public abstract bool Evaluate( object item );
    }

    /// <summary>
    /// Collection of business rules (<see cref="BusinessRule"/>).
    /// </summary>
    public class BusinessRuleCollection : Collection<BusinessRule>
    {
    }
}