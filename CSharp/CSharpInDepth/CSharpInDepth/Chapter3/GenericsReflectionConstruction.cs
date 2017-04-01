using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    // Listing 3.12

    class GenericsReflectionConstruction : Study
    {
        public override string StudyName
        {
            get { return "Using Reflection to create Generic and Closed type objects"; }
        }


        protected override void PerformStudy()
        {
            string listTypeName = "System.Collections.Generic.List`1";

            Type openByName = Type.GetType(listTypeName);

            Type closedByname = Type.GetType(listTypeName + "[System.String]");
            Type closedByMethod = openByName.MakeGenericType(typeof(string));
            Type closedByTypeOf = typeof(List<String>);

            Console.WriteLine(closedByname == closedByMethod);
            Console.WriteLine(closedByMethod == closedByTypeOf);

            Type openByTypeOf = typeof(List<>);
            Type openByMethod = closedByTypeOf.GetGenericTypeDefinition();

            Console.WriteLine(openByMethod == openByTypeOf);
            Console.WriteLine(openByName == openByTypeOf);

            //foreach(Type oneStudy in Assembly.GetCallingAssembly().GetTypes().Where(t=> t.IsSubclassOf(typeof(Study))))
            //{
            //    Console.WriteLine(oneStudy.GetProperty("StudyName").GetValue());
            //}
        }
    }
}
