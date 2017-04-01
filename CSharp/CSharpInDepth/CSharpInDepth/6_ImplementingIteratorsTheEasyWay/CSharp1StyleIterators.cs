using CSharpInDepth.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._6_ImplementingIteratorsTheEasyWay
{
    // Listing 6.1, 6.2, 6.3
    // Old way of implementing iterators in C# 1.0 style
    
    // Update : We dont have to implement the IEnumerable mandatorily.
    // C# compiler works on "magic" methods "GetEnumerator" which expects any object which has the methods
    // Current, MoveNext etc. thats it. IEnumerable and IEnumerator just makes it mandatory

    class IterationSample<T> //: IEnumerable<T>
    {
        T[] values;
        int startingPoint;

        public IterationSample(T[] values, int startingPoint)
        {
            this.values = values;
            this.startingPoint = startingPoint;

        }
        public IterationSampleEnumerator<T> GetEnumerator()
        {
            return new IterationSampleEnumerator<T>(this);
        }

        public class IterationSampleEnumerator<T> //: IEnumerator<T>
        {
            IterationSample<T> collection;
            int current = -1;

            public IterationSampleEnumerator(IterationSample<T> collection)
            {
                this.collection = collection;
            }

            public T Current
            {
                get { 
                    if(current <= -1 || current >= collection.values.Length)
                    {
                        throw new InvalidOperationException("Outside acceptable for loop");
                    }
                    
                    int position = current + collection.startingPoint;
                    position = position % collection.values.Length;
                    return collection.values[position];
                }
            }

            public bool MoveNext()
            {
                if (current < collection.values.Length)
                {
                    current++;
                }
                return current < collection.values.Length;
            }

            public void Reset()
            {
                current = -1;
            }

            public void Dispose()
            {
                
            }

            //object IEnumerator.Current
            //{
            //    get { return Current; }
            //}
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}
    }




    class CSharp1StyleIterators : Study
    {
        public override string StudyName
        {
            get { return "Explains how difficult it was to implement iterators in C# 1.0 style"; }
        }

        protected override void PerformStudy()
        {
            IterationSample<int> sampleIntegers = new IterationSample<int>(new int[]{1,2,3,4,5,6,7,8,9,10},4);

            foreach(int i in sampleIntegers)
            {
                Console.WriteLine(i);
            }
        }
    }
}
