using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._14_DynamicBindingInAStaticLanguage
{
    class DynamicIteration : Study
    {
        public override string StudyName
        {
            get { return "How to add dynamically"; }
        }

        protected override void PerformStudy()
        {
            AdditionTest();
        }


        private void AdditionTest()
        {
            //Set 1 : Will use String concatenation
            //dynamic items = new List<string> { "First", "Second", "Third" };
            //dynamic valeToAdd = "!";

            // Set 2 : Will use integer addition
            dynamic items = new List<int> { 1, 2, 3 };
            dynamic valeToAdd = 2;

            foreach(dynamic i in items)
            {
                Console.WriteLine(i + valeToAdd);
            }
        }
    }
}
