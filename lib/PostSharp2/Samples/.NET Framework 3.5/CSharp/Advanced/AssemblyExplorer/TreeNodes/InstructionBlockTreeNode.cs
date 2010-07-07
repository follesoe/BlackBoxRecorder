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

using System.Collections.Generic;
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class InstructionBlockTreeNode : BaseTreeNode
    {
        private readonly InstructionBlock block;

        public InstructionBlockTreeNode( InstructionBlock block, string name )
            : base( TreeViewImage.Namespace, block )
        {
            this.Text = name + " " + block.ToString();
            this.block = block;

            if ( this.block.HasChildrenBlocks || this.block.HasExceptionHandlers || this.block.HasLocalVariableSymbols )
            {
                this.EnableLatePopulate();
            }
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            if ( this.block.HasChildrenBlocks )
            {
                IEnumerator<InstructionBlock> children = this.block.GetChildrenEnumerator();
                while ( children.MoveNext() )
                {
                    this.Nodes.Add(
                        new InstructionBlockTreeNode( children.Current,
                                                      children.Current.HasExceptionHandlers
                                                          ? "protected block"
                                                          : "block" ) );
                }
            }

            if ( this.block.HasExceptionHandlers )
            {
                ExceptionHandler handler = this.block.FirstExceptionHandler;
                while ( handler != null )
                {
                    this.Nodes.Add( new ExceptionHandlerTreeNode( handler ) );
                    handler = handler.NextSiblingExceptionHandler;
                }
            }


            if ( this.block.HasLocalVariableSymbols )
            {
                this.Nodes.Add( new LocaVariableSymbolCollectionTreeNode( this.block ) );
            }
        }
    }
}