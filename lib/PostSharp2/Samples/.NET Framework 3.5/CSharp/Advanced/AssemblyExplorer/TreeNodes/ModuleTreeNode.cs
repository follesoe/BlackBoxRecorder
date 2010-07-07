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

using System;
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class ModuleTreeNode : DeclarationTreeNode
    {
        private readonly ModuleDeclaration module;
        private readonly string path;

        public ModuleTreeNode( ModuleDeclaration module, string path ) : base( module, TreeViewImage.Module )
        {
            this.Text = module.Name;
            this.module = module;
            this.path = path;

            this.EnableLatePopulate();
        }

        public string ModulePath { get { return this.path; } }

        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            if ( this.module.AssemblyManifest != null )
            {
                this.Nodes.Add( new AssemblyTreeNode( this.module.AssemblyManifest ) );
            }

            if ( this.module.AssemblyRefs.Count > 0 ||
                 this.module.ModuleRefs.Count > 0 ||
                 this.module.TypeSpecs.Count > 0 )
            {
                this.Nodes.Add( new ExternalFoldersTreeNode( this.module ) );
            }

            if ( this.module.TypeRefs.Count > 0 )
            {
                this.Nodes.Add( new ExternalTypeFolderTreeNode( this.module, "External Types" ) );
            }

            if ( this.module.Types.Count > 0 )
            {
                this.Nodes.Add( new ModuleTypesFolderTreeNode( this.module ) );
            }

            if ( this.module.UnmanagedResources.Count > 0 )
            {
                this.Nodes.Add( new UnmanagedResourceFolderTreeNode( this.module.UnmanagedResources ) );
            }
        }

        private void OnContextMenuCloseClicked( object sender, EventArgs e )
        {
            ( (MainForm) this.TreeView.FindForm() ).RemoveModule( this );
        }

        public override void OnPopulateContextMenu( ContextMenu contextMenu )
        {
            contextMenu.MenuItems.Add( new MenuItem( "Remove", this.OnContextMenuCloseClicked ) );
            base.OnPopulateContextMenu(contextMenu);

            base.OnPopulateContextMenu(contextMenu);

        }
    }
}
