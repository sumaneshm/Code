using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._14_DynamicBindingInAStaticLanguage
{
    class GuessNumberCorrectly : Study
    {
        // Doesn't do anything useful but a stupid Game

        // Implements IDynamicMetaObjectProvider to educate DLR on how to react when somemember is called
        class Guess : IDynamicMetaObjectProvider
        {
            public readonly int Number = new Random().Next(5);

            private int guessCount = 0;

            public Guess()
            {

            }

            public Guess(int number)
            {
                this.Number = number;
            }

            // How to react to various guesses.... (called directly from MetaData)
            private object LessThan(int userGuess)
            {
                Console.WriteLine("Guessed number {0} is < the actual number", userGuess);
                guessCount++;
                return false;
            }

            private object GreaterThan(int userGuess)
            {
                Console.WriteLine("Guessed number {0} is > the actual number", userGuess);
                guessCount++;
                return false;
            }

            private object CorrectGuess(int userGuess)
            {
                guessCount++;
                Console.WriteLine("Absolutely correct guess {0}....You took {1} guess(es)", userGuess, guessCount);
                return true;
            }

            // Gives MetaData to say how to reach to user guesses
            private class GuessMetaData : DynamicMetaObject
            {
                private static readonly MethodInfo lessThanMethod = typeof(Guess).GetMethod("LessThan", BindingFlags.Instance | BindingFlags.NonPublic);
                private static readonly MethodInfo greaterThanMethod = typeof(Guess).GetMethod("GreaterThan", BindingFlags.Instance | BindingFlags.NonPublic);
                private static readonly MethodInfo correctGuessMethod = typeof(Guess).GetMethod("CorrectGuess", BindingFlags.Instance | BindingFlags.NonPublic);

                internal GuessMetaData(System.Linq.Expressions.Expression expression, Guess source)
                    : base(expression, BindingRestrictions.Empty, source)
                {

                }

                public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
                {
                    Guess target = (Guess)base.Value;
                    Expression self = Expression.Convert(base.Expression, typeof(Guess));

                    string name = binder.Name.Replace("_", "");
                    Expression targetBehavior;
                    int userGuess;

                    // how to reach based on user guess

                    if (Int32.TryParse(name, out userGuess))
                    {
                        if (userGuess < target.Number)
                        {
                            targetBehavior = Expression.Call(self, lessThanMethod, Expression.Constant(userGuess));
                        }
                        else if (userGuess > target.Number)
                        {
                            targetBehavior = Expression.Call(self, greaterThanMethod, Expression.Constant(userGuess));
                        }
                        else
                        {
                            targetBehavior = Expression.Call(self, correctGuessMethod, Expression.Constant(userGuess));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException(String.Format("It has to be a number..... {0}", name));
                    }


                    // It has to be instance specific rather than common for all the instances

                    var restrictions = BindingRestrictions.GetInstanceRestriction(self, target);
                    return new DynamicMetaObject(targetBehavior, restrictions);
                }
            }

            public DynamicMetaObject GetMetaObject(Expression parameter)
            {
                return new GuessMetaData(parameter, this);
            }
        }

        public override string StudyName
        {
            get { return "Shows one of the advanced concepts on DLR to demonstrate a guess number game"; }
        }

        protected override void PerformStudy()
        {
            dynamic game = new Guess();

            game._5();
            game._3();
            game._1();
            game._0();
            game._2();
            game._4();

        }
    }
}
