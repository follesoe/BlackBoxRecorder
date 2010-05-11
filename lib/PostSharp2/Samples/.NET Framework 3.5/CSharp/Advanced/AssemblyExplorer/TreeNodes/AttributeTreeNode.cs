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

using System.Text;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class AttributeTreeNode : BaseTreeNode
    {
        public AttributeTreeNode( string name, object value ) : base( TreeViewImage.Property, null )
        {
            StringBuilder text = new StringBuilder();
            text.Append( name );
            text.Append( " = " );
            Formatter.Format( value, text );
            this.Text = text.ToString();
        }
    }
}