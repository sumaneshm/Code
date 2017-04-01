// Type: System.Linq.Expressions.Scope1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
  internal sealed class Scope1 : ScopeExpression
  {
    private object _body;

    internal override int ExpressionCount
    {
      get
      {
        return 1;
      }
    }

    internal Scope1(IList<ParameterExpression> variables, Expression body)
      : base(variables)
    {
      this._body = (object) body;
    }

    internal override Expression GetExpression(int index)
    {
      if (index == 0)
        return Expression.ReturnObject<Expression>(this._body);
      else
        throw new InvalidOperationException();
    }

    internal override ReadOnlyCollection<Expression> GetOrMakeExpressions()
    {
      return BlockExpression.ReturnReadOnlyExpressions((BlockExpression) this, ref this._body);
    }

    internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
    {
      return (BlockExpression) new Scope1(this.ReuseOrValidateVariables(variables), args[0]);
    }
  }
}
