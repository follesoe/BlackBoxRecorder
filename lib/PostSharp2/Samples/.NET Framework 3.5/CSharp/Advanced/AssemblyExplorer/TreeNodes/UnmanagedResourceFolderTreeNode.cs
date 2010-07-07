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
using System.Text;
using PostSharp.Sdk.CodeModel;
using PostSharp.Sdk.CodeModel.Collections;

namespace AssemblyExplorer.TreeNodes
{
    class UnmanagedResourceFolderTreeNode : BaseTreeNode
    {
        private readonly UnmanagedResourceCollection resources;
        public UnmanagedResourceFolderTreeNode( UnmanagedResourceCollection resources)
            : base(TreeViewImage.Folder, resources)
        {
            this.EnableLatePopulate();
            this.resources = resources;
            this.Text = "Unmanaged resources";
        }

        protected internal override void OnPopulate(System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            foreach ( UnmanagedResource resource in resources )
            {
                this.Nodes.Add( new UnmanagedResourceTreeNode( resource ) );
            }
            
        }
    }
}
