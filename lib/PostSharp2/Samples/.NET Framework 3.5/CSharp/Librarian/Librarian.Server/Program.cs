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
using System.Runtime.Remoting;
using Librarian.BusinessProcesses;
using Librarian.Data;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian.Server
{
    internal class Program
    {
        private static void Main()
        {
            // Initialize the business rules manager.
            BusinessRulesManager.RegisterAssembly( typeof(Host).Assembly );

            // Initialize the entity resolver for this domain.
            Entity.EntityResolver = new LocalEntityResolver();

            // Load the database (this could take a while!).
            string directory = Path.Combine( Path.GetDirectoryName( typeof(Host).Assembly.Location ), "data" );
            if ( !Directory.Exists( directory ) )
                Directory.CreateDirectory( directory );

            Storage.Initialize( directory );

            InitializationProcesses initializationProcess = new InitializationProcesses();

            initializationProcess.CheckDatabaseInitialized();

            // Configure remoting
            RemotingConfiguration.Configure( Path.Combine( Path.GetDirectoryName( typeof(Program).Assembly.Location ), "Librarian.Server.exe.config"), false );

            // Start the command interpreter.
            while ( true )
            {
                Console.Write( "> " );
                string command = Console.ReadLine();

                if ( command == "exit" )
                {
                    return;
                }
                else
                {
                    Console.WriteLine( "Invalid command: " + command );
                    Console.WriteLine( "Usage: exit" );
                }
            }
        }

        private class LocalEntityResolver : IEntityResolver
        {
            public BaseEntity GetEntity( EntityKey entityKey )
            {
                return StorageContext.Current.GetEntity( entityKey );
            }
        }
    }
}