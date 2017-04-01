// Type: System.Linq.Expressions.MethodCallExpression1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal class MethodCallExpression1 : MethodCallExpression, IArgumentProvider
  {
    private object _arg0;

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        return 1;
      }
    }

    public MethodCallExpression1(MethodInfo method, Expression arg0)
      : base(method)
    {
      this._arg0 = (object) arg0;
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      if (index == 0)
        return Expression.ReturnObject<Expression>(this._arg0);
      else
        throw new InvalidOperationException();
    }

    internal override ReadOnlyCollection<Expression> GetOrMakeArguments()
    {
      return Expression.ReturnReadOnly((IArgumentProvider) this, ref this._arg0);
    }

    internal override MethodCallExpression Rewrite(Expression instance, IList<Expression> args)
    {
      if (args != null)
        return Expression.Call(this.Method, args[0]);
      else
        return Expression.Call(this.Method, Expression.ReturnObject<Expression>(this._arg0));
    }
  }
}
