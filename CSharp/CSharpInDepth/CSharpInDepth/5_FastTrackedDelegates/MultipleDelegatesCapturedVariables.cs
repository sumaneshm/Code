using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpInDepth._5_FastTrackedDelegates
{
    //Listing 5.13

    class MultipleDelegatesCapturedVariables : Study
    {
        void TestMultipleInvokers()
        {
            List<Action> invokers = new List<Action>();

            for (int i = 0; i < 5; i++)
            {
                //Captured variable is NOT shared between all the delegates as the variable is declared inside the for loop
                // had we tried to capture the loop variable, i all the delegates will be sharing the same variable and would be a mess
                // and hence ReSharper recommends us to copy it to a local variable and capture it rather than capturing the loop variable itself.

                // Warning !!! Dont try to capture the variable declared in foreach loop statement
                int captured = i * 10;

                Action oneInv = delegate
                {
                    Console.WriteLine(captured);
                    captured++;
                };

                invokers.Add(oneInv);
            }

            foreach (Action inv in invokers)
            {
                inv();
            }

            invokers[0]();
            invokers[0]();
            invokers[0]();
            invokers[0]();

            invokers[1]();
        }

        public override string StudyName
        {
            get { return "Multiple delegates capturing single/multiple variables"; }
        }

        protected override void PerformStudy()
        {
            TestMultipleInvokers();
        }
    }
}
