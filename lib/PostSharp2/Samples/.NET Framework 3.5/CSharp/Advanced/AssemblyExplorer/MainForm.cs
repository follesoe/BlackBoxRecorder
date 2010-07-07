#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AssemblyExplorer.TreeNodes;
using PostSharp.Sdk.CodeModel;
using PostSharp.Sdk.Extensibility.Licensing;
using PostSharp.Sdk.Utilities;

#endregion

namespace AssemblyExplorer
{
    internal partial class MainForm : Form
    {
        private object currentObject;
        private object msilRenderedObject;
        private readonly Domain domain;
        private AssemblyResolver resolver;

        public MainForm()
        {
            // We don't have a host license key, so we will load the user key, if
            // stored at the expected location.
            string licenseFile = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), @"PostSharp 2.0\PostSharp.license" );
            if ( !File.Exists( licenseFile ) )
            {
                MessageBox.Show( "The PostSharp license file was not found." );
                Environment.Exit( 1 );
            }

            if ( !LicenseManager.Initialize( File.ReadAllText( licenseFile ) ) )
            {
                MessageBox.Show( "Invalid license." );
                Environment.Exit( 2 );
            }


            InitializeComponent();

            this.domain = new Domain();
            this.resolver = new AssemblyResolver( this.domain );


            // Load assemblies.
            foreach ( string path in UserConfiguration.GetLoadedAssemblies() )
            {
                try
                {
                    this.LoadAssembly( path );
                }
                catch ( Exception e )
                {
                    UserConfiguration.RemoveAssembly( path );
                    MessageBox.Show( this, string.Format( "Cannot load the assembly {0}: {1}",
                                                          Path.GetFileName( path ), e.Message ), "Load Error",
                                     MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
            }
        }


        private void treeView_AfterSelect( object sender, TreeViewEventArgs e )
        {
            this.treeViewDefinition.BeginUpdate();
            this.treeViewDefinition.Nodes.Clear();
            BaseTreeNode node = e.Node as BaseTreeNode;
            if ( node != null )
            {
                node.OnPopulateDefinitionTree( this.treeViewDefinition.Nodes );
                this.treeViewDefinition.EndUpdate();
            }
        }

        private void UpdateMsil()
        {
            if ( this.currentObject == this.msilRenderedObject )
                return;

            MetadataDeclaration declaration = this.currentObject as MetadataDeclaration;
            if ( declaration != null && MsilWriterUtility.CanWriteMsil( declaration ) )
            {
                if ( this.currentObject is ModuleDeclaration )
                {
                    if ( MessageBox.Show( this, "This may take a very long time to display. " +
                                                "Do you want to see the MSIL code of the whole module?", "Warning",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) != DialogResult.Yes )
                    {
                        this.tabControl.SelectedTab = this.tabPageProperties;
                        return;
                    }
                }

                StringWriter stringWriter = new StringWriter( CultureInfo.InvariantCulture );

                MsilWriterUtility.WriteMsil( declaration, stringWriter );

                this.textBoxMsil.Text = stringWriter.ToString();

                this.msilRenderedObject = this.currentObject;
            }
            else
            {
                this.textBoxMsil.Text = "Disassembly not available for the current element.";
            }
        }

        public void ChangeCurrentObject( object obj )
        {
            this.currentObject = obj;
            this.propertyGrid.SelectedObject = obj;
            if ( this.tabControl.SelectedTab == this.tabPageMsil )
            {
                this.UpdateMsil();
            }
            this.labelElementType.Text = obj.GetType().Name;
        }


        private void exitMenuItem_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void LoadAssembly( string path )
        {
            ModuleDeclaration module = domain.LoadAssembly( path, true ).ManifestModule;
            this.treeView.Nodes.Add( new ModuleTreeNode( module, path ) );
        }

        private void fileOpenMenuItem_Click( object sender, EventArgs e )
        {
            if ( this.openModuleFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                UserConfiguration.AddAssembly( this.openModuleFileDialog.FileName );
                this.LoadAssembly( this.openModuleFileDialog.FileName );
            }
        }

        public void RemoveModule( ModuleTreeNode treeNode )
        {
            this.treeView.Nodes.Remove( treeNode );
            UserConfiguration.RemoveAssembly( treeNode.ModulePath );
        }

        private void tabControl_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( this.tabControl.SelectedTab == this.tabPageMsil )
            {
                this.UpdateMsil();
            }
        }

        private void toolStripMenuItemOptions_Click( object sender, EventArgs e )
        {
            new OptionsForm().ShowDialog( this );
        }

        public void SaveMsil( MetadataDeclaration writeILDefinition )
        {
            if ( this.saveMsilFileDialog.ShowDialog( this ) == DialogResult.OK )
            {
                SaveMsilParameter parameter = new SaveMsilParameter();
                parameter.Declaration = writeILDefinition;
                parameter.FileName = this.saveMsilFileDialog.FileName;

                WaitForm waitForm = new WaitForm( this.SaveMsil, parameter );
                waitForm.ShowDialog( this );
            }
        }

        private void SaveMsil( object o )
        {
            SaveMsilParameter parameter = (SaveMsilParameter) o;

            using ( StreamWriter streamWriter = new StreamWriter( parameter.FileName, false, Encoding.ASCII ) )
            {
                MsilWriterUtility.WriteMsil( parameter.Declaration, streamWriter );
            }
        }

        private class SaveMsilParameter
        {
            public MetadataDeclaration Declaration;
            public string FileName;
        }
    }
}