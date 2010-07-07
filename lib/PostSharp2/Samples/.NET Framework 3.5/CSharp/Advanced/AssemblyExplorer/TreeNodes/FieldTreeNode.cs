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

using System.Reflection;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class FieldTreeNode : DeclarationTreeNode
    {
        public FieldTreeNode( FieldDefDeclaration field ) :
            base( field, GetImageKind( field ), field.Visibility )
        {
            this.Text = field.Name + " : " + field.FieldType.ToString();
        }

        private static TreeViewImage GetImageKind( FieldDefDeclaration field )
        {
            if ( ( field.Attributes & FieldAttributes.Literal ) != 0 )
            {
                if ( field.DeclaringType.BelongsToClassification( TypeClassifications.Enum ) )
                {
                    return TreeViewImage.EnumValue;
                }
                else
                {
                    return TreeViewImage.Const;
                }
            }
            else
            {
                return TreeViewImage.Field;
            }
        }
    }
}