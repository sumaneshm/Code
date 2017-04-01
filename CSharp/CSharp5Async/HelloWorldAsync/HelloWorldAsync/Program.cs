using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HelloWorldAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main : Starting the call now from main method");
            CallLongRunningMethodAsync();

            Console.WriteLine("Main : Immediately after this");
            Console.WriteLine("Main : going to sleep now");
            Thread.Sleep(5000);
            Console.WriteLine("Main : Completed the call");

        }

        public async static void CallLongRunningMethodAsync()
        {
            Console.WriteLine("Caller : Going to call now");

            int result = await Task.Run(() =>
            {
                return LongRunningMethod();
            });
            
            Console.WriteLine("Caller : Completed the call");
        }


        public static int LongRunningMethod()
        {
            Console.WriteLine("Callee : Long Running Method started...");
            Thread.Sleep(3000);
            Console.WriteLine("Callee : Long running method completed...");
            return 100;
        }
    }
}
