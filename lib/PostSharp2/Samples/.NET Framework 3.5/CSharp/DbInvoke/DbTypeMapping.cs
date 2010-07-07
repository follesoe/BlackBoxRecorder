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
using System.Data;
using System.Data.SqlTypes;

namespace DbInvoke
{
    /// <summary>
    /// Represents a mapping between a CLR type and a DbType, and provides
    /// a default mapping method.
    /// </summary>
    internal class DbTypeMapping : IComparable<DbTypeMapping>
    {
        private readonly Type clrType;
        private readonly int preference;
        private readonly DbCompactType dbType;

        /// <summary>
        /// Magic value indicating that the size is free (unlimited).
        /// </summary>
        public const int FreeSize = -1;

        /// <summary>
        /// Magic value indicating that the size does not apply to
        /// the current <see cref="DbType"/>.
        /// </summary>
        public const int NoSize = 0;

        /// <summary>
        /// Gets the CLI <see cref="Type"/>.
        /// </summary>
        public Type ClrType { get { return this.clrType; } }

        /// <summary>
        /// Gets the <see cref="DbType"/> on which <see cref="Type"/> is mapped to.
        /// </summary>
        public DbCompactType DbCompactType { get { return this.dbType; } }

        /// <summary>
        /// Mappings between values of type <see cref="DbType"/> and instances of type
        /// <see cref="DbTypeMapping"/>.
        /// </summary>
        private static readonly Dictionary<Type, List<DbTypeMapping>> mappings =
            new Dictionary<Type, List<DbTypeMapping>>();

        /// <summary>
        /// Initializes the type <see cref="DbTypeMapping"/>.
        /// </summary>
        static DbTypeMapping()
        {
            DbTypeMapping[] array = new DbTypeMapping[]
                {
                    new DbTypeMapping( typeof(bool), new DbCompactType( DbType.Boolean, false ), 0 ),
                    new DbTypeMapping( typeof(byte), new DbCompactType( DbType.Byte, false ), 0 ),
                    new DbTypeMapping( typeof(char), new DbCompactType( DbType.AnsiStringFixedLength, 1, false ), 10 ),
                    new DbTypeMapping( typeof(char), new DbCompactType( DbType.StringFixedLength, 1, false ), 20 ),
                    new DbTypeMapping( typeof(decimal), new DbCompactType( DbType.Decimal, false ), 0 ),
                    new DbTypeMapping( typeof(double), new DbCompactType( DbType.Double, false ), 0 ),
                    new DbTypeMapping( typeof(float), new DbCompactType( DbType.Single, false ), 0 ),
                    new DbTypeMapping( typeof(int), new DbCompactType( DbType.Int32, false ), 0 ),
                    new DbTypeMapping( typeof(long), new DbCompactType( DbType.Int64, false ), 0 ),
                    new DbTypeMapping( typeof(sbyte), new DbCompactType( DbType.SByte, false ), 0 ),
                    new DbTypeMapping( typeof(short), new DbCompactType( DbType.Int16, false ), 0 ),
                    new DbTypeMapping( typeof(uint), new DbCompactType( DbType.UInt32, false ), 0 ),
                    new DbTypeMapping( typeof(ulong), new DbCompactType( DbType.UInt64, false ), 0 ),
                    new DbTypeMapping( typeof(ushort), new DbCompactType( DbType.UInt16, false ), 0 ),
                    new DbTypeMapping( typeof(string), new DbCompactType( DbType.AnsiString, FreeSize, true ), 40 ),
                    new DbTypeMapping( typeof(string), new DbCompactType( DbType.AnsiString, FreeSize, false ), 30 ),
                    new DbTypeMapping( typeof(string), new DbCompactType( DbType.AnsiStringFixedLength, FreeSize, true ),
                                       80 ),
                    new DbTypeMapping( typeof(string),
                                       new DbCompactType( DbType.AnsiStringFixedLength, FreeSize, false ), 70 ),
                    new DbTypeMapping( typeof(string), new DbCompactType( DbType.String, FreeSize, true ), 10 ),
                    new DbTypeMapping( typeof(string), new DbCompactType( DbType.String, FreeSize, false ), 20 ),
                    new DbTypeMapping( typeof(string), new DbCompactType( DbType.StringFixedLength, FreeSize, true ),
                                       100 ),
                    new DbTypeMapping( typeof(string), new DbCompactType( DbType.StringFixedLength, FreeSize, false ),
                                       90 ),
                    new DbTypeMapping( typeof(byte[]), new DbCompactType( DbType.Binary, true ), 0 ),
                    new DbTypeMapping( typeof(decimal), new DbCompactType( DbType.Currency, false ), 0 ),
                    new DbTypeMapping( typeof(DateTime), new DbCompactType( DbType.Date, false ), 20 ),
                    new DbTypeMapping( typeof(DateTime), new DbCompactType( DbType.DateTime, false ), 30 ),
                    new DbTypeMapping( typeof(DateTime), new DbCompactType( DbType.Time, false ), 10 ),
                    new DbTypeMapping( typeof(Guid), new DbCompactType( DbType.Guid, false ), 0 ),
                    new DbTypeMapping( typeof(SqlBinary), new DbCompactType( DbType.Binary, true ), 0 ),
                    new DbTypeMapping( typeof(SqlBoolean), new DbCompactType( DbType.Boolean, true ), 0 ),
                    new DbTypeMapping( typeof(SqlByte), new DbCompactType( DbType.Byte, true ), 0 ),
                    new DbTypeMapping( typeof(SqlDateTime), new DbCompactType( DbType.DateTime, true ), 30 ),
                    new DbTypeMapping( typeof(SqlDateTime), new DbCompactType( DbType.Date, true ), 20 ),
                    new DbTypeMapping( typeof(SqlDateTime), new DbCompactType( DbType.Time, true ), 10 ),
                    new DbTypeMapping( typeof(SqlDecimal), new DbCompactType( DbType.Decimal, true ), 0 ),
                    new DbTypeMapping( typeof(SqlDouble), new DbCompactType( DbType.Double, true ), 0 ),
                    new DbTypeMapping( typeof(SqlGuid), new DbCompactType( DbType.Guid, true ), 0 ),
                    new DbTypeMapping( typeof(SqlInt16), new DbCompactType( DbType.Int16, true ), 0 ),
                    new DbTypeMapping( typeof(SqlInt32), new DbCompactType( DbType.Int32, true ), 0 ),
                    new DbTypeMapping( typeof(SqlInt64), new DbCompactType( DbType.Int64, true ), 0 ),
                    new DbTypeMapping( typeof(SqlMoney), new DbCompactType( DbType.Currency, true ), 0 ),
                    new DbTypeMapping( typeof(SqlSingle), new DbCompactType( DbType.Single, true ), 0 ),
                    new DbTypeMapping( typeof(SqlString), new DbCompactType( DbType.AnsiString, true ), 40 ),
                    new DbTypeMapping( typeof(SqlString), new DbCompactType( DbType.AnsiString, false ), 30 ),
                    new DbTypeMapping( typeof(SqlString),
                                       new DbCompactType( DbType.AnsiStringFixedLength, FreeSize, true ), 80 ),
                    new DbTypeMapping( typeof(SqlString),
                                       new DbCompactType( DbType.AnsiStringFixedLength, FreeSize, false ), 70 ),
                    new DbTypeMapping( typeof(SqlString), new DbCompactType( DbType.String, true ), 60 ),
                    new DbTypeMapping( typeof(SqlString), new DbCompactType( DbType.String, false ), 50 ),
                    new DbTypeMapping( typeof(SqlString), new DbCompactType( DbType.StringFixedLength, FreeSize, true ),
                                       100 ),
                    new DbTypeMapping( typeof(SqlString), new DbCompactType( DbType.StringFixedLength, FreeSize, false ),
                                       90 )
                };

            Array.Sort( array );

            foreach ( DbTypeMapping mapping in array )
            {
                List<DbTypeMapping> collection;

                if ( !mappings.TryGetValue( mapping.clrType, out collection ) )
                {
                    collection = new List<DbTypeMapping>();
                    mappings.Add( mapping.clrType, collection );
                }

                collection.Add( mapping );
            }
        }

