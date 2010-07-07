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
    internal class TypeTreeNode : DeclarationTreeNode
    {
        private readonly TypeDefDeclaration type;

        public TypeTreeNode( TypeDefDeclaration type, string name ) : base(
            type, GetImage( type ), type.Visibility )
        {
            this.type = type;
            this.Text = name;
            this.EnableLatePopulate();
        }

        private static TreeViewImage GetImage( TypeDefDeclaration type )
        {
            if ( type.BelongsToClassification( TypeClassifications.Class ) )
            {
                return TreeViewImage.Class;
            }
            else if ( type.BelongsToClassification( TypeClassifications.Interface ) )
            {
                return TreeViewImage.Interface;
            }
            else if ( type.BelongsToClassification( TypeClassifications.Struct ) )
            {
                return TreeViewImage.Struct;
            }
            else if ( type.BelongsToClassification( TypeClassifications.Enum ) )
            {
                return TreeViewImage.Enum;
            }
            else
            {
                return TreeViewImage.Class;
            }
        }

        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            if ( type.GenericParameters.Count > 0 )
            {
                this.Nodes.Add( new GenericParameterCollectionTreeNode( this.type.GenericParameters ) );
            }

            if ( type.InterfaceImplementations.Count > 0 )
            {
                this.Nodes.Add( new InterfaceImplementationCollectionTreeNode( this.type ) );
            }

            if ( type.Types.Count > 0 )
            {
                this.Nodes.Add( new NestedTypeFolderTreeNode( this.type ) );
            }

            /*
            if (type.GenericParameters.Count > 0)
            {
                this.Nodes.Add(new GenericArgumentCollectionTreeNode(this.type));
            }
            */

            Dictionary<IMethod, bool> ignoreMethod = new Dictionary<IMethod, bool>();


            foreach (
                PropertyDeclaration property in ArrayHelper.ToSortedArray<PropertyDeclaration>( this.type.Properties ) )
            {
                foreach ( MethodSemanticDeclaration method in property.Members )
                {
                    ignoreMethod.Add( method.Method, true );
                }
            }

            foreach ( EventDeclaration @event in ArrayHelper.ToSortedArray<EventDeclaration>( this.type.Events ) )
            {
                foreach ( MethodSemanticDeclaration method in @event.Members )
                {
                    ignoreMethod.Add( method.Method, true );
                }
            }

            foreach (
                MethodDefDeclaration method in ArrayHelper.ToSortedArray<MethodDefDeclaration>( this.type.Methods ) )
            {
                if ( !ignoreMethod.ContainsKey( method ) )
                {
                    this.Nodes.Add( new MethodTreeNode( method ) );
                }
            }

            foreach (
                PropertyDeclaration property in ArrayHelper.ToSortedArray<PropertyDeclaration>( this.type.Properties ) )
            {
                this.Nodes.Add( new PropertyTreeNode( property ) );
            }

            foreach ( EventDeclaration @event in ArrayHelper.ToSortedArray<EventDeclaration>( this.type.Events ) )
            {
                this.Nodes.Add( new EventTreeNode( @event ) );
            }

            foreach ( FieldDefDeclaration field in ArrayHelper.ToSortedArray<FieldDefDeclaration>( this.type.Fields ) )
            {
                this.Nodes.Add( new FieldTreeNode( field ) );
            }

          ExternalFieldTreeNode.PopulateExternalMembers( this.type, this.Nodes, true);

        }
    }

    internal class NestedTypeFolderTreeNode : BaseTreeNode
    {
        private readonly TypeDefDeclaration parentType;

        public NestedTypeFolderTreeNode( TypeDefDeclaration parentType )
            : base( TreeViewImage.Folder, null )
        {
            this.Text = "Nested Types";
            this.parentType = parentType;
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            foreach ( TypeDefDeclaration type in parentType.Types )
            {
                this.Nodes.Add( new TypeTreeNode( type, type.Name ) );
            }
        }
    }
}
