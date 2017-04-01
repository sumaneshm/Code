// Type: System.Linq.Expressions.FieldExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal class FieldExpression : MemberExpression
  {
    private readonly FieldInfo _field;

    public override sealed Type Type
    {
      get
      {
        return this._field.FieldType;
      }
    }

    public FieldExpression(Expression expression, FieldInfo member)
      : base(expression)
    {
      this._field = member;
    }

    internal override MemberInfo GetMember()
    {
      return (MemberInfo) this._field;
    }
  }
}
