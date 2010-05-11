namespace Librarian.WinForms
{
    partial class CustomerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.customerNameLabel = new System.Windows.Forms.Label();
            this.balanceLabel = new System.Windows.Forms.Label();
            this.rentaledBooksLabel = new System.Windows.Forms.Label();
            this.delayedBooksLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonLostBook = new System.Windows.Forms.Button();
            this.buttonReturnBook = new System.Windows.Forms.Button();
            this.buttonNewRental = new System.Windows.Forms.Button();
            this.booksListView = new System.Windows.Forms.ListView();
            this.bookIdColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.bookAuthorsColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.bookTitleColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.rentalStartDateColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.rentalScheduledReturnDateColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.rentalReturnDateColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.rentalNotesColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.checkBoxOnlyPendingRentals = new System.Windows.Forms.CheckBox();
            this.listViewAccountLines = new System.Windows.Forms.ListView();
            this.columnHeaderAccountLineDate = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAccountLineEmployee = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAccountLineDescription = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAccountLineAmout = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonShowNotes = new System.Windows.Forms.Button();
            this.buttonAcceptPayment = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customerNameLabel
            // 
            this.customerNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerNameLabel.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customerNameLabel.Location = new System.Drawing.Point(3, 0);
            this.customerNameLabel.Name = "customerNameLabel";
            this.customerNameLabel.Size = new System.Drawing.Size(1110, 30);
            this.customerNameLabel.TabIndex = 0;
            this.customerNameLabel.Text = "<Number> (<FirstName <LastName>)";
            // 
            // balanceLabel
            // 
            this.balanceLabel.AutoSize = true;
            this.balanceLabel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.balanceLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.balanceLabel.Location = new System.Drawing.Point(4, 30);
            this.balanceLabel.Name = "balanceLabel";
            this.balanceLabel.Size = new System.Drawing.Size(196, 22);
            this.balanceLabel.TabIndex = 1;
            this.balanceLabel.Text = "Balance: -99,99 EUR";
            // 
            // rentaledBooksLabel
            // 
            this.rentaledBooksLabel.AutoSize = true;
            this.rentaledBooksLabel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rentaledBooksLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rentaledBooksLabel.Location = new System.Drawing.Point(206, 30);
            this.rentaledBooksLabel.Name = "rentaledBooksLabel";
            this.rentaledBooksLabel.Size = new System.Drawing.Size(175, 22);
            this.rentaledBooksLabel.TabIndex = 2;
            this.rentaledBooksLabel.Text = "99 book(s) rentaled";
            // 
            // delayedBooksLabel
            // 
            this.delayedBooksLabel.AutoSize = true;
            this.delayedBooksLabel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delayedBooksLabel.ForeColor = System.Drawing.Color.Red;
            this.delayedBooksLabel.Location = new System.Drawing.Point(387, 30);
            this.delayedBooksLabel.Name = "delayedBooksLabel";
            this.delayedBooksLabel.Size = new System.Drawing.Size(135, 22);
            this.delayedBooksLabel.TabIndex = 3;
            this.delayedBooksLabel.Text = "99 with delay!";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonAcceptPayment);
            this.splitContainer1.Panel1.Controls.Add(this.buttonShowNotes);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxOnlyPendingRentals);
            this.splitContainer1.Panel1.Controls.Add(this.buttonLostBook);
            this.splitContainer1.Panel1.Controls.Add(this.buttonReturnBook);
            this.splitContainer1.Panel1.Controls.Add(this.buttonNewRental);
            this.splitContainer1.Panel1.Controls.Add(this.booksListView);
            this.splitContainer1.Panel1.Controls.Add(this.customerNameLabel);
            this.splitContainer1.Panel1.Controls.Add(this.delayedBooksLabel);
            this.splitContainer1.Panel1.Controls.Add(this.balanceLabel);
            this.splitContainer1.Panel1.Controls.Add(this.rentaledBooksLabel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.listViewAccountLines);
            this.splitContainer1.Size = new System.Drawing.Size(902, 575);
            this.splitContainer1.SplitterDistance = 382;
            this.splitContainer1.TabIndex = 4;
            // 
            // buttonLostBook
            // 
            this.buttonLostBook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLostBook.Location = new System.Drawing.Point(90, 342);
            this.buttonLostBook.Name = "buttonLostBook";
            this.buttonLostBook.Size = new System.Drawing.Size(75, 23);
            this.buttonLostBook.TabIndex = 7;
            this.buttonLostBook.Text = "Lost Book";
            this.buttonLostBook.UseVisualStyleBackColor = true;
            this.buttonLostBook.Click += new System.EventHandler(this.buttonLostBook_Click);
            // 
            // buttonReturnBook
            // 
            this.buttonReturnBook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReturnBook.Location = new System.Drawing.Point(8, 342);
            this.buttonReturnBook.Name = "buttonReturnBook";
            this.buttonReturnBook.Size = new System.Drawing.Size(75, 23);
            this.buttonReturnBook.TabIndex = 6;
            this.buttonReturnBook.Text = "Return Book";
            this.buttonReturnBook.UseVisualStyleBackColor = true;
            this.buttonReturnBook.Click += new System.EventHandler(this.buttonReturnBook_Click);
            // 
            // buttonNewRental
            // 
            this.buttonNewRental.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNewRental.Location = new System.Drawing.Point(815, 341);
            this.buttonNewRental.Name = "buttonNewRental";
            this.buttonNewRental.Size = new System.Drawing.Size(75, 23);
            this.buttonNewRental.TabIndex = 5;
            this.buttonNewRental.Text = "New Rental";
            this.buttonNewRental.UseVisualStyleBackColor = true;
            this.buttonNewRental.Click += new System.EventHandler(this.buttonNewRental_Click);
            // 
            // booksListView
            // 
            this.booksListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.booksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.bookIdColumnHeader,
            this.bookAuthorsColumnHeader,
            this.bookTitleColumnHeader,
            this.rentalStartDateColumnHeader,
            this.rentalScheduledReturnDateColumnHeader1,
            this.rentalReturnDateColumnHeader,
            this.rentalNotesColumnHeader1});
            this.booksListView.FullRowSelect = true;
            this.booksListView.GridLines = true;
            this.booksListView.Location = new System.Drawing.Point(12, 72);
            this.booksListView.MultiSelect = false;
            this.booksListView.Name = "booksListView";
            this.booksListView.Size = new System.Drawing.Size(882, 263);
            this.booksListView.TabIndex = 4;
            this.booksListView.UseCompatibleStateImageBehavior = false;
            this.booksListView.View = System.Windows.Forms.View.Details;
            this.booksListView.DoubleClick += new System.EventHandler(this.booksListView_DoubleClick);
            // 
            // bookIdColumnHeader
            // 
            this.bookIdColumnHeader.Text = "ID";
            // 
            // bookAuthorsColumnHeader
            // 
            this.bookAuthorsColumnHeader.Text = "Authors";
            this.bookAuthorsColumnHeader.Width = 132;
            // 
            // bookTitleColumnHeader
            // 
            this.bookTitleColumnHeader.Text = "Title";
            this.bookTitleColumnHeader.Width = 244;
            // 
            // rentalStartDateColumnHeader
            // 
            this.rentalStartDateColumnHeader.Text = "Rental Date";
            this.rentalStartDateColumnHeader.Width = 95;
            // 
            // rentalScheduledReturnDateColumnHeader1
            // 
            this.rentalScheduledReturnDateColumnHeader1.Text = "Scheduled Return";
            this.rentalScheduledReturnDateColumnHeader1.Width = 126;
            // 
            // rentalReturnDateColumnHeader
            // 
            this.rentalReturnDateColumnHeader.Text = "Return Date";
            this.rentalReturnDateColumnHeader.Width = 84;
            // 
            // rentalNotesColumnHeader1
            // 
            this.rentalNotesColumnHeader1.Text = "Notes";
            // 
            // checkBoxOnlyPendingRentals
            // 
            this.checkBoxOnlyPendingRentals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxOnlyPendingRentals.AutoSize = true;
            this.checkBoxOnlyPendingRentals.Checked = true;
            this.checkBoxOnlyPendingRentals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOnlyPendingRentals.Location = new System.Drawing.Point(251, 345);
            this.checkBoxOnlyPendingRentals.Name = "checkBoxOnlyPendingRentals";
            this.checkBoxOnlyPendingRentals.Size = new System.Drawing.Size(151, 17);
            this.checkBoxOnlyPendingRentals.TabIndex = 8;
            this.checkBoxOnlyPendingRentals.Text = "Show Only Pending Rentals";
            this.checkBoxOnlyPendingRentals.UseVisualStyleBackColor = true;
            this.checkBoxOnlyPendingRentals.CheckedChanged += new System.EventHandler(this.checkBoxOnlyPendingRentals_CheckedChanged);
            // 
            // listViewAccountLines
            // 
            this.listViewAccountLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewAccountLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAccountLineDate,
            this.columnHeaderAccountLineEmployee,
            this.columnHeaderAccountLineDescription,
            this.columnHeaderAccountLineAmout});
            this.listViewAccountLines.Location = new System.Drawing.Point(12, 23);
            this.listViewAccountLines.Name = "listViewAccountLines";
            this.listViewAccountLines.Size = new System.Drawing.Size(882, 154);
            this.listViewAccountLines.TabIndex = 0;
            this.listViewAccountLines.UseCompatibleStateImageBehavior = false;
            this.listViewAccountLines.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderAccountLineDate
            // 
            this.columnHeaderAccountLineDate.Text = "Date";
            // 
            // columnHeaderAccountLineEmployee
            // 
            this.columnHeaderAccountLineEmployee.Text = "Employee";
            this.columnHeaderAccountLineEmployee.Width = 91;
            // 
            // columnHeaderAccountLineDescription
            // 
            this.columnHeaderAccountLineDescription.Text = "Description";
            this.columnHeaderAccountLineDescription.Width = 282;
            // 
            // columnHeaderAccountLineAmout
            // 
            this.columnHeaderAccountLineAmout.Text = "Amount";
            this.columnHeaderAccountLineAmout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Customer Account Detail";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Rentals";
            // 
            // buttonShowNotes
            // 
            this.buttonShowNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonShowNotes.Location = new System.Drawing.Point(172, 341);
            this.buttonShowNotes.Name = "buttonShowNotes";
            this.buttonShowNotes.Size = new System.Drawing.Size(75, 23);
            this.buttonShowNotes.TabIndex = 10;
            this.buttonShowNotes.Text = "Show Notes";
            this.buttonShowNotes.UseVisualStyleBackColor = true;
            this.buttonShowNotes.Click += new System.EventHandler(this.buttonShowNotes_Click);
            // 
            // buttonAcceptPayment
            // 
            this.buttonAcceptPayment.Location = new System.Drawing.Point(781, 32);
            this.buttonAcceptPayment.Name = "buttonAcceptPayment";
            this.buttonAcceptPayment.Size = new System.Drawing.Size(109, 23);
            this.buttonAcceptPayment.TabIndex = 11;
            this.buttonAcceptPayment.Text = "Accept Payment";
            this.buttonAcceptPayment.UseVisualStyleBackColor = true;
            this.buttonAcceptPayment.Click += new System.EventHandler(this.buttonAcceptPayment_Click);
            // 
            // CustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 575);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CustomerForm";
            this.Text = "Customer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label customerNameLabel;
        private System.Windows.Forms.Label balanceLabel;
        private System.Windows.Forms.Label rentaledBooksLabel;
        private System.Windows.Forms.Label delayedBooksLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView booksListView;
        private System.Windows.Forms.ColumnHeader bookIdColumnHeader;
        private System.Windows.Forms.ColumnHeader bookAuthorsColumnHeader;
        private System.Windows.Forms.ColumnHeader bookTitleColumnHeader;
        private System.Windows.Forms.ColumnHeader rentalStartDateColumnHeader;
        private System.Windows.Forms.ColumnHeader rentalScheduledReturnDateColumnHeader1;
        private System.Windows.Forms.ColumnHeader rentalNotesColumnHeader1;
        private System.Windows.Forms.Button buttonNewRental;
        private System.Windows.Forms.ColumnHeader rentalReturnDateColumnHeader;
        private System.Windows.Forms.Button buttonLostBook;
        private System.Windows.Forms.Button buttonReturnBook;
        private System.Windows.Forms.CheckBox checkBoxOnlyPendingRentals;
        private System.Windows.Forms.ListView listViewAccountLines;
        private System.Windows.Forms.ColumnHeader columnHeaderAccountLineDate;
        private System.Windows.Forms.ColumnHeader columnHeaderAccountLineEmployee;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeaderAccountLineDescription;
        private System.Windows.Forms.ColumnHeader columnHeaderAccountLineAmout;
        private System.Windows.Forms.Button buttonShowNotes;
        private System.Windows.Forms.Button buttonAcceptPayment;
    }
}
