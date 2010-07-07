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

using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class ExternalTypeTreeNode : DeclarationTreeNode
    {
        public ExternalTypeTreeNode( TypeRefDeclaration externalType )
            : base( externalType, TreeViewImage.Class, TreeViewImage.Reference )
        {
            this.Text = externalType.Name;
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            TypeRefDeclaration externalType = this.Declaration as TypeRefDeclaration;

            if ( externalType.TypeRefs.Count > 0 )
            {
                this.Nodes.Add( new ExternalTypeFolderTreeNode( externalType, "Nested Types" ) );
            }

            ExternalFieldTreeNode.PopulateExternalMembers( externalType, this.Nodes, false );
        }
    }

    internal class ExternalTypeFolderTreeNode : BaseTreeNode
    {
        private readonly ITypeRefResolutionScope decl;

        public ExternalTypeFolderTreeNode( ITypeRefResolutionScope decl, string name )
            : base( TreeViewImage.Folder, null )
        {
            this.decl = decl;
            this.Text = name;
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            foreach ( TypeRefDeclaration externalType in this.decl.TypeRefs )
            {
                this.Nodes.Add( new ExternalTypeTreeNode( externalType ) );
            }
        }
    }
}
