using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WindowCount
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public List<Document> Documents { set; get; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Documents = new List<Document>();
        }
    }
}
