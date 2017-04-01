// Type: System.Linq.Expressions.DynamicExpressionN
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  internal class DynamicExpressionN : DynamicExpression, IArgumentProvider
  {
    private IList<Expression> _arguments;

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        return this._arguments.Count;
      }
    }

    internal DynamicExpressionN(Type delegateType, CallSiteBinder binder, IList<Expression> arguments)
      : base(delegateType, binder)
    {
      this._arguments = arguments;
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      return this._arguments[index];
    }

    internal override ReadOnlyCollection<Expression> GetOrMakeArguments()
    {
      return Expression.ReturnReadOnly<Expression>(ref this._arguments);
    }

    internal override DynamicExpression Rewrite(Expression[] args)
    {
      return Expression.MakeDynamic(this.DelegateType, this.Binder, args);
    }
  }
}
