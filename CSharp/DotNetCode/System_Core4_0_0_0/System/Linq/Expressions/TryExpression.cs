// Type: System.Linq.Expressions.TryExpression
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
  [DebuggerTypeProxy(typeof (Expression.TryExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class TryExpression : Expression
  {
    private readonly Type _type;
    private readonly Expression _body;
    private readonly ReadOnlyCollection<CatchBlock> _handlers;
    private readonly Expression _finally;
    private readonly Expression _fault;

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
        return ExpressionType.Try;
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
    public ReadOnlyCollection<CatchBlock> Handlers
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._handlers;
      }
    }

    [__DynamicallyInvokable]
    public Expression Finally
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._finally;
      }
    }

    [__DynamicallyInvokable]
    public Expression Fault
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._fault;
      }
    }

    internal TryExpression(Type type, Expression body, Expression @finally, Expression fault, ReadOnlyCollection<CatchBlock> handlers)
    {
      this._type = type;
      this._body = body;
      this._handlers = handlers;
      this._finally = @finally;
      this._fault = fault;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitTry(this);
    }

    [__DynamicallyInvokable]
    public TryExpression Update(Expression body, IEnumerable<CatchBlock> handlers, Expression @finally, Expression fault)
    {
      if (body == this.Body && handlers == this.Handlers && (@finally == this.Finally && fault == this.Fault))
        return this;
      else
        return Expression.MakeTry(this.Type, body, @finally, fault, handlers);
    }
  }
}
