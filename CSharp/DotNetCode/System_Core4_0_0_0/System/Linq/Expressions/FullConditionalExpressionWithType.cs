// Type: System.Linq.Expressions.FullConditionalExpressionWithType
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Linq.Expressions
{
  internal class FullConditionalExpressionWithType : FullConditionalExpression
  {
    private readonly Type _type;

    public override sealed Type Type
    {
      get
      {
        return this._type;
      }
    }

    internal FullConditionalExpressionWithType(Expression test, Expression ifTrue, Expression ifFalse, Type type)
      : base(test, ifTrue, ifFalse)
    {
      this._type = type;
    }
  }
}
