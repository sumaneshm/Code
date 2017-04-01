using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._5_FastTrackedDelegates
{

    //Section 5.5.3 Example
    // Demonstrates the power of captured variables

    class SimpleCaptureVariable : Study
    {
        List<int> GetLessThan(List<int> allNumbers,int limit)
        {
            return allNumbers.FindAll(delegate(int a)
            {
                // limit is a captured variable as it is used inside the anonymous method.
                // captured variable is THE same variable as the other methods sees it
                return a < limit;                           
            });
                
        }

        public override string StudyName
        {
            get { return "A simple captured variables"; }
        }

        protected override void PerformStudy()
        {
            foreach(int oneInt in GetLessThan(new List<int>() { 1, 3, 51, 120, 5, 10 }, 10))
            {
                Console.WriteLine(oneInt);
            }
        }
    }
}
