using System;
using System.Windows;
using System.Windows.Controls;
using ContactManager.Aspects;
using ContactManager.Entities;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
        }

        private void OnAddContactClick( object sender, RoutedEventArgs e )
        {
            Contact contact = new Contact( "New", "Contact" );
            contactListBox.Items.Add(contact);
            contactListBox.SelectedItem = contact;
        }

        protected override void OnInitialized( EventArgs e )
        {
            base.OnInitialized( e );

            Entity.Initialize();

            foreach ( Contact contact in Contact.GetContacts() )
            {
                contactListBox.Items.Add( contact );
            }
        }

        [Dispatch]
        public string SetStatusText( string text )
        {
            string previousText = this.pendingOperationStatusBarItem.Content as string;
            this.pendingOperationStatusBarItem.Content = text ?? "Ready";
            return previousText;
        }

        private void OnRefreshClick( object sender, RoutedEventArgs e )
        {
            contactListBox.Items.Clear();
            foreach ( Contact contact in Contact.GetContacts() )
            {
                contactListBox.Items.Add( contact );
            }
        }

        private void OnRepopulateClick( object sender, RoutedEventArgs e )
        {
            Entity.Repopulate();
            MessageBox.Show( "You need to restart the application." );
            this.Close();
        }


        private void OnUndoClick( object sender, RoutedEventArgs e )
        {
            if ( UndoManager.CurrentItem != null )
            {
                UndoManager.Undo( UndoManager.CurrentItem );
            }
        }

        private void OnRedoClick( object sender, RoutedEventArgs e )
        {
            if ( UndoManager.CurrentItem != null && UndoManager.CurrentItem.Next != null )
            {
                UndoManager.Redo( UndoManager.CurrentItem.Next );
            }
        }

        private void ContactListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Children.Clear();

            Contact contact = contactListBox.SelectedItem as Contact;
            if (contact != null)
            {
                detailPanel.Children.Add(new ContactControl(contact));
            }
        }
    }
}