        /// <summary>
        /// Builds a new instance of the class <see cref="DbTypeMapping"/>.
        /// </summary>
        /// <param name="clrType">CLI type.</param>
        /// <param name="dbType">Database type.</param>
        /// <param name="preference">Preference (higher is better).</param>
        protected DbTypeMapping( Type clrType, DbCompactType dbType, int preference )
        {
            if ( clrType == null )
                throw new ArgumentNullException( "clrType" );

            this.clrType = clrType;
            this.dbType = dbType;
            this.preference = preference;
        }

        /// <summary>
        /// Gets a list  of <see cref="DbTypeMapping"/>
        /// associated to a specified CLI <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The CLI <see cref="Type"/> of which mappings are asked.</param>
        /// <returns>A list of <see cref="DbTypeMapping"/>
        /// associated to <paramref name="type"/>.</returns>
        public static IList<DbTypeMapping> GetMappings( Type type )
        {
            Type underlyingType = type.IsEnum ? type.UnderlyingSystemType : type;
            List<DbTypeMapping> result;
            mappings.TryGetValue( underlyingType, out result );
            return result;
        }

        /// <summary>
        /// Gets the preferred database mapping for a specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A CLI Type whose database type is asked.</param>
        /// <returns>The preferred <see cref="DbTypeMapping"/> corresponding 
        /// to <paramref name="type"/>, or <c>null</c> if no mapping is found.</returns>
        public static DbTypeMapping GetPreferredMapping( Type type )
        {
            IList<DbTypeMapping> types = GetMappings( type );
            if ( types == null )
                return null;
            else
            {
                return types[0];
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance. </param>
        /// <returns></returns>
        int IComparable<DbTypeMapping>.CompareTo( DbTypeMapping obj )
        {
            int c = this.clrType.FullName.CompareTo( obj.clrType.FullName );

            if ( c == 0 )
                return this.preference - obj.preference;
            else
                return c;
        }
    }
}