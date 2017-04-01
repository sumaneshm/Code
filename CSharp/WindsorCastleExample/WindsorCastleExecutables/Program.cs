using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindsorCastleLibrary;

namespace WindsorCastleExecutables
{
    static class Program
    {
        internal static WindsorContainer container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WindsorWireup();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void WindsorWireup()
        {
            container = new WindsorContainer();
            container.Register(Component.For<IDependency1>().ImplementedBy<Dependency1>());
            container.Register(Component.For<IDependency2>().ImplementedBy<Dependency2>());
            container.Register(Component.For<MainClass>());
        }
    }
}
