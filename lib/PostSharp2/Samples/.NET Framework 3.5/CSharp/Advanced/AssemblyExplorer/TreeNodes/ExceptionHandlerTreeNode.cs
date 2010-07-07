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

using System.Reflection;
using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class ExceptionHandlerTreeNode : BaseTreeNode
    {
        private readonly ExceptionHandler handler;

        public ExceptionHandlerTreeNode( ExceptionHandler handler ) :
            base( TreeViewImage.Method, handler )
        {
            this.handler = handler;
            this.Text = handler.Options.ToString();
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            this.Nodes.Add( new InstructionBlockTreeNode( this.handler.HandlerBlock, "handler" ) );

            if ( this.handler.Options == ExceptionHandlingClauseOptions.Filter )
            {
                this.Nodes.Add( new InstructionBlockTreeNode( this.handler.FilterBlock, "filter" ) );
            }

            base.OnPopulate( e );
        }
    }
}