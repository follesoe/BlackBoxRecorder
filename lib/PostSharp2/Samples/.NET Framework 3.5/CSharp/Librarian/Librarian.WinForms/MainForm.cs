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
using System.Windows.Forms;

namespace Librarian.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private void MainForm_Shown( object sender, EventArgs e )
        {
            this.Login();
        }

        private void Login()
        {
            using ( LoginForm login = new LoginForm() )
            {
                if ( login.ShowDialog() != DialogResult.OK )
                {
                    this.Close();
                }
                else
                {
                    this.Text =
                        string.Format( "PostSharp.Samples.Librarian ({0} {1})", ClientSession.Current.Employee.FirstName,
                                       ClientSession.Current.Employee.LastName );
                }
            }
        }


        private void registerCustomerToolStripMenuItem_Click( object sender, EventArgs e )
        {
            UpdateCustomerForm updateCustomerForm = new UpdateCustomerForm();
            updateCustomerForm.MdiParent = this;
            updateCustomerForm.Show();
        }

        private void openCustomerToolStripMenuItem_Click( object sender, EventArgs e )
        {
            using ( SelectCustomerForm selectCustomerForm = new SelectCustomerForm() )
            {
                if ( selectCustomerForm.ShowDialog() == DialogResult.OK )
                {
                    CustomerForm customerForm = new CustomerForm();
                    customerForm.Customer = selectCustomerForm.SelectedCustomer;
                    customerForm.MdiParent = this;
                    customerForm.Show();
                }
            }
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void newBookToolStripMenuItem_Click( object sender, EventArgs e )
        {
            UpdateBookForm updateBookForm = new UpdateBookForm();
            updateBookForm.MdiParent = this;
            updateBookForm.Show();
        }

        private void logoutToolStripMenuItem_Click( object sender, EventArgs e )
        {
            this.Text = "PostSharp.Samples.Librarian";
            this.Login();
        }
    }
}