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

using System.Collections.Generic;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer
{
    internal class NamedDeclarationComparer : Comparer<NamedMetadataDeclaration>
    {
        public static readonly NamedDeclarationComparer Instance = new NamedDeclarationComparer();

        private NamedDeclarationComparer()
        {
        }

        public override int Compare(NamedMetadataDeclaration x, NamedMetadataDeclaration y)
        {
            return x.Name.CompareTo( y.Name );
        }
    }
}