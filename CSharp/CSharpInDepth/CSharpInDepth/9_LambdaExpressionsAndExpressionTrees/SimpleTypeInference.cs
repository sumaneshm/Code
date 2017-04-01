using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._9_LambdaExpressionsAndExpressionTrees
{
    class SimpleTypeInference : Study
    {
        public override string StudyName
        {
            get { return "Simple type inference, \n compiler will try its best to inter the types"; }
        }


        private void Infer1<T>(T t1, T t2)
        {
            Console.WriteLine(typeof(T));
        }

        private void ConvertTwice<TInput, TMiddle, TOutput>(
                TInput input,
                Func<TInput, TMiddle> firstConversion,
                Func<TMiddle, TOutput> secondConversion
            )
        {
            TMiddle middle = firstConversion(input);
            TOutput output = secondConversion(middle);

            Console.WriteLine("Final output is : {0}", output);
        }

        private void Check1(Func<int> input)
        {
            Console.WriteLine("Integer Value : {0}", input);
        }

        private void Check1(Func<double> input)
        {
            Console.WriteLine("Double Value : {0}", input);
        }

        protected override void PerformStudy()
        {
            DrawHeader("Simple inference");

            //C# 3.0 compiler is intelligent enough to convert 1 to object as it can implicitly box it, but C# 2.0 compiler will fail
            Infer1(1, (object)1);

            //The following lines will fail to compile and the reasons are given below 

            //Case 1 : There is no implicit conversion applicable to convert 1 to the anonymous type
            //Infer1(1, new { Name = "Sumanesh" });

            //Case 2 : Eventhough both are of type "object" and implements IDisposable, compiler wont convert it
            // It will map it only when other items can be "implicitly" convert any of "THE" other items specified (and not its interfaces/base classes)
            //Infer1(new StreamWriter(), new MemoryStream());

            //The above case will If we give explicitly convert one of the parameters either to object/IDisposable or if you explicitly specify the type
            //Infer1((IDisposable)new StreamWriter("C:\\Temp\\Test.txt", true), new MemoryStream());
            //Infer1((object)new StreamWriter("C:\\Temp\\Test.txt", true), new MemoryStream());
            //Infer1<IDisposable>(new StreamWriter("C:\\Temp\\Test.txt", true), new MemoryStream());


            DrawHeader("Explains how compiler infers the following type parameters automatically without any hints");

            ConvertTwice(
                "Sumanesh",
                i => i.Length,
                l => Math.Sqrt(l)
            );

            /*
             How does compiler infers the above type
             * 1. By first round of inspection, it infers the simple type of the parameter TInput which is string (from the first parameter value "Sumanesh")
             * 2. In the second round of inspection, it substitutes String in the parameter firstConversion such that Func<string, TMiddle> and by inspecting the anonymous function, it
             *      infers the type of TMiddle as int
             * 3. In the third round of inspection, it substitutes TMiddle to the secondConversion parameter so Func<int, TOutput> and hence TOutput is infered to double
             * 4. Fixes the TOutput double and hence type inference succeeds
             */

            DrawHeader("Which is a good match");

            //The following line will match Check1(Func<int>) because it is the "BEST" match
            Check1(() => { return 1; });
        }
    }
}
