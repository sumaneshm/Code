// Type: System.Linq.Expressions.NewArrayInitExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
  internal sealed class NewArrayInitExpression : NewArrayExpression
  {
    public override sealed ExpressionType NodeType
    {
      get
      {
        return ExpressionType.NewArrayInit;
      }
    }

    internal NewArrayInitExpression(Type type, ReadOnlyCollection<Expression> expressions)
      : base(type, expressions)
    {
    }
  }
}
