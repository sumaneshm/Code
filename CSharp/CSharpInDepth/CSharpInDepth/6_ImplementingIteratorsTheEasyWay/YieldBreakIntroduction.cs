using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpInDepth._6_ImplementingIteratorsTheEasyWay
{
    class YieldBreakIntroduction : Study
    {
        int[] data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        IEnumerable<int> GetCollection(DateTime limit)
        {
            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (DateTime.Now > limit)
                    {
                        Console.WriteLine("Yield completed, now breaking...");
                        yield break;
                    }
                    yield return data[i];
                }
            }
            finally
            {
                Console.WriteLine("Finally...");
            }
        }

        public override string StudyName
        {
            get { return "An introduction to understand how Yield break works"; }
        }

        protected override void PerformStudy()
        {
            //foreach - Explicitly calls Dispose method and hence the code in finally block will get executed...
            foreach(int i in GetCollection(DateTime.Now.AddSeconds(3)))
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }

            //When we manually loop through the iterator class, it wont call finally, unless we explicitly call the Dispose method
            var itr = GetCollection(DateTime.Now.AddSeconds(5)).GetEnumerator();
            itr.MoveNext();
            Console.WriteLine(itr.Current);
            Thread.Sleep(1000);
            itr.MoveNext();
            Console.WriteLine(itr.Current);
            Thread.Sleep(1000);
            itr.MoveNext();
            Console.WriteLine(itr.Current);

            Console.WriteLine("Completed");
        }
    }
}
