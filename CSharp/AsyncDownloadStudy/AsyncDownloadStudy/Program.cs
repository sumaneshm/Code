using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncDownloadStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Download();
        }

        static void Download() 
        {
        //    WebClient client = new WebClient();
        //    string page = client.DownloadString("http://www.google.com");
        //    Console.WriteLine("Downloaded");
        //    Console.WriteLine(page);
        //}

        //static async void DownloadAsync()
        //{
        //    WebClient client = new WebClient();
        //    string page = await client.DownloadDataAsync("http://www.google.com");
            MethodB();
        }

        private static void MethodB()
        {
            MethodC();
        }

        private static void MethodC()
        {
            MethodD();
        }

        private static void MethodD()
        {
            Console.WriteLine("Nothing");
        }
    }
}
