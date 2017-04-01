using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._5_FastTrackedDelegates
{
    //Listing 5.14
    // Warning !!! Only to show the impact of capturing variables at different scope, DONT try this 
    class DifferentScopeCapturedVariables : Study
    {
        public override string StudyName
        {
            get { return "Shows the effect of capturing variables at different scope"; }
        }

        protected override void PerformStudy()
        {
            List<Action> allActions = new List<Action>();
            int outter = 10;
            for(int i=0;i<10;i++)
            {
                int inner = i * 10;

                allActions.Add(() =>
                {
                    Console.WriteLine("outter : {0}, inner : {1}", outter, inner);
                    outter++;
                    inner++;
                });
            }

            foreach(Action oneAction in allActions)
            {
                oneAction();
            }

            allActions[0]();
            allActions[0]();

            allActions[1]();
        }
    }
}
