// Type: System.Linq.Expressions.ConstantExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.ConstantExpressionProxy))]
  [__DynamicallyInvokable]
  public class ConstantExpression : Expression
  {
    private readonly object _value;

    [__DynamicallyInvokable]
    public override Type Type
    {
      [__DynamicallyInvokable] get
      {
        if (this._value == null)
          return typeof (object);
        else
          return this._value.GetType();
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Constant;
      }
    }

    [__DynamicallyInvokable]
    public object Value
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._value;
      }
    }

    internal ConstantExpression(object value)
    {
      this._value = value;
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitConstant(this);
    }

    internal static ConstantExpression Make(object value, Type type)
    {
      if (value == null && type == typeof (object) || value != null && value.GetType() == type)
        return new ConstantExpression(value);
      else
        return (ConstantExpression) new TypedConstantExpression(value, type);
    }
  }
}
