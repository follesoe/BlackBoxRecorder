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
using System.Text;
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class ExternalMethodTreeNode : DeclarationTreeNode
    {
        private readonly MethodRefDeclaration method;

        public ExternalMethodTreeNode( MethodRefDeclaration externalMethod, bool red )
            : base( externalMethod, TreeViewImage.Method, TreeViewImage.Reference )
        {
            this.method = externalMethod;

            StringBuilder name = new StringBuilder( 255 );
            name.Append( method.Name );

            bool first = true;
            if ( method.Signature.ParameterTypes.Count > 0 )
            {
                name.Append( "(" );
                for ( int i = 0 ; i < method.Signature.ParameterTypes.Count ; i++ )
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

                    name.Append( method.Signature.ParameterTypes[i].ToString() );
                }
                name.Append( " )" );
            }

            name.Append( " : " );
            name.Append( method.Signature.ReturnType.ToString() );

            this.Text = name.ToString();

            if ( red )
                this.ForeColor = Color.Red;

            this.EnableLatePopulate();
        }

        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            if ( method.MethodSpecs.Count > 0 )
            {
                this.Nodes.Add( new MethodSpecCollectionTreeNode( method.MethodSpecs ) );
            }
        }
    }
}
