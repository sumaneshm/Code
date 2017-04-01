using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._14_DynamicBindingInAStaticLanguage
{
    public static class IntegerExtensions
    {
        public static TimeSpan Hours(this int h)
        {
            return new TimeSpan(h, 0, 0);
        }

        public static TimeSpan Minutes(this int m)
        {
            return new TimeSpan(0, m, 0);
        }

        public static TimeSpan Seconds(this int s)
        {
            return new TimeSpan(0, 0, s);
        }
    }

    class DynamicAddition : Study
    {
        public override string StudyName
        {
            get { return "Dynamically add items"; }
        }

        private T Add<T>(List<T> itemsToAdd)
        {
            dynamic toReturn = default(T);

            // If T is a byte, without the below typecasting to T it will fail with an exception as C#
            // will convert it to integer before addition and the subsequent addition will fail
            // we can either typecast while additing it or finally

            foreach (T item in itemsToAdd)
            {
                toReturn = (item + toReturn);
            }

            return (T)toReturn;
        }

        protected override void PerformStudy()
        {
            Console.WriteLine(Add(new List<string> { "A", "B", "C", "D" }));

            // will roll it to 4
            Console.WriteLine(Add(new List<byte>{255,5}));

            List<TimeSpan> times = new List<TimeSpan>
            {
                5.Hours(), 30.Minutes(), 10.Seconds()
            };

            Console.WriteLine(Add(times));
        }
    }
}
