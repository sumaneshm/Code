// Type: System.Linq.Expressions.UnaryExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.UnaryExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class UnaryExpression : Expression
  {
    private readonly Expression _operand;
    private readonly MethodInfo _method;
    private readonly ExpressionType _nodeType;
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
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._nodeType;
      }
    }

    [__DynamicallyInvokable]
    public Expression Operand
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._operand;
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
    public bool IsLifted
    {
      [__DynamicallyInvokable] get
      {
        if (this.NodeType == ExpressionType.TypeAs || this.NodeType == ExpressionType.Quote || this.NodeType == ExpressionType.Throw)
          return false;
        bool flag1 = TypeUtils.IsNullableType(this._operand.Type);
        bool flag2 = TypeUtils.IsNullableType(this.Type);
        if (this._method != (MethodInfo) null)
        {
          if (flag1 && !TypeUtils.AreEquivalent(TypeExtensions.GetParametersCached((MethodBase) this._method)[0].ParameterType, this._operand.Type))
            return true;
          if (flag2)
            return !TypeUtils.AreEquivalent(this._method.ReturnType, this.Type);
          else
            return false;
        }
        else if (!flag1)
          return flag2;
        else
          return true;
      }
    }

    [__DynamicallyInvokable]
    public bool IsLiftedToNull
    {
      [__DynamicallyInvokable] get
      {
        if (this.IsLifted)
          return TypeUtils.IsNullableType(this.Type);
        else
          return false;
      }
    }

    [__DynamicallyInvokable]
    public override bool CanReduce
    {
      [__DynamicallyInvokable] get
      {
        switch (this._nodeType)
        {
          case ExpressionType.PreIncrementAssign:
          case ExpressionType.PreDecrementAssign:
          case ExpressionType.PostIncrementAssign:
          case ExpressionType.PostDecrementAssign:
            return true;
          default:
            return false;
        }
      }
    }

    private bool IsPrefix
    {
      get
      {
        if (this._nodeType != ExpressionType.PreIncrementAssign)
          return this._nodeType == ExpressionType.PreDecrementAssign;
        else
          return true;
      }
    }

    internal UnaryExpression(ExpressionType nodeType, Expression expression, Type type, MethodInfo method)
    {
      this._operand = expression;
      this._method = method;
      this._nodeType = nodeType;
      this._type = type;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitUnary(this);
    }

    [__DynamicallyInvokable]
    public override Expression Reduce()
    {
      if (!this.CanReduce)
        return (Expression) this;
      switch (this._operand.NodeType)
      {
        case ExpressionType.MemberAccess:
          return this.ReduceMember();
        case ExpressionType.Index:
          return this.ReduceIndex();
        default:
          return this.ReduceVariable();
      }
    }

    [__DynamicallyInvokable]
    public UnaryExpression Update(Expression operand)
    {
      if (operand == this.Operand)
        return this;
      else
        return Expression.MakeUnary(this.NodeType, operand, this.Type, this.Method);
    }

    private UnaryExpression FunctionalOp(Expression operand)
    {
      return new UnaryExpression(this._nodeType == ExpressionType.PreIncrementAssign || this._nodeType == ExpressionType.PostIncrementAssign ? ExpressionType.Increment : ExpressionType.Decrement, operand, operand.Type, this._method);
    }

    private Expression ReduceVariable()
    {
      if (this.IsPrefix)
        return (Expression) Expression.Assign(this._operand, (Expression) this.FunctionalOp(this._operand));
      ParameterExpression parameterExpression = Expression.Parameter(this._operand.Type, (string) null);
      return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
      {
        parameterExpression
      }, new Expression[3]
      {
        (Expression) Expression.Assign((Expression) parameterExpression, this._operand),
        (Expression) Expression.Assign(this._operand, (Expression) this.FunctionalOp((Expression) parameterExpression)),
        (Expression) parameterExpression
      });
    }

    private Expression ReduceMember()
    {
      MemberExpression memberExpression1 = (MemberExpression) this._operand;
      if (memberExpression1.Expression == null)
        return this.ReduceVariable();
      ParameterExpression parameterExpression1 = Expression.Parameter(memberExpression1.Expression.Type, (string) null);
      BinaryExpression binaryExpression = Expression.Assign((Expression) parameterExpression1, memberExpression1.Expression);
      MemberExpression memberExpression2 = Expression.MakeMemberAccess((Expression) parameterExpression1, memberExpression1.Member);
      if (this.IsPrefix)
      {
        return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
        {
          parameterExpression1
        }, new Expression[2]
        {
          (Expression) binaryExpression,
          (Expression) Expression.Assign((Expression) memberExpression2, (Expression) this.FunctionalOp((Expression) memberExpression2))
        });
      }
      else
      {
        ParameterExpression parameterExpression2 = Expression.Parameter(memberExpression2.Type, (string) null);
        return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[2]
        {
          parameterExpression1,
          parameterExpression2
        }, new Expression[4]
        {
          (Expression) binaryExpression,
          (Expression) Expression.Assign((Expression) parameterExpression2, (Expression) memberExpression2),
          (Expression) Expression.Assign((Expression) memberExpression2, (Expression) this.FunctionalOp((Expression) parameterExpression2)),
          (Expression) parameterExpression2
        });
      }
    }

    private Expression ReduceIndex()
    {
      bool isPrefix = this.IsPrefix;
      IndexExpression indexExpression1 = (IndexExpression) this._operand;
      int count = indexExpression1.Arguments.Count;
      Expression[] list1 = new Expression[count + (isPrefix ? 2 : 4)];
      ParameterExpression[] list2 = new ParameterExpression[count + (isPrefix ? 1 : 2)];
      ParameterExpression[] parameterExpressionArray = new ParameterExpression[count];
      int index1 = 0;
      list2[index1] = Expression.Parameter(indexExpression1.Object.Type, (string) null);
      list1[index1] = (Expression) Expression.Assign((Expression) list2[index1], indexExpression1.Object);
      int index2;
      for (index2 = index1 + 1; index2 <= count; ++index2)
      {
        Expression right = indexExpression1.Arguments[index2 - 1];
        parameterExpressionArray[index2 - 1] = list2[index2] = Expression.Parameter(right.Type, (string) null);
        list1[index2] = (Expression) Expression.Assign((Expression) list2[index2], right);
      }
      IndexExpression indexExpression2 = Expression.MakeIndex((Expression) list2[0], indexExpression1.Indexer, (IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>((Expression[]) parameterExpressionArray));
      int num1;
      if (!isPrefix)
      {
        ParameterExpression parameterExpression1 = list2[index2] = Expression.Parameter(indexExpression2.Type, (string) null);
        list1[index2] = (Expression) Expression.Assign((Expression) list2[index2], (Expression) indexExpression2);
        int num2 = index2 + 1;
        Expression[] expressionArray1 = list1;
        int index3 = num2;
        int num3 = 1;
        int num4 = index3 + num3;
        BinaryExpression binaryExpression = Expression.Assign((Expression) indexExpression2, (Expression) this.FunctionalOp((Expression) parameterExpression1));
        expressionArray1[index3] = (Expression) binaryExpression;
        Expression[] expressionArray2 = list1;
        int index4 = num4;
        int num5 = 1;
        num1 = index4 + num5;
        ParameterExpression parameterExpression2 = parameterExpression1;
        expressionArray2[index4] = (Expression) parameterExpression2;
      }
      else
      {
        Expression[] expressionArray = list1;
        int index3 = index2;
        int num2 = 1;
        num1 = index3 + num2;
        BinaryExpression binaryExpression = Expression.Assign((Expression) indexExpression2, (Expression) this.FunctionalOp((Expression) indexExpression2));
        expressionArray[index3] = (Expression) binaryExpression;
      }
      return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new TrueReadOnlyCollection<ParameterExpression>(list2), (IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>(list1));
    }
  }
}
