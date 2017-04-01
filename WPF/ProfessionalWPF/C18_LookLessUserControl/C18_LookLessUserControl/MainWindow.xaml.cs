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

namespace C18_LookLessUserControl
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cp.Color = Colors.Chocolate;
        }

        private void cp_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if(lblColor != null)
                lblColor.Content = "Current color : " + cp.Color.ToString();
        }
    }
}
