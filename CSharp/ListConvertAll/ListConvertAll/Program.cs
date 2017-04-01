using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListConvertAll
{
    class Program
    {
        static double TakeSquareRoot(int i)
        {
            return Math.Sqrt(i);
        }

        static void Main(string[] args)
        {
            List<int> integers = new List<int>() { 1, 2, 3, 4, 5 };

            Converter<int, double> squareRootConverter = TakeSquareRoot;

            List<double> squareRoots = integers.ConvertAll(squareRootConverter);

            foreach(var oneSqRt in squareRoots)
            {
                Console.WriteLine(oneSqRt);
            }
        }
    }
}
