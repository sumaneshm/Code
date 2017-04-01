using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    class Pair<T1,T2> : IEquatable<Pair<T1,T2>>
    {
        private static readonly IEqualityComparer<T1> T1Comparer = EqualityComparer<T1>.Default;
        private static readonly IEqualityComparer<T2> T2Comparer = EqualityComparer<T2>.Default;

        private T1 firstValue;

        public T1 FirstValue
        {
            get { return firstValue; }
            set { firstValue = value; }
        }

        private T2 secondValue;

        public T2 SecondValue
        {
            get { return secondValue; }
            set { secondValue = value; }
        }

        public Pair(T1 firstValue, T2 secondValue)
        {
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
        }

        public bool Equals(Pair<T1, T2> other)
        {
            return other != null
                && T1Comparer.Equals(this.FirstValue, other.FirstValue)
                && T2Comparer.Equals(this.SecondValue, other.SecondValue);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pair<T1, T2>); 
        }

        public override int GetHashCode()
        {
            return this.FirstValue.GetHashCode() * 37 + this.SecondValue.GetHashCode();
        }

    }

    static class Pair
    {
        public static Pair<T1, T2> Of<T1, T2>(T1 first, T2 second)
        {
            return new Pair<T1, T2>(first, second);
        }
    }

    class GenericPair : Study
    {
        public override string StudyName
        {
            get { return "Generics Pair class study"; }
        }

        protected override void PerformStudy()
        {
            var pair1 = Pair.Of("Sumanesh", 32);
            var pair2 = Pair.Of("Sumanesh", 33);

            Console.WriteLine(pair1 == pair2);
        }
    }
}
