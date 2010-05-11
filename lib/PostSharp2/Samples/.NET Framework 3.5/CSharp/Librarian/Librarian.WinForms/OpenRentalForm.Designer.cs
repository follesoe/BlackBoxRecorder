namespace Librarian.WinForms
{
    partial class OpenRentalForm
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
            this.selectBookControl1 = new SelectBookControl();
            this.labelScheduleDate = new System.Windows.Forms.Label();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectBookControl1
            // 
            this.selectBookControl1.Location = new System.Drawing.Point(13, 13);
            this.selectBookControl1.Name = "selectBookControl1";
            this.selectBookControl1.SelectedBook = null;
            this.selectBookControl1.Size = new System.Drawing.Size(593, 222);
            this.selectBookControl1.TabIndex = 0;
            // 
            // labelScheduleDate
            // 
            this.labelScheduleDate.AutoSize = true;
            this.labelScheduleDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScheduleDate.Location = new System.Drawing.Point(34, 242);
            this.labelScheduleDate.Name = "labelScheduleDate";
            this.labelScheduleDate.Size = new System.Drawing.Size(190, 19);
            this.labelScheduleDate.TabIndex = 1;
            this.labelScheduleDate.Text = "Return Date: 00/00/0000";
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(424, 242);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 2;
            this.buttonAccept.Text = "Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(505, 241);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // OpenRentalForm
            // 
            this.AcceptButton = this.buttonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(626, 286);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.labelScheduleDate);
            this.Controls.Add(this.selectBookControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenRentalForm";
            this.ShowInTaskbar = false;
            this.Text = "Open Rental";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SelectBookControl selectBookControl1;
        private System.Windows.Forms.Label labelScheduleDate;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonCancel;
    }
}
