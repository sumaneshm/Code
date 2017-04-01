// Type: System.Linq.Expressions.ListInitExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.ListInitExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class ListInitExpression : Expression
  {
    private readonly NewExpression _newExpression;
    private readonly ReadOnlyCollection<ElementInit> _initializers;

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.ListInit;
      }
    }

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        return this._newExpression.Type;
      }
    }

    [__DynamicallyInvokable]
    public override bool CanReduce
    {
      [__DynamicallyInvokable] get
      {
        return true;
      }
    }

    [__DynamicallyInvokable]
    public NewExpression NewExpression
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._newExpression;
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<ElementInit> Initializers
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._initializers;
      }
    }

    internal ListInitExpression(NewExpression newExpression, ReadOnlyCollection<ElementInit> initializers)
    {
      this._newExpression = newExpression;
      this._initializers = initializers;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitListInit(this);
    }

    [__DynamicallyInvokable]
    public override Expression Reduce()
    {
      return MemberInitExpression.ReduceListInit((Expression) this._newExpression, this._initializers, true);
    }

    [__DynamicallyInvokable]
    public ListInitExpression Update(NewExpression newExpression, IEnumerable<ElementInit> initializers)
    {
      if (newExpression == this.NewExpression && initializers == this.Initializers)
        return this;
      else
        return Expression.ListInit(newExpression, initializers);
    }
  }
}
