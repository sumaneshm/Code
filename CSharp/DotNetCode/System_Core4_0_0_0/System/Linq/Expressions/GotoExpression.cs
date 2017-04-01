// Type: System.Linq.Expressions.GotoExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.GotoExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class GotoExpression : Expression
  {
    private readonly GotoExpressionKind _kind;
    private readonly Expression _value;
    private readonly LabelTarget _target;
    private readonly Type _type;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._type;
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Goto;
      }
    }

    [__DynamicallyInvokable]
    public Expression Value
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._value;
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
    public GotoExpressionKind Kind
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._kind;
      }
    }

    internal GotoExpression(GotoExpressionKind kind, LabelTarget target, Expression value, Type type)
    {
      this._kind = kind;
      this._value = value;
      this._target = target;
      this._type = type;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitGoto(this);
    }

    [__DynamicallyInvokable]
    public GotoExpression Update(LabelTarget target, Expression value)
    {
      if (target == this.Target && value == this.Value)
        return this;
      else
        return Expression.MakeGoto(this.Kind, target, value, this.Type);
    }
  }
}
