using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    class GenericsReflectionMethods : Study
    {
        public override string StudyName
        {
            get { return "how to call a Generics method using reflection "; }
        }

        public static void AGenericMethod<T>()
        {
            Console.WriteLine(typeof(T));
        }

        protected override void PerformStudy()
        {
            Type thisType = typeof(GenericsReflectionMethods);
            MethodInfo definition = thisType.GetMethod("AGenericMethod");
            MethodInfo constructed = definition.MakeGenericMethod(typeof(int));


            constructed.Invoke(null, null);
        }
    }
}
