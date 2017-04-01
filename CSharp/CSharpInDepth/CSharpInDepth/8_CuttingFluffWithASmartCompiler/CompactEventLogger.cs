using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;


namespace CSharpInDepth._8_CuttingFluffWithASmartCompiler
{
    class CompactEventLogger : Study
    {
        public override string StudyName
        {
            get { return "How to reuse LogWriter"; }
        }

        private void Log(string title, object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Title {0}", title);
            Console.WriteLine("Sender {0}", sender);
            foreach (PropertyDescriptor descr in TypeDescriptor.GetProperties(eventArgs))
            {
                Console.WriteLine("\t{0} - {1}", descr.DisplayName, descr.GetValue(eventArgs));
            }
        }

        protected override void PerformStudy()
        {
            Button button = new Button { Text = "Click Me" };
            button.KeyPress += (src, e) =>  Log("KeyPress", src, e);
            button.Click += (src, e) => Log("Click", src, e);
            button.MouseClick += (src, e) => Log("MouseClick", src, e);

            Form form = new Form { AutoSize = true, Controls = { button } };
            Application.Run(form);
        }
    }
}
