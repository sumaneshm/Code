using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace MutexStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            //MutexStudy();
            YieldStudy();
        }

        private static void MutexStudy()
        {
            using (var mutex = new Mutex(false, "Sumanesh"))
            {
                Console.WriteLine("Going to get the mutex");
                if (!mutex.WaitOne(TimeSpan.FromSeconds(10), false))
                {
                    Console.WriteLine("Another application is not yet dead");
                    return;
                }

                Console.WriteLine("I am still running");

                Console.ReadLine();
                mutex.ReleaseMutex();
            }
        }

        private static void YieldStudy()
        {
            DaysOfTheWeek days = new DaysOfTheWeek();

            foreach (string day in days)
            {
                Console.Write(day + " ");
            }
            // Output: Sun Mon Tue Wed Thu Fri Sat
            Console.ReadKey();
        }

    }


    public class DaysOfTheWeek : IEnumerable
    {
        private string[] days = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

        public IEnumerator GetEnumerator()
        {
            for (int index = 0; index < days.Length; index++)
            {
                // Yield each day of the week. 
                yield return days[index];
            }
        }
    }
}
