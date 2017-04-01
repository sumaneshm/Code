// Type: System.Linq.Expressions.MethodCallExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.MethodCallExpressionProxy))]
  [__DynamicallyInvokable]
  public class MethodCallExpression : Expression, IArgumentProvider
  {
    private readonly MethodInfo _method;

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Call;
      }
    }

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        return this._method.ReturnType;
      }
    }

    [__DynamicallyInvokable]
    public MethodInfo Method
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._method;
      }
    }

    [__DynamicallyInvokable]
    public Expression Object
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetInstance();
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<Expression> Arguments
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetOrMakeArguments();
      }
    }

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        throw ContractUtils.Unreachable;
      }
    }

    internal MethodCallExpression(MethodInfo method)
    {
      this._method = method;
    }

    internal virtual Expression GetInstance()
    {
      return (Expression) null;
    }

    [__DynamicallyInvokable]
    public MethodCallExpression Update(Expression @object, IEnumerable<Expression> arguments)
    {
      if (@object == this.Object && arguments == this.Arguments)
        return this;
      else
        return Expression.Call(@object, this.Method, arguments);
    }

    internal virtual ReadOnlyCollection<Expression> GetOrMakeArguments()
    {
      throw ContractUtils.Unreachable;
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitMethodCall(this);
    }

    internal virtual MethodCallExpression Rewrite(Expression instance, IList<Expression> args)
    {
      throw ContractUtils.Unreachable;
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      throw ContractUtils.Unreachable;
    }
  }
}
