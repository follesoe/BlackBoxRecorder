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
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;
using PostSharp.Sdk.CodeModel.Collections;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class GenericParameterTreeNode : DeclarationTreeNode
    {
        public GenericParameterTreeNode( GenericParameterDeclaration genericParameter )
            : base( genericParameter, TreeViewImage.Class )
        {
            StringBuilder text = new StringBuilder();
            text.Append( genericParameter.Name );
            if ( genericParameter.Constraints.Count > 0 )
            {
                text.Append( " : " );
                for ( int i = 0 ; i < genericParameter.Constraints.Count ; i++ )
                {
                    if ( i > 0 )
                    {
                        text.Append( ", " );
                    }
                    text.Append( genericParameter.Constraints[i].ToString() );
                }
            }
            this.Text = text.ToString();
        }
    }

    internal class GenericParameterCollectionTreeNode : BaseTreeNode
    {
        private readonly GenericParameterDeclarationCollection genericParameters;

        public GenericParameterCollectionTreeNode( GenericParameterDeclarationCollection genericParameters )
            : base( TreeViewImage.Folder, null )
        {
            this.Text = "Generic parameters";
            this.genericParameters = genericParameters;
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            foreach ( GenericParameterDeclaration genericParameter in genericParameters )
            {
                this.Nodes.Add( new GenericParameterTreeNode( genericParameter ) );
            }
        }
    }
}