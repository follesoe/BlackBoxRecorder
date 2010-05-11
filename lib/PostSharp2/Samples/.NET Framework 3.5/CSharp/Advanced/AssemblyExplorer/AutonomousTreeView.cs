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
    internal class AutonomousTreeView : TreeView
    {
        public AutonomousTreeView()
        {
        }


        protected override void OnAfterCheck( TreeViewEventArgs e )
        {
            base.OnAfterCheck( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnAfterCheck( e );
            }
        }


        protected override void OnAfterCollapse( TreeViewEventArgs e )
        {
            base.OnAfterCollapse( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnAfterCollapse( e );
            }
        }


        protected override void OnAfterExpand( TreeViewEventArgs e )
        {
            base.OnAfterExpand( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnAfterExpand( e );
            }
        }


        protected override void OnAfterLabelEdit( NodeLabelEditEventArgs e )
        {
            base.OnAfterLabelEdit( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnAfterLabelEdit( e );
            }
        }


        protected override void OnAfterSelect( TreeViewEventArgs e )
        {
            base.OnAfterSelect( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnAfterSelect( e );
            }
        }


        protected override void OnBeforeCheck( TreeViewCancelEventArgs e )
        {
            base.OnBeforeCheck( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnBeforeCheck( e );
            }
        }


        protected override void OnBeforeCollapse( TreeViewCancelEventArgs e )
        {
            base.OnBeforeCollapse( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnBeforeCollapse( e );
            }
        }


        protected override void OnBeforeExpand( TreeViewCancelEventArgs e )
        {
            base.OnBeforeExpand( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnBeforeExpand( e );
            }
        }


        protected override void OnBeforeLabelEdit( NodeLabelEditEventArgs e )
        {
            base.OnBeforeLabelEdit( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnBeforeLabelEdit( e );
            }
        }


        protected override void OnBeforeSelect( TreeViewCancelEventArgs e )
        {
            base.OnBeforeSelect( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnBeforeSelect( e );
            }
        }


        protected override void OnNodeMouseClick( TreeNodeMouseClickEventArgs e )
        {
            base.OnNodeMouseClick( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnNodeMouseClick( e );
            }
        }


        protected override void OnNodeMouseDoubleClick( TreeNodeMouseClickEventArgs e )
        {
            base.OnNodeMouseDoubleClick( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnNodeMouseDoubleClick( e );
            }
        }


        protected override void OnNodeMouseHover( TreeNodeMouseHoverEventArgs e )
        {
            base.OnNodeMouseHover( e );
            AutonomousTreeNode node = e.Node as AutonomousTreeNode;
            if ( node != null )
            {
                node.OnNodeMouseHover( e );
            }
        }
    }
}