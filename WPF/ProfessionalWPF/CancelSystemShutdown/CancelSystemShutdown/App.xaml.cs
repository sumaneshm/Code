using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace CancelSystemShutdown
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool UnsavedData { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

        }
    }
}
