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

namespace Librarian.Framework
{
    /// <summary>
    /// Custom attribute that, when applied to a type derived
    /// from <see cref="BusinessRule"/>, indicates in which
    /// situation the business rule should be invoked.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple=true )]
    public sealed class BusinessRuleAppliesAttribute : Attribute
    {
        private readonly string situation;

        /// <summary>
        /// Initializes a new <see cref="BusinessRuleAppliesAttribute"/>
        /// </summary>
        /// <param name="situation">Name of the situation in which
        /// the business rule should be invoked.</param>
        public BusinessRuleAppliesAttribute( string situation )
        {
            this.situation = situation;
        }

        /// <summary>
        /// Gets the name of the situation in which the business
        /// rule should be invoked.
        /// </summary>
        public string Situation { get { return situation; } }
    }
}