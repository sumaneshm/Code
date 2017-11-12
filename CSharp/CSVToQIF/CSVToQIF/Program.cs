using CSVToQIF.Model;
using System;
using System.IO;

namespace CSVToQIF
{
    class Program
    {
        static string today = ".\\" + DateTime.Today.ToString("ddMMyyyy");

        static void Main(string[] args)
        {
            CsvQifConverter converter = new CsvQifConverter();

            Console.WriteLine("CSV to Qif converter");

            if (!CheckUsage(args))
            {
                Console.WriteLine("Check the error message and rerun again");
            }
            else
            {

                if (args.Length == 0)
                {
                    Console.WriteLine($"Trying to convert all the files in {today} folder");
                    foreach (var file in Directory.GetFiles(today, "*.csv"))
                    {
                        Console.WriteLine($"Converting...{file}");
                        converter.Convert(file);
                    }
                }
                else
                {
                    Console.WriteLine($"Converting...{args[0]}");
                    converter.Convert(args[0]);
                }
                Console.WriteLine("Completed processing");
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }

        private static bool CheckUsage(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("USAGE: ");
                Console.WriteLine("\n\n\tCSVToQIF \n\tWithout any parameter, we will look for a folder with the name ddMMYYYY and converts all csv inside that");
                Console.WriteLine("\n\n\tCSVToQIF <<csv file to transform>>\n\tIf you want to convert a specific csv file");

                return false;
            }

            if (args.Length == 1)
            {
                var fi = new FileInfo(args[0]);
                if (!fi.Exists)
                {
                    Console.WriteLine("File doesn't exist");
                    return false;
                }

                if (string.Compare(fi.Extension, ".csv", true) != 0)
                {
                    Console.WriteLine("Only csv file is supported");
                    return false;
                }
            }
            else
            {
                if(!Directory.Exists(today))
                {
                    Console.WriteLine($"{today} folder doesn't exist");
                    return false;
                }
            }

            return true;
        }
    }
}
