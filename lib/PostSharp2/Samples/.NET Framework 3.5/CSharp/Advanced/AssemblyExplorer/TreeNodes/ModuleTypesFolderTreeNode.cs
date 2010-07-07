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


using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

namespace AssemblyExplorer.TreeNodes
{
    internal class ModuleTypesFolderTreeNode : DeclarationTreeNode
    {
        private readonly ModuleDeclaration module;

        public ModuleTypesFolderTreeNode( ModuleDeclaration module )
            : base( module, TreeViewImage.Folder )
        {
            this.Text = "Types";
            this.module = module;

            this.EnableLatePopulate();
        }

        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            // Populate the namespaces
            TypeDefDeclaration[] types = ArrayHelper.ToArray<TypeDefDeclaration>( this.module.Types );
            Array.Sort( types, TypeNameComparer.Instance );

            string oldNamespace = null;
            NamespaceTreeNode nsTreeNode = null;

            foreach ( TypeDefDeclaration type in types )
            {
                string ns;
                string typeName;
                SplitNamespace( type.Name, out typeName, out ns );

                if ( ns.Length > 0 )
                {
                    if ( ns != oldNamespace )
                    {
                        oldNamespace = ns;
                        nsTreeNode = new NamespaceTreeNode( ns );
                        this.Nodes.Add( nsTreeNode );
                    }

                    nsTreeNode.Nodes.Add( new TypeTreeNode( type, typeName ) );
                }
                else
                {
                    this.Nodes.Add( new TypeTreeNode( type, typeName ) );
                }
            }
        }


        private static void SplitNamespace( string typeName, out string rawTypeName, out string ns )
        {
            int separator = typeName.LastIndexOf( '.' );
            if ( separator > 0 )
            {
                rawTypeName = typeName.Substring( separator + 1 );
                ns = typeName.Substring( 0, separator );
            }
            else
            {
                rawTypeName = typeName;
                ns = "";
            }
        }

        private class TypeNameComparer : IComparer<TypeDefDeclaration>
        {
            public static readonly TypeNameComparer Instance = new TypeNameComparer();

            private TypeNameComparer()
            {
            }

            public int Compare( TypeDefDeclaration x, TypeDefDeclaration y )
            {
                string xNs, xName, yNs, yName;
                SplitNamespace( x.Name, out xName, out xNs );
                SplitNamespace( y.Name, out yName, out yNs );

                int compNs = xNs.CompareTo( yNs );

                if ( compNs == 0 )
                {
                    return xName.CompareTo( yName );
                }
                else
                    return compNs;
            }
        }
    }
}