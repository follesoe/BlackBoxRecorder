namespace Librarian.WinForms
{
    partial class SelectBookControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBookId = new System.Windows.Forms.TextBox();
            this.textBoxAuthors = new System.Windows.Forms.TextBox();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxIsbn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.listViewBooks = new System.Windows.Forms.ListView();
            this.columnHeaderBookId = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAuthors = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderTitle = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Book Id";
            // 
            // textBoxBookId
            // 
            this.textBoxBookId.Location = new System.Drawing.Point(68, 7);
            this.textBoxBookId.Name = "textBoxBookId";
            this.textBoxBookId.Size = new System.Drawing.Size(100, 20);
            this.textBoxBookId.TabIndex = 1;
            // 
            // textBoxAuthors
            // 
            this.textBoxAuthors.Location = new System.Drawing.Point(303, 7);
            this.textBoxAuthors.Name = "textBoxAuthors";
            this.textBoxAuthors.Size = new System.Drawing.Size(165, 20);
            this.textBoxAuthors.TabIndex = 2;
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(303, 34);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(165, 20);
            this.textBoxTitle.TabIndex = 3;
            // 
            // textBoxIsbn
            // 
            this.textBoxIsbn.Location = new System.Drawing.Point(68, 34);
            this.textBoxIsbn.Name = "textBoxIsbn";
            this.textBoxIsbn.Size = new System.Drawing.Size(165, 20);
            this.textBoxIsbn.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(248, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Author(s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Title";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "ISBN";
            // 
            // buttonFilter
            // 
            this.buttonFilter.Location = new System.Drawing.Point(493, 31);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonFilter.TabIndex = 8;
            this.buttonFilter.Text = "Filter";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // listViewBooks
            // 
            this.listViewBooks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderBookId,
            this.columnHeaderAuthors,
            this.columnHeaderTitle});
            this.listViewBooks.FullRowSelect = true;
            this.listViewBooks.GridLines = true;
            this.listViewBooks.Location = new System.Drawing.Point(21, 71);
            this.listViewBooks.MultiSelect = false;
            this.listViewBooks.Name = "listViewBooks";
            this.listViewBooks.Size = new System.Drawing.Size(547, 134);
            this.listViewBooks.TabIndex = 9;
            this.listViewBooks.UseCompatibleStateImageBehavior = false;
            this.listViewBooks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderBookId
            // 
            this.columnHeaderBookId.Text = "Book Id";
            // 
            // columnHeaderAuthors
            // 
            this.columnHeaderAuthors.Text = "Authors";
            // 
            // columnHeaderTitle
            // 
            this.columnHeaderTitle.Text = "Title";
            // 
            // SelectBookControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewBooks);
            this.Controls.Add(this.buttonFilter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxIsbn);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.textBoxAuthors);
            this.Controls.Add(this.textBoxBookId);
            this.Controls.Add(this.label1);
            this.Name = "SelectBookControl";
            this.Size = new System.Drawing.Size(593, 222);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxBookId;
        private System.Windows.Forms.TextBox textBoxAuthors;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxIsbn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.ListView listViewBooks;
        private System.Windows.Forms.ColumnHeader columnHeaderBookId;
        private System.Windows.Forms.ColumnHeader columnHeaderAuthors;
        private System.Windows.Forms.ColumnHeader columnHeaderTitle;
    }
}
