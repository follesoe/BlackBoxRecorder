#region Using directives

using System.Windows.Forms;
using PostSharp.Sdk.CodeModel;

#endregion

namespace AssemblyExplorer.TreeNodes
{
    internal class LocaVariableCollectionTreeNode : BaseTreeNode
    {
        private readonly MethodBodyDeclaration body;

        public LocaVariableCollectionTreeNode( MethodBodyDeclaration body )
            : base( TreeViewImage.Folder, null )
        {
            this.body = body;
            this.Text = "Locals";
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            for ( int i = 0 ; i < body.LocalVariableMaxOrdinal ; i++ )
            {
                LocalVariableDeclaration variable = body.GetLocalVariable( i );
                if (variable != null)
                {
                    this.Nodes.Add( new LocalTreeNode( variable ) );
                }
            }
        }
    }

    internal class LocaVariableSymbolCollectionTreeNode : BaseTreeNode
    {
        private readonly InstructionBlock block;

        public LocaVariableSymbolCollectionTreeNode( InstructionBlock block )
            : base( TreeViewImage.Folder, null )
        {
            this.block = block;
            this.Text = "Symbols";
            this.EnableLatePopulate();
        }


        protected internal override void OnPopulate( TreeViewCancelEventArgs e )
        {
            for ( int i = 0 ; i < block.LocalVariableSymbolCount ; i++ )
            {
                LocalVariableSymbol symbol = block.GetLocalVariableSymbol( i );
                TreeNode node = new TreeNode( symbol.Name + ": " + symbol.LocalVariable.Ordinal.ToString() );
                this.Nodes.Add( node );
            }
        }
    }
}