using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Librarian.BusinessProcesses;
using Librarian.Entities;

namespace Librarian.WinForms
{
    public partial class CustomerForm : Form
    {
        private Customer customer;
        private readonly Accessor<ICustomerProcesses> customerProcesses = ClientSession.GetService<ICustomerProcesses>();
        private readonly Accessor<IRentalProcesses> rentalProcesses = ClientSession.GetService<IRentalProcesses>();


        public CustomerForm()
        {
            InitializeComponent();

            if ( this.components == null )
                this.components = new Container();
            this.components.Add( customerProcesses );
            this.components.Add( rentalProcesses );
        }

        public Customer Customer { get { return customer; } set { customer = value; } }

        public Rental SelectedRental
        {
            get
            {
                return this.booksListView.SelectedItems.Count == 0
                           ? null
                           :
                               (Rental) this.booksListView.SelectedItems[0].Tag;
            }
        }

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            this.ReloadData();
        }

        private void ReloadData()
        {
            CustomerInfo customerInfo =
                this.customerProcesses.Value.GetCustomerInfo( this.customer, !this.checkBoxOnlyPendingRentals.Checked );
            this.customer = customerInfo.Customer;


            this.Text =
                string.Format( "Customer {0} ({1} {2})",
                               this.customer.CustomerId,
                               this.customer.FirstName,
                               this.customer.LastName );

            this.customerNameLabel.Text = string.Format( "{0} ({1} {2})",
                                                         this.customer.CustomerId,
                                                         this.customer.FirstName,
                                                         this.customer.LastName );

            this.balanceLabel.Text = string.Format( "Balance: {0} EUR",
                                                    this.customer.CurrentBalance );
            this.balanceLabel.ForeColor = this.customer.CurrentBalance > 0
                                              ?
                                                  Color.Green
                                              : Color.Red;

            this.rentaledBooksLabel.Text = string.Format( "{0} book(s) rentaled.",
                                                        customerInfo.Rentals.Count );

            int delayed = 0;
            foreach ( RentalInfo rental in customerInfo.Rentals )
            {
                if ( !rental.Rental.Closed && rental.Rental.ScheduledReturnDate < DateTime.Now )
                {
                    delayed++;
                }
            }

            this.delayedBooksLabel.Text = string.Format( "{0} with delay.",
                                                         delayed );

            this.delayedBooksLabel.Visible = delayed != 0;

            this.booksListView.BeginUpdate();
            this.booksListView.Items.Clear();
            foreach ( RentalInfo rentalInfo in customerInfo.Rentals )
            {
                ListViewItem lvi = new ListViewItem(
                    new string[]
                        {
                            rentalInfo.Book.BookId,
                            rentalInfo.Book.Authors,
                            rentalInfo.Book.Title,
                            rentalInfo.Rental.StartDate.ToShortDateString(),
                            rentalInfo.Rental.ScheduledReturnDate.ToShortDateString(),
                            rentalInfo.Rental.ReturnDate.HasValue ? rentalInfo.Rental.ReturnDate.Value.ToShortDateString() : "",
                            rentalInfo.Notes.Count.ToString()
                        } );
                lvi.Tag = rentalInfo.Rental;

                if ( rentalInfo.Rental.IsDelayed() )
                {
                    lvi.ForeColor = Color.Red;
                }
                else if ( rentalInfo.Rental.Closed )
                {
                    lvi.ForeColor = Color.Gray;
                }

                this.booksListView.Items.Add( lvi );
            }
            this.booksListView.EndUpdate();

            this.listViewAccountLines.BeginUpdate();
            this.listViewAccountLines.Items.Clear();
            foreach ( CustomerAccountLine line in customerInfo.AccountLines )
            {
                ListViewItem lvi = new ListViewItem(
                    new string[]
                        {
                            line.Date.ToShortDateString(),
                            line.Employee.Entity.Login,
                            line.Description,
                            line.Amount.ToString()
                        } );
                lvi.Tag = line;
                this.listViewAccountLines.Items.Add( lvi );
            }
            this.listViewAccountLines.EndUpdate();
        }


        private void buttonNewRental_Click( object sender, EventArgs e )
        {
            using ( OpenRentalForm openRentalForm = new OpenRentalForm() )
            {
                openRentalForm.Customer = this.customer;
                if ( openRentalForm.ShowDialog() == DialogResult.OK )
                {
                    this.ReloadData();
                }
            }
        }

        [ExceptionMessageBox]
        private void buttonReturnBook_Click( object sender, EventArgs e )
        {
            if ( this.SelectedRental == null )
            {
                MessageBox.Show( this, "Please select a rental." );
                return;
            }

            if ( MessageBox.Show( this, "Are you sure you want to return this book?",
                                  "Return Book", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
            {
                this.rentalProcesses.Value.ReturnBook( this.SelectedRental );
                this.ReloadData();
            }
        }

        [ExceptionMessageBox]
        private void buttonLostBook_Click( object sender, EventArgs e )
        {
            if ( this.SelectedRental == null )
            {
                MessageBox.Show( this, "Please select a rental." );
                return;
            }

            if ( MessageBox.Show( this, "Are you sure you want to report this book as lost?",
                                  "Return Book", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
            {
                this.rentalProcesses.Value.ReportLostBook( this.SelectedRental );
                this.ReloadData();
            }
        }

        private void checkBoxOnlyPendingRentals_CheckedChanged( object sender, EventArgs e )
        {
            this.ReloadData();
        }

        private void booksListView_DoubleClick( object sender, EventArgs e )
        {
            this.ShowRentalNotes();
        }

        private void ShowRentalNotes()
        {
            if ( this.SelectedRental == null )
            {
                MessageBox.Show( this, "Please select a rental." );
                return;
            }

            using ( ShowNotesForm showNotesForm = new ShowNotesForm() )
            {
                showNotesForm.Entity = this.SelectedRental;
                if ( showNotesForm.ShowDialog() == DialogResult.OK )
                {
                    this.ReloadData();
                }
            }
        }

        private void buttonShowNotes_Click( object sender, EventArgs e )
        {
            this.ShowRentalNotes();
        }

        private void buttonAcceptPayment_Click( object sender, EventArgs e )
        {
            using ( AcceptCustomerPaymentForm paymentForm = new AcceptCustomerPaymentForm() )
            {
                paymentForm.Customer = this.customer;
                if ( paymentForm.ShowDialog() == DialogResult.OK )
                {
                    this.ReloadData();
                }
            }
        }
    }
}
