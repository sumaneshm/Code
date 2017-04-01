using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;



namespace CSharpInDepth._5_FastTrackedDelegates
{
    class CSharp1EventsSubscription : Study
    {
        public override string StudyName
        {
            get { return "How events and delegates are done in C# 1"; }
        }

        protected virtual void AddHandlers(Button button)
        {
            button.Click += new EventHandler(LogPlainEvent);
            button.KeyPress += new KeyPressEventHandler(LogKeyDownEvent);
            button.MouseClick += new MouseEventHandler(LogMouseEvent);
        }

        protected void PrepareControls(Action<Button> addHandlers)
        {
            Button button = new Button();
            button.Text = "Click Me";
            addHandlers.Invoke(button);

            Form form = new Form();
            form.Controls.Add(button);
            form.AutoSize = true;

            Application.Run(form);
        }

        protected override void PerformStudy()
        {
            PrepareControls(AddHandlers);
        }

        protected void LogMouseEvent(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Logging Mouse event");
        }

        protected void LogKeyDownEvent(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("Logging KeyDown event");
        }

        protected void LogPlainEvent(object sender, EventArgs e)
        {
            Console.WriteLine("Logging plain event");
        }
    }
}
