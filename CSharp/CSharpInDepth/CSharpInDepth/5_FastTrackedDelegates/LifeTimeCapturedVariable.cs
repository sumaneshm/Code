using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpInDepth._5_FastTrackedDelegates
{
    //Listing 5.12 

    class LifeTimeCapturedVariable : Study
    {
        public override string StudyName
        {
            get { return "Life time of a captured variable"; }
        }

        MethodInvoker GetMethodInvoker()
        {
            int x = 5;

            // This x captured variable will live as long as the delegate lives
            MethodInvoker ret = delegate
            {
                Console.WriteLine(x);
                x++;
            };

            ret();
            return ret;
        }

        protected override void PerformStudy()
        {
            MethodInvoker x = GetMethodInvoker();
            x();
            x();
        }
    }
}
