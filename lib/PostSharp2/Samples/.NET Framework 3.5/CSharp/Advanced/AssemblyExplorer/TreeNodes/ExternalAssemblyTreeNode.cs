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
    internal class ExternalAssemblyTreeNode : DeclarationTreeNode
    {
        public ExternalAssemblyTreeNode( AssemblyRefDeclaration externalAssembly ) : base( externalAssembly,
                                                                                           TreeViewImage.Assembly,
                                                                                           TreeViewImage.Reference )
        {
            this.Text = externalAssembly.ToString();
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            this.Nodes.Add(
                new ExternalTypeFolderTreeNode( (AssemblyRefDeclaration) this.Declaration, "Type References" ) );
        }
    }

    internal class ExternalAssemblyFolderTreeNode : BaseTreeNode
    {
        private readonly ModuleDeclaration module;

        public ExternalAssemblyFolderTreeNode( ModuleDeclaration module ) : base( TreeViewImage.Folder, null )
        {
            this.module = module;
            this.Text = "External Assemblies";
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            foreach ( AssemblyRefDeclaration externalAssembly in this.module.AssemblyRefs )
            {
                this.Nodes.Add( new ExternalAssemblyTreeNode( externalAssembly ) );
            }

            foreach (ModuleRefDeclaration moduleRef in this.module.ModuleRefs)
            {
                this.Nodes.Add(new ModuleRefTreeNode(moduleRef));
            }

        }
    }
}