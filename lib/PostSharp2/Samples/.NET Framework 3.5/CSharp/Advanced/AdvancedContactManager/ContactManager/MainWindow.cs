using System.Windows;
using System.Windows.Controls;
using ContactManager.Entities;
using ContactManager.Framework;

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
            Client.Initialize();
            InitializeComponent();
            this.DataContext = this;
        }

        public ContactSynchronizedCollection Contacts
        {
            get { return ContactSynchronizedCollection.Instance; }
        }


        private void OnAddContactClick( object sender, RoutedEventArgs e )
        {
            this.detailPanel.Children.Clear();
            this.detailPanel.Children.Add( new ContactControl( new Contact {FirstName = "New", LastName = "Contact"} ) );
        }

        private void OnContactSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            this.detailPanel.Children.Clear();
            this.detailPanel.Children.Add( new ContactControl( (Contact) this.contactListBox.SelectedItem ) );
        }
    }
}