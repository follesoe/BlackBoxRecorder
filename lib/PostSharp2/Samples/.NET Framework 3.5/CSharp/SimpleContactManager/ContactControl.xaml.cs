using System.Windows;
using System.Windows.Controls;
using ContactManager.Aspects;
using ContactManager.Entities;

[assembly: ExceptionDialog( 
    AttributeTargetTypes = "ContactManager.*Control",
    AttributeTargetMembers = "*Click")]

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for ContactControl.xaml
    /// </summary>
    public partial class ContactControl : UserControl
    {
        private readonly Contact contact;

        public ContactControl()
        {
            InitializeComponent();
        }

        public ContactControl( Contact contact ) : this()
        {
            this.DataContext = contact;
            this.contact = contact;
        }

        [StatusText("Applying")]
        [Async]
        private void OnApplyClick( object sender, RoutedEventArgs e )
        {
                this.contact.Save();
            }

        [StatusText("Deleting")]
        [Async]
        private void OnDeleteClick( object sender, RoutedEventArgs e )
        {
                this.contact.Delete();
        }
    }
}