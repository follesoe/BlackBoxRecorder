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
    internal class EventTreeNode : DeclarationTreeNode
    {
        private readonly EventDeclaration @event;

        public EventTreeNode( EventDeclaration @event ) : base( @event, TreeViewImage.Event, @event.Visibility )
        {
            this.@event = @event;
            this.Text = @event.Name + " : " + @event.EventType.ToString();
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            foreach ( MethodSemanticDeclaration method in @event.Members )
            {
                this.Nodes.Add( new MethodTreeNode( method.Method ) );
            }
        }
    }
}