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

using System.Text;
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;
using PostSharp.Sdk.CodeModel.Collections;

namespace AssemblyExplorer.TreeNodes
{
    internal class MethodSpecTreeNode : DeclarationTreeNode
    {
        public MethodSpecTreeNode( MethodSpecDeclaration method )
            : base( method, TreeViewImage.Method )
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append( '<' );

            bool first = true;
            foreach ( ITypeSignature type in method.GenericArguments )
            {
                if ( !first )
                {
                    stringBuilder.Append( ", " );
                }
                else
                {
                    first = true;
                }

                stringBuilder.Append( type.ToString() );
            }

            stringBuilder.Append( '>' );
            this.Text = stringBuilder.ToString();
        }
    }

    internal class MethodSpecCollectionTreeNode : BaseTreeNode
    {
        private readonly MethodSpecDeclarationCollection methodSpecs;

        public MethodSpecCollectionTreeNode( MethodSpecDeclarationCollection methodSpecs )
            :
                base( TreeViewImage.Folder, null )
        {
            this.methodSpecs = methodSpecs;
            this.Text = "Generic Instances";
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            foreach ( MethodSpecDeclaration methodSpec in methodSpecs )
            {
                this.Nodes.Add( new MethodSpecTreeNode( methodSpec ) );
            }
        }
    }
}