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
    internal class InterfaceImplementationTreeNode : BaseTreeNode
    {
        public InterfaceImplementationTreeNode(ITypeSignature type)
            :
            base( TreeViewImage.Interface, type )
        {
            this.Text = type.ToString();
        }
    }

    internal class InterfaceImplementationCollectionTreeNode : BaseTreeNode
    {
        private readonly TypeDefDeclaration type;

        public InterfaceImplementationCollectionTreeNode( TypeDefDeclaration type ) :
            base( TreeViewImage.Folder, null )
        {
            this.type = type;
            this.Text = "Interfaces";
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            foreach ( InterfaceImplementationDeclaration interfaceImplementation in type.InterfaceImplementations )
            {
                this.Nodes.Add( new InterfaceImplementationTreeNode( interfaceImplementation.ImplementedInterface ) );
            }
        }
    }
}