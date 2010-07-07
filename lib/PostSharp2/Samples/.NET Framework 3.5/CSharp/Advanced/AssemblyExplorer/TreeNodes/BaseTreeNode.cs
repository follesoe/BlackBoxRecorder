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
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Reflection;
using PostSharp.Sdk.CodeModel;


#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal abstract class BaseTreeNode : AutonomousTreeNode
    {
        protected BaseTreeNode( int imageIndex, object tag )
        {
            this.ImageIndex = imageIndex;
            this.SelectedImageIndex = imageIndex;
            this.Tag = tag;
        }

        protected BaseTreeNode( TreeViewImage image, object tag )
            : this( (int) image - 1, tag )
        {
        }

        protected BaseTreeNode( TreeViewImage imageKind, TreeViewImage imageModifier, object tag )
            : this( (int) imageKind - 1 + (int) imageModifier, tag )
        {
        }

        public virtual void OnPopulateDefinitionTree( TreeNodeCollection rootNodes )
        {
        }

        public virtual void OnPopulateContextMenu( ContextMenu contextMenu )
        {
            MetadataDeclaration declaration = this.Tag as MetadataDeclaration;
            if ( declaration != null && PostSharp.Sdk.Utilities.MsilWriterUtility.CanWriteMsil( declaration ))
            {
                contextMenu.MenuItems.Add(new MenuItem("Save MSIL...", this.OnContextMenuSaveMsilClicked));
            }
        }

        private void OnContextMenuSaveMsilClicked(object sender, EventArgs e)
         {
            ((MainForm) this.TreeView.FindForm()).SaveMsil((MetadataDeclaration) this.Tag);
         }

        protected internal override void OnNodeMouseClick( TreeNodeMouseClickEventArgs e )
        {
            base.OnNodeMouseClick( e );

            if ( e.Button == MouseButtons.Right )
            {
                ContextMenu contextMenu = new ContextMenu();
                this.OnPopulateContextMenu( contextMenu );
                if ( contextMenu.MenuItems.Count > 0 )
                {
                    contextMenu.Show( this.TreeView, e.Location );
                }
            }
        }


        protected internal override void OnAfterSelect( TreeViewEventArgs e )
        {
            if ( this.Tag != null )
            {
                ( (MainForm) this.TreeView.FindForm() ).ChangeCurrentObject( this.Tag );
            }
        }
    }

    internal abstract class DeclarationTreeNode : BaseTreeNode
    {
        protected DeclarationTreeNode( Declaration declaration, TreeViewImage image ) :
            base( image, declaration )
        {
        }

        protected DeclarationTreeNode( Declaration declaration, TreeViewImage imageKind, TreeViewImage imageModifier )
            :
                base( imageKind, imageModifier, declaration )
        {
        }

        protected DeclarationTreeNode( Declaration declaration, TreeViewImage imageKind, Visibility visibility )
            :
                base( imageKind, GetImageModifier( visibility ), declaration )
        {
            switch ( visibility )
            {
                case Visibility.Assembly:
                case Visibility.FamilyAndAssembly:
                case Visibility.Private:
                    this.ForeColor = SystemColors.GrayText;
                    break;
            }
        }


        public override void OnPopulateDefinitionTree( TreeNodeCollection rootNodes )
        {
            MetadataDeclaration target = this.Declaration as MetadataDeclaration;
            if ( target != null )
            {
                foreach ( CustomAttributeDeclaration customAttribute in target.CustomAttributes )
                {
                    rootNodes.Add( new CustomAttributeTreeNode( customAttribute ) );
                }
            }
        }

        protected static TreeViewImage GetImageModifier( Visibility visibility )
        {
            switch ( visibility )
            {
                case Visibility.Assembly:
                    return TreeViewImage.Internal;

                case Visibility.Family:
                    return TreeViewImage.Protected;

                case Visibility.FamilyAndAssembly:
                    return TreeViewImage.Mixed;

                case Visibility.FamilyOrAssembly:
                    return TreeViewImage.Mixed;

                case Visibility.Private:
                    return TreeViewImage.Private;

                default:
                    return TreeViewImage.Public;
            }
        }

        public Declaration Declaration { get { return (Declaration) this.Tag; } }

        protected internal override void OnNodeMouseDoubleClick( TreeNodeMouseClickEventArgs e )
        {
        }
    }
}