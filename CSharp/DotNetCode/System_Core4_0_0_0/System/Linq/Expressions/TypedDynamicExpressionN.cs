// Type: System.Linq.Expressions.TypedDynamicExpressionN
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  internal class TypedDynamicExpressionN : DynamicExpressionN
  {
    private readonly Type _returnType;

    public override sealed Type Type
    {
      get
      {
        return this._returnType;
      }
    }

    internal TypedDynamicExpressionN(Type returnType, Type delegateType, CallSiteBinder binder, IList<Expression> arguments)
      : base(delegateType, binder, arguments)
    {
      this._returnType = returnType;
    }
  }
}
