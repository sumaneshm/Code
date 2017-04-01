// Type: System.Linq.Expressions.LogicalBinaryExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Linq.Expressions
{
  internal sealed class LogicalBinaryExpression : BinaryExpression
  {
    private readonly ExpressionType _nodeType;

    public override sealed Type Type
    {
      get
      {
        return typeof (bool);
      }
    }

    public override sealed ExpressionType NodeType
    {
      get
      {
        return this._nodeType;
      }
    }

    internal LogicalBinaryExpression(ExpressionType nodeType, Expression left, Expression right)
      : base(left, right)
    {
      this._nodeType = nodeType;
    }
  }
}
