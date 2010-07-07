namespace Librarian.WinForms
{
    partial class SelectCustomerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.customerIdTextBox = new System.Windows.Forms.TextBox();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            this.filterButton = new System.Windows.Forms.Button();
            this.customersListView = new System.Windows.Forms.ListView();
            this.customerIdColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.firstNameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.lastNameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.filterGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Customer ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Last Name:";
            // 
            // customerIdTextBox
            // 
            this.customerIdTextBox.Location = new System.Drawing.Point(81, 13);
            this.customerIdTextBox.Name = "customerIdTextBox";
            this.customerIdTextBox.Size = new System.Drawing.Size(190, 20);
            this.customerIdTextBox.TabIndex = 3;
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.Location = new System.Drawing.Point(81, 40);
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(190, 20);
            this.firstNameTextBox.TabIndex = 4;
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.Location = new System.Drawing.Point(81, 67);
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(190, 20);
            this.lastNameTextBox.TabIndex = 5;
            // 
            // filterGroupBox
            // 
            this.filterGroupBox.Controls.Add(this.filterButton);
            this.filterGroupBox.Controls.Add(this.label2);
            this.filterGroupBox.Controls.Add(this.lastNameTextBox);
            this.filterGroupBox.Controls.Add(this.label1);
            this.filterGroupBox.Controls.Add(this.firstNameTextBox);
            this.filterGroupBox.Controls.Add(this.label3);
            this.filterGroupBox.Controls.Add(this.customerIdTextBox);
            this.filterGroupBox.Location = new System.Drawing.Point(12, 12);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(386, 99);
            this.filterGroupBox.TabIndex = 6;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "Filter";
            // 
            // filterButton
            // 
            this.filterButton.Location = new System.Drawing.Point(293, 13);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(75, 23);
            this.filterButton.TabIndex = 6;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.filterButton_Click);
            // 
            // customersListView
            // 
            this.customersListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.customersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.customerIdColumnHeader,
            this.firstNameColumnHeader,
            this.lastNameColumnHeader});
            this.customersListView.FullRowSelect = true;
            this.customersListView.GridLines = true;
            this.customersListView.Location = new System.Drawing.Point(12, 118);
            this.customersListView.MultiSelect = false;
            this.customersListView.Name = "customersListView";
            this.customersListView.Size = new System.Drawing.Size(386, 97);
            this.customersListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.customersListView.TabIndex = 7;
            this.customersListView.UseCompatibleStateImageBehavior = false;
            this.customersListView.View = System.Windows.Forms.View.Details;
            this.customersListView.DoubleClick += new System.EventHandler(this.customersListView_DoubleClick);
            // 
            // customerIdColumnHeader
            // 
            this.customerIdColumnHeader.Text = "Customer Id";
            this.customerIdColumnHeader.Width = 81;
            // 
            // firstNameColumnHeader
            // 
            this.firstNameColumnHeader.Text = "First Name";
            this.firstNameColumnHeader.Width = 121;
            // 
            // lastNameColumnHeader
            // 
            this.lastNameColumnHeader.Text = "Last Name";
            this.lastNameColumnHeader.Width = 161;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.Location = new System.Drawing.Point(125, 231);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(206, 231);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // SelectCustomerForm
            // 
            this.AcceptButton = this.filterButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(425, 266);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.customersListView);
            this.Controls.Add(this.filterGroupBox);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(433, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(433, 293);
            this.Name = "SelectCustomerForm";
            this.ShowInTaskbar = false;
            this.Text = "Customer Selection";
            this.filterGroupBox.ResumeLayout(false);
            this.filterGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox customerIdTextBox;
        private System.Windows.Forms.TextBox firstNameTextBox;
        private System.Windows.Forms.TextBox lastNameTextBox;
        private System.Windows.Forms.GroupBox filterGroupBox;
        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.ListView customersListView;
        private System.Windows.Forms.ColumnHeader customerIdColumnHeader;
        private System.Windows.Forms.ColumnHeader firstNameColumnHeader;
        private System.Windows.Forms.ColumnHeader lastNameColumnHeader;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}