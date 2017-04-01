// Type: System.Linq.Expressions.InvocationExpression
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
  [DebuggerTypeProxy(typeof (Expression.InvocationExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class InvocationExpression : Expression, IArgumentProvider
  {
    private IList<Expression> _arguments;
    private readonly Expression _lambda;
    private readonly Type _returnType;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._returnType;
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Invoke;
      }
    }

    [__DynamicallyInvokable]
    public Expression Expression
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._lambda;
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<Expression> Arguments
    {
      [__DynamicallyInvokable] get
      {
        return Expression.ReturnReadOnly<Expression>(ref this._arguments);
      }
    }

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        return this._arguments.Count;
      }
    }

    internal LambdaExpression LambdaOperand
    {
      get
      {
        if (this._lambda.NodeType != ExpressionType.Quote)
          return this._lambda as LambdaExpression;
        else
          return (LambdaExpression) ((UnaryExpression) this._lambda).Operand;
      }
    }

    internal InvocationExpression(Expression lambda, IList<Expression> arguments, Type returnType)
    {
      this._lambda = lambda;
      this._arguments = arguments;
      this._returnType = returnType;
    }

    [__DynamicallyInvokable]
    public InvocationExpression Update(Expression expression, IEnumerable<Expression> arguments)
    {
      if (expression == this.Expression && arguments == this.Arguments)
        return this;
      else
        return Expression.Invoke(expression, arguments);
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      return this._arguments[index];
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitInvocation(this);
    }

    internal InvocationExpression Rewrite(Expression lambda, Expression[] arguments)
    {
      Expression expression = lambda;
      Expression[] expressionArray = arguments;
      IList<Expression> list = expressionArray != null ? (IList<Expression>) expressionArray : this._arguments;
      return Expression.Invoke(expression, (IEnumerable<Expression>) list);
    }
  }
}
