using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth
{
    class Program
    {

        static void Main(string[] args)
        {
            Study study = CastleHelper.Container.Resolve<Study>();
            study.Exihibit();
        }
    }
}
