using CLRViaCSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRViaCSharp._03_SharedAssembliesAndStronglyNamedAssemblies
{
    class HelloWorld : Study
    {
        public override string StudyName
        {
            get { return "Dissecting hellow world"; }
        }

        protected override void PerformStudy()
        {
            Console.WriteLine("Hello world....");
        }
    }
}
