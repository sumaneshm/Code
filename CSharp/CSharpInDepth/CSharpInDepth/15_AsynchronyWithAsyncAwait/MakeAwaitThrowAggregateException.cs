using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._15_AsynchronyWithAsyncAwait
{

    //New async feature in C# 5.0 does not expect Awaitable class to implement any special interface to make it "awaitable", instead it is pattern based like foreach and Linq.
    //it just expects that there is a magic method called GetAwaiter which will give back an Awaiter(yet another magic pattern type)

    public class CustomAwaitable
    {
        private Task task;

        public CustomAwaitable(Task task)
        {
            this.task = task;
        }

        public CustomAwaiter GetAwaiter()
        {
            return new CustomAwaiter(task);
        }
    }

    // Compiler expects the Awaiter should have few details as shown 
    // 1. The Awaiter should have implemented INotifyCompletion (only for continuation purpose)
    // 2. It should have a bool IsCompleted property (Not necessarily a public, but should be accessible from the code) which uses it (magic property)
    // 3. It should have GetResult method (magic method)

    public class CustomAwaiter : INotifyCompletion
    {
        private Task task;

        public CustomAwaiter(Task task)
        {
            this.task = task;
        }

        public bool IsCompleted
        {
            get { return task.GetAwaiter().IsCompleted; }
        }

        public void GetResult()
        {
            task.Wait();
        }

        public void OnCompleted(Action continuation)
        {
            task.GetAwaiter().OnCompleted(continuation);
        }
    }


    public static class CustomTaskExtensionFunctions
    {
        public static CustomAwaitable UseCustomAwaitable(this Task task)
        {
            return new CustomAwaitable(task);
        }
    }

    class MakeAwaitThrowAggregateException : Study
    {
        // By default "await"ing an async method will get the first exception rather than AggregateException and this would mean that other Exceptions are not catchable...
        // There is a fix for that...

        public override string StudyName
        {
            get { return "A minor change to the way how 'await' Exceptions are thrown"; }
        }

        private async static Task CatchMultipleExceptions()
        {
            Task task1 = Task.Run(() => { throw new Exception("Message 1"); });
            Task task2 = Task.Run(() => { throw new Exception("Message 2"); });

            try
            {
                // the following line will throw the first exception and not AggregateException
                await Task.WhenAll(task1, task2);

                // Following line would throw AggregateException, thanks to UseCustomAwaitable extension method
                //await Task.WhenAll(task1, task2).UseCustomAwaitable();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Aggregate Exception caught. Message : " + string.Join(",", ae.InnerExceptions.Select(i => i.Message)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught. Message : " + ex.Message);
            }

            Console.WriteLine("Task1 status : " + task1.Status);
            Console.WriteLine("Task2 status : " + task2.Status);

            Console.WriteLine("Task1, exception : " + task1.Exception.InnerException.Message);
            Console.WriteLine("Task2, exception : " + task2.Exception.InnerException.Message);
        }

        protected override void PerformStudy()
        {
            var task = CatchMultipleExceptions();

            task.Wait();
        }
    }
}
