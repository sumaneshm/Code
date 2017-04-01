// Type: System.Linq.Expressions.NewExpression
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
  [DebuggerTypeProxy(typeof (Expression.NewExpressionProxy))]
  [__DynamicallyInvokable]
  public class NewExpression : Expression, IArgumentProvider
  {
    private readonly ConstructorInfo _constructor;
    private IList<Expression> _arguments;
    private readonly ReadOnlyCollection<MemberInfo> _members;

    [__DynamicallyInvokable]
    public override Type Type
    {
      [__DynamicallyInvokable] get
      {
        return this._constructor.DeclaringType;
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.New;
      }
    }

    [__DynamicallyInvokable]
    public ConstructorInfo Constructor
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._constructor;
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

    [__DynamicallyInvokable]
    public ReadOnlyCollection<MemberInfo> Members
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._members;
      }
    }

    internal NewExpression(ConstructorInfo constructor, IList<Expression> arguments, ReadOnlyCollection<MemberInfo> members)
    {
      this._constructor = constructor;
      this._arguments = arguments;
      this._members = members;
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      return this._arguments[index];
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitNew(this);
    }

    [__DynamicallyInvokable]
    public NewExpression Update(IEnumerable<Expression> arguments)
    {
      if (arguments == this.Arguments)
        return this;
      if (this.Members != null)
        return Expression.New(this.Constructor, arguments, (IEnumerable<MemberInfo>) this.Members);
      else
        return Expression.New(this.Constructor, arguments);
    }
  }
}
