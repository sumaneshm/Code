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

namespace KeyPressEventStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            txtInput.PreviewKeyDown += KeyEvents;
            txtInput.PreviewTextInput += KeyEvents;
            txtInput.PreviewKeyUp += KeyEvents;
            txtInput.KeyDown += KeyEvents;
            txtInput.TextInput += KeyEvents;
            txtInput.KeyUp += KeyEvents;

        }

        private int count = 1;

        public void KeyEvents(object sender, RoutedEventArgs args)
        {
            if (args is KeyEventArgs)
                lstMessages.Items.Add(string.Format("#{0}\nKey : {1}\nEvent : {2}", count++, ((KeyEventArgs)args).Key, args.RoutedEvent));
            else
                lstMessages.Items.Add(String.Format("Something else {0}",args.RoutedEvent));
        }
    }
}
