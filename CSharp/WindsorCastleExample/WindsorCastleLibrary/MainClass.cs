using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsorCastleLibrary
{
    public class MainClass
    {
            public IDependency1 object1;
            public IDependency2 object2;

            public MainClass(IDependency1 dependency1, IDependency2 dependency2)
            {
                object1 = dependency1;
                object2 = dependency2;
            }


            public void DoSomething()
            {
                object1.SomeObject = "Hello World";
                object2.SomeOtherObject = "Hello Mars";
            }
        
    }
}
