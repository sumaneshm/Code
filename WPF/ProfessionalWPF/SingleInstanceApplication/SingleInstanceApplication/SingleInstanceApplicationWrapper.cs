using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.ApplicationServices;

namespace SingleInstanceApplication
{
    public class SingleInstanceApplicationWrapper : WindowsFormsApplicationBase
    {
        public SingleInstanceApplicationWrapper()
        {
            IsSingleInstance = true;
        }

        private App wpfApp;

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            wpfApp = new App();
            wpfApp.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            if (eventArgs.CommandLine.Count > 0)
            {
                wpfApp.ShowDocument(eventArgs.CommandLine[0]);
            }
        }
    }
}
