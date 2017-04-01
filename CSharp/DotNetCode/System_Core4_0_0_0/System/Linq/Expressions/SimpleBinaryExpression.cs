// Type: System.Linq.Expressions.SimpleBinaryExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Linq.Expressions
{
  internal class SimpleBinaryExpression : BinaryExpression
  {
    private readonly ExpressionType _nodeType;
    private readonly Type _type;

    public override sealed ExpressionType NodeType
    {
      get
      {
        return this._nodeType;
      }
    }

    public override sealed Type Type
    {
      get
      {
        return this._type;
      }
    }

    internal SimpleBinaryExpression(ExpressionType nodeType, Expression left, Expression right, Type type)
      : base(left, right)
    {
      this._nodeType = nodeType;
      this._type = type;
    }
  }
}
