// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

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
            ObservableCollection<string> uris = new ObservableCollection<string>();
            startButton.IsEnabled = false;

            try
            {
                var response = await new HttpClient().GetAsync("http://msdn.microsoft.com");
                string result = await response.Content.ReadAsStringAsync();

                textBox1.Text = result;

                await Task.Run(() =>
                {
                    MatchCollection mc = Regex.Matches(result, "href\\s*=\\s*(?:\"(?<1>http://[^\"]*)\")", RegexOptions.IgnoreCase);
                    foreach (Match m in mc)
                    {
                        uris.Add(m.Groups[1].Value);
                    }
                });

                listBox1.ItemsSource = await Task.WhenAll(
                    from uri in uris select DownloadItemAsync(new Uri(uri)));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            startButton.IsEnabled = true;
        }

        private static async Task<LinkInfo> DownloadItemAsync(Uri itemUri)
        {
            string item;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = 1000000;

                var response = await httpClient.GetAsync(itemUri);
                item = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                item = string.Empty;
            }

            LinkInfo linkInfo = new LinkInfo { 
                Length = item.Length, Title = GetTitle(item), Html = item };
            return linkInfo;
        }

        private static string GetTitle(string html)
        {
            if (html.Length == 0)
            {
                return "Not Found";
            }

            Match m = Regex.Match(html,
                @"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase);

            return m.Value;
        }

    }
}
