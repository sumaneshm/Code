using CSharpInDepth.Common;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._14_DynamicBindingInAStaticLanguage
{
    class PythonIntegration : Study
    {
        public override string StudyName
        {
            get { return "How to run Pyhthon inside C#"; }
        }

        protected override void PerformStudy()
        {
            // SayHelloWorld();
            // VariablesUsage();
            PythonFunction();

        }

        private void PythonFunction()
        {
            string python =
@"
def sayHello(user):
    print 'Hello Mr. %(name)s' % {'name' : user}
";

            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            engine.Execute(python, scope);

            dynamic function = scope.GetVariable("sayHello");
            function("Sumanesh");
            function("Aadhavan");
        }

        private void VariablesUsage()
        {

            // Python is sensitive to whitespace.

            string python =
@"
text = 'Hello'
output = input + 1;
";
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            //Pass a variable from C# to python
            scope.SetVariable("input", 10);
            engine.Execute(python, scope);

            // Get the variable back to C#
            Console.WriteLine(scope.GetVariable("text"));
            Console.WriteLine(scope.GetVariable("output"));
        }

        private void SayHelloWorld()
        {
            ScriptEngine engine = Python.CreateEngine();

            //We can execute a string literal 
            engine.Execute("print 'hello world'");

            // Call the script file
            //engine.ExecuteFile();
        }
    }
}
