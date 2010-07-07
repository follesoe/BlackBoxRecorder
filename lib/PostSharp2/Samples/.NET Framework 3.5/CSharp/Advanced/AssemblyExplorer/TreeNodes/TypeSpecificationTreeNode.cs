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
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class TypeSpecificationTreeNode : DeclarationTreeNode
    {
        private readonly TypeSpecDeclaration typeSpec;

        public TypeSpecificationTreeNode( TypeSpecDeclaration typeSpec ) :
            base( typeSpec, TreeViewImage.Class )
        {
            this.typeSpec = typeSpec;
            this.Text = typeSpec.ToString();
            this.EnableLatePopulate();
        }

        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            ExternalFieldTreeNode.PopulateExternalMembers( this.typeSpec, this.Nodes, false );
        }
    }

    internal class TypeSpecificationFolderTreeNode : BaseTreeNode
    {
        private readonly ModuleDeclaration module;

        public TypeSpecificationFolderTreeNode( ModuleDeclaration module )
            : base( TreeViewImage.Folder, null )
        {
            this.module = module;
            this.Text = "Type Constructions";
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            List<TypeSpecDeclaration> sortedTypeSpecs = new List<TypeSpecDeclaration>( this.module.TypeSpecs );
            sortedTypeSpecs.Sort(
                delegate( TypeSpecDeclaration x, TypeSpecDeclaration y ) { return string.Compare( x.ToString(), y.ToString(), StringComparison.InvariantCultureIgnoreCase ); } );

            foreach ( TypeSpecDeclaration entry in sortedTypeSpecs )
            {
                this.Nodes.Add( new TypeSpecificationTreeNode( entry ) );
            }
        }
    }
}
