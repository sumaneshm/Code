using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._15_AsynchronyWithAsyncAwait
{
    class ExceptionHandlingInAsync : Study
    {
        public override string StudyName
        {
            get { return "Explains how Exception handling works for async case"; }
        }

        private async Task ThrowAnException()
        {
            Console.WriteLine("Going to throw ArgumentNullException in one second...");
            await Task.Delay(1000);
            throw new ArgumentNullException();
        }

        //When we "Await" for any async function, if there is any exception in the called funciton, it would not throw an AggregateException
        //instead it will throw the first exception so that the code looks verymuch like synchronous code...
        private async Task AwaitThrowsFirstActualException()
        {
            try
            {
                await ThrowAnException();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("Caught an Argument Null Exception");
            }
        }

        private void DemonstrateTaskBasedExceptionWithAction<T>(string title, Action<Task<T>> action)
        {
            Console.WriteLine("{0} ->  Starting", title);

            // Note that this call is outside the try/catch block.
            var task = ThrowAnException();

            //Exception on the async method will not be thrown here until we 

            try
            {
                task.Wait();

            }
            catch (AggregateException ae)
            {
                Console.WriteLine("{0} -> Aggregated exception is caught.", title);
                Console.WriteLine("{0} -> Inner exception : {1}", title, ae.InnerException);
            }

            Console.WriteLine("{0} ->  Task status : {1}", title, task.Status);
        }

        private void TaskWaitThrowsAggregatedException()
        {
            Console.WriteLine("Going to call the asynch method which will throw an exception");


            // The exception will be thrown only when we call the 
            //      Wait method called
            //      Trying to access the Result

            DemonstrateTaskBasedExceptionWithAction<int>("Wait", t => t.Wait());
            DemonstrateTaskBasedExceptionWithAction<int>("Result", t => Console.WriteLine(t.Result));

        }

        protected override void PerformStudy()
        {
            //var t = AwaitThrowsFirstActualException();
            //t.Wait();
            //Console.WriteLine("Status : " + t.Status);

            TaskWaitThrowsAggregatedException();
        }
    }
}
