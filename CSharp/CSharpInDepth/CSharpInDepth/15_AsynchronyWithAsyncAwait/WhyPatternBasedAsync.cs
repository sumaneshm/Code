using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpInDepth._15_AsynchronyWithAsyncAwait
{

    // http://blogs.msdn.com/b/pfxteam/archive/2011/01/13/10115642.aspx

    // "Magic" methods
    public static class MyExtensionMethods
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timespan)
        {
            return Task.Delay(timespan).GetAwaiter();
        }

        public static TaskAwaiter GetAwaiter(this IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks).GetAwaiter();
        }


        public static TaskAwaiter GetAwaiter(this CancellationToken cancelToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            Task t = tcs.Task;

            if(cancelToken.IsCancellationRequested)
            {
                tcs.SetResult(true);
            }
            else
            {
                cancelToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            }

            return t.GetAwaiter();
        }
    }

    class WhyPatternBasedAsync : Study
    {

        public override string StudyName
        {
            get { return "Explains why Pattern based "; }
        }

        private async Task DoSomethingWithThis(string abc)
        {
            Console.WriteLine("Doing something with " + abc);
            await Task.Delay(1);
        }

        private async Task Demonstrate()
        {
            // await expects that "expression" has a magic method called "GetAwaiter" (which can even be supplied by extension method as explained in this case) which returns a type
            // which contains few more magic patterns
            // If you see as we can "extend" anything to return "Awaiter" special case, it has been implemented as a pattern based rather than specific interface driven.
            // More details on http://blogs.msdn.com/b/pfxteam/archive/2011/01/13/10115642.aspx for more justifications

            Console.WriteLine("Going to wait for few seconds directly controlled by TimeSpan...");
            await TimeSpan.FromSeconds(3);

            Console.WriteLine("Going to perform somethign directly and wait for all of them");
            await from s in new List<string>{"a","b","c"} select DoSomethingWithThis(s);

            Console.WriteLine("This will automatically complete when the token is cancelled in few seconds");
            CancellationTokenSource source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(5));
            CancellationToken canTok = source.Token;

            await canTok;

            Console.WriteLine("All done");
        }

        protected override void PerformStudy()
        {
            var t = Demonstrate();
            t.Wait();
        }
    }
}
