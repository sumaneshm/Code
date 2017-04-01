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

namespace C11_StyleEventHandlerStudy
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

        public void element_MouseEnter(object sender, MouseEventArgs m)
        {
            TextBlock txt = (TextBlock)sender;
            txt.Background = new SolidColorBrush(Colors.Red);

        }

        public void element_MouseLeave(object sender, MouseEventArgs m)
        {

            TextBlock txt = (TextBlock)sender;
            txt.Background = null;
        }
    }
}
