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
    public partial class OpenRentalForm : Form
    {
        private EntityRef<Customer> customer;
        private readonly DateTime returnDate;
        private readonly Accessor<IRentalProcesses> rentalProcesses = ClientSession.GetService<IRentalProcesses>();


        public OpenRentalForm()
        {
            InitializeComponent();

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( rentalProcesses );

            this.returnDate = DateTime.Now.AddDays( 7 ).Date.AddHours( 18 );
            this.labelScheduleDate.Text =
                string.Format( "Return before {0}.", this.returnDate );
        }

        public EntityRef<Customer> Customer { get { return customer; } set { customer = value; } }


        [ExceptionMessageBox]
        private void buttonAccept_Click( object sender, EventArgs e )
        {
            Book book = this.selectBookControl1.SelectedBook;
            if ( book == null )
            {
                MessageBox.Show( this, "Please select a book." );
                return;
            }

            Rental rental = new Rental();
            rental.Book = book;
            rental.Customer = this.customer;
            rental.StartDate = DateTime.Now;
            rental.ScheduledReturnDate = this.returnDate;

            this.rentalProcesses.Value.OpenRental( rental );

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
