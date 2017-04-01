using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._12_LinqBeyondCollections
{

    public static class EnumerableExtension
    {
        public static T Random<T>(this IEnumerable<T> source, Random random)
        {
            // Always good to check the expectations on the arguments
            if (source == null) throw new ArgumentNullException("Source can't be null");
            if(random == null) throw new ArgumentNullException("Random can't be null");


            // Optimize if the passed source is a collection
            var sourceAsCollection = source as ICollection<T>;
            if(sourceAsCollection != null)
            {
                int count = sourceAsCollection.Count;
                if(count == 0)
                {
                    throw new InvalidOperationException("Source collection can't be empty");
                }

                int ran = random.Next(count);
                return sourceAsCollection.ElementAt(ran);
            }

            //If it is not a collection, perform normal random processing
            using(var enumer = source.GetEnumerator())
            {
                if(!enumer.MoveNext())
                {
                    throw new InvalidOperationException("Source IEnumerable can't be empty");
                }

                int count = 1;
                T current = enumer.Current;
                while(enumer.MoveNext())
                {
                    count++;
                    if(random.Next(count) == 0)
                    {
                        current = enumer.Current;
                    }
                }

                return current;
            }
        }
    }

    class EnumerableRandomExtension : Study
    {
        public override string StudyName
        {
            get { return "Extension method - Random generation for IEnumerable"; }
        }

        protected override void PerformStudy()
        {
            var enumeration = Enumerable.Range(1, 100);
            var rnd = new Random();

            for(int i=0;i<10;i++)
            {
                Console.WriteLine(enumeration.Random(rnd));
            }
        }
    }
}
