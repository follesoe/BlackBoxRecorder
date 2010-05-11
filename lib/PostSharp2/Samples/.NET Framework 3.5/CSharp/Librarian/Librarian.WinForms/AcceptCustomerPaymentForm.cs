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
    public partial class AcceptCustomerPaymentForm : Form
    {
        private EntityRef<Customer> customer;
        private readonly Accessor<ICustomerProcesses> customerProcesses = ClientSession.GetService<ICustomerProcesses>();

        public AcceptCustomerPaymentForm()
        {
            InitializeComponent();

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( this.customerProcesses );
        }

        public EntityRef<Customer> Customer { get { return customer; } set { customer = value; } }

        [ExceptionMessageBox]
        private void buttonOk_Click( object sender, EventArgs e )
        {
            decimal amount;
            try
            {
                amount = decimal.Parse( this.maskedTextBox1.Text );
            }
            catch ( FormatException exc )
            {
                MessageBox.Show( this, "The amount is incorrectly formatted: " + exc.Message );
                return;
            }

            this.customerProcesses.Value.AcceptCustomerPayment( this.customer, amount );
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}