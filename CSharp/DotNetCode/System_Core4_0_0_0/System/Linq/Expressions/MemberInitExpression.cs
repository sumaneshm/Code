// Type: System.Linq.Expressions.MemberInitExpression
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
  [DebuggerTypeProxy(typeof (Expression.MemberInitExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class MemberInitExpression : Expression
  {
    private readonly NewExpression _newExpression;
    private readonly ReadOnlyCollection<MemberBinding> _bindings;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        return this._newExpression.Type;
      }
    }

    [__DynamicallyInvokable]
    public override bool CanReduce
    {
      [__DynamicallyInvokable] get
      {
        return true;
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.MemberInit;
      }
    }

    [__DynamicallyInvokable]
    public NewExpression NewExpression
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._newExpression;
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<MemberBinding> Bindings
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._bindings;
      }
    }

    internal MemberInitExpression(NewExpression newExpression, ReadOnlyCollection<MemberBinding> bindings)
    {
      this._newExpression = newExpression;
      this._bindings = bindings;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitMemberInit(this);
    }

    [__DynamicallyInvokable]
    public override Expression Reduce()
    {
      return MemberInitExpression.ReduceMemberInit((Expression) this._newExpression, this._bindings, true);
    }

    [__DynamicallyInvokable]
    public MemberInitExpression Update(NewExpression newExpression, IEnumerable<MemberBinding> bindings)
    {
      if (newExpression == this.NewExpression && bindings == this.Bindings)
        return this;
      else
        return Expression.MemberInit(newExpression, bindings);
    }

    internal static Expression ReduceMemberInit(Expression objExpression, ReadOnlyCollection<MemberBinding> bindings, bool keepOnStack)
    {
      ParameterExpression objVar = Expression.Variable(objExpression.Type, (string) null);
      int count = bindings.Count;
      Expression[] list = new Expression[count + 2];
      list[0] = (Expression) Expression.Assign((Expression) objVar, objExpression);
      for (int index = 0; index < count; ++index)
        list[index + 1] = MemberInitExpression.ReduceMemberBinding(objVar, bindings[index]);
      list[count + 1] = keepOnStack ? (Expression) objVar : (Expression) Expression.Empty();
      return (Expression) Expression.Block((IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>(list));
    }

    internal static Expression ReduceListInit(Expression listExpression, ReadOnlyCollection<ElementInit> initializers, bool keepOnStack)
    {
      ParameterExpression parameterExpression = Expression.Variable(listExpression.Type, (string) null);
      int count = initializers.Count;
      Expression[] list = new Expression[count + 2];
      list[0] = (Expression) Expression.Assign((Expression) parameterExpression, listExpression);
      for (int index = 0; index < count; ++index)
      {
        ElementInit elementInit = initializers[index];
        list[index + 1] = (Expression) Expression.Call((Expression) parameterExpression, elementInit.AddMethod, (IEnumerable<Expression>) elementInit.Arguments);
      }
      list[count + 1] = keepOnStack ? (Expression) parameterExpression : (Expression) Expression.Empty();
      return (Expression) Expression.Block((IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>(list));
    }

    internal static Expression ReduceMemberBinding(ParameterExpression objVar, MemberBinding binding)
    {
      MemberExpression memberExpression = Expression.MakeMemberAccess((Expression) objVar, binding.Member);
      switch (binding.BindingType)
      {
        case MemberBindingType.Assignment:
          return (Expression) Expression.Assign((Expression) memberExpression, ((MemberAssignment) binding).Expression);
        case MemberBindingType.MemberBinding:
          return MemberInitExpression.ReduceMemberInit((Expression) memberExpression, ((MemberMemberBinding) binding).Bindings, false);
        case MemberBindingType.ListBinding:
          return MemberInitExpression.ReduceListInit((Expression) memberExpression, ((MemberListBinding) binding).Initializers, false);
        default:
          throw ContractUtils.Unreachable;
      }
    }
  }
}
