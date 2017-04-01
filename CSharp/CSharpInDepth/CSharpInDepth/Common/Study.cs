using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Common
{
    internal abstract class Study
    {
        internal virtual void Exihibit()
        {
            Console.WriteLine("Study name : {0}", StudyName);
            PerformStudy();
        }

        public abstract string StudyName
        {
            get;
        }

        protected void DrawHeader(string header)
        {
            '-'.Separate();
            Console.WriteLine(header);
            '-'.Separate();
        }

        protected abstract void PerformStudy();
    }
}
