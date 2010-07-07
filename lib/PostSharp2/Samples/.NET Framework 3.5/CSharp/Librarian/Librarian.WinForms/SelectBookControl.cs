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
    public partial class SelectBookControl : UserControl
    {
        private readonly Accessor<IBookProcesses> bookProcesses = ClientSession.GetService<IBookProcesses>();

        public SelectBookControl()
        {
            InitializeComponent();

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( bookProcesses );
        }

        public Book SelectedBook
        {
            get
            {
                return this.listViewBooks.SelectedItems.Count == 0
                           ? null
                           :
                               (Book) this.listViewBooks.SelectedItems[0].Tag;
            }
            set
            {
                Book selectedBook = value;
                if ( selectedBook == null )
                {
                    this.textBoxAuthors.Text = "";
                    this.textBoxBookId.Text = "";
                    this.textBoxIsbn.Text = "";
                    this.textBoxTitle.Text = "";
                    this.listViewBooks.Items.Clear();
                }
                else
                {
                    this.textBoxAuthors.Text = selectedBook.Authors;
                    this.textBoxBookId.Text = selectedBook.BookId;
                    this.textBoxIsbn.Text = selectedBook.Isbn;
                    this.textBoxTitle.Text = selectedBook.Title;
                    this.listViewBooks.Items.Clear();
                    this.AddListItem( selectedBook );
                }
            }
        }

        private void AddListItem( Book book )
        {
            ListViewItem lvi = new ListViewItem( new string[]
                                                     {
                                                         book.BookId, book.Authors, book.Title
                                                     } );
            lvi.Tag = book;
            this.listViewBooks.Items.Add( lvi );
        }

        private void buttonFilter_Click( object sender, EventArgs e )
        {
            this.listViewBooks.BeginUpdate();
            this.listViewBooks.Items.Clear();
            foreach ( Book book in this.bookProcesses.Value.FindBooks(
                this.textBoxBookId.Text, this.textBoxAuthors.Text, this.textBoxTitle.Text,
                this.textBoxIsbn.Text, 50 ) )
            {
                this.AddListItem( book );
            }
            this.listViewBooks.EndUpdate();
        }
    }
}