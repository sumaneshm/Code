// Type: System.Linq.Expressions.LabelExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.LabelExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class LabelExpression : Expression
  {
    private readonly Expression _defaultValue;
    private readonly LabelTarget _target;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        return this._target.Type;
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Label;
      }
    }

    [__DynamicallyInvokable]
    public LabelTarget Target
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._target;
      }
    }

    [__DynamicallyInvokable]
    public Expression DefaultValue
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._defaultValue;
      }
    }

    internal LabelExpression(LabelTarget label, Expression defaultValue)
    {
      this._target = label;
      this._defaultValue = defaultValue;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitLabel(this);
    }

    [__DynamicallyInvokable]
    public LabelExpression Update(LabelTarget target, Expression defaultValue)
    {
      if (target == this.Target && defaultValue == this.DefaultValue)
        return this;
      else
        return Expression.Label(target, defaultValue);
    }
  }
}
