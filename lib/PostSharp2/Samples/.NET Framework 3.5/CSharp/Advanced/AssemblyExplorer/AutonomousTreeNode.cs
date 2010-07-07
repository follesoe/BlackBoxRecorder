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

#endregion

namespace AssemblyExplorer
{
    internal class AutonomousTreeNode : TreeNode
    {
        private bool needsPopulate = false;


        protected void EnableLatePopulate()
        {
            if ( !this.needsPopulate )
            {
                this.needsPopulate = true;
                this.Nodes.Add( "dummy" );
            }
        }

        protected internal virtual void OnBeforeExpand( TreeViewCancelEventArgs e )
        {
            if ( !e.Cancel )
            {
                if ( needsPopulate )
                {
                    this.TreeView.BeginUpdate();
                    try
                    {
                        this.Nodes.Clear();
                        this.OnPopulate( e );
                        if ( !e.Cancel )
                        {
                            this.needsPopulate = false;
                        }
                        else
                        {
                            this.Nodes.Add( "dummy" );
                        }
                    }
                    finally
                    {
                        this.TreeView.EndUpdate();
                    }
                }
            }
        }

        protected internal virtual void OnBeforeCollapse( TreeViewCancelEventArgs e )
        {
        }

        protected internal virtual void OnBeforeCheck( TreeViewCancelEventArgs e )
        {
        }

        protected internal virtual void OnBeforeLabelEdit( NodeLabelEditEventArgs e )
        {
        }

        protected internal virtual void OnBeforeSelect( TreeViewCancelEventArgs e )
        {
        }

        protected internal virtual void OnAfterCollapse( TreeViewEventArgs e )
        {
        }

        protected internal virtual void OnAfterExpand( TreeViewEventArgs e )
        {
        }

        protected internal virtual void OnAfterCheck( TreeViewEventArgs e )
        {
        }

        protected internal virtual void OnAfterLabelEdit( NodeLabelEditEventArgs e )
        {
        }

        protected internal virtual void OnAfterSelect( TreeViewEventArgs e )
        {
        }

        protected internal virtual void OnDrawNode( DrawTreeNodeEventArgs e )
        {
        }

        protected internal virtual void OnItemDrag( ItemDragEventArgs e )
        {
        }

        protected internal virtual void OnNodeMouseClick( TreeNodeMouseClickEventArgs e )
        {
        }

        protected internal virtual void OnNodeMouseDoubleClick( TreeNodeMouseClickEventArgs e )
        {
        }

        protected internal virtual void OnNodeMouseHover( TreeNodeMouseHoverEventArgs e )
        {
        }

        protected internal virtual void OnPopulate( TreeViewCancelEventArgs e )
        {
        }

        public AutonomousTreeNode()
        {
        }

        public AutonomousTreeNode( bool latePopulate )
        {
            if ( latePopulate )
                this.EnableLatePopulate();
        }
    }
}