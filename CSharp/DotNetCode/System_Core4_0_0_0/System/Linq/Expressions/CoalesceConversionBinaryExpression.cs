// Type: System.Linq.Expressions.CoalesceConversionBinaryExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Linq.Expressions
{
  internal sealed class CoalesceConversionBinaryExpression : BinaryExpression
  {
    private readonly LambdaExpression _conversion;

    public override sealed ExpressionType NodeType
    {
      get
      {
        return ExpressionType.Coalesce;
      }
    }

    public override sealed Type Type
    {
      get
      {
        return this.Right.Type;
      }
    }

    internal CoalesceConversionBinaryExpression(Expression left, Expression right, LambdaExpression conversion)
      : base(left, right)
    {
      this._conversion = conversion;
    }

    internal override LambdaExpression GetConversion()
    {
      return this._conversion;
    }
  }
}
