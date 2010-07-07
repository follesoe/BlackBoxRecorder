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
    internal class ExternalFoldersTreeNode : BaseTreeNode
    {
        private readonly ModuleDeclaration module;

        public ExternalFoldersTreeNode( ModuleDeclaration module )
            : base( TreeViewImage.Folder, null )
        {
            this.module = module;
            this.Text = "References";
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            if ( this.module.AssemblyRefs.Count > 0 )
            {
                this.Nodes.Add( new ExternalAssemblyFolderTreeNode( this.module ) );
            }
            if ( this.module.TypeRefs.Count > 0 )
            {
                this.Nodes.Add( new ExternalTypeFolderTreeNode( this.module, "Module Type References" ) );
            }
            if ( this.module.TypeSpecs.Count > 0 )
            {
                this.Nodes.Add( new TypeSpecificationFolderTreeNode( this.module ) );
            }
           
        }
    }
}
