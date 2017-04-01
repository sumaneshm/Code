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

namespace C10_StaticDynamicResourcesButtonStudy
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source == btnStaticStudy)
            {
                ImageBrush img = (ImageBrush)this.Resources["TileBrush"];
                img.Viewport = new Rect(0, 0, 5, 5);
            }
            else if (e.Source == btnDynamicStudy)
            {
                this.Resources["TileBrush"] = new SolidColorBrush(Colors.Blue);
            }
        }
    }
}
