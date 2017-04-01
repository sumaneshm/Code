using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    class GenericsCompareToDefault : Study
    {
        public override string StudyName
        {
            get { return "Generics Compare To default"; }
        }

        private static int CompareToDefault<T>(T comparedAgainst) where T : IComparable<T>
        {
            return comparedAgainst.CompareTo(default(T));
        }

        class Person : IComparable<Person>
        {
            public string Name { get; set; }
            public int Age { get; set; }


            public int CompareTo(Person other)
            {

                if (other == null)
                    return -1;

                return Name.CompareTo(other.Name) + (Age == other.Age ? 0 : 1);
            }
        }

        protected override void PerformStudy()
        {
            Console.WriteLine("Compared against \"\" -> {0}", CompareToDefault(""));
            Console.WriteLine("Compared against String.Empty -> {0}",CompareToDefault(String.Empty));
            Console.WriteLine("Compared aginst 0 -> {0}",CompareToDefault(0));
            Console.WriteLine("Compared against 0.0 -> {0} ",CompareToDefault(0.0));
            Console.WriteLine("Compared against DateTime.MinValue -> {0}",CompareToDefault(DateTime.MinValue));

            Console.WriteLine("Compared against Person -> {0}", CompareToDefault(new Person{Name="Sumanesh",Age=32}));
        }
    }
}
