// Type: System.Linq.Expressions.FullConditionalExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

namespace System.Linq.Expressions
{
  internal class FullConditionalExpression : ConditionalExpression
  {
    private readonly Expression _false;

    internal FullConditionalExpression(Expression test, Expression ifTrue, Expression ifFalse)
      : base(test, ifTrue)
    {
      this._false = ifFalse;
    }

    internal override Expression GetFalse()
    {
      return this._false;
    }
  }
}
