// Type: System.Linq.Expressions.NewValueTypeExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal class NewValueTypeExpression : NewExpression
  {
    private readonly Type _valueType;

    public override sealed Type Type
    {
      get
      {
        return this._valueType;
      }
    }

    internal NewValueTypeExpression(Type type, ReadOnlyCollection<Expression> arguments, ReadOnlyCollection<MemberInfo> members)
      : base((ConstructorInfo) null, (IList<Expression>) arguments, members)
    {
      this._valueType = type;
    }
  }
}
