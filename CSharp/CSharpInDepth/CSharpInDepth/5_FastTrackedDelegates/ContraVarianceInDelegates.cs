using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpInDepth._5_FastTrackedDelegates
{
    class ContraVarianceInDelegates : CSharp1EventsSubscription
    {
        public override string StudyName
        {
            get { return "Learn how Contravariance is done in Delegates"; }
        }

        protected override void AddHandlers(Button button)
        {
            button.Click += LogContraVarianceDelegate;
            button.KeyPress += LogContraVarianceDelegate;   // Uses method conversion and contra variance
            button.MouseClick += LogContraVarianceDelegate; // Uses method conversion and contra variance
        }

        private void LogContraVarianceDelegate(object source, EventArgs eventArgs)
        {
            Console.WriteLine("Contra variance delegate");
        }
    }
}
