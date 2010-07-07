using System;
using System.IO;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Librarian.WinForms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Initializes remoting.
            RemotingConfiguration.Configure(Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location),
                "Librarian.WinForms.exe.config"), false);

            // Load the GUI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainForm() );
        }
    }
}