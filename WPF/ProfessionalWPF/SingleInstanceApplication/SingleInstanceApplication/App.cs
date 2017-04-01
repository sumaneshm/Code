using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SingleInstanceApplication
{
    public class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            App.Current = this;
            
        }
    }
}
