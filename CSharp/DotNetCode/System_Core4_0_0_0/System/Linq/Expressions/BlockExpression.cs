// Type: System.Linq.Expressions.BlockExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Runtime;
using System.Threading;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.BlockExpressionProxy))]
  [__DynamicallyInvokable]
  public class BlockExpression : Expression
  {
    [__DynamicallyInvokable]
    public ReadOnlyCollection<Expression> Expressions
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetOrMakeExpressions();
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<ParameterExpression> Variables
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetOrMakeVariables();
      }
    }

    [__DynamicallyInvokable]
    public Expression Result
    {
      [__DynamicallyInvokable] get
      {
        return this.GetExpression(this.ExpressionCount - 1);
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Block;
      }
    }

    [__DynamicallyInvokable]
    public override Type Type
    {
      [__DynamicallyInvokable] get
      {
        return this.GetExpression(this.ExpressionCount - 1).Type;
      }
    }

    internal virtual int ExpressionCount
    {
      get
      {
        throw ContractUtils.Unreachable;
      }
    }

    internal virtual int VariableCount
    {
      get
      {
        return 0;
      }
    }

    internal BlockExpression()
    {
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitBlock(this);
    }

    [__DynamicallyInvokable]
    public BlockExpression Update(IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions)
    {
      if (variables == this.Variables && expressions == this.Expressions)
        return this;
      else
        return Expression.Block(this.Type, variables, expressions);
    }

    internal virtual Expression GetExpression(int index)
    {
      throw ContractUtils.Unreachable;
    }

    internal virtual ReadOnlyCollection<Expression> GetOrMakeExpressions()
    {
      throw ContractUtils.Unreachable;
    }

    internal virtual ParameterExpression GetVariable(int index)
    {
      throw ContractUtils.Unreachable;
    }

    internal virtual ReadOnlyCollection<ParameterExpression> GetOrMakeVariables()
    {
      return EmptyReadOnlyCollection<ParameterExpression>.Instance;
    }

    internal virtual BlockExpression Rewrite(ReadOnlyCollection<ParameterExpression> variables, Expression[] args)
    {
      throw ContractUtils.Unreachable;
    }

    internal static ReadOnlyCollection<Expression> ReturnReadOnlyExpressions(BlockExpression provider, ref object collection)
    {
      Expression expression = collection as Expression;
      if (expression != null)
        Interlocked.CompareExchange(ref collection, (object) new ReadOnlyCollection<Expression>((IList<Expression>) new BlockExpressionList(provider, expression)), (object) expression);
      return (ReadOnlyCollection<Expression>) collection;
    }
  }
}
