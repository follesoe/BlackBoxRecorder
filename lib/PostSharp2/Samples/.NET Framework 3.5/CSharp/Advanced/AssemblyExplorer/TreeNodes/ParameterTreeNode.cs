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
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class ParameterTreeNode : DeclarationTreeNode
    {
        public ParameterTreeNode( ParameterDeclaration parameter ) : base( parameter, TreeViewImage.Parameter )
        {
            this.Text = ( ( parameter.Attributes & ParameterAttributes.Retval ) == 0 ? parameter.Name : "return" ) +
                        " : " + parameter.ParameterType.ToString();
        }
    }

    internal class ParameterCollectionTreeNode : BaseTreeNode
    {
        private readonly MethodDefDeclaration declaration;

        public ParameterCollectionTreeNode( MethodDefDeclaration declaration ) : base( TreeViewImage.Folder, null )
        {
            this.declaration = declaration;
            this.Text = "Parameters";
            this.EnableLatePopulate();
        }

        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            foreach ( ParameterDeclaration parameter in this.declaration.Parameters )
            {
                this.Nodes.Add( new ParameterTreeNode( parameter ) );
            }

            this.Nodes.Add( new ParameterTreeNode( this.declaration.ReturnParameter ) );
        }
    }
}