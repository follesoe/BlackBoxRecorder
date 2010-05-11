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
    public partial class SelectCustomerForm : Form
    {
        private readonly Accessor<ICustomerProcesses> customerProcesses = ClientSession.GetService<ICustomerProcesses>();

        private Customer selectedCustomer;

        public SelectCustomerForm()
        {
            InitializeComponent();

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( customerProcesses );
        }

        public Customer SelectedCustomer { get { return selectedCustomer; } }

        private void filterButton_Click( object sender, EventArgs e )
        {
            this.customersListView.BeginUpdate();
            this.customersListView.Items.Clear();
            foreach ( Customer customer in this.customerProcesses.Value.FindCustomers(
                this.customerIdTextBox.Text,
                this.firstNameTextBox.Text,
                this.lastNameTextBox.Text, 50 ) )
            {
                ListViewItem listViewItem = new ListViewItem(
                    new string[] {customer.CustomerId, customer.FirstName, customer.LastName} );
                listViewItem.Tag = customer;
                this.customersListView.Items.Add( listViewItem );
            }
            this.customersListView.Sort();
            this.customersListView.EndUpdate();
            this.customersListView.Focus();
        }

        private void SelectCustomer()
        {
            if ( this.customersListView.SelectedItems.Count == 0 )
            {
                MessageBox.Show( this, "No customer selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            else
            {
                this.selectedCustomer = (Customer) this.customersListView.SelectedItems[0].Tag;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void okButton_Click( object sender, EventArgs e )
        {
            this.SelectCustomer();
        }

        private void customersListView_DoubleClick( object sender, EventArgs e )
        {
            this.SelectCustomer();
        }
    }
}