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
using System.IO;
using System.Reflection;
using PostSharp.Sdk.CodeModel;

namespace AssemblyExplorer
{
    internal class AssemblyResolver : IDisposable
    {
        private static readonly string[] assemblyExtensions = {"dll", "exe"};
        private Domain domain;
        private bool inResolve = false;
        public bool useAssemblyNameOnly;
        private static AssemblyResolver current;

        public AssemblyResolver( Domain domain )
        {
            this.domain = domain;
            current = this;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public static AssemblyResolver Current { get { return current; } }

        public bool UseAssemblyNameOnly { get { return this.useAssemblyNameOnly; } set { this.useAssemblyNameOnly = value; } }

        private Assembly CurrentDomain_AssemblyResolve( object sender, ResolveEventArgs args )
        {
            // Avoid recursion.
            if ( inResolve )
                return null;

            inResolve = true;
            try
            {
                // First we try to load the assembly without manual hints.
                try
                {
                    AssemblyName assemblyName = new AssemblyName( args.Name );
                    if ( this.useAssemblyNameOnly )
                    {
                        assemblyName = new AssemblyName( assemblyName.Name );
                    }
                    return Assembly.Load( assemblyName );
                }
                catch
                {
                    // Ignore exceptions.
                }

                // We will look in private path of loaded domains.
                AssemblyName name = new AssemblyName( args.Name );

                foreach ( AssemblyEnvelope assembly in this.domain.Assemblies )
                {
                    Assembly existingAssembly = assembly.GetSystemAssembly();
                    if ( !existingAssembly.GlobalAssemblyCache )
                    {
                        string directory = Path.GetDirectoryName( existingAssembly.Location );

                        foreach ( string extension in assemblyExtensions )
                        {
                            string fileName = Path.Combine(
                                directory, name.Name + "." + extension );
                            if ( File.Exists( fileName ) )
                            {
                                AssemblyName candidateName = AssemblyName.GetAssemblyName( fileName );
                                if ( candidateName.FullName == name.FullName )
                                {
                                    // We found it!
                                    return Assembly.LoadFrom( fileName );
                                }
                            }
                        }
                    }
                }

                return null;
            }
            finally
            {
                inResolve = false;
            }
        }

        public void Dispose()
        {
            if ( this.domain != null )
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
                this.domain = null;
            }
        }
    }
}