using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Text.RegularExpressions;
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

namespace AsyncLab
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

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> uris = new List<string>();

            startButton.IsEnabled = false;

            try
            {
                //-------------------Sync way ------------------------------//
                //WebClient client = new WebClient();

                //string result = client.DownloadString("http://msdn.microsoft.com");

                //textBox1.Text = result;

                //MatchCollection mc = Regex.Matches(result, "href\\s*=\\s*(?:\"(?<1>http://[^\"]*)\")", RegexOptions.IgnoreCase);

                //foreach (Match m in mc)
                //{
                //    uris.Add(m.Groups[1].Value);
                //}

                //listBox1.ItemsSource = uris;

                //------------------- Async way ------------------------------//
                var client = await new HttpClient().GetAsync("http://msdn.microsoft.com");

                string result = await client.Content.ReadAsStringAsync();

                textBox1.Text = result;

                MatchCollection mc = Regex.Matches(result, "href\\s*=\\s*(?:\"(?<1>http://[^\"]*)\")", RegexOptions.IgnoreCase);

                foreach (Match m in mc)
                {
                    uris.Add(m.Groups[1].Value);
                }

                listBox1.ItemsSource = uris;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            startButton.IsEnabled = true;
        }
    }
}
