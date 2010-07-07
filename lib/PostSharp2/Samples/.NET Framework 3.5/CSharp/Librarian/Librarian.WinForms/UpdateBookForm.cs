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

namespace Librarian.WinForms
{
    public partial class UpdateBookForm : Form
    {
        private readonly Accessor<IBookProcesses> bookProcesses = ClientSession.GetService<IBookProcesses>();

        public UpdateBookForm()
        {
            InitializeComponent();

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( bookProcesses );
        }

        [ExceptionMessageBox]
        private void okButton_Click( object sender, EventArgs e )
        {
            Book book = new Book();
            book.Authors = this.authorsTextBox.Text;
            book.BookId = this.bookIdTextBox.Text;
            book.Isbn = this.isbnTextBox.Text;
            book.Title = this.titleTextBox.Text;
            try
            {
                book.LostPenalty = decimal.Parse( this.maskedTextBoxPenalty.Text );
            }
            catch ( FormatException exc )
            {
                MessageBox.Show( this, "The amount is incorrectly formatted: " + exc.Message );
                return;
            }
            this.bookProcesses.Value.CreateBook( book );
            this.Close();
        }
    }
}