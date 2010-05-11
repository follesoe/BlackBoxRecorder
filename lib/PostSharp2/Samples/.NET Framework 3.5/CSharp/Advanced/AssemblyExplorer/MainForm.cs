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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AssemblyExplorer.TreeNodes;
using PostSharp.CodeModel;
using PostSharp.Extensibility.Licensing;
using PostSharp.ModuleWriter;

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
            LicenseManager.Initialize(null);

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

            IWriteILDefinition writeILDefinition = this.currentObject as IWriteILDefinition;

            if ( writeILDefinition != null )
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
                ILWriter writer = new ILWriter( stringWriter );
                writer.Options.VerboseCustomAttributes = true;
                writeILDefinition.WriteILDefinition( writer );

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
            ModuleDeclaration module = domain.LoadAssembly(path, true).ManifestModule;
            this.treeView.Nodes.Add( new ModuleTreeNode( module, path ) );
        }

        private void fileOpenMenuItem_Click( object sender, EventArgs e )
        {
            if ( this.openModuleFileDialog.ShowDialog(this) == DialogResult.OK )
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

        public void SaveMsil(IWriteILDefinition writeILDefinition)
        {
            if ( this.saveMsilFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveMsilParameter parameter = new SaveMsilParameter();
                parameter.Declaration = writeILDefinition;
                parameter.FileName = this.saveMsilFileDialog.FileName;

                WaitForm waitForm = new WaitForm(this.SaveMsil, parameter);
                waitForm.ShowDialog(this);
            }
        }

        private void SaveMsil(object o)
        {
            SaveMsilParameter parameter = (SaveMsilParameter)o;

            using (StreamWriter streamWriter = new StreamWriter(parameter.FileName, false, Encoding.ASCII))
            {
                ILWriter writer = new ILWriter(streamWriter);
                writer.Options.VerboseCustomAttributes = true;
                parameter.Declaration.WriteILDefinition(writer);
            }
        }

        private class SaveMsilParameter
        {
            public IWriteILDefinition Declaration;
            public string FileName;
        }
    }
}
