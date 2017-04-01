// Type: System.Linq.Expressions.ScopeWithType
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
  internal class ScopeWithType : ScopeN
  {
    private readonly Type _type;

    public override sealed Type Type
    {
      get
      {
        return this._type;
      }
    }

    internal ScopeWithType(IList<ParameterExpression> variables, IList<Expression> expressions, Type type)
      : base(variables, expressions)
    {
      this._type = type;
    }

    internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
    {
      return (BlockExpression) new ScopeWithType(this.ReuseOrValidateVariables(variables), (IList<Expression>) args, this._type);
    }
  }
}
