// Type: System.Linq.Expressions.ConditionalExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.ConditionalExpressionProxy))]
  [__DynamicallyInvokable]
  public class ConditionalExpression : Expression
  {
    private readonly Expression _test;
    private readonly Expression _true;

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Conditional;
      }
    }

    [__DynamicallyInvokable]
    public override Type Type
    {
      [__DynamicallyInvokable] get
      {
        return this.IfTrue.Type;
      }
    }

    [__DynamicallyInvokable]
    public Expression Test
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._test;
      }
    }

    [__DynamicallyInvokable]
    public Expression IfTrue
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._true;
      }
    }

    [__DynamicallyInvokable]
    public Expression IfFalse
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetFalse();
      }
    }

    internal ConditionalExpression(Expression test, Expression ifTrue)
    {
      this._test = test;
      this._true = ifTrue;
    }

    internal virtual Expression GetFalse()
    {
      return (Expression) Expression.Empty();
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitConditional(this);
    }

    [__DynamicallyInvokable]
    public ConditionalExpression Update(Expression test, Expression ifTrue, Expression ifFalse)
    {
      if (test == this.Test && ifTrue == this.IfTrue && ifFalse == this.IfFalse)
        return this;
      else
        return Expression.Condition(test, ifTrue, ifFalse, this.Type);
    }

    internal static ConditionalExpression Make(Expression test, Expression ifTrue, Expression ifFalse, Type type)
    {
      if (ifTrue.Type != type || ifFalse.Type != type)
        return (ConditionalExpression) new FullConditionalExpressionWithType(test, ifTrue, ifFalse, type);
      if (ifFalse is DefaultExpression && ifFalse.Type == typeof (void))
        return new ConditionalExpression(test, ifTrue);
      else
        return (ConditionalExpression) new FullConditionalExpression(test, ifTrue, ifFalse);
    }
  }
}
