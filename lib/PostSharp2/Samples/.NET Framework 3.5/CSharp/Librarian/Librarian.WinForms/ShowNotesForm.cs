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

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Librarian.BusinessProcesses;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian.WinForms
{
    public partial class ShowNotesForm : Form
    {
        private bool changed;
        private EntityRef<Entity> entity;
        private readonly Accessor<INoteProcesses> noteProcesses = ClientSession.GetService<INoteProcesses>();


        public ShowNotesForm()
        {
            InitializeComponent();

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( noteProcesses );
        }

        public EntityRef<Entity> Entity { get { return entity; } set { entity = value; } }


        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            this.ReloadData();
        }

        private void ReloadData()
        {
            this.listView.BeginUpdate();
            this.listView.Items.Clear();
            foreach ( Note note in this.noteProcesses.Value.GetNotes( this.entity, 50 ) )
            {
                ListViewItem lvi = new ListViewItem( new string[]
                                                         {
                                                             note.Date.ToShortDateString(),
                                                             note.Employee.Entity.Login,
                                                             note.Title
                                                         } );
                lvi.Tag = note;
                this.listView.Items.Add( lvi );
            }
            this.listView.EndUpdate();
        }

        private void listView_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( this.listView.SelectedItems.Count > 0 )
            {
                Note note = (Note) this.listView.SelectedItems[0].Tag;
                this.labelDescription.Text = note.Text;
            }
            else
            {
                this.labelDescription.Text = "";
            }
        }

        private void buttonClose_Click( object sender, EventArgs e )
        {
            this.DialogResult = this.changed ? DialogResult.OK : DialogResult.Cancel;
            this.Close();
        }

        private void buttonAddNote_Click( object sender, EventArgs e )
        {
            using ( CreateNoteForm createNoteForm = new CreateNoteForm() )
            {
                createNoteForm.Entity = this.entity;
                if ( createNoteForm.ShowDialog() == DialogResult.OK )
                {
                    this.changed = true;
                    this.ReloadData();
                }
            }
        }
    }
}