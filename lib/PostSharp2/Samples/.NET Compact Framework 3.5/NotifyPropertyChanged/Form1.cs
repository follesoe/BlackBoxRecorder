using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PostSharp;

namespace NotifyPropertyChanged
{
    public partial class Form1 : Form
    {
        private readonly Contact contact = new Contact {FirstName = "Yuri", LastName = "Gagarin"};

        public Form1()
        {
            InitializeComponent();

            this.firstNameTextBox.Text = contact.FirstName;
            this.lastNameTextBox.Text = contact.LastName;

            Post.Cast<Contact, INotifyPropertyChanged>( this.contact ).PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            this.eventsListBox.Items.Add( string.Format( "Property changed: {0}", e.PropertyName ) );
        }


        private void firstNameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.contact.FirstName = this.firstNameTextBox.Text;

        }

        private void lastNameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.contact.LastName = this.lastNameTextBox.Text;
        }
    }
}