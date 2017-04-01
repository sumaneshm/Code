// Type: System.Linq.Expressions.TypeBinaryExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.TypeBinaryExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class TypeBinaryExpression : Expression
  {
    private readonly Expression _expression;
    private readonly Type _typeOperand;
    private readonly ExpressionType _nodeKind;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        return typeof (bool);
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._nodeKind;
      }
    }

    [__DynamicallyInvokable]
    public Expression Expression
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._expression;
      }
    }

    [__DynamicallyInvokable]
    public Type TypeOperand
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._typeOperand;
      }
    }

    internal TypeBinaryExpression(Expression expression, Type typeOperand, ExpressionType nodeKind)
    {
      this._expression = expression;
      this._typeOperand = typeOperand;
      this._nodeKind = nodeKind;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitTypeBinary(this);
    }

    [__DynamicallyInvokable]
    public TypeBinaryExpression Update(Expression expression)
    {
      if (expression == this.Expression)
        return this;
      if (this.NodeType == ExpressionType.TypeIs)
        return Expression.TypeIs(expression, this.TypeOperand);
      else
        return Expression.TypeEqual(expression, this.TypeOperand);
    }

    internal Expression ReduceTypeEqual()
    {
      Type type = this.Expression.Type;
      if (type.IsValueType && !TypeUtils.IsNullableType(type))
        return (Expression) Expression.Block(this.Expression, (Expression) Expression.Constant((object) (bool) (type == TypeUtils.GetNonNullableType(this._typeOperand) ? 1 : 0)));
      if (this.Expression.NodeType == ExpressionType.Constant)
        return this.ReduceConstantTypeEqual();
      if (type.IsSealed && type == this._typeOperand)
      {
        if (TypeUtils.IsNullableType(type))
          return (Expression) Expression.NotEqual(this.Expression, (Expression) Expression.Constant((object) null, this.Expression.Type));
        else
          return (Expression) Expression.ReferenceNotEqual(this.Expression, (Expression) Expression.Constant((object) null, this.Expression.Type));
      }
      else
      {
        ParameterExpression parameterExpression1 = this.Expression as ParameterExpression;
        if (parameterExpression1 != null && !parameterExpression1.IsByRef)
          return this.ByValParameterTypeEqual(parameterExpression1);
        ParameterExpression parameterExpression2 = Expression.Parameter(typeof (object));
        Expression expression = this.Expression;
        if (!TypeUtils.AreReferenceAssignable(typeof (object), expression.Type))
          expression = (Expression) Expression.Convert(expression, typeof (object));
        return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
        {
          parameterExpression2
        }, new Expression[2]
        {
          (Expression) Expression.Assign((Expression) parameterExpression2, expression),
          this.ByValParameterTypeEqual(parameterExpression2)
        });
      }
    }

    private Expression ByValParameterTypeEqual(ParameterExpression value)
    {
      Expression expression = (Expression) Expression.Call((Expression) value, typeof (object).GetMethod("GetType"));
      if (this._typeOperand.IsInterface)
      {
        ParameterExpression parameterExpression = Expression.Parameter(typeof (Type));
        expression = (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
        {
          parameterExpression
        }, new Expression[2]
        {
          (Expression) Expression.Assign((Expression) parameterExpression, expression),
          (Expression) parameterExpression
        });
      }
      return (Expression) Expression.AndAlso((Expression) Expression.ReferenceNotEqual((Expression) value, (Expression) Expression.Constant((object) null)), (Expression) Expression.ReferenceEqual(expression, (Expression) Expression.Constant((object) TypeUtils.GetNonNullableType(this._typeOperand), typeof (Type))));
    }

    private Expression ReduceConstantTypeEqual()
    {
      ConstantExpression constantExpression = this.Expression as ConstantExpression;
      if (constantExpression.Value == null)
        return (Expression) Expression.Constant((object) false);
      else
        return (Expression) Expression.Constant((object) (bool) (TypeUtils.GetNonNullableType(this._typeOperand) == constantExpression.Value.GetType() ? 1 : 0));
    }
  }
}
