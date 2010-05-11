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
    public partial class UpdateCustomerForm : Form
    {
        private Customer customer;
        private readonly Accessor<ICustomerProcesses> customerProcesses = ClientSession.GetService<ICustomerProcesses>();

        public UpdateCustomerForm()
        {
            InitializeComponent();
            this.updateButton.Text = "Create";

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( customerProcesses );
        }


        public Customer Customer
        {
            get { return customer; }
            set
            {
                this.customer = value;
                if ( value != null )
                {
                    this.customerIdTextBox.Text = this.customer.CustomerId;
                    this.customerIdTextBox.Enabled = false;
                    this.firstNameTextBox.Text = this.customer.FirstName;
                    this.lastNameTextBox.Text = this.customer.LastName;
                    this.updateButton.Text = "Update";
                }
                else
                {
                    this.customerIdTextBox.Text = this.customer.CustomerId;
                    this.customerIdTextBox.Enabled = true;
                    this.firstNameTextBox.Text = this.customer.FirstName;
                    this.lastNameTextBox.Text = this.customer.LastName;
                    this.updateButton.Text = "Create";
                }
            }
        }

        [ExceptionMessageBox]
        private void updateButton_Click( object sender, EventArgs e )
        {
            if ( this.customer == null )
            {
                this.customer = new Customer();
                this.customer.FirstName = this.firstNameTextBox.Text;
                this.customer.LastName = this.lastNameTextBox.Text;
                this.customer.CustomerId = this.customerIdTextBox.Text;
                this.customer = this.customerProcesses.Value.CreateCustomer( this.customer );
            }
            else
            {
                this.customer.FirstName = this.firstNameTextBox.Text;
                this.customer.LastName = this.lastNameTextBox.Text;
                this.customerProcesses.Value.UpdateCustomer( this.customer );
            }

            CustomerForm customerForm = new CustomerForm();
            customerForm.Customer = this.customer;
            customerForm.MdiParent = this.MdiParent;
            customerForm.Show();
            this.Close();
        }

        private void closeButton_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}