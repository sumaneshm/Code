// Type: System.Linq.Expressions.ConstantCheck
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Dynamic.Utils;

namespace System.Linq.Expressions
{
  internal static class ConstantCheck
  {
    internal static bool IsNull(Expression e)
    {
      if (e.NodeType == ExpressionType.Constant)
        return ((ConstantExpression) e).Value == null;
      else
        return false;
    }

    internal static AnalyzeTypeIsResult AnalyzeTypeIs(TypeBinaryExpression typeIs)
    {
      return ConstantCheck.AnalyzeTypeIs(typeIs.Expression, typeIs.TypeOperand);
    }

    private static AnalyzeTypeIsResult AnalyzeTypeIs(Expression operand, Type testType)
    {
      Type type = operand.Type;
      if (type == typeof (void))
        return AnalyzeTypeIsResult.KnownFalse;
      Type nonNullableType = TypeUtils.GetNonNullableType(type);
      if (!TypeUtils.GetNonNullableType(testType).IsAssignableFrom(nonNullableType))
        return AnalyzeTypeIsResult.Unknown;
      return type.IsValueType && !TypeUtils.IsNullableType(type) ? AnalyzeTypeIsResult.KnownTrue : AnalyzeTypeIsResult.KnownAssignable;
    }
  }
}
