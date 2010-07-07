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

using System.Text;
using PostSharp.Sdk.CodeModel;
using PostSharp.Sdk.CodeModel.Helpers;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class CustomAttributeTreeNode : DeclarationTreeNode
    {
        public CustomAttributeTreeNode( CustomAttributeDeclaration customAttribute )
            : base( customAttribute, TreeViewImage.CustomAttribute )
        {
            StringBuilder name = new StringBuilder( 256 );
            name.Append( CustomAttributeHelper.Render( customAttribute ) );

            this.Text = name.ToString();
        }
    }
}