using System.Windows;
using ContactManager.Aspects;

namespace ContactManager
{
    public static class MessageBoxHelper
    {
        [Dispatch]
        public static void Display( DependencyObject owner, string message )
        {
            Window window = owner == null ? null : Window.GetWindow( owner );

            // Display the dialog box.
            if ( window == null )
                MessageBox.Show( message, "Exception" );
            else
                MessageBox.Show( window, message, "Exception" );
        }
    }
}