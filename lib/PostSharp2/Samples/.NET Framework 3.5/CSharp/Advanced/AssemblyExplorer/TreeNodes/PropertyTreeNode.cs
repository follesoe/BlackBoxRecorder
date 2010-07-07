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
    internal class PropertyTreeNode : DeclarationTreeNode
    {
        private readonly PropertyDeclaration property;

        public PropertyTreeNode( PropertyDeclaration property )
            : base( property, TreeViewImage.Property, property.Visibility )
        {
            this.property = property;


            StringBuilder name = new StringBuilder( 255 );
            this.property = property;
            name.Append( property.Name );

            bool first = true;
            if ( property.Parameters.Count > 0 )
            {
                name.Append( "(" );
                foreach ( ITypeSignature parameter in property.Parameters )
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

                    name.Append( ' ' );
                    name.Append( parameter.ToString() );
                }
                name.Append( " )" );
            }

            name.Append( " : " );

            name.Append( property.PropertyType.ToString() );

            this.Text = name.ToString();

            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            base.OnPopulate( e );


            foreach ( MethodSemanticDeclaration method in property.Members )
            {
                this.Nodes.Add( new MethodTreeNode( method.Method ) );
            }
        }
    }
}