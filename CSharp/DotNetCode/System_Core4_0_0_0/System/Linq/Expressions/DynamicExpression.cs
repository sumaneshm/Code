// Type: System.Linq.Expressions.DynamicExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.DynamicExpressionProxy))]
  [__DynamicallyInvokable]
  public class DynamicExpression : Expression, IArgumentProvider
  {
    private readonly CallSiteBinder _binder;
    private readonly Type _delegateType;

    [__DynamicallyInvokable]
    public override Type Type
    {
      [__DynamicallyInvokable] get
      {
        return typeof (object);
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Dynamic;
      }
    }

    [__DynamicallyInvokable]
    public CallSiteBinder Binder
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._binder;
      }
    }

    [__DynamicallyInvokable]
    public Type DelegateType
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._delegateType;
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

    internal DynamicExpression(Type delegateType, CallSiteBinder binder)
    {
      this._delegateType = delegateType;
      this._binder = binder;
    }

    internal virtual ReadOnlyCollection<Expression> GetOrMakeArguments()
    {
      throw ContractUtils.Unreachable;
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitDynamic(this);
    }

    internal virtual DynamicExpression Rewrite(Expression[] args)
    {
      throw ContractUtils.Unreachable;
    }

    [__DynamicallyInvokable]
    public DynamicExpression Update(IEnumerable<Expression> arguments)
    {
      if (arguments == this.Arguments)
        return this;
      else
        return Expression.MakeDynamic(this.DelegateType, this.Binder, arguments);
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      throw ContractUtils.Unreachable;
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, params Expression[] arguments)
    {
      return Expression.Dynamic(binder, returnType, arguments);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, IEnumerable<Expression> arguments)
    {
      return Expression.Dynamic(binder, returnType, arguments);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0)
    {
      return Expression.Dynamic(binder, returnType, arg0);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1)
    {
      return Expression.Dynamic(binder, returnType, arg0, arg1);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2)
    {
      return Expression.Dynamic(binder, returnType, arg0, arg1, arg2);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
    {
      return Expression.Dynamic(binder, returnType, arg0, arg1, arg2, arg3);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, IEnumerable<Expression> arguments)
    {
      return Expression.MakeDynamic(delegateType, binder, arguments);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, params Expression[] arguments)
    {
      return Expression.MakeDynamic(delegateType, binder, arguments);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0)
    {
      return Expression.MakeDynamic(delegateType, binder, arg0);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1)
    {
      return Expression.MakeDynamic(delegateType, binder, arg0, arg1);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2)
    {
      return Expression.MakeDynamic(delegateType, binder, arg0, arg1, arg2);
    }

    [__DynamicallyInvokable]
    public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
    {
      return Expression.MakeDynamic(delegateType, binder, arg0, arg1, arg2, arg3);
    }

    internal static DynamicExpression Make(Type returnType, Type delegateType, CallSiteBinder binder, ReadOnlyCollection<Expression> arguments)
    {
      if (returnType == typeof (object))
        return (DynamicExpression) new DynamicExpressionN(delegateType, binder, (IList<Expression>) arguments);
      else
        return (DynamicExpression) new TypedDynamicExpressionN(returnType, delegateType, binder, (IList<Expression>) arguments);
    }

    internal static DynamicExpression Make(Type returnType, Type delegateType, CallSiteBinder binder, Expression arg0)
    {
      if (returnType == typeof (object))
        return (DynamicExpression) new DynamicExpression1(delegateType, binder, arg0);
      else
        return (DynamicExpression) new TypedDynamicExpression1(returnType, delegateType, binder, arg0);
    }

    internal static DynamicExpression Make(Type returnType, Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1)
    {
      if (returnType == typeof (object))
        return (DynamicExpression) new DynamicExpression2(delegateType, binder, arg0, arg1);
      else
        return (DynamicExpression) new TypedDynamicExpression2(returnType, delegateType, binder, arg0, arg1);
    }

    internal static DynamicExpression Make(Type returnType, Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2)
    {
      if (returnType == typeof (object))
        return (DynamicExpression) new DynamicExpression3(delegateType, binder, arg0, arg1, arg2);
      else
        return (DynamicExpression) new TypedDynamicExpression3(returnType, delegateType, binder, arg0, arg1, arg2);
    }

    internal static DynamicExpression Make(Type returnType, Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
    {
      if (returnType == typeof (object))
        return (DynamicExpression) new DynamicExpression4(delegateType, binder, arg0, arg1, arg2, arg3);
      else
        return (DynamicExpression) new TypedDynamicExpression4(returnType, delegateType, binder, arg0, arg1, arg2, arg3);
    }
  }
}
