using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasicCommandStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommandBinding cmdBinding = new CommandBinding(ApplicationCommands.New, new ExecutedRoutedEventHandler(delegate(object o, ExecutedRoutedEventArgs args)
                {
                    MessageBox.Show("New Command was called by " + args.Source + " clicked by " + args.Parameter);
                }));

            this.CommandBindings.Add(cmdBinding);

        }
    }
}
