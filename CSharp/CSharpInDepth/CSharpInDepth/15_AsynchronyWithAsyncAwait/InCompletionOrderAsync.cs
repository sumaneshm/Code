using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpInDepth._15_AsynchronyWithAsyncAwait
{
    public static class EnumerableTaskExtensions
    {
        public static IEnumerable<Task<T>> InCompletionOrder<T>(this IEnumerable<Task<T>> source)
        {
            var inputs = source.ToList();
            var boxes = inputs.Select(x => new TaskCompletionSource<T>()).ToList();

            int currentIndex = -1;
            foreach (var oneTask in inputs)
            {
                oneTask.ContinueWith(completed =>
                {
                    var nextBox = boxes[Interlocked.Increment(ref currentIndex)];
                    PropagateResult(completed, nextBox);

                }, TaskContinuationOptions.ExecuteSynchronously);
            }

            return boxes.Select(x => x.Task);
        }

        private static void PropagateResult<T>(Task<T> completedTask, TaskCompletionSource<T> completionSource)
        {
            switch (completedTask.Status)
            {
                case TaskStatus.Canceled:
                    completionSource.TrySetCanceled();
                    break;
                case TaskStatus.Faulted:
                    completionSource.TrySetException(completedTask.Exception.InnerExceptions);
                    break;
                case TaskStatus.RanToCompletion:
                    completionSource.TrySetResult(completedTask.Result);
                    break;
                default:
                    // TODO: Work out whether this is really appropriate. Could set 
                    // an exception in the completion source, of course... 
                    throw new ArgumentException("Task was not completed");
            }
        }

    }


    class InCompletionOrderAsync : Study
    {
        static async Task<int> ShowPageLengthsAsync(params string[] urls)
        {
            var tasks = urls.Select(async url =>
            {
                using (var client = new HttpClient())
                {
                    return new KeyValuePair<string,string> (url, await client.GetStringAsync(url));
                }
            }).ToList();

            int total = 0;
            foreach (var task in tasks.InCompletionOrder())
            {
                KeyValuePair<string, string> urlpage = await task;
                Console.WriteLine("Got page length {0}  for the url {1}", urlpage.Value.Length, urlpage.Key);
                total += urlpage.Value.Length;
            }
            return total;
        }


        public override string StudyName
        {
            get { return "Returns Task results in the order of completion"; }
        }


        protected override void PerformStudy()
        {
            
            var task = ShowPageLengthsAsync("http://stackoverflow.com", "http://www.google.com", "http://csharpindepth.com","http://www.yahoo.com","http://www.google.com","http://www.barclays.com");
            Console.WriteLine("Total length: {0}", task.Result);
        }
    }
}
