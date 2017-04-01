using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRViaCSharp.Common
{
    public static class ExtensionMethods
    {
        public static void Separate(this char repeatable, int count = 50)
        {
            Console.WriteLine("{0}", new string(repeatable, count));
        }

        public static string Repeat(this string repeatable, int count)
        {
            return string.Concat(Enumerable.Repeat(repeatable, count));
        }
    }
}
