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
using System.Threading;

namespace AsyncLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancellationToken;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            cancellationToken = new CancellationTokenSource();
            ObservableCollection<string> uris = new ObservableCollection<string>();
            startButton.IsEnabled = false;

            try
            {
                var message = new HttpRequestMessage(
                    HttpMethod.Get, "http://msdn.microsoft.com");
                var response = await new HttpClient().SendAsync(message,
                    cancellationToken.Token);

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

                listBox1.ItemsSource = await Task.Run(() =>
                {
                    return Task.WhenAll(from uri in uris
                                        select DownloadItemAsync(
                                        new Uri(uri), cancellationToken.Token));
                });

            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation Cancelled");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            startButton.IsEnabled = true;
        }

        private static async Task<LinkInfo> DownloadItemAsync(
            Uri itemUri, CancellationToken cancellationToken)
        {
            string item;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = 1000000;

                var message = new HttpRequestMessage(HttpMethod.Get, itemUri);
                var response = await httpClient.SendAsync(message, cancellationToken);

                item = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                item = string.Empty;
            }

            LinkInfo linkInfo = new LinkInfo
            {
                Length = item.Length,
                Title = GetTitle(item),
                Html = item
            };
            PollItem(itemUri, linkInfo, cancellationToken);

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

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            cancellationToken.Cancel();
        }

        private static async void PollItem(
            Uri itemUri, LinkInfo link, CancellationToken cancellationToken)
        {
            Random r = new Random();
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 1000000;

            try
            {
                while (true)
                {
                    await Task.Delay(5000, cancellationToken);
                    HttpRequestMessage requestMessage = new 
                        HttpRequestMessage(HttpMethod.Get, itemUri);
                    var response = await httpClient.SendAsync(requestMessage,
                        cancellationToken);

                    string item = await response.Content.ReadAsStringAsync();

                    if (item.Length != link.Length)
                    {
                        link.Title = GetTitle(item);
                        link.Length = item.Length;
                        link.Html = item;
                        link.Color = Color.FromArgb((byte)255, (byte)r.Next(256),
                            (byte)r.Next(256), (byte)r.Next(256));
                    }
                }
            }
            catch
            {

            }
        }

    }
}
