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

namespace MultiButtonClickRoutedEvent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClickedAButton(object sender, RoutedEventArgs e)
        {
            lblResult.Content = String.Format("Sender : {0}\nSource : {1}\nOriginal Source : {2}\n\n\nName : {3}\nTag : {4}", sender,e.Source, e.OriginalSource, ((Button)e.Source).Name, ((FrameworkElement)e.Source).Tag);
        }
    }
}
