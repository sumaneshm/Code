// Type: System.Linq.Expressions.OpAssignMethodConversionBinaryExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal sealed class OpAssignMethodConversionBinaryExpression : MethodBinaryExpression
  {
    private readonly LambdaExpression _conversion;

    internal OpAssignMethodConversionBinaryExpression(ExpressionType nodeType, Expression left, Expression right, Type type, MethodInfo method, LambdaExpression conversion)
      : base(nodeType, left, right, type, method)
    {
      this._conversion = conversion;
    }

    internal override LambdaExpression GetConversion()
    {
      return this._conversion;
    }
  }
}
