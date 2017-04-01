using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfChartAndRibbonStudy
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

        private void ChkRecentRequests_OnChecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(e.Source.ToString());
        }

        private void BtnExit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAbout_OnClick(object sender, RoutedEventArgs e)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            MessageBox.Show(String.Format("Comet Tracker\n\nVersion : {0}", asm.GetName().Version),"About", MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {

            if (e.Command == CustomCommands.ShowRecentRequests)
                chkRecentRequests.IsChecked = !chkRecentRequests.IsChecked;

        }
    }
}
