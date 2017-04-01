// Type: System.Linq.Expressions.IndexExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.IndexExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class IndexExpression : Expression, IArgumentProvider
  {
    private readonly Expression _instance;
    private readonly PropertyInfo _indexer;
    private IList<Expression> _arguments;

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Index;
      }
    }

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        if (this._indexer != (PropertyInfo) null)
          return this._indexer.PropertyType;
        else
          return this._instance.Type.GetElementType();
      }
    }

    [__DynamicallyInvokable]
    public Expression Object
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._instance;
      }
    }

    [__DynamicallyInvokable]
    public PropertyInfo Indexer
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._indexer;
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

    internal IndexExpression(Expression instance, PropertyInfo indexer, IList<Expression> arguments)
    {
      int num = indexer == (PropertyInfo) null ? 1 : 0;
      this._instance = instance;
      this._indexer = indexer;
      this._arguments = arguments;
    }

    [__DynamicallyInvokable]
    public IndexExpression Update(Expression @object, IEnumerable<Expression> arguments)
    {
      if (@object == this.Object && arguments == this.Arguments)
        return this;
      else
        return Expression.MakeIndex(@object, this.Indexer, arguments);
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      return this._arguments[index];
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitIndex(this);
    }

    internal Expression Rewrite(Expression instance, Expression[] arguments)
    {
      Expression instance1 = instance;
      PropertyInfo indexer = this._indexer;
      Expression[] expressionArray = arguments;
      IList<Expression> list = expressionArray != null ? (IList<Expression>) expressionArray : this._arguments;
      return (Expression) Expression.MakeIndex(instance1, indexer, (IEnumerable<Expression>) list);
    }
  }
}
