using System;
using System.Collections.Generic;

namespace Ch06_WorkingWithVisualStudio.Tests
{
    public class Comparer
    {
        public static Comparer<U> Get<U>(Func<U, U, bool> func)
        {
            return new Comparer<U>(func);
        }
    }
    public class Comparer<T> : Comparer, IEqualityComparer<T>
    {
        private Func<T, T, bool> comparisonFuncition;

        public Comparer(Func<T,T,bool> func)
        {
            comparisonFuncition = func;
        }

        public bool Equals(T x, T y)
        {
            return comparisonFuncition(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }

}
