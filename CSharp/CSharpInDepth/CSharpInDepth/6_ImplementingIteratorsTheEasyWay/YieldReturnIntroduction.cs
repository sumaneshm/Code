using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._6_ImplementingIteratorsTheEasyWay
{
    class YieldIterator<T> : IEnumerable<T>
    {
        private int startingPoint;
        private List<T> data;

        public YieldIterator(List<T> dataToBeYielded, int startingPoint)
        {
            data = dataToBeYielded;
            this.startingPoint = startingPoint;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for(int i=0;i<data.Count;i++)
            {
                yield return data[(startingPoint + i) % data.Count];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    class YieldReturnIntroduction : Study
    {
        public override string StudyName
        {
            get { return "An introduction to yield return and how it simplifies"; }
        }

        protected override void PerformStudy()
        {
            List<int> newList = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 };
            YieldIterator<int> yi = new YieldIterator<int>(newList, 3);

            foreach (int i in yi)
            {
                Console.WriteLine(i);
            }
        }
    }
}
