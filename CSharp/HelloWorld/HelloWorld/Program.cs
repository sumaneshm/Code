using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Method1();         // 1. Set a breakpoint here
            // 2. Then step into Method1 
            int y = Method2(); // 3. Set a breakpoint here
            // 4. Then step into Method2 
        }

        static void Method1()
        {
            // 1. Step over the following line
            int result = Multiply(FourTimes(Five()), Six());
            // 2. Then view the return values in the Autos window
        }

        static int Method2()
        {
            // 1. Step over the following line
            return Five();
            // 2. Then view the return values in the Autos window
        }

        static int Multiply(int x, int y)
        {
            return x * y;
        }

        static int FourTimes(int x)
        {
            return 4 * x;
        }

        static int Five()
        {
            return 5;
        }

        static int Six()
        {
            return 6;
        }
    }
}
