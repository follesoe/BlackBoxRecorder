using System.ComponentModel;
using System.Windows.Controls;
using ContactManager.Aspects;
using ContactManager.Entities;
using PostSharp;

namespace ContactManager
{
    internal class ContactTreeNode : TreeViewItem
    {
        private bool deleted;

        public ContactTreeNode( Contact contact )
        {
            this.Contact = contact;

            Post.Cast<Contact, INotifyPropertyChanged>( this.Contact ).PropertyChanged += OnContactChanged;

            this.SetHeader();
        }

        public Contact Contact { get; private set; }


        private void SetHeader()
        {
            string header = string.Format( "{0} {1}", this.Contact.FirstName,
                                           this.Contact.LastName );
            if ( !string.IsNullOrEmpty( this.Contact.Company ) )
                header += string.Format( " ({0})", this.Contact.Company );

            this.Header = header;
        }

        [Dispatch]
        private void OnContactChanged( object sender, PropertyChangedEventArgs e )
        {
            if ( this.Contact.IsDeleted )
            {
                if ( !this.deleted )
                {
                    deleted = true;
                    ((ItemsControl) this.Parent).Items.Remove( this );
                }
            }
            else
            {
                this.SetHeader();
            }
        }
    }
}