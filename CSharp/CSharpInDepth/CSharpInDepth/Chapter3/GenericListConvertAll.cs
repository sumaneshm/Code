using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    class GenericListConvertAll : Study
    {
        public override string StudyName
        {
            get { return "Generic List Convert All"; }
        }

        protected override void PerformStudy() 
        {

            List<int> integers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<double> results = integers.ConvertAll((i) => { return Math.Sqrt(i); });

            foreach(var res in results)
            {
                Console.WriteLine(res);
            }
        }
    }
}
