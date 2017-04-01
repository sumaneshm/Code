using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Common
{
    internal static class Logger
    {
        [DebuggerHidden()]
        internal static void Log(object message, [CallerMemberName] string caller = null)
        {
            Console.WriteLine("{0}\n{1}", caller, message);
        }
    }
}
