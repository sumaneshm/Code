using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._14_DynamicBindingInAStaticLanguage
{
    class DynamicTypeInference : Study
    {
        public override string StudyName
        {
            get { return "Explains how dynamic inference works"; }
        }

        private static bool AddConditionallyImpl<T>(IList<T> list, T item)
        {
            if(list.Count < 5)
            {
                Console.WriteLine("Adding " + item);
                list.Add(item);
                return true;
            }
            else
            {
                Console.WriteLine("Skipping " + item);
            }
            return false;
        }

        public static bool AddConditionally(dynamic list, dynamic item)
        {
            return AddConditionallyImpl(list, item);
        }

        protected override void PerformStudy()
        {
            var list = new List<string> { "a", "b", "c" };
            var item = "d";

            AddConditionally(list, item);
            AddConditionally(list, "e");
            AddConditionally(list, "F");
            AddConditionally(list, "G");

            
        }
    }
}
