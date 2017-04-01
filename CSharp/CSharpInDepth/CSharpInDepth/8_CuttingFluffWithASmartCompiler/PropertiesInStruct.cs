using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._8_CuttingFluffWithASmartCompiler
{
    class PropertiesInStruct : Study
    {
        public override string StudyName
        {
            get { return "Properties in Struct"; }
        }

        struct MyStruct
        {
            public string Name { get; private set; }

            public int Age;

            public readonly int ID;

            //We have to explicitly call the default constructor provided by C# if we have to use the property
            public MyStruct(string name) : this()
            {
                Name = name;
                Age = 33;
                ID = 10;
            }
        }

        protected override void PerformStudy()
        {
            MyStruct s = new MyStruct("Sumanesh");
            Console.WriteLine("Name is : {0}, {1}",  s.Name, s.Age);
        }
    }
}
