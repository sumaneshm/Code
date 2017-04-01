// Type: System.Linq.Expressions.TypedParameterExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Linq.Expressions
{
  internal class TypedParameterExpression : ParameterExpression
  {
    private readonly Type _paramType;

    public override sealed Type Type
    {
      get
      {
        return this._paramType;
      }
    }

    internal TypedParameterExpression(Type type, string name)
      : base(name)
    {
      this._paramType = type;
    }
  }
}
