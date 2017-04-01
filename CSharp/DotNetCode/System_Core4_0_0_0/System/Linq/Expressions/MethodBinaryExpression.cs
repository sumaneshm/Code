// Type: System.Linq.Expressions.MethodBinaryExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal class MethodBinaryExpression : SimpleBinaryExpression
  {
    private readonly MethodInfo _method;

    internal MethodBinaryExpression(ExpressionType nodeType, Expression left, Expression right, Type type, MethodInfo method)
      : base(nodeType, left, right, type)
    {
      this._method = method;
    }

    internal override MethodInfo GetMethod()
    {
      return this._method;
    }
  }
}
