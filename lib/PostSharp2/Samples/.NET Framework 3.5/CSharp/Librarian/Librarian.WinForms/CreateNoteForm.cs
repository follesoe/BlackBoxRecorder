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
    public partial class CreateNoteForm : Form
    {
        private readonly Accessor<INoteProcesses> noteProcesses = ClientSession.GetService<INoteProcesses>();

        public CreateNoteForm()
        {
            InitializeComponent();

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( noteProcesses );
        }

        private EntityRef<Entity> entity;

        public EntityRef<Entity> Entity { get { return entity; } set { entity = value; } }


        private void buttonCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        [ExceptionMessageBox]
        private void buttonOk_Click( object sender, EventArgs e )
        {
            Note note = new Note();
            note.Date = DateTime.Now;
            note.Owner = this.entity;
            note.Text = this.textBoxText.Text;
            note.Title = this.textBoxTitle.Text;

            this.noteProcesses.Value.CreateNote( note );

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}