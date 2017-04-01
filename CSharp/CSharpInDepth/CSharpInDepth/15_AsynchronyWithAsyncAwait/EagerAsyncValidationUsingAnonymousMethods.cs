using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._15_AsynchronyWithAsyncAwait
{
    class EagerAsyncValidationUsingAnonymousMethods : Study
    {
        public override string StudyName
        {
            get { return "Explains how to perform eager validation and use anonymous async mehod"; }
        }

        private Task EagerValidation(String data)
        {
            if(string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException("data");
            }

            Func<Task> func = 
                async () => 
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                };

            return func();
        }


        private async Task NormalValidation(String data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException("data");
            }

            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        protected override void PerformStudy()
        {
            // Eager validation employs async anonymous function and will perform validation early
            try
            {
                var t = EagerValidation(null);    
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Eager ArgumentNullException : " + ex.Message);
            }

            // Note that the call is outside the try/catch block as it is using normal validaiton
            var nt = NormalValidation(null);

            try
            {
                //Exception will be thrown only when we wait on the async task
                nt.Wait();
            }
            catch(AggregateException ex)
            {
                Console.WriteLine("Normal AggregateException.InnerException : " + ex.InnerException.Message);
            }
        }
    }
}
