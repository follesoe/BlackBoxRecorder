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

#endregion

namespace AssemblyExplorer.TreeNodes
{
    [Flags]
    public enum TreeViewImage
    {
        // Elements
        Class = 1,
        Const = 7,
        Delegate = 13,
        Enum = 19,
        EnumValue = 25,
        Event = 31,
        Event2 = 37,
        Field = 43,
        Interface = 49,
        Code = 55,
        Code1 = 61,
        Code2 = 67,
        Method = 73,
        Method2 = 79,
        Assembly = 85,
        Namespace = 91,
        Operator = 97,
        Property = 103,
        Struct = 109,
        CustomAttribute = 115,
        Folder = 121,
        Module = 122,
        Parameter = 123,

        // Visibilities
        Public = 0,
        Internal = 1,
        Mixed = 2,
        Protected = 3,
        Private = 4,
        Reference = 5
    }
}