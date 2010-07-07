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

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class MethodTreeNode : DeclarationTreeNode
    {
        private readonly MethodDefDeclaration method;

        public MethodTreeNode( MethodDefDeclaration method ) :
            base( method, TreeViewImage.Method, method.Visibility )
        {
            this.method = method;

            StringBuilder name = new StringBuilder( 255 );
            this.method = method;
            name.Append( method.Name );

            bool first = true;
            if ( method.Parameters.Count > 0 )
            {
                name.Append( "(" );
                foreach ( ParameterDeclaration parameter in method.Parameters )
                {
                    if ( !first )
                    {
                        name.Append( ", " );
                    }
                    else
                    {
                        name.Append( ' ' );
                        first = false;
                    }

                    name.Append( parameter.ParameterType.ToString() );
                    name.Append( ' ' );
                    name.Append( parameter.Name );
                }
                name.Append( " )" );
            }

            name.Append( " : " );
            name.Append( method.ReturnParameter.ParameterType.ToString() );

            this.Text = name.ToString();

            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            this.Nodes.Add( new ParameterCollectionTreeNode( this.method ) );

            if ( method.HasBody )
            {
                this.Nodes.Add( new InstructionBlockTreeNode( method.MethodBody.RootInstructionBlock, "Root" ) );
                //this.Nodes.Add(new InstructionSequenceFolderTreeNode(method.MethodBody));
            }

            if ( method.MethodSpecs.Count > 0 )
            {
                this.Nodes.Add( new MethodSpecCollectionTreeNode( method.MethodSpecs ) );
            }
        }
    }
}