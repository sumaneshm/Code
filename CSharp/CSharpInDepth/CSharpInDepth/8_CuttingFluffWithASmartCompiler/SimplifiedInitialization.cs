using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._8_CuttingFluffWithASmartCompiler
{
    class SimplifiedInitialization : Study
    {
        public override string StudyName
        {
            get { return "New types of C# 3+ initialization"; }
        }

        class Person
        {
            public string Name { get; set; }

            public int RollNumber { get; set; }

            public Location Home { get; set; }

            private Location native = new Location();
            public Location Native { get { return native; } }

            private readonly List<Person> friends = new List<Person>();
            public List<Person> Friends { get { return friends; } }


            public class Location
            {
                public string Town { get; set; }
                public int Pin { set; get; }
            }
        }

        protected override void PerformStudy()
        {
            //Implicitly call Parameterless constructor
            var sumanesh = new Person
            {
                //Directly set the property
                Name = "Sumanesh",
                RollNumber = 28,

                //Create a new reference to the RW property
                Home = new Person.Location { Town = "Singapore", Pin = 530954 },

                //Directly use the Readonly property and change the data inside that
                Native = { Town = "Salem", Pin = 636001 },

                // Initialize collection for an existing list (note that we are NOT using new keyword and Friends is a readonly property)
                Friends = {
                    new Person { Name = "Pradeep",RollNumber=18},
                    new Person{Name="Aadhavan",RollNumber=1},
                    new Person{Name="Kumar",RollNumber=106}
                }
            };
        }
    }
}
