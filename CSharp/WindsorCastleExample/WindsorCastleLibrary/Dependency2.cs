using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsorCastleLibrary
{
    public class Dependency2 : IDependency2
    {
        public object SomeOtherObject
        {
            get;
            set;
        }
    }
}
