using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ch04_LanguageFeatures.Models
{
    public class LongRunningTask
    {
        public async static Task<long?> GetPageLength()
        {
            HttpClient client = new HttpClient();
            var httpMessage = await client.GetAsync("http://www.apress.com");
            return httpMessage.Content.Headers.ContentLength;

        }
    }
}
