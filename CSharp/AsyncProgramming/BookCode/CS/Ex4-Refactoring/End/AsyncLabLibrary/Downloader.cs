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
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Media;

namespace AsyncLabLibrary
{
    public class Downloader
    {
        private static async Task<LinkInfo> DownloadItemAsyncInternal(
            Uri itemUri, CancellationToken cancellationToken)
        {
            string item;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = 1000000;

                HttpRequestMessage message = new HttpRequestMessage(
                    HttpMethod.Get, itemUri);
                var response = await httpClient.SendAsync(
                    message, cancellationToken).ConfigureAwait(false);

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
            PollItemAsync(itemUri, linkInfo, cancellationToken);

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

        private static async void PollItemAsync(
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
                      cancellationToken).ConfigureAwait(false);

                    string item = await response.Content.ReadAsStringAsync();

                    if (item.Length != link.Length)
                    {
                        link.Title = GetTitle(item);
                        link.Length = item.Length;
                        link.Html = item;
                        link.Color = Color.FromArgb((byte)255, (byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256));
                    }
                }
            }
            catch
            {

            }
        }

        public static Task<LinkInfo> DownloadItemAsync(
            Uri itemUri, CancellationToken cancellationToken)
        {
            if (itemUri == null)
            {
                throw new ArgumentNullException("itemUri");
            }

            return DownloadItemAsyncInternal(itemUri, cancellationToken);
        }

        public static Task<LinkInfo> DownloadItemAsync(Uri itemUri)
        {
            return DownloadItemAsync(itemUri, CancellationToken.None);
        }

    }
}
