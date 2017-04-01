using CSharpInDepth.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._12_LinqBeyondCollections
{
    class FakeQuery<T> : IQueryable<T>
    {
        public FakeQuery(IQueryProvider provider, Expression expression)
        {
            Expression = expression;
            Provider = provider;
            ElementType = typeof(T);
        }

        internal FakeQuery()
            : this(new FakeQueryProvider(), null)
        {
            Expression = Expression.Constant(this);
        }

        public Type ElementType { get; private set; }

        public Expression Expression { get; private set; }

        public IQueryProvider Provider { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            Logger.Log(Expression);
            return Enumerable.Empty<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class FakeQueryProvider : IQueryProvider
    {

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            Logger.Log(expression);
            return new FakeQuery<TElement>(this, expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Logger.Log(expression);
            Type queryType = typeof(FakeQuery<>).MakeGenericType(expression.Type);
            var args = new object[] { this, expression };
            return (IQueryable)Activator.CreateInstance(queryType, args);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            Logger.Log(expression);
            return default(TResult);
        }

        public object Execute(Expression expression)
        {
            Logger.Log(expression);
            return null;
        }
    }
    class ImplementDummyLinqProvider : Study
    {
        public override string StudyName
        {
            get { return "Study on core interfaces which have to be implemented for creating own Linq provider"; }
        }

        protected override void PerformStudy()
        {
            var query = from x in new FakeQuery<string>()
                        where x.StartsWith("abc")
                        //let len = x.Length
                        select x.Length;

            //foreach (var x in query) { }

            double mean = query.Average();

        }
    }
}
