// Type: System.Linq.Expressions.BlockN
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
  internal class BlockN : BlockExpression
  {
    private IList<Expression> _expressions;

    internal override int ExpressionCount
    {
      get
      {
        return this._expressions.Count;
      }
    }

    internal BlockN(IList<Expression> expressions)
    {
      this._expressions = expressions;
    }

    internal override Expression GetExpression(int index)
    {
      return this._expressions[index];
    }

    internal override ReadOnlyCollection<Expression> GetOrMakeExpressions()
    {
      return Expression.ReturnReadOnly<Expression>(ref this._expressions);
    }

    internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
    {
      return (BlockExpression) new BlockN((IList<Expression>) args);
    }
  }
}
