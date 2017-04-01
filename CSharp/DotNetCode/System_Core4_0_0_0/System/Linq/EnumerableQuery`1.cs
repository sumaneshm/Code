// Type: System.Linq.EnumerableQuery`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class EnumerableQuery<T> : EnumerableQuery, IOrderedQueryable<T>, IQueryable<T>, IOrderedQueryable, IQueryable, IQueryProvider, IEnumerable<T>, IEnumerable
  {
    private Expression expression;
    private IEnumerable<T> enumerable;

    [__DynamicallyInvokable]
    IQueryProvider IQueryable.Provider
    {
      [__DynamicallyInvokable] get
      {
        return (IQueryProvider) this;
      }
    }

    internal override Expression Expression
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.expression;
      }
    }

    internal override IEnumerable Enumerable
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return (IEnumerable) this.enumerable;
      }
    }

    [__DynamicallyInvokable]
    Expression IQueryable.Expression
    {
      [__DynamicallyInvokable] get
      {
        return this.expression;
      }
    }

    [__DynamicallyInvokable]
    Type IQueryable.ElementType
    {
      [__DynamicallyInvokable] get
      {
        return typeof (T);
      }
    }

    [__DynamicallyInvokable]
    public EnumerableQuery(IEnumerable<T> enumerable)
    {
      this.enumerable = enumerable;
      this.expression = (Expression) Expression.Constant((object) this);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EnumerableQuery(Expression expression)
    {
      this.expression = expression;
    }

    [__DynamicallyInvokable]
    IQueryable IQueryProvider.CreateQuery(Expression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull("expression");
      Type genericType = TypeHelper.FindGenericType(typeof (IQueryable<>), expression.Type);
      if (genericType == (Type) null)
        throw Error.ArgumentNotValid((object) "expression");
      else
        return EnumerableQuery.Create(genericType.GetGenericArguments()[0], expression);
    }

    [__DynamicallyInvokable]
    IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull("expression");
      if (!typeof (IQueryable<S>).IsAssignableFrom(expression.Type))
        throw Error.ArgumentNotValid((object) "expression");
      else
        return (IQueryable<S>) new EnumerableQuery<S>(expression);
    }

    [__DynamicallyInvokable]
    object IQueryProvider.Execute(Expression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull("expression");
      typeof (EnumerableExecutor<>).MakeGenericType(new Type[1]
      {
        expression.Type
      });
      return EnumerableExecutor.Create(expression).ExecuteBoxed();
    }

    [__DynamicallyInvokable]
    S IQueryProvider.Execute<S>(Expression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull("expression");
      if (!typeof (S).IsAssignableFrom(expression.Type))
        throw Error.ArgumentNotValid((object) "expression");
      else
        return new EnumerableExecutor<S>(expression).Execute();
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    private IEnumerator<T> GetEnumerator()
    {
      if (this.enumerable == null)
        this.enumerable = Expression.Lambda<Func<IEnumerable<T>>>(new EnumerableRewriter().Visit(this.expression), (IEnumerable<ParameterExpression>) null).Compile()();
      return this.enumerable.GetEnumerator();
    }

    [__DynamicallyInvokable]
    public override string ToString()
    {
      ConstantExpression constantExpression = this.expression as ConstantExpression;
      if (constantExpression == null || constantExpression.Value != this)
        return this.expression.ToString();
      if (this.enumerable != null)
        return this.enumerable.ToString();
      else
        return "null";
    }
  }
}
