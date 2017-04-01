using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpInDepth._5_FastTrackedDelegates
{
    class EventsSubscriptionUsingMethodsGroup : CSharp1EventsSubscription
    {
        public override string StudyName
        {
            get { return "Learn how method group simplified event handling"; }
        }

        protected override void AddHandlers(Button button)
        {
            button.Click += LogPlainEvent;
            button.KeyPress += LogKeyDownEvent;
            button.MouseClick += LogMouseEvent;
        }

    }
}
