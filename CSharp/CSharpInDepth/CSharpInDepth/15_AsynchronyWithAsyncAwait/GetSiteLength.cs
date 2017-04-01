using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Drawing;
using System.Net.Http;

namespace CSharpInDepth._15_AsynchronyWithAsyncAwait
{
   

    class GetSiteLength : Study
    {
        public override string StudyName
        {
            get { return "Demonstrates how easy it is to create asynchronous program in C# 5.0"; }
        }

        protected override void PerformStudy()
        {
            Application.Run(new AsyncForm());
        }
    }

    class AsyncForm : Form
    {
        Label label;
        Button button;

        public AsyncForm()
        {
            label = new Label { Location = new Point(10, 20), Text = "Length" };
            button = new Button { Text = "Get Length", Location = new Point(10, 50) };
            button.Click += DisplayWebsiteLength;
            AutoSize = true;
            Controls.Add(label);
            Controls.Add(button);
        }

        async void DisplayWebsiteLength(object sender, EventArgs e)
        {
            label.Text = "Fetching...";
            using (HttpClient client = new HttpClient())
            {
                string text = await client.GetStringAsync("http://www.csharpindepth.com");
                label.Text = "Length : " + text.Length;
            }
        }
    }
}
