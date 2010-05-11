using System.Windows;
using ContactManager.Framework;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool IsDesignTime { get; set; }

        static App()
        {
            IsDesignTime = true;
        }

        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );
            IsDesignTime = false;
            Client.Initialize();
        }
    }
}