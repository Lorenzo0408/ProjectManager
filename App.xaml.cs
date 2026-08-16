using ProjectManager.Model;
using ProjectManager.Services;
using ProjectManager.ViewModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // OnExit NE fait plus rien
            base.OnExit(e);
        }

    }

}
