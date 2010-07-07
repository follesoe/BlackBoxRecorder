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

using System.Drawing;
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class ExternalFieldTreeNode : DeclarationTreeNode
    {
        public ExternalFieldTreeNode( FieldRefDeclaration externalField, bool red )
            : base( externalField, TreeViewImage.Field, TreeViewImage.Reference )
        {
            this.Text = externalField.Name;

            if (red)
                this.ForeColor = Color.Red;

        }

        public static void PopulateExternalMembers( IMemberRefResolutionScope decl, TreeNodeCollection nodes, bool red )
        {
            foreach ( MethodRefDeclaration externalMethod in decl.MethodRefs )
            {
                nodes.Add( new ExternalMethodTreeNode( externalMethod, red ) );
            }

            foreach ( FieldRefDeclaration externalField in decl.FieldRefs )
            {
                nodes.Add( new ExternalFieldTreeNode( externalField, red ) );
            }
        }
    }
}
