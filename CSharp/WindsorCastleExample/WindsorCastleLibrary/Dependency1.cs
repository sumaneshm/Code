using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsorCastleLibrary
{
    public class Dependency1 : IDependency1
    {
        public object SomeObject
        {
            get;
            set;
        }
    }
}
