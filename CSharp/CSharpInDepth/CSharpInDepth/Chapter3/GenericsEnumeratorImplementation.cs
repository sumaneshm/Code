using CSharpInDepth.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    //Listing 3.10

    //When you decide to implement Generic IEnumerable<T> which in turn is inherited from IEnumerable, there will 
    //some amount of duplication involved as we have to implement both IEnumerable.GetEnumerator and
    //IEnumerable<T>.GetEnumerator. This study shows how to handle them gracefully.

    class CountingEnumerable : IEnumerable<int>
    {    
        public IEnumerator<int> GetEnumerator()
        {
            return new CountingEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class CountingEnumerator : IEnumerator<int>
    {
        int current = -1;

        public int Current
        {
            get { return current; }
        }

        public void Dispose()
        {

        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            if (current < 9)
            {
                current++;
                return true;
            }
            else
                return false;
        }

        public void Reset()
        {
            current = -1;
        }
    }

    //class CountingEnumerable : IEnumerable
    //{

    //    public IEnumerator GetEnumerator()
    //    {
    //        return new CountingEnumerator();
    //    }
    //}

    //class CountingEnumerator : IEnumerator
    //{
    //    int current = -1;
    //    public object Current
    //    {
    //        get { return current; }
    //    }

    //    public bool MoveNext()
    //    {
    //        if(current < 9)
    //        {
    //            current++;
    //            return true;
    //        }

    //        return false;
    //    }

    //    public void Reset()
    //    {
    //        current = -1;
    //    }
    //}

    class GenericsEnumeratorImplementation : Study
    {
        public override string StudyName
        {
            get { return "How to implement IEnumerable<T> and IEnumerator<T>"; }
        }

        protected override void PerformStudy()
        {
            CountingEnumerable enumerable = new CountingEnumerable();

            foreach(var i in enumerable)
            {
                Console.WriteLine(i);
            }
        }
    }
}
