using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SampleCallerInformation
{
    class Program
    {
        static void Main(string[] args)
        {
            CallAMethod();
        }

        private static void CallAMethod()
        {
            TraceMe("I am watching you....");
            CallBMethod();
        }

        private static void CallBMethod()
        {
            TraceMe("I am inside B Method");
        }
        static void TraceMe(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilepath = "",
            [CallerLineNumber]int sourceLineNumber = 0)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("Member name : {0} ",memberName);
            Console.WriteLine("Source file path : {0}", sourceFilepath);
            Console.WriteLine("Source line number : {0}",sourceLineNumber);
            Console.WriteLine("Message : {0}",message);
        }


    }
}
