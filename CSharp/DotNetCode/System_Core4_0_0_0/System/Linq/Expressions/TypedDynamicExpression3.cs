// Type: System.Linq.Expressions.TypedDynamicExpression3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  internal sealed class TypedDynamicExpression3 : DynamicExpression3
  {
    private readonly Type _retType;

    public override sealed Type Type
    {
      get
      {
        return this._retType;
      }
    }

    internal TypedDynamicExpression3(Type retType, Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2)
      : base(delegateType, binder, arg0, arg1, arg2)
    {
      this._retType = retType;
    }
  }
}
