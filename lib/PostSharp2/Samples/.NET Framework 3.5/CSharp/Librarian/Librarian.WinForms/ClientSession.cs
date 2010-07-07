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
using System.Configuration;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian.WinForms
{
    internal static class ClientSession
    {
        private static Accessor<ISession> current;

        static ClientSession()
        {
        }

        public static bool OpenSession( string login, string password )
        {
            string factoryUrl = ConfigurationManager.AppSettings["SectionFactoryUrl"];
            ISessionFactory sessionFactory = (ISessionFactory) Activator.GetObject( typeof(ISession), factoryUrl );
            current = new Accessor<ISession>( sessionFactory.OpenSession( login, password ) );

            // Set an entity resolver.
            Entity.EntityResolver = new RemoteEntityResolver();

            return current != null;
        }

        public static ISession Current { get { return current.Value; } }

        public static Accessor<T> GetService<T>()
            where T : class
        {
            return new Accessor<T>( (T) current.Value.GetService( typeof(T).Name.Substring( 1 ) ) );
        }


        private class RemoteEntityResolver : IEntityResolver
        {
            private readonly Accessor<IEntityResolver> impl = GetService<IEntityResolver>();

            public BaseEntity GetEntity( EntityKey entityKey )
            {
                return this.impl.Value.GetEntity( entityKey );
            }
        }
    }
}