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
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Librarian.Framework;

namespace Librarian.Entities
{
    [Serializable]
    public sealed class Employee : Entity, IIdentity, IPrincipal
    {
        private static readonly HashAlgorithm sha = new SHA1CryptoServiceProvider();

        [Required] public string Login;

        [Required] public string FirstName;

        [Required] public string LastName;

        [Required] private string passwordHash;

        [NonSerialized] private bool authenticated;

        public void SetPassword( string password )
        {
            this.passwordHash = ComputeHash( password );
        }

        public bool Authenticate( string password )
        {
            if ( ComputeHash( password ) == this.passwordHash )
            {
                this.authenticated = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string ComputeHash( string password )
        {
            return Convert.ToBase64String( sha.ComputeHash( Encoding.UTF8.GetBytes( password ) ) );
        }

        #region IIdentity Members

        string IIdentity.AuthenticationType { get { return this.authenticated ? "Basic authentication" : null; } }

        bool IIdentity.IsAuthenticated { get { return this.authenticated; } }

        string IIdentity.Name { get { return this.Login; } }

        #endregion

        #region IPrincipal Members

        public IIdentity Identity { get { return this; } }

        public bool IsInRole( string role )
        {
            return role == "Employee";
        }

        #endregion
    }
}