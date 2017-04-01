﻿// Type: System.Linq.Expressions.Block2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
  internal sealed class Block2 : BlockExpression
  {
    private object _arg0;
    private readonly Expression _arg1;

    internal override int ExpressionCount
    {
      get
      {
        return 2;
      }
    }

    internal Block2(Expression arg0, Expression arg1)
    {
      this._arg0 = (object) arg0;
      this._arg1 = arg1;
    }

    internal override Expression GetExpression(int index)
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

    internal override ReadOnlyCollection<Expression> GetOrMakeExpressions()
    {
      return BlockExpression.ReturnReadOnlyExpressions((BlockExpression) this, ref this._arg0);
    }

    internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
    {
      return (BlockExpression) new Block2(args[0], args[1]);
    }
  }
}
