// Type: System.Linq.Expressions.DynamicExpression1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  internal class DynamicExpression1 : DynamicExpression, IArgumentProvider
  {
    private object _arg0;

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        return 1;
      }
    }

    internal DynamicExpression1(Type delegateType, CallSiteBinder binder, Expression arg0)
      : base(delegateType, binder)
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

    internal override DynamicExpression Rewrite(Expression[] args)
    {
      return Expression.MakeDynamic(this.DelegateType, this.Binder, args[0]);
    }
  }
}
