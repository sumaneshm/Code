// Type: System.Linq.Expressions.DynamicExpression2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  internal class DynamicExpression2 : DynamicExpression, IArgumentProvider
  {
    private object _arg0;
    private readonly Expression _arg1;

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        return 2;
      }
    }

    internal DynamicExpression2(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1)
      : base(delegateType, binder)
    {
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

    internal override ReadOnlyCollection<Expression> GetOrMakeArguments()
    {
      return Expression.ReturnReadOnly((IArgumentProvider) this, ref this._arg0);
    }

    internal override DynamicExpression Rewrite(Expression[] args)
    {
      return Expression.MakeDynamic(this.DelegateType, this.Binder, args[0], args[1]);
    }
  }
}
