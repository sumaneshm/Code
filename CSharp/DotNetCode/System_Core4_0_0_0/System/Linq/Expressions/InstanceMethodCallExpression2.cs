// Type: System.Linq.Expressions.InstanceMethodCallExpression2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal class InstanceMethodCallExpression2 : MethodCallExpression, IArgumentProvider
  {
    private readonly Expression _instance;
    private object _arg0;
    private readonly Expression _arg1;

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        return 2;
      }
    }

    public InstanceMethodCallExpression2(MethodInfo method, Expression instance, Expression arg0, Expression arg1)
      : base(method)
    {
      this._instance = instance;
      this._arg0 = (object) arg0;
      this._arg1 = arg1;
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      switch (index)
      {
        case 0:
          return Expression.ReturnObject<Expression>(this._arg0);
        case 1:
          return this._arg1;
        default:
          throw new InvalidOperationException();
      }
    }

    internal override Expression GetInstance()
    {
      return this._instance;
    }

    internal override ReadOnlyCollection<Expression> GetOrMakeArguments()
    {
      return Expression.ReturnReadOnly((IArgumentProvider) this, ref this._arg0);
    }

    internal override MethodCallExpression Rewrite(Expression instance, IList<Expression> args)
    {
      if (args != null)
        return Expression.Call(instance, this.Method, args[0], args[1]);
      else
        return Expression.Call(instance, this.Method, Expression.ReturnObject<Expression>(this._arg0), this._arg1);
    }
  }
}
