using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._15_AsynchronyWithAsyncAwait
{
    class SelfWrittenAsync : Study
    {
        public override string StudyName
        {
            get { return "My first self written async function"; }
        }

        private async Task<int> GetDataAfter(int secondsToWait)
        {
            Console.WriteLine("INNER : Starting to generate data");
            await Task.Delay(TimeSpan.FromSeconds(secondsToWait));
            Console.WriteLine("INNER : After the Delay, now going to return result");
            return secondsToWait * 10;
        }

        protected override void PerformStudy()
        {
            Console.WriteLine("OUTER : Now starting performance");
            Task<int> resultTask = GetDataAfter(10);
            Console.WriteLine("OUTER : Returned with Task, going to wait now for the result");
            int result = resultTask.Result;
            Console.WriteLine("OUTER : Result : " + result);
            
        }
    }
}
