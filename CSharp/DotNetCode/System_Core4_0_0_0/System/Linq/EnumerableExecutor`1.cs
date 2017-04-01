// Type: System.Linq.EnumerableExecutor`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class EnumerableExecutor<T> : EnumerableExecutor
  {
    private Expression expression;
    private Func<T> func;

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EnumerableExecutor(Expression expression)
    {
      this.expression = expression;
    }

    internal override object ExecuteBoxed()
    {
      return (object) this.Execute();
    }

    internal T Execute()
    {
      if (this.func == null)
        this.func = Expression.Lambda<Func<T>>(new EnumerableRewriter().Visit(this.expression), (IEnumerable<ParameterExpression>) null).Compile();
      return this.func();
    }
  }
}
