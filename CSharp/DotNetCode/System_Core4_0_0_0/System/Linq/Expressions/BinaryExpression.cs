// Type: System.Linq.Expressions.BinaryExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.BinaryExpressionProxy))]
  [__DynamicallyInvokable]
  public class BinaryExpression : Expression
  {
    private readonly Expression _left;
    private readonly Expression _right;

    [__DynamicallyInvokable]
    public override bool CanReduce
    {
      [__DynamicallyInvokable] get
      {
        return BinaryExpression.IsOpAssignment(this.NodeType);
      }
    }

    [__DynamicallyInvokable]
    public Expression Right
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._right;
      }
    }

    [__DynamicallyInvokable]
    public Expression Left
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._left;
      }
    }

    [__DynamicallyInvokable]
    public MethodInfo Method
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetMethod();
      }
    }

    [__DynamicallyInvokable]
    public LambdaExpression Conversion
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetConversion();
      }
    }

    [__DynamicallyInvokable]
    public bool IsLifted
    {
      [__DynamicallyInvokable] get
      {
        if (this.NodeType == ExpressionType.Coalesce || this.NodeType == ExpressionType.Assign || !TypeUtils.IsNullableType(this._left.Type))
          return false;
        MethodInfo method = this.GetMethod();
        if (!(method == (MethodInfo) null))
          return !TypeUtils.AreEquivalent(TypeUtils.GetNonRefType(TypeExtensions.GetParametersCached((MethodBase) method)[0].ParameterType), this._left.Type);
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

    internal bool IsLiftedLogical
    {
      get
      {
        Type type1 = this._left.Type;
        Type type2 = this._right.Type;
        MethodInfo method = this.GetMethod();
        switch (this.NodeType)
        {
          case ExpressionType.AndAlso:
          case ExpressionType.OrElse:
            if (TypeUtils.AreEquivalent(type2, type1) && TypeUtils.IsNullableType(type1) && method != (MethodInfo) null)
              return TypeUtils.AreEquivalent(method.ReturnType, TypeUtils.GetNonNullableType(type1));
            else
              break;
        }
        return false;
      }
    }

    internal bool IsReferenceComparison
    {
      get
      {
        Type type1 = this._left.Type;
        Type type2 = this._right.Type;
        MethodInfo method = this.GetMethod();
        switch (this.NodeType)
        {
          case ExpressionType.Equal:
          case ExpressionType.NotEqual:
            if (method == (MethodInfo) null && !type1.IsValueType)
              return !type2.IsValueType;
            else
              break;
        }
        return false;
      }
    }

    internal BinaryExpression(Expression left, Expression right)
    {
      this._left = left;
      this._right = right;
    }

    internal virtual MethodInfo GetMethod()
    {
      return (MethodInfo) null;
    }

    [__DynamicallyInvokable]
    public BinaryExpression Update(Expression left, LambdaExpression conversion, Expression right)
    {
      if (left == this.Left && right == this.Right && conversion == this.Conversion)
        return this;
      if (!this.IsReferenceComparison)
        return Expression.MakeBinary(this.NodeType, left, right, this.IsLiftedToNull, this.Method, conversion);
      if (this.NodeType == ExpressionType.Equal)
        return Expression.ReferenceEqual(left, right);
      else
        return Expression.ReferenceNotEqual(left, right);
    }

    [__DynamicallyInvokable]
    public override Expression Reduce()
    {
      if (!BinaryExpression.IsOpAssignment(this.NodeType))
        return (Expression) this;
      switch (this._left.NodeType)
      {
        case ExpressionType.MemberAccess:
          return this.ReduceMember();
        case ExpressionType.Index:
          return this.ReduceIndex();
        default:
          return this.ReduceVariable();
      }
    }

    internal virtual LambdaExpression GetConversion()
    {
      return (LambdaExpression) null;
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitBinary(this);
    }

    private static bool IsOpAssignment(ExpressionType op)
    {
      switch (op)
      {
        case ExpressionType.AddAssign:
        case ExpressionType.AndAssign:
        case ExpressionType.DivideAssign:
        case ExpressionType.ExclusiveOrAssign:
        case ExpressionType.LeftShiftAssign:
        case ExpressionType.ModuloAssign:
        case ExpressionType.MultiplyAssign:
        case ExpressionType.OrAssign:
        case ExpressionType.PowerAssign:
        case ExpressionType.RightShiftAssign:
        case ExpressionType.SubtractAssign:
        case ExpressionType.AddAssignChecked:
        case ExpressionType.MultiplyAssignChecked:
        case ExpressionType.SubtractAssignChecked:
          return true;
        default:
          return false;
      }
    }

    private static ExpressionType GetBinaryOpFromAssignmentOp(ExpressionType op)
    {
      switch (op)
      {
        case ExpressionType.AddAssign:
          return ExpressionType.Add;
        case ExpressionType.AndAssign:
          return ExpressionType.And;
        case ExpressionType.DivideAssign:
          return ExpressionType.Divide;
        case ExpressionType.ExclusiveOrAssign:
          return ExpressionType.ExclusiveOr;
        case ExpressionType.LeftShiftAssign:
          return ExpressionType.LeftShift;
        case ExpressionType.ModuloAssign:
          return ExpressionType.Modulo;
        case ExpressionType.MultiplyAssign:
          return ExpressionType.Multiply;
        case ExpressionType.OrAssign:
          return ExpressionType.Or;
        case ExpressionType.PowerAssign:
          return ExpressionType.Power;
        case ExpressionType.RightShiftAssign:
          return ExpressionType.RightShift;
        case ExpressionType.SubtractAssign:
          return ExpressionType.Subtract;
        case ExpressionType.AddAssignChecked:
          return ExpressionType.AddChecked;
        case ExpressionType.MultiplyAssignChecked:
          return ExpressionType.MultiplyChecked;
        case ExpressionType.SubtractAssignChecked:
          return ExpressionType.SubtractChecked;
        default:
          throw Error.InvalidOperation((object) "op");
      }
    }

    private Expression ReduceVariable()
    {
      Expression right = (Expression) Expression.MakeBinary(BinaryExpression.GetBinaryOpFromAssignmentOp(this.NodeType), this._left, this._right, false, this.Method);
      LambdaExpression conversion = this.GetConversion();
      if (conversion != null)
        right = (Expression) Expression.Invoke((Expression) conversion, new Expression[1]
        {
          right
        });
      return (Expression) Expression.Assign(this._left, right);
    }

    private Expression ReduceMember()
    {
      MemberExpression memberExpression = (MemberExpression) this._left;
      if (memberExpression.Expression == null)
        return this.ReduceVariable();
      ParameterExpression parameterExpression1 = Expression.Variable(memberExpression.Expression.Type, "temp1");
      Expression expression1 = (Expression) Expression.Assign((Expression) parameterExpression1, memberExpression.Expression);
      Expression right = (Expression) Expression.MakeBinary(BinaryExpression.GetBinaryOpFromAssignmentOp(this.NodeType), (Expression) Expression.MakeMemberAccess((Expression) parameterExpression1, memberExpression.Member), this._right, false, this.Method);
      LambdaExpression conversion = this.GetConversion();
      if (conversion != null)
        right = (Expression) Expression.Invoke((Expression) conversion, new Expression[1]
        {
          right
        });
      ParameterExpression parameterExpression2 = Expression.Variable(right.Type, "temp2");
      Expression expression2 = (Expression) Expression.Assign((Expression) parameterExpression2, right);
      Expression expression3 = (Expression) Expression.Assign((Expression) Expression.MakeMemberAccess((Expression) parameterExpression1, memberExpression.Member), (Expression) parameterExpression2);
      Expression expression4 = (Expression) parameterExpression2;
      return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[2]
      {
        parameterExpression1,
        parameterExpression2
      }, new Expression[4]
      {
        expression1,
        expression2,
        expression3,
        expression4
      });
    }

    private Expression ReduceIndex()
    {
      IndexExpression indexExpression1 = (IndexExpression) this._left;
      List<ParameterExpression> list1 = new List<ParameterExpression>(indexExpression1.Arguments.Count + 2);
      List<Expression> list2 = new List<Expression>(indexExpression1.Arguments.Count + 3);
      ParameterExpression parameterExpression1 = Expression.Variable(indexExpression1.Object.Type, "tempObj");
      list1.Add(parameterExpression1);
      list2.Add((Expression) Expression.Assign((Expression) parameterExpression1, indexExpression1.Object));
      List<Expression> list3 = new List<Expression>(indexExpression1.Arguments.Count);
      foreach (Expression right in indexExpression1.Arguments)
      {
        ParameterExpression parameterExpression2 = Expression.Variable(right.Type, "tempArg" + (object) list3.Count);
        list1.Add(parameterExpression2);
        list3.Add((Expression) parameterExpression2);
        list2.Add((Expression) Expression.Assign((Expression) parameterExpression2, right));
      }
      IndexExpression indexExpression2 = Expression.MakeIndex((Expression) parameterExpression1, indexExpression1.Indexer, (IEnumerable<Expression>) list3);
      Expression right1 = (Expression) Expression.MakeBinary(BinaryExpression.GetBinaryOpFromAssignmentOp(this.NodeType), (Expression) indexExpression2, this._right, false, this.Method);
      LambdaExpression conversion = this.GetConversion();
      if (conversion != null)
        right1 = (Expression) Expression.Invoke((Expression) conversion, new Expression[1]
        {
          right1
        });
      ParameterExpression parameterExpression3 = Expression.Variable(right1.Type, "tempValue");
      list1.Add(parameterExpression3);
      list2.Add((Expression) Expression.Assign((Expression) parameterExpression3, right1));
      list2.Add((Expression) Expression.Assign((Expression) indexExpression2, (Expression) parameterExpression3));
      return (Expression) Expression.Block((IEnumerable<ParameterExpression>) list1, (IEnumerable<Expression>) list2);
    }

    internal static Expression Create(ExpressionType nodeType, Expression left, Expression right, Type type, MethodInfo method, LambdaExpression conversion)
    {
      if (nodeType == ExpressionType.Assign)
        return (Expression) new AssignBinaryExpression(left, right);
      if (conversion != null)
        return (Expression) new CoalesceConversionBinaryExpression(left, right, conversion);
      if (method != (MethodInfo) null)
        return (Expression) new MethodBinaryExpression(nodeType, left, right, type, method);
      if (type == typeof (bool))
        return (Expression) new LogicalBinaryExpression(nodeType, left, right);
      else
        return (Expression) new SimpleBinaryExpression(nodeType, left, right, type);
    }

    internal Expression ReduceUserdefinedLifted()
    {
      ParameterExpression parameterExpression1 = Expression.Parameter(this._left.Type, "left");
      ParameterExpression parameterExpression2 = Expression.Parameter(this.Right.Type, "right");
      MethodInfo booleanOperator = TypeUtils.GetBooleanOperator(this.Method.DeclaringType, this.NodeType == ExpressionType.AndAlso ? "op_False" : "op_True");
      return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
      {
        parameterExpression1
      }, new Expression[2]
      {
        (Expression) Expression.Assign((Expression) parameterExpression1, this._left),
        (Expression) Expression.Condition((Expression) Expression.Property((Expression) parameterExpression1, "HasValue"), (Expression) Expression.Condition((Expression) Expression.Call(booleanOperator, (Expression) Expression.Call((Expression) parameterExpression1, "GetValueOrDefault", (Type[]) null, new Expression[0])), (Expression) parameterExpression1, (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
        {
          parameterExpression2
        }, new Expression[2]
        {
          (Expression) Expression.Assign((Expression) parameterExpression2, this._right),
          (Expression) Expression.Condition((Expression) Expression.Property((Expression) parameterExpression2, "HasValue"), (Expression) Expression.Convert((Expression) Expression.Call(this.Method, (Expression) Expression.Call((Expression) parameterExpression1, "GetValueOrDefault", (Type[]) null, new Expression[0]), (Expression) Expression.Call((Expression) parameterExpression2, "GetValueOrDefault", (Type[]) null, new Expression[0])), this.Type), (Expression) Expression.Constant((object) null, this.Type))
        })), (Expression) Expression.Constant((object) null, this.Type))
      });
    }
  }
}
