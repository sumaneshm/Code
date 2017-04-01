// Type: System.Linq.Expressions.LoopExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.LoopExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class LoopExpression : Expression
  {
    private readonly Expression _body;
    private readonly LabelTarget _break;
    private readonly LabelTarget _continue;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        if (this._break != null)
          return this._break.Type;
        else
          return typeof (void);
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Loop;
      }
    }

    [__DynamicallyInvokable]
    public Expression Body
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._body;
      }
    }

    [__DynamicallyInvokable]
    public LabelTarget BreakLabel
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._break;
      }
    }

    [__DynamicallyInvokable]
    public LabelTarget ContinueLabel
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._continue;
      }
    }

    internal LoopExpression(Expression body, LabelTarget @break, LabelTarget @continue)
    {
      this._body = body;
      this._break = @break;
      this._continue = @continue;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitLoop(this);
    }

    [__DynamicallyInvokable]
    public LoopExpression Update(LabelTarget breakLabel, LabelTarget continueLabel, Expression body)
    {
      if (breakLabel == this.BreakLabel && continueLabel == this.ContinueLabel && body == this.Body)
        return this;
      else
        return Expression.Loop(body, breakLabel, continueLabel);
    }
  }
}
