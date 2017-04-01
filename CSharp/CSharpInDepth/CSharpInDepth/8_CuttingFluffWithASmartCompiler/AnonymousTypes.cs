using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._8_CuttingFluffWithASmartCompiler
{
    class AnonymousTypes : Study
    {
        public override string StudyName
        {
            get { return "Anonymous Types "; }
        }

        protected override void PerformStudy()
        {
            // Compiler will create two different anonymous types for the following
            // It will create new types 
                // whenever the order of the property changed
                // obviously type/number/names of fields change
            var a = new { Name = "Sumanesh", Age = 10 };
            var b = new { Age = 10, Name = "Sumanesh" };

        }
    }
}
