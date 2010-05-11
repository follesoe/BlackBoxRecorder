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

using System.Data;

namespace DbInvoke
{
    /// <summary>
    /// Contains all characteristics of a database type.
    /// </summary>
    internal struct DbCompactType
    {
        public DbType DbType;
        public byte Precision;
        public byte Scale;
        public int Size;
        public bool IsNullable;

        public DbCompactType( DbType dbType, int size, byte precision, byte scale, bool isNullable )
        {
            this.DbType = dbType;
            this.Precision = precision;
            this.Scale = scale;
            this.Size = size;
            this.IsNullable = isNullable;
        }

        public DbCompactType( DbType dbType, bool isNullable )
        {
            this.DbType = dbType;
            this.Size = 0;
            this.Scale = 0;
            this.Precision = 0;
            this.IsNullable = isNullable;
        }

        public DbCompactType( DbType dbType, int size, bool isNullable )
        {
            this.DbType = dbType;
            this.Size = size;
            this.Scale = 0;
            this.Precision = 0;
            this.IsNullable = isNullable;
        }
    }
}