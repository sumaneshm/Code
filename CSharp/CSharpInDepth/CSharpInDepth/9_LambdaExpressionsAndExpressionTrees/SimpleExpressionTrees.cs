using CSharpInDepth.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CSharpInDepth._9_LambdaExpressionsAndExpressionTrees
{
    internal class SimpleExpressionTrees : Study
    {
        public override string StudyName
        {
            get { return "How to construct simple Expression trees using code"; }
        }

        protected override void PerformStudy()
        {
            Console.WriteLine("--------------------------------------------------------------------");
            SimpleAdder();
            Console.WriteLine("--------------------------------------------------------------------");
            BasicLambdaExpressionTree();
            Console.WriteLine("--------------------------------------------------------------------");
            LamdaExpressionStartsWith();
            Console.WriteLine("--------------------------------------------------------------------");
            StartsWithExpressionTreeUsingCode();
            Console.WriteLine("--------------------------------------------------------------------");

            Console.WriteLine("A simple lambda expression");
        }

        private void BasicLambdaExpressionTree()
        {
            Console.WriteLine("A second example which uses Lambda expression");

            Expression<Func<int>> expr = () => 5;
            Func<int> compiled = expr.Compile();

            Console.WriteLine(compiled);

            Console.WriteLine(compiled());
        }

        private void LamdaExpressionStartsWith()
        {
            Console.WriteLine("A bit more complex example using Lambda expression");
            Expression<Func<string, string, bool>> startsWith = (a, b) => a.StartsWith(b);
            var compiled = startsWith.Compile();

            Console.WriteLine(compiled("Sumanesh", "Aadhavan"));
            Console.WriteLine(compiled("Sumanesh", "Sum"));
        }

        private void SimpleAdder()
        {
            Console.WriteLine("A simple example to show how to create expression tree");
            var two = Expression.Constant(2);
            var three = Expression.Constant(3);

            //Construct a simple expression tree to add two numbers
            Expression added = Expression.Multiply(two, three);
            Expression divided = Expression.Divide(added, Expression.Constant(2));

            Console.WriteLine(divided);

            //Convert the expression tree to lambda expression
            Func<int> expr = Expression.Lambda<Func<int>>(added).Compile();

            //Invoke the lambda expression
            Console.WriteLine("Invoked lambda expression : {0}", expr());
        }
        private void StartsWithExpressionTreeUsingCode()
        {
            //The same example explained in LambdaExpressionStartsWith
            Console.WriteLine("How to construct StartsWith Expression tree using code");

            MethodInfo miSubstring = typeof(string).GetMethod("Substring", new[] { typeof(int), typeof(int) });
            var target = Expression.Parameter(typeof(string), "t");
            var startarg = Expression.Parameter(typeof(int), "start");
            var endarg = Expression.Parameter(typeof(int), "end");

            Expression[] substringMethodArgs = new[] { startarg, endarg };

            var substringCall = Expression.Call(target, miSubstring, substringMethodArgs);
            var substringLambdaParams = new[] { target, startarg, endarg };
            var substringLambda = Expression.Lambda<Func<string, int, int, string>>(substringCall, substringLambdaParams);

            MethodInfo miStartsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var t = Expression.Parameter(typeof(string), "t");
            var arg = Expression.Parameter(typeof(string), "b");
            Expression[] methodArgs = new[] { arg };

            Expression call = Expression.Call(t, miStartsWith, arg);

            var lambdaParameters = new[] { t, arg };
            var startsWithLambda = Expression.Lambda<Func<string, string, bool>>(call, lambdaParameters);

            var startsWithCompiled = startsWithLambda.Compile();
            var substringCompiled = substringLambda.Compile();

            Console.WriteLine(substringCompiled("Sumanesh", 1, 2));
            Console.WriteLine(substringCompiled("Sumanesh", 2, 4));

            Console.WriteLine(startsWithCompiled("Sumanesh", "Aadhavan"));
            Console.WriteLine(startsWithCompiled("Sumanesh", "Sum"));
        }
    }
}