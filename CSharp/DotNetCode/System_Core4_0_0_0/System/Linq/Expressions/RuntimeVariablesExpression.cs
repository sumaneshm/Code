// Type: System.Linq.Expressions.RuntimeVariablesExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.RuntimeVariablesExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class RuntimeVariablesExpression : Expression
  {
    private readonly ReadOnlyCollection<ParameterExpression> _variables;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        return typeof (IRuntimeVariables);
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.RuntimeVariables;
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<ParameterExpression> Variables
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._variables;
      }
    }

    internal RuntimeVariablesExpression(ReadOnlyCollection<ParameterExpression> variables)
    {
      this._variables = variables;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitRuntimeVariables(this);
    }

    [__DynamicallyInvokable]
    public RuntimeVariablesExpression Update(IEnumerable<ParameterExpression> variables)
    {
      if (variables == this.Variables)
        return this;
      else
        return Expression.RuntimeVariables(variables);
    }
  }
}
