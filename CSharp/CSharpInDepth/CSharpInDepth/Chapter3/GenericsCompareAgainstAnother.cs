using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    class GenericsCompareAgainstAnother : Study
    {
        public override string StudyName
        {
            get { return "Generics comparison against another type"; }
        }

        private bool GenericsCompare<T>(T first, T second) where T : class
        {
            return first == second;
        }

        protected override void PerformStudy()
        {
            string a, b;

            string name = "Aadhavan";
            a = "My son's name is " + name;
            b = "My son's name is " + name;

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Compare against concatenated strings");
            Compare(a, b);

            a = "My son's name is Aadhavan";
            b = "My son's name is Aadhavan";

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Compare against fully constructed strings");
            Compare(a, b);
        }

        private void Compare(string a, string b)
        {
            Console.WriteLine("Hash -> a : {0}, b : {1}", a.GetHashCode(), b.GetHashCode());

            Console.WriteLine("a==b -> {0}", a == b);
            Console.WriteLine("a.Equals(b) -> {0}", a.Equals(b));
            Console.WriteLine("ReferenceEquals(a,b) -> {0}", ReferenceEquals(a, b));
            Console.WriteLine("GenericsCompare(a,b) -> {0}", GenericsCompare(a, b));
        }
    }
}
