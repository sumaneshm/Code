using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._10_ExtensionMethods
{
    static class StreamUtils
    {
        private const int BatchSize = 8192;

        public static void MyCopyTo(this Stream input, Stream output)
        {
            byte[] buffer = new byte[BatchSize];

            int readCount;
            while ((readCount = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, readCount);
            }
        }

        public static byte[] ReadFully(this Stream input)
        {
            using (MemoryStream tempStream = new MemoryStream())
            {
                input.MyCopyTo(tempStream);
                return tempStream.ToArray();
            }
        }
    }

    class ExtendStreams : Study
    {
        public override string StudyName
        {
            get { return "A must have extension methods for Stream"; }
        }

        protected override void PerformStudy()
        {
            DrawHeader("Going to get the text from internet");

            WebRequest request = WebRequest.Create("http://www.google.com");
            using (WebResponse webResp = request.GetResponse())
            using (Stream respStream = webResp.GetResponseStream())
            using (FileStream fileStream = File.Create("C:\\temp\\GoogleText.txt"))
            {
                respStream.MyCopyTo(fileStream);
            }

            Console.WriteLine("Completed writing the text file");

        }
    }
}
