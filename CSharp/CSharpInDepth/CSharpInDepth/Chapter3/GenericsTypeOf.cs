using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    class GenericsTypeOf : Study
    {
        public override string StudyName
        {
            get { return "Demonstrate the typeof usage in Generics"; }
        }

        private void DemonstrateGenericsTypeOf<X>()
        {
            // X will be closed 
            Console.WriteLine("typeof(X) => {0}", typeof(X));

            Console.WriteLine("typeof(List<>) => {0}", typeof(List<>));
            Console.WriteLine("typeof(Dictionary<,> => {0}", typeof(Dictionary<,>));

            // Even when we use List<X>, it gets "closed" during compile time itself 
            Console.WriteLine("typeof(List<X>)=>{0}",typeof(List<X>));
        }

        protected override void PerformStudy()
        {
            DemonstrateGenericsTypeOf<int>();
        }
    }
}
