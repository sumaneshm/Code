using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CodeOnlyWPF
{
    class Program : Application
    {
        [STAThread]
        static void Main(string[] args)
        {
            Program app = new Program();
            app.MainWindow = new Window1();
            app.MainWindow.ShowDialog();
            
        }
    }
}
