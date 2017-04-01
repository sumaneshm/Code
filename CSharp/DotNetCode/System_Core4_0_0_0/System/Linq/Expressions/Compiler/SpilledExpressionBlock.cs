// Type: System.Linq.Expressions.Compiler.SpilledExpressionBlock
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq.Expressions;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class SpilledExpressionBlock : BlockN
  {
    internal SpilledExpressionBlock(IList<Expression> expressions)
      : base(expressions)
    {
    }

    internal override BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
    {
      throw ContractUtils.Unreachable;
    }
  }
}
