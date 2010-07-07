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

using Librarian.Framework;

namespace Librarian.Data
{
    /// <summary>
    /// Encapsulates an operation done in a <see cref="StorageContext"/>. Used to implement
    /// transaction logs.
    /// </summary>
    internal class StorageOperation
    {
        private readonly StorageOperationKind operationKind;
        private EntityRef<BaseEntity> entity;

        public StorageOperation(StorageOperationKind operationKind, EntityRef<BaseEntity> entity)
        {
            this.operationKind = operationKind;
            this.entity = entity;
        }

        public StorageOperationKind OperationKind { get { return operationKind; } }

        public EntityRef<BaseEntity> Entity { get { return entity; } }

        public override string ToString()
        {
            return "StorageOperation " + this.operationKind.ToString() + " " + this.entity.ToString();
        }
    }

    internal enum StorageOperationKind
    {
        Update,
        Insert,
        Delete
    }
}