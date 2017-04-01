// Type: System.Linq.Expressions.NewArrayExpression
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
  [DebuggerTypeProxy(typeof (Expression.NewArrayExpressionProxy))]
  [__DynamicallyInvokable]
  public class NewArrayExpression : Expression
  {
    private readonly ReadOnlyCollection<Expression> _expressions;
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
    public ReadOnlyCollection<Expression> Expressions
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._expressions;
      }
    }

    internal NewArrayExpression(Type type, ReadOnlyCollection<Expression> expressions)
    {
      this._expressions = expressions;
      this._type = type;
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitNewArray(this);
    }

    [__DynamicallyInvokable]
    public NewArrayExpression Update(IEnumerable<Expression> expressions)
    {
      if (expressions == this.Expressions)
        return this;
      if (this.NodeType == ExpressionType.NewArrayInit)
        return Expression.NewArrayInit(this.Type.GetElementType(), expressions);
      else
        return Expression.NewArrayBounds(this.Type.GetElementType(), expressions);
    }

    internal static NewArrayExpression Make(ExpressionType nodeType, Type type, ReadOnlyCollection<Expression> expressions)
    {
      if (nodeType == ExpressionType.NewArrayInit)
        return (NewArrayExpression) new NewArrayInitExpression(type, expressions);
      else
        return (NewArrayExpression) new NewArrayBoundsExpression(type, expressions);
    }
  }
}
