using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelInheritence
{
    class BaseClass
    {
        public string Name { get; set; }
    }

    class DerivedClass : BaseClass
    {
        public int Age { get; set; }
    }

    class StudentDerivedClass : BaseClass
    {
        public int RollNumber { get; set; }
    }
}
