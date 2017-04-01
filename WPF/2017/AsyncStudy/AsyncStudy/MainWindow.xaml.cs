using System.Net;
using System.Windows;

namespace AsyncStudy
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

        private void btnClickMe_Click(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
           // client.Open
        }
    }
}
