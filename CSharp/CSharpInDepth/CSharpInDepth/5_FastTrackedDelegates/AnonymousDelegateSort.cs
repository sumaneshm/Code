using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._5_FastTrackedDelegates
{
    // Listing 5.8
    // This is C# 2.0 style of coding

    class AnonymousDelegateSort : Study
    {
        public override string StudyName
        {
            get { return "Shows how to pass anonymous methods as parameters"; }
        }

        private static void MyOwnSort(string folderPath, Comparison<FileInfo> sortOrder)
        {
            FileInfo[] files = new DirectoryInfo(folderPath).GetFiles();
            Array.Sort(files, sortOrder);

            foreach(FileInfo oneFile in files)
            {
                Console.WriteLine("{0}-{1}",oneFile.Name,oneFile.Length);
            }
        }

        protected override void PerformStudy()
        {
            string folderPath = @"C:\";

            Console.WriteLine("Now sorting by Name");

            MyOwnSort(folderPath, delegate(FileInfo a, FileInfo b)
            {
                return String.Compare(a.Name , b.Name);
            });

            Console.WriteLine("Now sorting by Legnth");
            MyOwnSort(folderPath, delegate(FileInfo a, FileInfo b)
            {
                return a.Length.CompareTo(b.Length);
            });
        }
    }
}
