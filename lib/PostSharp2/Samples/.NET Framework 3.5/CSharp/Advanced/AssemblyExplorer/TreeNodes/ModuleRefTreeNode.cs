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

namespace AssemblyExplorer.TreeNodes
{
    class ModuleRefTreeNode : DeclarationTreeNode
    {
        private ModuleRefDeclaration moduleRef;

        public ModuleRefTreeNode(ModuleRefDeclaration moduleRef)
            : base(moduleRef, TreeViewImage.Module, TreeViewImage.Reference)
        {
            this.moduleRef = moduleRef;
            this.Text = moduleRef.Name;
            this.EnableLatePopulate();
        }

        protected internal override void OnPopulate(System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            foreach ( TypeRefDeclaration typeRef in this.moduleRef.TypeRefs )
            {
                this.Nodes.Add( new ExternalTypeTreeNode( typeRef ) );
                
            }
        }
    }

 
}
