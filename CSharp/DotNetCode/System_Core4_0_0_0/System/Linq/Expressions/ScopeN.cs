// Type: System.Linq.Expressions.ScopeN
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
  internal class ScopeN : ScopeExpression
  {
    private IList<Expression> _body;

    internal override int ExpressionCount
    {
      get
      {
        return this._body.Count;
      }
    }

    internal ScopeN(IList<ParameterExpression> variables, IList<Expression> body)
      : base(variables)
    {
      this._body = body;
    }

    internal override Expression GetExpression(int index)
    {
      return this._body[index];
    }

    internal override ReadOnlyCollection<Expression> GetOrMakeExpressions()
    {
      return Expression.ReturnReadOnly<Expression>(ref this._body);
    }

    internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
    {
      return (BlockExpression) new ScopeN(this.ReuseOrValidateVariables(variables), (IList<Expression>) args);
    }
  }
}
