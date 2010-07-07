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

#region Using directives

using System;
using System.Collections.Generic;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer
{
    public static class ArrayHelper
    {
        public static T[] ToArray<T>( ICollection<T> collection )
            where T : class
        {
            int i = 0;
            T[] array = new T[collection.Count];
            foreach ( T item in collection )
            {
                array[i] = item;
                i++;
            }

            return array;
        }

        public static T[] ToSortedArray<T>( ICollection<T> collection )
            where T : Declaration
        {
            T[] array = ToArray<T>( collection );
            Array.Sort( array, NamedDeclarationComparer.Instance );
            return array;
        }
    }
}