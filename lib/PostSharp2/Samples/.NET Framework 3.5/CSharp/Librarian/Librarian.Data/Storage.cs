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
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using Librarian.Framework;
using EntityKey=Librarian.Framework.EntityKey;

namespace Librarian.Data
{
    /// <summary>
    /// Manages the storage to the file system.
    /// </summary>
    public sealed class Storage : EntityRepository
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static Storage instance;

        /// <summary>
        /// Directory in which database files are stored.
        /// </summary>
        private readonly string directory;

        /// <summary>
        /// Serialization formatter.
        /// </summary>
        private readonly IFormatter formatter;

        /// <summary>
        /// Extension of object files.
        /// </summary>
        private const string objectFileMask = "object-{0}.bin";

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        public static Storage Current { get { return instance; } }

        /// <summary>
        /// Initializes a new <see cref="Storage"/>.
        /// </summary>
        /// <param name="directory">Directory in which database files are stored.</param>
        private Storage( string directory )
        {
            this.directory = directory;
            BinaryFormatter binaryFormatter = new BinaryFormatter
                                                  {
                                                      TypeFormat = FormatterTypeStyle.XsdString | FormatterTypeStyle.TypesWhenNeeded,
                                                      AssemblyFormat = FormatterAssemblyStyle.Simple
                                                  };
            this.formatter = binaryFormatter;
        }

        /// <summary>
        /// Initializes a <see cref="Storage"/>, i.e.
        /// creates the <see cref="Current"/> instance and reads data from the filesystem.
        /// </summary>
        /// <param name="directory">Directory in which database files are stored.</param>
        public static void Initialize( string directory )
        {
            instance = new Storage( directory );
            instance.Load();
        }

        /// <summary>
        /// Loads objects on the filesystem into the database.
        /// </summary>
        [Framework.Trace]
        private void Load()
        {
            string[] objectFiles = Directory.GetFiles( this.directory, string.Format( objectFileMask, "*" ) );

            for ( int i = 0 ; i < objectFiles.Length ; i++ )
            {
                try
                {
                    using ( FileStream file = File.OpenRead( objectFiles[i] ) )
                    {
                        BaseEntity entity = (BaseEntity) formatter.Deserialize( file );
                        Trace.TraceInformation( "Loading object {0} of type {1}.", entity.EntityKey, entity );
                        this.InternalSet( entity );
                    }
                }
                catch ( Exception  e )
                {
                    Trace.TraceError( "Cannot load the file {0}: {1}", objectFiles[i], e.Message );
                    File.Delete( objectFiles[i] );
                }
            }
        }


        /// <summary>
        /// Makes a new (and unique) <see cref="Framework.EntityKey"/>.
        /// </summary>
        /// <returns>A new <see cref="Framework.EntityKey"/>.</returns>
        internal static EntityKey MakeEntityKey()
        {
            EntityKey entityKey = new EntityKey( Guid.NewGuid().ToString() );
            return entityKey;
        }

        /// <summary>
        /// Gets the path of the object file for a given entity key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The path where the object with this key should be stored.</returns>
        private string GetFilePath( EntityKey key )
        {
            return Path.Combine( directory, string.Format( objectFileMask, key ) );
        }


        /// <summary>
        /// Writes an object to the filesystem.
        /// </summary>
        /// <param name="entity">BaseEntity to be written.</param>
        [Framework.Trace]
        private void WriteObject( BaseEntity entity )
        {
            string filePath = this.GetFilePath( entity.EntityKey );

            try
            {
                using ( FileStream file = File.Create( filePath ) )
                {
                    formatter.Serialize( file, entity );
                }
            }
            catch
            {
                File.Delete( filePath );
                throw;
            }
        }

        /// <summary>
        /// Gets an entity from the <see cref="Storage"/>.
        /// </summary>
        /// <param name="key">BaseEntity key.</param>
        /// <returns>The entity, or <b>null</b> if it was not found.</returns>
        internal BaseEntity GetEntity( EntityKey key )
        {
            return this.InternalGet( key );
        }

        /// <summary>
        /// Gets all entities contained in this repository.
        /// </summary>
        /// <returns>An enumerator.</returns>
        internal new IEnumerator<BaseEntity> GetAllEntitiesEnumerator()
        {
            return base.GetAllEntitiesEnumerator();
        }


        /// <summary>
        /// Executes a list of operations (typically the list of operations of a transaction).
        /// </summary>
        /// <param name="operations">List of operations to be executed.</param>
        [Trace]
        internal void DoOperations( IEnumerable<StorageOperation> operations )
        {
            lock ( this )
            {
                foreach ( StorageOperation operation in operations )
                {
                    switch ( operation.OperationKind )
                    {
                        case StorageOperationKind.Delete:
                            this.InternalRemove( operation.Entity.EntityKey );
                            File.Delete(this.GetFilePath(operation.Entity.EntityKey));
                            break;

                        case StorageOperationKind.Insert:
                        case StorageOperationKind.Update:
                            this.InternalSet(operation.Entity.Entity);
                            this.WriteObject(operation.Entity.Entity);
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Storage";
        }
    }
}