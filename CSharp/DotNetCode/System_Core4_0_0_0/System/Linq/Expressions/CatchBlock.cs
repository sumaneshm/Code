// Type: System.Linq.Expressions.CatchBlock
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.CatchBlockProxy))]
  [__DynamicallyInvokable]
  public sealed class CatchBlock
  {
    private readonly Type _test;
    private readonly ParameterExpression _var;
    private readonly Expression _body;
    private readonly Expression _filter;

    [__DynamicallyInvokable]
    public ParameterExpression Variable
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._var;
      }
    }

    [__DynamicallyInvokable]
    public Type Test
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._test;
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
    public Expression Filter
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._filter;
      }
    }

    internal CatchBlock(Type test, ParameterExpression variable, Expression body, Expression filter)
    {
      this._test = test;
      this._var = variable;
      this._body = body;
      this._filter = filter;
    }

    [__DynamicallyInvokable]
    public override string ToString()
    {
      return ExpressionStringBuilder.CatchBlockToString(this);
    }

    [__DynamicallyInvokable]
    public CatchBlock Update(ParameterExpression variable, Expression filter, Expression body)
    {
      if (variable == this.Variable && filter == this.Filter && body == this.Body)
        return this;
      else
        return Expression.MakeCatchBlock(this.Test, variable, body, filter);
    }
  }
}
