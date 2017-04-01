// Type: System.Linq.Expressions.Expression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions.Compiler;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace System.Linq.Expressions
{
  [__DynamicallyInvokable]
  public abstract class Expression
  {
    private static readonly CacheDict<Type, MethodInfo> _LambdaDelegateCache = new CacheDict<Type, MethodInfo>(40);
    private static volatile CacheDict<Type, Expression.LambdaFactory> _LambdaFactories;
    private static ConditionalWeakTable<Expression, Expression.ExtensionInfo> _legacyCtorSupportTable;

    [__DynamicallyInvokable]
    public virtual ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        Expression.ExtensionInfo extensionInfo;
        if (Expression._legacyCtorSupportTable != null && Expression._legacyCtorSupportTable.TryGetValue(this, out extensionInfo))
          return extensionInfo.NodeType;
        else
          throw Error.ExtensionNodeMustOverrideProperty((object) "Expression.NodeType");
      }
    }

    [__DynamicallyInvokable]
    public virtual Type Type
    {
      [__DynamicallyInvokable] get
      {
        Expression.ExtensionInfo extensionInfo;
        if (Expression._legacyCtorSupportTable != null && Expression._legacyCtorSupportTable.TryGetValue(this, out extensionInfo))
          return extensionInfo.Type;
        else
          throw Error.ExtensionNodeMustOverrideProperty((object) "Expression.Type");
      }
    }

    [__DynamicallyInvokable]
    public virtual bool CanReduce
    {
      [__DynamicallyInvokable] get
      {
        return false;
      }
    }

    private string DebugView
    {
      get
      {
        using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.CurrentCulture))
        {
          DebugViewWriter.WriteTo(this, (TextWriter) stringWriter);
          return stringWriter.ToString();
        }
      }
    }

    static Expression()
    {
    }

    [Obsolete("use a different constructor that does not take ExpressionType. Then override NodeType and Type properties to provide the values that would be specified to this constructor.")]
    protected Expression(ExpressionType nodeType, Type type)
    {
      if (Expression._legacyCtorSupportTable == null)
        Interlocked.CompareExchange<ConditionalWeakTable<Expression, Expression.ExtensionInfo>>(ref Expression._legacyCtorSupportTable, new ConditionalWeakTable<Expression, Expression.ExtensionInfo>(), (ConditionalWeakTable<Expression, Expression.ExtensionInfo>) null);
      Expression._legacyCtorSupportTable.Add(this, new Expression.ExtensionInfo(nodeType, type));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected Expression()
    {
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Assign(Expression left, Expression right)
    {
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      TypeUtils.ValidateType(left.Type);
      TypeUtils.ValidateType(right.Type);
      if (!TypeUtils.AreReferenceAssignable(left.Type, right.Type))
        throw Error.ExpressionTypeDoesNotMatchAssignment((object) right.Type, (object) left.Type);
      else
        return (BinaryExpression) new AssignBinaryExpression(left, right);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right)
    {
      return Expression.MakeBinary(binaryType, left, right, false, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo method)
    {
      return Expression.MakeBinary(binaryType, left, right, liftToNull, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo method, LambdaExpression conversion)
    {
      switch (binaryType)
      {
        case ExpressionType.Add:
          return Expression.Add(left, right, method);
        case ExpressionType.AddChecked:
          return Expression.AddChecked(left, right, method);
        case ExpressionType.And:
          return Expression.And(left, right, method);
        case ExpressionType.AndAlso:
          return Expression.AndAlso(left, right, method);
        case ExpressionType.ArrayIndex:
          return Expression.ArrayIndex(left, right);
        case ExpressionType.Coalesce:
          return Expression.Coalesce(left, right, conversion);
        case ExpressionType.Divide:
          return Expression.Divide(left, right, method);
        case ExpressionType.Equal:
          return Expression.Equal(left, right, liftToNull, method);
        case ExpressionType.ExclusiveOr:
          return Expression.ExclusiveOr(left, right, method);
        case ExpressionType.GreaterThan:
          return Expression.GreaterThan(left, right, liftToNull, method);
        case ExpressionType.GreaterThanOrEqual:
          return Expression.GreaterThanOrEqual(left, right, liftToNull, method);
        case ExpressionType.LeftShift:
          return Expression.LeftShift(left, right, method);
        case ExpressionType.LessThan:
          return Expression.LessThan(left, right, liftToNull, method);
        case ExpressionType.LessThanOrEqual:
          return Expression.LessThanOrEqual(left, right, liftToNull, method);
        case ExpressionType.Modulo:
          return Expression.Modulo(left, right, method);
        case ExpressionType.Multiply:
          return Expression.Multiply(left, right, method);
        case ExpressionType.MultiplyChecked:
          return Expression.MultiplyChecked(left, right, method);
        case ExpressionType.NotEqual:
          return Expression.NotEqual(left, right, liftToNull, method);
        case ExpressionType.Or:
          return Expression.Or(left, right, method);
        case ExpressionType.OrElse:
          return Expression.OrElse(left, right, method);
        case ExpressionType.Power:
          return Expression.Power(left, right, method);
        case ExpressionType.RightShift:
          return Expression.RightShift(left, right, method);
        case ExpressionType.Subtract:
          return Expression.Subtract(left, right, method);
        case ExpressionType.SubtractChecked:
          return Expression.SubtractChecked(left, right, method);
        case ExpressionType.Assign:
          return Expression.Assign(left, right);
        case ExpressionType.AddAssign:
          return Expression.AddAssign(left, right, method, conversion);
        case ExpressionType.AndAssign:
          return Expression.AndAssign(left, right, method, conversion);
        case ExpressionType.DivideAssign:
          return Expression.DivideAssign(left, right, method, conversion);
        case ExpressionType.ExclusiveOrAssign:
          return Expression.ExclusiveOrAssign(left, right, method, conversion);
        case ExpressionType.LeftShiftAssign:
          return Expression.LeftShiftAssign(left, right, method, conversion);
        case ExpressionType.ModuloAssign:
          return Expression.ModuloAssign(left, right, method, conversion);
        case ExpressionType.MultiplyAssign:
          return Expression.MultiplyAssign(left, right, method, conversion);
        case ExpressionType.OrAssign:
          return Expression.OrAssign(left, right, method, conversion);
        case ExpressionType.PowerAssign:
          return Expression.PowerAssign(left, right, method, conversion);
        case ExpressionType.RightShiftAssign:
          return Expression.RightShiftAssign(left, right, method, conversion);
        case ExpressionType.SubtractAssign:
          return Expression.SubtractAssign(left, right, method, conversion);
        case ExpressionType.AddAssignChecked:
          return Expression.AddAssignChecked(left, right, method, conversion);
        case ExpressionType.MultiplyAssignChecked:
          return Expression.MultiplyAssignChecked(left, right, method, conversion);
        case ExpressionType.SubtractAssignChecked:
          return Expression.SubtractAssignChecked(left, right, method, conversion);
        default:
          throw Error.UnhandledBinary((object) binaryType);
      }
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Equal(Expression left, Expression right)
    {
      return Expression.Equal(left, right, false, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Equal(Expression left, Expression right, bool liftToNull, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
        return Expression.GetEqualityComparisonOperator(ExpressionType.Equal, "op_Equality", left, right, liftToNull);
      else
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.Equal, left, right, method, liftToNull);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression ReferenceEqual(Expression left, Expression right)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (TypeUtils.HasReferenceEquality(left.Type, right.Type))
        return (BinaryExpression) new LogicalBinaryExpression(ExpressionType.Equal, left, right);
      else
        throw Error.ReferenceEqualityNotDefined((object) left.Type, (object) right.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression NotEqual(Expression left, Expression right)
    {
      return Expression.NotEqual(left, right, false, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression NotEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
        return Expression.GetEqualityComparisonOperator(ExpressionType.NotEqual, "op_Inequality", left, right, liftToNull);
      else
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.NotEqual, left, right, method, liftToNull);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression ReferenceNotEqual(Expression left, Expression right)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (TypeUtils.HasReferenceEquality(left.Type, right.Type))
        return (BinaryExpression) new LogicalBinaryExpression(ExpressionType.NotEqual, left, right);
      else
        throw Error.ReferenceEqualityNotDefined((object) left.Type, (object) right.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression GreaterThan(Expression left, Expression right)
    {
      return Expression.GreaterThan(left, right, false, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression GreaterThan(Expression left, Expression right, bool liftToNull, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
        return Expression.GetComparisonOperator(ExpressionType.GreaterThan, "op_GreaterThan", left, right, liftToNull);
      else
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.GreaterThan, left, right, method, liftToNull);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression LessThan(Expression left, Expression right)
    {
      return Expression.LessThan(left, right, false, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression LessThan(Expression left, Expression right, bool liftToNull, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
        return Expression.GetComparisonOperator(ExpressionType.LessThan, "op_LessThan", left, right, liftToNull);
      else
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.LessThan, left, right, method, liftToNull);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right)
    {
      return Expression.GreaterThanOrEqual(left, right, false, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
        return Expression.GetComparisonOperator(ExpressionType.GreaterThanOrEqual, "op_GreaterThanOrEqual", left, right, liftToNull);
      else
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.GreaterThanOrEqual, left, right, method, liftToNull);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression LessThanOrEqual(Expression left, Expression right)
    {
      return Expression.LessThanOrEqual(left, right, false, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression LessThanOrEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
        return Expression.GetComparisonOperator(ExpressionType.LessThanOrEqual, "op_LessThanOrEqual", left, right, liftToNull);
      else
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.LessThanOrEqual, left, right, method, liftToNull);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression AndAlso(Expression left, Expression right)
    {
      return Expression.AndAlso(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression AndAlso(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
      {
        if (left.Type == right.Type)
        {
          if (left.Type == typeof (bool))
            return (BinaryExpression) new LogicalBinaryExpression(ExpressionType.AndAlso, left, right);
          if (left.Type == typeof (bool?))
            return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.AndAlso, left, right, left.Type);
        }
        method = Expression.GetUserDefinedBinaryOperator(ExpressionType.AndAlso, left.Type, right.Type, "op_BitwiseAnd");
        if (!(method != (MethodInfo) null))
          throw Error.BinaryOperatorNotDefined((object) ExpressionType.AndAlso, (object) left.Type, (object) right.Type);
        Expression.ValidateUserDefinedConditionalLogicOperator(ExpressionType.AndAlso, left.Type, right.Type, method);
        Type type = !TypeUtils.IsNullableType(left.Type) || !TypeUtils.AreEquivalent(method.ReturnType, TypeUtils.GetNonNullableType(left.Type)) ? method.ReturnType : left.Type;
        return (BinaryExpression) new MethodBinaryExpression(ExpressionType.AndAlso, left, right, type, method);
      }
      else
      {
        Expression.ValidateUserDefinedConditionalLogicOperator(ExpressionType.AndAlso, left.Type, right.Type, method);
        Type type = !TypeUtils.IsNullableType(left.Type) || !TypeUtils.AreEquivalent(method.ReturnType, TypeUtils.GetNonNullableType(left.Type)) ? method.ReturnType : left.Type;
        return (BinaryExpression) new MethodBinaryExpression(ExpressionType.AndAlso, left, right, type, method);
      }
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression OrElse(Expression left, Expression right)
    {
      return Expression.OrElse(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression OrElse(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
      {
        if (left.Type == right.Type)
        {
          if (left.Type == typeof (bool))
            return (BinaryExpression) new LogicalBinaryExpression(ExpressionType.OrElse, left, right);
          if (left.Type == typeof (bool?))
            return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.OrElse, left, right, left.Type);
        }
        method = Expression.GetUserDefinedBinaryOperator(ExpressionType.OrElse, left.Type, right.Type, "op_BitwiseOr");
        if (!(method != (MethodInfo) null))
          throw Error.BinaryOperatorNotDefined((object) ExpressionType.OrElse, (object) left.Type, (object) right.Type);
        Expression.ValidateUserDefinedConditionalLogicOperator(ExpressionType.OrElse, left.Type, right.Type, method);
        Type type = !TypeUtils.IsNullableType(left.Type) || !(method.ReturnType == TypeUtils.GetNonNullableType(left.Type)) ? method.ReturnType : left.Type;
        return (BinaryExpression) new MethodBinaryExpression(ExpressionType.OrElse, left, right, type, method);
      }
      else
      {
        Expression.ValidateUserDefinedConditionalLogicOperator(ExpressionType.OrElse, left.Type, right.Type, method);
        Type type = !TypeUtils.IsNullableType(left.Type) || !(method.ReturnType == TypeUtils.GetNonNullableType(left.Type)) ? method.ReturnType : left.Type;
        return (BinaryExpression) new MethodBinaryExpression(ExpressionType.OrElse, left, right, type, method);
      }
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Coalesce(Expression left, Expression right)
    {
      return Expression.Coalesce(left, right, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Coalesce(Expression left, Expression right, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (conversion == null)
      {
        Type type = Expression.ValidateCoalesceArgTypes(left.Type, right.Type);
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.Coalesce, left, right, type);
      }
      else
      {
        if (left.Type.IsValueType && !TypeUtils.IsNullableType(left.Type))
          throw Error.CoalesceUsedOnNonNullType();
        MethodInfo method = conversion.Type.GetMethod("Invoke");
        if (method.ReturnType == typeof (void))
          throw Error.UserDefinedOperatorMustNotBeVoid((object) conversion);
        ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method);
        if (parametersCached.Length != 1)
          throw Error.IncorrectNumberOfMethodCallArguments((object) conversion);
        if (!TypeUtils.AreEquivalent(method.ReturnType, right.Type))
          throw Error.OperandTypesDoNotMatchParameters((object) ExpressionType.Coalesce, (object) conversion.ToString());
        if (!Expression.ParameterIsAssignable(parametersCached[0], TypeUtils.GetNonNullableType(left.Type)) && !Expression.ParameterIsAssignable(parametersCached[0], left.Type))
          throw Error.OperandTypesDoNotMatchParameters((object) ExpressionType.Coalesce, (object) conversion.ToString());
        else
          return (BinaryExpression) new CoalesceConversionBinaryExpression(left, right, conversion);
      }
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Add(Expression left, Expression right)
    {
      return Expression.Add(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Add(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.Add, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsArithmetic(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.Add, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Add, "op_Addition", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression AddAssign(Expression left, Expression right)
    {
      return Expression.AddAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression AddAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.AddAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression AddAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.AddAssign, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsArithmetic(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.AddAssign, "op_Addition", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.AddAssign, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression AddAssignChecked(Expression left, Expression right)
    {
      return Expression.AddAssignChecked(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression AddAssignChecked(Expression left, Expression right, MethodInfo method)
    {
      return Expression.AddAssignChecked(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression AddAssignChecked(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.AddAssignChecked, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsArithmetic(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.AddAssignChecked, "op_Addition", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.AddAssignChecked, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression AddChecked(Expression left, Expression right)
    {
      return Expression.AddChecked(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression AddChecked(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.AddChecked, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsArithmetic(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.AddChecked, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.AddChecked, "op_Addition", left, right, false);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Subtract(Expression left, Expression right)
    {
      return Expression.Subtract(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Subtract(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.Subtract, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsArithmetic(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.Subtract, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Subtract, "op_Subtraction", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression SubtractAssign(Expression left, Expression right)
    {
      return Expression.SubtractAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression SubtractAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.SubtractAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression SubtractAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.SubtractAssign, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsArithmetic(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.SubtractAssign, "op_Subtraction", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.SubtractAssign, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression SubtractAssignChecked(Expression left, Expression right)
    {
      return Expression.SubtractAssignChecked(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression SubtractAssignChecked(Expression left, Expression right, MethodInfo method)
    {
      return Expression.SubtractAssignChecked(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression SubtractAssignChecked(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.SubtractAssignChecked, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsArithmetic(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.SubtractAssignChecked, "op_Subtraction", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.SubtractAssignChecked, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression SubtractChecked(Expression left, Expression right)
    {
      return Expression.SubtractChecked(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression SubtractChecked(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.SubtractChecked, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsArithmetic(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.SubtractChecked, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.SubtractChecked, "op_Subtraction", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Divide(Expression left, Expression right)
    {
      return Expression.Divide(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Divide(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.Divide, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsArithmetic(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.Divide, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Divide, "op_Division", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression DivideAssign(Expression left, Expression right)
    {
      return Expression.DivideAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression DivideAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.DivideAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression DivideAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.DivideAssign, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsArithmetic(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.DivideAssign, "op_Division", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.DivideAssign, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Modulo(Expression left, Expression right)
    {
      return Expression.Modulo(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Modulo(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.Modulo, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsArithmetic(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.Modulo, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Modulo, "op_Modulus", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression ModuloAssign(Expression left, Expression right)
    {
      return Expression.ModuloAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression ModuloAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.ModuloAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression ModuloAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.ModuloAssign, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsArithmetic(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.ModuloAssign, "op_Modulus", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.ModuloAssign, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Multiply(Expression left, Expression right)
    {
      return Expression.Multiply(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Multiply(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.Multiply, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsArithmetic(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.Multiply, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Multiply, "op_Multiply", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression MultiplyAssign(Expression left, Expression right)
    {
      return Expression.MultiplyAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression MultiplyAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.MultiplyAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression MultiplyAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.MultiplyAssign, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsArithmetic(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.MultiplyAssign, "op_Multiply", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.MultiplyAssign, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right)
    {
      return Expression.MultiplyAssignChecked(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right, MethodInfo method)
    {
      return Expression.MultiplyAssignChecked(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression MultiplyAssignChecked(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.MultiplyAssignChecked, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsArithmetic(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.MultiplyAssignChecked, "op_Multiply", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.MultiplyAssignChecked, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression MultiplyChecked(Expression left, Expression right)
    {
      return Expression.MultiplyChecked(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression MultiplyChecked(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.MultiplyChecked, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsArithmetic(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.MultiplyChecked, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.MultiplyChecked, "op_Multiply", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression LeftShift(Expression left, Expression right)
    {
      return Expression.LeftShift(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression LeftShift(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.LeftShift, left, right, method, true);
      if (!Expression.IsSimpleShift(left.Type, right.Type))
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.LeftShift, "op_LeftShift", left, right, true);
      Type resultTypeOfShift = Expression.GetResultTypeOfShift(left.Type, right.Type);
      return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.LeftShift, left, right, resultTypeOfShift);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression LeftShiftAssign(Expression left, Expression right)
    {
      return Expression.LeftShiftAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression LeftShiftAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.LeftShiftAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression LeftShiftAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.LeftShiftAssign, left, right, method, conversion, true);
      if (!Expression.IsSimpleShift(left.Type, right.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.LeftShiftAssign, "op_LeftShift", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      Type resultTypeOfShift = Expression.GetResultTypeOfShift(left.Type, right.Type);
      return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.LeftShiftAssign, left, right, resultTypeOfShift);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression RightShift(Expression left, Expression right)
    {
      return Expression.RightShift(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression RightShift(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.RightShift, left, right, method, true);
      if (!Expression.IsSimpleShift(left.Type, right.Type))
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.RightShift, "op_RightShift", left, right, true);
      Type resultTypeOfShift = Expression.GetResultTypeOfShift(left.Type, right.Type);
      return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.RightShift, left, right, resultTypeOfShift);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression RightShiftAssign(Expression left, Expression right)
    {
      return Expression.RightShiftAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression RightShiftAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.RightShiftAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression RightShiftAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.RightShiftAssign, left, right, method, conversion, true);
      if (!Expression.IsSimpleShift(left.Type, right.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.RightShiftAssign, "op_RightShift", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      Type resultTypeOfShift = Expression.GetResultTypeOfShift(left.Type, right.Type);
      return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.RightShiftAssign, left, right, resultTypeOfShift);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression And(Expression left, Expression right)
    {
      return Expression.And(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression And(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.And, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsIntegerOrBool(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.And, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.And, "op_BitwiseAnd", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression AndAssign(Expression left, Expression right)
    {
      return Expression.AndAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression AndAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.AndAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression AndAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.AndAssign, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsIntegerOrBool(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.AndAssign, "op_BitwiseAnd", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.AndAssign, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Or(Expression left, Expression right)
    {
      return Expression.Or(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Or(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.Or, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsIntegerOrBool(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.Or, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.Or, "op_BitwiseOr", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression OrAssign(Expression left, Expression right)
    {
      return Expression.OrAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression OrAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.OrAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression OrAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.OrAssign, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsIntegerOrBool(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.OrAssign, "op_BitwiseOr", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.OrAssign, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression ExclusiveOr(Expression left, Expression right)
    {
      return Expression.ExclusiveOr(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression ExclusiveOr(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedBinaryOperator(ExpressionType.ExclusiveOr, left, right, method, true);
      if (left.Type == right.Type && TypeUtils.IsIntegerOrBool(left.Type))
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.ExclusiveOr, left, right, left.Type);
      else
        return Expression.GetUserDefinedBinaryOperatorOrThrow(ExpressionType.ExclusiveOr, "op_ExclusiveOr", left, right, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right)
    {
      return Expression.ExclusiveOrAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.ExclusiveOrAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression ExclusiveOrAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedAssignOperator(ExpressionType.ExclusiveOrAssign, left, right, method, conversion, true);
      if (!(left.Type == right.Type) || !TypeUtils.IsIntegerOrBool(left.Type))
        return Expression.GetUserDefinedAssignOperatorOrThrow(ExpressionType.ExclusiveOrAssign, "op_ExclusiveOr", left, right, conversion, true);
      if (conversion != null)
        throw Error.ConversionIsNotSupportedForArithmeticTypes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.ExclusiveOrAssign, left, right, left.Type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression Power(Expression left, Expression right)
    {
      return Expression.Power(left, right, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression Power(Expression left, Expression right, MethodInfo method)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
      {
        method = typeof (Math).GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);
        if (method == (MethodInfo) null)
          throw Error.BinaryOperatorNotDefined((object) ExpressionType.Power, (object) left.Type, (object) right.Type);
      }
      return Expression.GetMethodBasedBinaryOperator(ExpressionType.Power, left, right, method, true);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression PowerAssign(Expression left, Expression right)
    {
      return Expression.PowerAssign(left, right, (MethodInfo) null, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static BinaryExpression PowerAssign(Expression left, Expression right, MethodInfo method)
    {
      return Expression.PowerAssign(left, right, method, (LambdaExpression) null);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression PowerAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion)
    {
      Expression.RequiresCanRead(left, "left");
      Expression.RequiresCanWrite(left, "left");
      Expression.RequiresCanRead(right, "right");
      if (method == (MethodInfo) null)
      {
        method = typeof (Math).GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);
        if (method == (MethodInfo) null)
          throw Error.BinaryOperatorNotDefined((object) ExpressionType.PowerAssign, (object) left.Type, (object) right.Type);
      }
      return Expression.GetMethodBasedAssignOperator(ExpressionType.PowerAssign, left, right, method, conversion, true);
    }

    [__DynamicallyInvokable]
    public static BinaryExpression ArrayIndex(Expression array, Expression index)
    {
      Expression.RequiresCanRead(array, "array");
      Expression.RequiresCanRead(index, "index");
      if (index.Type != typeof (int))
        throw Error.ArgumentMustBeArrayIndexType();
      Type type = array.Type;
      if (!type.IsArray)
        throw Error.ArgumentMustBeArray();
      if (type.GetArrayRank() != 1)
        throw Error.IncorrectNumberOfIndexes();
      else
        return (BinaryExpression) new SimpleBinaryExpression(ExpressionType.ArrayIndex, array, index, type.GetElementType());
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(Expression arg0, Expression arg1)
    {
      Expression.RequiresCanRead(arg0, "arg0");
      Expression.RequiresCanRead(arg1, "arg1");
      return (BlockExpression) new Block2(arg0, arg1);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2)
    {
      Expression.RequiresCanRead(arg0, "arg0");
      Expression.RequiresCanRead(arg1, "arg1");
      Expression.RequiresCanRead(arg2, "arg2");
      return (BlockExpression) new Block3(arg0, arg1, arg2);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2, Expression arg3)
    {
      Expression.RequiresCanRead(arg0, "arg0");
      Expression.RequiresCanRead(arg1, "arg1");
      Expression.RequiresCanRead(arg2, "arg2");
      Expression.RequiresCanRead(arg3, "arg3");
      return (BlockExpression) new Block4(arg0, arg1, arg2, arg3);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4)
    {
      Expression.RequiresCanRead(arg0, "arg0");
      Expression.RequiresCanRead(arg1, "arg1");
      Expression.RequiresCanRead(arg2, "arg2");
      Expression.RequiresCanRead(arg3, "arg3");
      Expression.RequiresCanRead(arg4, "arg4");
      return (BlockExpression) new Block5(arg0, arg1, arg2, arg3, arg4);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(params Expression[] expressions)
    {
      ContractUtils.RequiresNotNull((object) expressions, "expressions");
      switch (expressions.Length)
      {
        case 2:
          return Expression.Block(expressions[0], expressions[1]);
        case 3:
          return Expression.Block(expressions[0], expressions[1], expressions[2]);
        case 4:
          return Expression.Block(expressions[0], expressions[1], expressions[2], expressions[3]);
        case 5:
          return Expression.Block(expressions[0], expressions[1], expressions[2], expressions[3], expressions[4]);
        default:
          ContractUtils.RequiresNotEmpty<Expression>((ICollection<Expression>) expressions, "expressions");
          Expression.RequiresCanRead((IEnumerable<Expression>) expressions, "expressions");
          return (BlockExpression) new BlockN((IList<Expression>) CollectionExtensions.Copy<Expression>(expressions));
      }
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(IEnumerable<Expression> expressions)
    {
      return Expression.Block((IEnumerable<ParameterExpression>) EmptyReadOnlyCollection<ParameterExpression>.Instance, expressions);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(Type type, params Expression[] expressions)
    {
      ContractUtils.RequiresNotNull((object) expressions, "expressions");
      return Expression.Block(type, (IEnumerable<Expression>) expressions);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(Type type, IEnumerable<Expression> expressions)
    {
      return Expression.Block(type, (IEnumerable<ParameterExpression>) EmptyReadOnlyCollection<ParameterExpression>.Instance, expressions);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(IEnumerable<ParameterExpression> variables, params Expression[] expressions)
    {
      return Expression.Block(variables, (IEnumerable<Expression>) expressions);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(Type type, IEnumerable<ParameterExpression> variables, params Expression[] expressions)
    {
      return Expression.Block(type, variables, (IEnumerable<Expression>) expressions);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions)
    {
      ContractUtils.RequiresNotNull((object) expressions, "expressions");
      ReadOnlyCollection<Expression> readOnlyCollection = CollectionExtensions.ToReadOnly<Expression>(expressions);
      ContractUtils.RequiresNotEmpty<Expression>((ICollection<Expression>) readOnlyCollection, "expressions");
      Expression.RequiresCanRead((IEnumerable<Expression>) readOnlyCollection, "expressions");
      return Expression.Block(Enumerable.Last<Expression>((IEnumerable<Expression>) readOnlyCollection).Type, variables, (IEnumerable<Expression>) readOnlyCollection);
    }

    [__DynamicallyInvokable]
    public static BlockExpression Block(Type type, IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      ContractUtils.RequiresNotNull((object) expressions, "expressions");
      ReadOnlyCollection<Expression> readOnlyCollection = CollectionExtensions.ToReadOnly<Expression>(expressions);
      ReadOnlyCollection<ParameterExpression> varList = CollectionExtensions.ToReadOnly<ParameterExpression>(variables);
      ContractUtils.RequiresNotEmpty<Expression>((ICollection<Expression>) readOnlyCollection, "expressions");
      Expression.RequiresCanRead((IEnumerable<Expression>) readOnlyCollection, "expressions");
      Expression.ValidateVariables(varList, "variables");
      Expression expression = Enumerable.Last<Expression>((IEnumerable<Expression>) readOnlyCollection);
      if (type != typeof (void) && !TypeUtils.AreReferenceAssignable(type, expression.Type))
        throw Error.ArgumentTypesMustMatch();
      if (!TypeUtils.AreEquivalent(type, expression.Type))
        return (BlockExpression) new ScopeWithType((IList<ParameterExpression>) varList, (IList<Expression>) readOnlyCollection, type);
      if (readOnlyCollection.Count == 1)
        return (BlockExpression) new Scope1((IList<ParameterExpression>) varList, readOnlyCollection[0]);
      else
        return (BlockExpression) new ScopeN((IList<ParameterExpression>) varList, (IList<Expression>) readOnlyCollection);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static CatchBlock Catch(Type type, Expression body)
    {
      return Expression.MakeCatchBlock(type, (ParameterExpression) null, body, (Expression) null);
    }

    [__DynamicallyInvokable]
    public static CatchBlock Catch(ParameterExpression variable, Expression body)
    {
      ContractUtils.RequiresNotNull((object) variable, "variable");
      return Expression.MakeCatchBlock(variable.Type, variable, body, (Expression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static CatchBlock Catch(Type type, Expression body, Expression filter)
    {
      return Expression.MakeCatchBlock(type, (ParameterExpression) null, body, filter);
    }

    [__DynamicallyInvokable]
    public static CatchBlock Catch(ParameterExpression variable, Expression body, Expression filter)
    {
      ContractUtils.RequiresNotNull((object) variable, "variable");
      return Expression.MakeCatchBlock(variable.Type, variable, body, filter);
    }

    [__DynamicallyInvokable]
    public static CatchBlock MakeCatchBlock(Type type, ParameterExpression variable, Expression body, Expression filter)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      ContractUtils.Requires(variable == null || TypeUtils.AreEquivalent(variable.Type, type), "variable");
      if (variable != null && variable.IsByRef)
        throw Error.VariableMustNotBeByRef((object) variable, (object) variable.Type);
      Expression.RequiresCanRead(body, "body");
      if (filter != null)
      {
        Expression.RequiresCanRead(filter, "filter");
        if (filter.Type != typeof (bool))
          throw Error.ArgumentMustBeBoolean();
      }
      return new CatchBlock(type, variable, body, filter);
    }

    [__DynamicallyInvokable]
    public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse)
    {
      Expression.RequiresCanRead(test, "test");
      Expression.RequiresCanRead(ifTrue, "ifTrue");
      Expression.RequiresCanRead(ifFalse, "ifFalse");
      if (test.Type != typeof (bool))
        throw Error.ArgumentMustBeBoolean();
      if (!TypeUtils.AreEquivalent(ifTrue.Type, ifFalse.Type))
        throw Error.ArgumentTypesMustMatch();
      else
        return ConditionalExpression.Make(test, ifTrue, ifFalse, ifTrue.Type);
    }

    [__DynamicallyInvokable]
    public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse, Type type)
    {
      Expression.RequiresCanRead(test, "test");
      Expression.RequiresCanRead(ifTrue, "ifTrue");
      Expression.RequiresCanRead(ifFalse, "ifFalse");
      ContractUtils.RequiresNotNull((object) type, "type");
      if (test.Type != typeof (bool))
        throw Error.ArgumentMustBeBoolean();
      if (type != typeof (void) && (!TypeUtils.AreReferenceAssignable(type, ifTrue.Type) || !TypeUtils.AreReferenceAssignable(type, ifFalse.Type)))
        throw Error.ArgumentTypesMustMatch();
      else
        return ConditionalExpression.Make(test, ifTrue, ifFalse, type);
    }

    [__DynamicallyInvokable]
    public static ConditionalExpression IfThen(Expression test, Expression ifTrue)
    {
      return Expression.Condition(test, ifTrue, (Expression) Expression.Empty(), typeof (void));
    }

    [__DynamicallyInvokable]
    public static ConditionalExpression IfThenElse(Expression test, Expression ifTrue, Expression ifFalse)
    {
      return Expression.Condition(test, ifTrue, ifFalse, typeof (void));
    }

    [__DynamicallyInvokable]
    public static ConstantExpression Constant(object value)
    {
      return ConstantExpression.Make(value, value == null ? typeof (object) : value.GetType());
    }

    [__DynamicallyInvokable]
    public static ConstantExpression Constant(object value, Type type)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      if (value == null && type.IsValueType && !TypeUtils.IsNullableType(type))
        throw Error.ArgumentTypesMustMatch();
      if (value != null && !type.IsAssignableFrom(value.GetType()))
        throw Error.ArgumentTypesMustMatch();
      else
        return ConstantExpression.Make(value, type);
    }

    [__DynamicallyInvokable]
    public static DebugInfoExpression DebugInfo(SymbolDocumentInfo document, int startLine, int startColumn, int endLine, int endColumn)
    {
      ContractUtils.RequiresNotNull((object) document, "document");
      if (startLine == 16707566 && startColumn == 0 && (endLine == 16707566 && endColumn == 0))
        return (DebugInfoExpression) new ClearDebugInfoExpression(document);
      Expression.ValidateSpan(startLine, startColumn, endLine, endColumn);
      return (DebugInfoExpression) new SpanDebugInfoExpression(document, startLine, startColumn, endLine, endColumn);
    }

    [__DynamicallyInvokable]
    public static DebugInfoExpression ClearDebugInfo(SymbolDocumentInfo document)
    {
      ContractUtils.RequiresNotNull((object) document, "document");
      return (DebugInfoExpression) new ClearDebugInfoExpression(document);
    }

    [__DynamicallyInvokable]
    public static DefaultExpression Empty()
    {
      return new DefaultExpression(typeof (void));
    }

    [__DynamicallyInvokable]
    public static DefaultExpression Default(Type type)
    {
      if (type == typeof (void))
        return Expression.Empty();
      else
        return new DefaultExpression(type);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, params Expression[] arguments)
    {
      return Expression.MakeDynamic(delegateType, binder, (IEnumerable<Expression>) arguments);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, IEnumerable<Expression> arguments)
    {
      ContractUtils.RequiresNotNull((object) delegateType, "delegateType");
      ContractUtils.RequiresNotNull((object) binder, "binder");
      if (!delegateType.IsSubclassOf(typeof (MulticastDelegate)))
        throw Error.TypeMustBeDerivedFromSystemDelegate();
      MethodInfo methodForDynamic = Expression.GetValidMethodForDynamic(delegateType);
      ReadOnlyCollection<Expression> arguments1 = CollectionExtensions.ToReadOnly<Expression>(arguments);
      Expression.ValidateArgumentTypes((MethodBase) methodForDynamic, ExpressionType.Dynamic, ref arguments1);
      return DynamicExpression.Make(TypeExtensions.GetReturnType((MethodBase) methodForDynamic), delegateType, binder, arguments1);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0)
    {
      ContractUtils.RequiresNotNull((object) delegateType, "delegateType");
      ContractUtils.RequiresNotNull((object) binder, "binder");
      if (!delegateType.IsSubclassOf(typeof (MulticastDelegate)))
        throw Error.TypeMustBeDerivedFromSystemDelegate();
      MethodInfo methodForDynamic = Expression.GetValidMethodForDynamic(delegateType);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) methodForDynamic);
      Expression.ValidateArgumentCount((MethodBase) methodForDynamic, ExpressionType.Dynamic, 2, parametersCached);
      Expression.ValidateDynamicArgument(arg0);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg0, parametersCached[1]);
      return DynamicExpression.Make(TypeExtensions.GetReturnType((MethodBase) methodForDynamic), delegateType, binder, arg0);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1)
    {
      ContractUtils.RequiresNotNull((object) delegateType, "delegateType");
      ContractUtils.RequiresNotNull((object) binder, "binder");
      if (!delegateType.IsSubclassOf(typeof (MulticastDelegate)))
        throw Error.TypeMustBeDerivedFromSystemDelegate();
      MethodInfo methodForDynamic = Expression.GetValidMethodForDynamic(delegateType);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) methodForDynamic);
      Expression.ValidateArgumentCount((MethodBase) methodForDynamic, ExpressionType.Dynamic, 3, parametersCached);
      Expression.ValidateDynamicArgument(arg0);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg0, parametersCached[1]);
      Expression.ValidateDynamicArgument(arg1);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg1, parametersCached[2]);
      return DynamicExpression.Make(TypeExtensions.GetReturnType((MethodBase) methodForDynamic), delegateType, binder, arg0, arg1);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2)
    {
      ContractUtils.RequiresNotNull((object) delegateType, "delegateType");
      ContractUtils.RequiresNotNull((object) binder, "binder");
      if (!delegateType.IsSubclassOf(typeof (MulticastDelegate)))
        throw Error.TypeMustBeDerivedFromSystemDelegate();
      MethodInfo methodForDynamic = Expression.GetValidMethodForDynamic(delegateType);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) methodForDynamic);
      Expression.ValidateArgumentCount((MethodBase) methodForDynamic, ExpressionType.Dynamic, 4, parametersCached);
      Expression.ValidateDynamicArgument(arg0);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg0, parametersCached[1]);
      Expression.ValidateDynamicArgument(arg1);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg1, parametersCached[2]);
      Expression.ValidateDynamicArgument(arg2);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg2, parametersCached[3]);
      return DynamicExpression.Make(TypeExtensions.GetReturnType((MethodBase) methodForDynamic), delegateType, binder, arg0, arg1, arg2);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
    {
      ContractUtils.RequiresNotNull((object) delegateType, "delegateType");
      ContractUtils.RequiresNotNull((object) binder, "binder");
      if (!delegateType.IsSubclassOf(typeof (MulticastDelegate)))
        throw Error.TypeMustBeDerivedFromSystemDelegate();
      MethodInfo methodForDynamic = Expression.GetValidMethodForDynamic(delegateType);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) methodForDynamic);
      Expression.ValidateArgumentCount((MethodBase) methodForDynamic, ExpressionType.Dynamic, 5, parametersCached);
      Expression.ValidateDynamicArgument(arg0);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg0, parametersCached[1]);
      Expression.ValidateDynamicArgument(arg1);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg1, parametersCached[2]);
      Expression.ValidateDynamicArgument(arg2);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg2, parametersCached[3]);
      Expression.ValidateDynamicArgument(arg3);
      Expression.ValidateOneArgument((MethodBase) methodForDynamic, ExpressionType.Dynamic, arg3, parametersCached[4]);
      return DynamicExpression.Make(TypeExtensions.GetReturnType((MethodBase) methodForDynamic), delegateType, binder, arg0, arg1, arg2, arg3);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, params Expression[] arguments)
    {
      return Expression.Dynamic(binder, returnType, (IEnumerable<Expression>) arguments);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      Expression.ValidateDynamicArgument(arg0);
      DelegateHelpers.TypeInfo nextTypeInfo = DelegateHelpers.GetNextTypeInfo(returnType, DelegateHelpers.GetNextTypeInfo(arg0.Type, DelegateHelpers.NextTypeInfo(typeof (CallSite))));
      Type delegateType = nextTypeInfo.DelegateType;
      if (delegateType == (Type) null)
        delegateType = nextTypeInfo.MakeDelegateType(returnType, new Expression[1]
        {
          arg0
        });
      return DynamicExpression.Make(returnType, delegateType, binder, arg0);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      Expression.ValidateDynamicArgument(arg0);
      Expression.ValidateDynamicArgument(arg1);
      DelegateHelpers.TypeInfo nextTypeInfo = DelegateHelpers.GetNextTypeInfo(returnType, DelegateHelpers.GetNextTypeInfo(arg1.Type, DelegateHelpers.GetNextTypeInfo(arg0.Type, DelegateHelpers.NextTypeInfo(typeof (CallSite)))));
      Type delegateType = nextTypeInfo.DelegateType;
      if (delegateType == (Type) null)
        delegateType = nextTypeInfo.MakeDelegateType(returnType, arg0, arg1);
      return DynamicExpression.Make(returnType, delegateType, binder, arg0, arg1);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      Expression.ValidateDynamicArgument(arg0);
      Expression.ValidateDynamicArgument(arg1);
      Expression.ValidateDynamicArgument(arg2);
      DelegateHelpers.TypeInfo nextTypeInfo = DelegateHelpers.GetNextTypeInfo(returnType, DelegateHelpers.GetNextTypeInfo(arg2.Type, DelegateHelpers.GetNextTypeInfo(arg1.Type, DelegateHelpers.GetNextTypeInfo(arg0.Type, DelegateHelpers.NextTypeInfo(typeof (CallSite))))));
      Type delegateType = nextTypeInfo.DelegateType;
      if (delegateType == (Type) null)
        delegateType = nextTypeInfo.MakeDelegateType(returnType, arg0, arg1, arg2);
      return DynamicExpression.Make(returnType, delegateType, binder, arg0, arg1, arg2);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      Expression.ValidateDynamicArgument(arg0);
      Expression.ValidateDynamicArgument(arg1);
      Expression.ValidateDynamicArgument(arg2);
      Expression.ValidateDynamicArgument(arg3);
      DelegateHelpers.TypeInfo nextTypeInfo = DelegateHelpers.GetNextTypeInfo(returnType, DelegateHelpers.GetNextTypeInfo(arg3.Type, DelegateHelpers.GetNextTypeInfo(arg2.Type, DelegateHelpers.GetNextTypeInfo(arg1.Type, DelegateHelpers.GetNextTypeInfo(arg0.Type, DelegateHelpers.NextTypeInfo(typeof (CallSite)))))));
      Type delegateType = nextTypeInfo.DelegateType;
      if (delegateType == (Type) null)
        delegateType = nextTypeInfo.MakeDelegateType(returnType, arg0, arg1, arg2, arg3);
      return DynamicExpression.Make(returnType, delegateType, binder, arg0, arg1, arg2, arg3);
    }

    [__DynamicallyInvokable]
    public static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, IEnumerable<Expression> arguments)
    {
      ContractUtils.RequiresNotNull((object) arguments, "arguments");
      ContractUtils.RequiresNotNull((object) returnType, "returnType");
      ReadOnlyCollection<Expression> args = CollectionExtensions.ToReadOnly<Expression>(arguments);
      ContractUtils.RequiresNotEmpty<Expression>((ICollection<Expression>) args, "args");
      return Expression.MakeDynamic(binder, returnType, args);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ElementInit ElementInit(MethodInfo addMethod, params Expression[] arguments)
    {
      return Expression.ElementInit(addMethod, (IEnumerable<Expression>) arguments);
    }

    [__DynamicallyInvokable]
    public static ElementInit ElementInit(MethodInfo addMethod, IEnumerable<Expression> arguments)
    {
      ContractUtils.RequiresNotNull((object) addMethod, "addMethod");
      ContractUtils.RequiresNotNull((object) arguments, "arguments");
      ReadOnlyCollection<Expression> arguments1 = CollectionExtensions.ToReadOnly<Expression>(arguments);
      Expression.RequiresCanRead((IEnumerable<Expression>) arguments1, "arguments");
      Expression.ValidateElementInitAddMethodInfo(addMethod);
      Expression.ValidateArgumentTypes((MethodBase) addMethod, ExpressionType.Call, ref arguments1);
      return new ElementInit(addMethod, arguments1);
    }

    [__DynamicallyInvokable]
    public virtual Expression Reduce()
    {
      if (this.CanReduce)
        throw Error.ReducibleMustOverrideReduce();
      else
        return this;
    }

    [__DynamicallyInvokable]
    protected internal virtual Expression VisitChildren(ExpressionVisitor visitor)
    {
      if (!this.CanReduce)
        throw Error.MustBeReducible();
      else
        return visitor.Visit(this.ReduceAndCheck());
    }

    [__DynamicallyInvokable]
    protected internal virtual Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitExtension(this);
    }

    [__DynamicallyInvokable]
    public Expression ReduceAndCheck()
    {
      if (!this.CanReduce)
        throw Error.MustBeReducible();
      Expression expression = this.Reduce();
      if (expression == null || expression == this)
        throw Error.MustReduceToDifferent();
      if (!TypeUtils.AreReferenceAssignable(this.Type, expression.Type))
        throw Error.ReducedNotCompatible();
      else
        return expression;
    }

    [__DynamicallyInvokable]
    public Expression ReduceExtensions()
    {
      Expression expression = this;
      while (expression.NodeType == ExpressionType.Extension)
        expression = expression.ReduceAndCheck();
      return expression;
    }

    [__DynamicallyInvokable]
    public override string ToString()
    {
      return ExpressionStringBuilder.ExpressionToString(this);
    }

    internal static ReadOnlyCollection<T> ReturnReadOnly<T>(ref IList<T> collection)
    {
      IList<T> comparand = collection;
      ReadOnlyCollection<T> readOnlyCollection = comparand as ReadOnlyCollection<T>;
      if (readOnlyCollection != null)
        return readOnlyCollection;
      Interlocked.CompareExchange<IList<T>>(ref collection, (IList<T>) CollectionExtensions.ToReadOnly<T>((IEnumerable<T>) comparand), comparand);
      return (ReadOnlyCollection<T>) collection;
    }

    internal static T ReturnObject<T>(object collectionOrT) where T : class
    {
      T obj = collectionOrT as T;
      if ((object) obj != null)
        return obj;
      else
        return ((ReadOnlyCollection<T>) collectionOrT)[0];
    }

    [__DynamicallyInvokable]
    public static GotoExpression Break(LabelTarget target)
    {
      return Expression.MakeGoto(GotoExpressionKind.Break, target, (Expression) null, typeof (void));
    }

    [__DynamicallyInvokable]
    public static GotoExpression Break(LabelTarget target, Expression value)
    {
      return Expression.MakeGoto(GotoExpressionKind.Break, target, value, typeof (void));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static GotoExpression Break(LabelTarget target, Type type)
    {
      return Expression.MakeGoto(GotoExpressionKind.Break, target, (Expression) null, type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static GotoExpression Break(LabelTarget target, Expression value, Type type)
    {
      return Expression.MakeGoto(GotoExpressionKind.Break, target, value, type);
    }

    [__DynamicallyInvokable]
    public static GotoExpression Continue(LabelTarget target)
    {
      return Expression.MakeGoto(GotoExpressionKind.Continue, target, (Expression) null, typeof (void));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static GotoExpression Continue(LabelTarget target, Type type)
    {
      return Expression.MakeGoto(GotoExpressionKind.Continue, target, (Expression) null, type);
    }

    [__DynamicallyInvokable]
    public static GotoExpression Return(LabelTarget target)
    {
      return Expression.MakeGoto(GotoExpressionKind.Return, target, (Expression) null, typeof (void));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static GotoExpression Return(LabelTarget target, Type type)
    {
      return Expression.MakeGoto(GotoExpressionKind.Return, target, (Expression) null, type);
    }

    [__DynamicallyInvokable]
    public static GotoExpression Return(LabelTarget target, Expression value)
    {
      return Expression.MakeGoto(GotoExpressionKind.Return, target, value, typeof (void));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static GotoExpression Return(LabelTarget target, Expression value, Type type)
    {
      return Expression.MakeGoto(GotoExpressionKind.Return, target, value, type);
    }

    [__DynamicallyInvokable]
    public static GotoExpression Goto(LabelTarget target)
    {
      return Expression.MakeGoto(GotoExpressionKind.Goto, target, (Expression) null, typeof (void));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static GotoExpression Goto(LabelTarget target, Type type)
    {
      return Expression.MakeGoto(GotoExpressionKind.Goto, target, (Expression) null, type);
    }

    [__DynamicallyInvokable]
    public static GotoExpression Goto(LabelTarget target, Expression value)
    {
      return Expression.MakeGoto(GotoExpressionKind.Goto, target, value, typeof (void));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static GotoExpression Goto(LabelTarget target, Expression value, Type type)
    {
      return Expression.MakeGoto(GotoExpressionKind.Goto, target, value, type);
    }

    [__DynamicallyInvokable]
    public static GotoExpression MakeGoto(GotoExpressionKind kind, LabelTarget target, Expression value, Type type)
    {
      Expression.ValidateGoto(target, ref value, "target", "value");
      return new GotoExpression(kind, target, value, type);
    }

    [__DynamicallyInvokable]
    public static IndexExpression MakeIndex(Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments)
    {
      if (indexer != (PropertyInfo) null)
        return Expression.Property(instance, indexer, arguments);
      else
        return Expression.ArrayAccess(instance, arguments);
    }

    [__DynamicallyInvokable]
    public static IndexExpression ArrayAccess(Expression array, params Expression[] indexes)
    {
      return Expression.ArrayAccess(array, (IEnumerable<Expression>) indexes);
    }

    [__DynamicallyInvokable]
    public static IndexExpression ArrayAccess(Expression array, IEnumerable<Expression> indexes)
    {
      Expression.RequiresCanRead(array, "array");
      Type type = array.Type;
      if (!type.IsArray)
        throw Error.ArgumentMustBeArray();
      ReadOnlyCollection<Expression> readOnlyCollection = CollectionExtensions.ToReadOnly<Expression>(indexes);
      if (type.GetArrayRank() != readOnlyCollection.Count)
        throw Error.IncorrectNumberOfIndexes();
      foreach (Expression expression in readOnlyCollection)
      {
        Expression.RequiresCanRead(expression, "indexes");
        if (expression.Type != typeof (int))
          throw Error.ArgumentMustBeArrayIndexType();
      }
      return new IndexExpression(array, (PropertyInfo) null, (IList<Expression>) readOnlyCollection);
    }

    [__DynamicallyInvokable]
    public static IndexExpression Property(Expression instance, string propertyName, params Expression[] arguments)
    {
      Expression.RequiresCanRead(instance, "instance");
      ContractUtils.RequiresNotNull((object) propertyName, "indexerName");
      PropertyInfo instanceProperty = Expression.FindInstanceProperty(instance.Type, propertyName, arguments);
      return Expression.Property(instance, instanceProperty, arguments);
    }

    [__DynamicallyInvokable]
    public static IndexExpression Property(Expression instance, PropertyInfo indexer, params Expression[] arguments)
    {
      return Expression.Property(instance, indexer, (IEnumerable<Expression>) arguments);
    }

    [__DynamicallyInvokable]
    public static IndexExpression Property(Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments)
    {
      ReadOnlyCollection<Expression> argList = CollectionExtensions.ToReadOnly<Expression>(arguments);
      Expression.ValidateIndexedProperty(instance, indexer, ref argList);
      return new IndexExpression(instance, indexer, (IList<Expression>) argList);
    }

    [__DynamicallyInvokable]
    public static InvocationExpression Invoke(Expression expression, params Expression[] arguments)
    {
      return Expression.Invoke(expression, (IEnumerable<Expression>) arguments);
    }

    [__DynamicallyInvokable]
    public static InvocationExpression Invoke(Expression expression, IEnumerable<Expression> arguments)
    {
      Expression.RequiresCanRead(expression, "expression");
      ReadOnlyCollection<Expression> arguments1 = CollectionExtensions.ToReadOnly<Expression>(arguments);
      MethodInfo invokeMethod = Expression.GetInvokeMethod(expression);
      Expression.ValidateArgumentTypes((MethodBase) invokeMethod, ExpressionType.Invoke, ref arguments1);
      return new InvocationExpression(expression, (IList<Expression>) arguments1, invokeMethod.ReturnType);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LabelExpression Label(LabelTarget target)
    {
      return Expression.Label(target, (Expression) null);
    }

    [__DynamicallyInvokable]
    public static LabelExpression Label(LabelTarget target, Expression defaultValue)
    {
      Expression.ValidateGoto(target, ref defaultValue, "label", "defaultValue");
      return new LabelExpression(target, defaultValue);
    }

    [__DynamicallyInvokable]
    public static LabelTarget Label()
    {
      return Expression.Label(typeof (void), (string) null);
    }

    [__DynamicallyInvokable]
    public static LabelTarget Label(string name)
    {
      return Expression.Label(typeof (void), name);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LabelTarget Label(Type type)
    {
      return Expression.Label(type, (string) null);
    }

    [__DynamicallyInvokable]
    public static LabelTarget Label(Type type, string name)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      TypeUtils.ValidateType(type);
      return new LabelTarget(type, name);
    }

    [__DynamicallyInvokable]
    public static Expression<TDelegate> Lambda<TDelegate>(Expression body, params ParameterExpression[] parameters)
    {
      return Expression.Lambda<TDelegate>(body, false, (IEnumerable<ParameterExpression>) parameters);
    }

    [__DynamicallyInvokable]
    public static Expression<TDelegate> Lambda<TDelegate>(Expression body, bool tailCall, params ParameterExpression[] parameters)
    {
      return Expression.Lambda<TDelegate>(body, tailCall, (IEnumerable<ParameterExpression>) parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static Expression<TDelegate> Lambda<TDelegate>(Expression body, IEnumerable<ParameterExpression> parameters)
    {
      return Expression.Lambda<TDelegate>(body, (string) null, false, parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static Expression<TDelegate> Lambda<TDelegate>(Expression body, bool tailCall, IEnumerable<ParameterExpression> parameters)
    {
      return Expression.Lambda<TDelegate>(body, (string) null, tailCall, parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static Expression<TDelegate> Lambda<TDelegate>(Expression body, string name, IEnumerable<ParameterExpression> parameters)
    {
      return Expression.Lambda<TDelegate>(body, name, false, parameters);
    }

    [__DynamicallyInvokable]
    public static Expression<TDelegate> Lambda<TDelegate>(Expression body, string name, bool tailCall, IEnumerable<ParameterExpression> parameters)
    {
      ReadOnlyCollection<ParameterExpression> parameters1 = CollectionExtensions.ToReadOnly<ParameterExpression>(parameters);
      Expression.ValidateLambdaArgs(typeof (TDelegate), ref body, parameters1);
      return new Expression<TDelegate>(body, name, tailCall, parameters1);
    }

    [__DynamicallyInvokable]
    public static LambdaExpression Lambda(Expression body, params ParameterExpression[] parameters)
    {
      return Expression.Lambda(body, false, (IEnumerable<ParameterExpression>) parameters);
    }

    [__DynamicallyInvokable]
    public static LambdaExpression Lambda(Expression body, bool tailCall, params ParameterExpression[] parameters)
    {
      return Expression.Lambda(body, tailCall, (IEnumerable<ParameterExpression>) parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LambdaExpression Lambda(Expression body, IEnumerable<ParameterExpression> parameters)
    {
      return Expression.Lambda(body, (string) null, false, parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LambdaExpression Lambda(Expression body, bool tailCall, IEnumerable<ParameterExpression> parameters)
    {
      return Expression.Lambda(body, (string) null, tailCall, parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LambdaExpression Lambda(Type delegateType, Expression body, params ParameterExpression[] parameters)
    {
      return Expression.Lambda(delegateType, body, (string) null, false, (IEnumerable<ParameterExpression>) parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LambdaExpression Lambda(Type delegateType, Expression body, bool tailCall, params ParameterExpression[] parameters)
    {
      return Expression.Lambda(delegateType, body, (string) null, tailCall, (IEnumerable<ParameterExpression>) parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LambdaExpression Lambda(Type delegateType, Expression body, IEnumerable<ParameterExpression> parameters)
    {
      return Expression.Lambda(delegateType, body, (string) null, false, parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LambdaExpression Lambda(Type delegateType, Expression body, bool tailCall, IEnumerable<ParameterExpression> parameters)
    {
      return Expression.Lambda(delegateType, body, (string) null, tailCall, parameters);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LambdaExpression Lambda(Expression body, string name, IEnumerable<ParameterExpression> parameters)
    {
      return Expression.Lambda(body, name, false, parameters);
    }

    [__DynamicallyInvokable]
    public static LambdaExpression Lambda(Expression body, string name, bool tailCall, IEnumerable<ParameterExpression> parameters)
    {
      ContractUtils.RequiresNotNull((object) body, "body");
      ReadOnlyCollection<ParameterExpression> parameters1 = CollectionExtensions.ToReadOnly<ParameterExpression>(parameters);
      int count = parameters1.Count;
      Type[] types = new Type[count + 1];
      if (count > 0)
      {
        Set<ParameterExpression> set = new Set<ParameterExpression>(parameters1.Count);
        for (int index = 0; index < count; ++index)
        {
          ParameterExpression parameterExpression = parameters1[index];
          ContractUtils.RequiresNotNull((object) parameterExpression, "parameter");
          types[index] = parameterExpression.IsByRef ? parameterExpression.Type.MakeByRefType() : parameterExpression.Type;
          if (set.Contains(parameterExpression))
            throw Error.DuplicateVariable((object) parameterExpression);
          set.Add(parameterExpression);
        }
      }
      types[count] = body.Type;
      return Expression.CreateLambda(DelegateHelpers.MakeDelegateType(types), body, name, tailCall, parameters1);
    }

    [__DynamicallyInvokable]
    public static LambdaExpression Lambda(Type delegateType, Expression body, string name, IEnumerable<ParameterExpression> parameters)
    {
      ReadOnlyCollection<ParameterExpression> parameters1 = CollectionExtensions.ToReadOnly<ParameterExpression>(parameters);
      Expression.ValidateLambdaArgs(delegateType, ref body, parameters1);
      return Expression.CreateLambda(delegateType, body, name, false, parameters1);
    }

    [__DynamicallyInvokable]
    public static LambdaExpression Lambda(Type delegateType, Expression body, string name, bool tailCall, IEnumerable<ParameterExpression> parameters)
    {
      ReadOnlyCollection<ParameterExpression> parameters1 = CollectionExtensions.ToReadOnly<ParameterExpression>(parameters);
      Expression.ValidateLambdaArgs(delegateType, ref body, parameters1);
      return Expression.CreateLambda(delegateType, body, name, tailCall, parameters1);
    }

    private static void ValidateLambdaArgs(Type delegateType, ref Expression body, ReadOnlyCollection<ParameterExpression> parameters)
    {
      ContractUtils.RequiresNotNull((object) delegateType, "delegateType");
      Expression.RequiresCanRead(body, "body");
      if (!typeof (MulticastDelegate).IsAssignableFrom(delegateType) || delegateType == typeof (MulticastDelegate))
        throw Error.LambdaTypeMustBeDerivedFromSystemDelegate();
      CacheDict<Type, MethodInfo> cacheDict = Expression._LambdaDelegateCache;
      MethodInfo method;
      if (!cacheDict.TryGetValue(delegateType, out method))
      {
        method = delegateType.GetMethod("Invoke");
        if (TypeUtils.CanCache(delegateType))
          cacheDict[delegateType] = method;
      }
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method);
      if (parametersCached.Length > 0)
      {
        if (parametersCached.Length != parameters.Count)
          throw Error.IncorrectNumberOfLambdaDeclarationParameters();
        Set<ParameterExpression> set = new Set<ParameterExpression>(parametersCached.Length);
        int index = 0;
        for (int length = parametersCached.Length; index < length; ++index)
        {
          ParameterExpression parameterExpression = parameters[index];
          ParameterInfo parameterInfo = parametersCached[index];
          Expression.RequiresCanRead((Expression) parameterExpression, "parameters");
          Type src = parameterInfo.ParameterType;
          if (parameterExpression.IsByRef)
          {
            if (!src.IsByRef)
              throw Error.ParameterExpressionNotValidAsDelegate((object) parameterExpression.Type.MakeByRefType(), (object) src);
            src = src.GetElementType();
          }
          if (!TypeUtils.AreReferenceAssignable(parameterExpression.Type, src))
            throw Error.ParameterExpressionNotValidAsDelegate((object) parameterExpression.Type, (object) src);
          if (set.Contains(parameterExpression))
            throw Error.DuplicateVariable((object) parameterExpression);
          set.Add(parameterExpression);
        }
      }
      else if (parameters.Count > 0)
        throw Error.IncorrectNumberOfLambdaDeclarationParameters();
      if (method.ReturnType != typeof (void) && !TypeUtils.AreReferenceAssignable(method.ReturnType, body.Type) && !Expression.TryQuote(method.ReturnType, ref body))
        throw Error.ExpressionTypeDoesNotMatchReturn((object) body.Type, (object) method.ReturnType);
    }

    [__DynamicallyInvokable]
    public static Type GetFuncType(params Type[] typeArgs)
    {
      if (!Expression.ValidateTryGetFuncActionArgs(typeArgs))
        throw Error.TypeMustNotBeByRef();
      Type funcType = DelegateHelpers.GetFuncType(typeArgs);
      if (funcType == (Type) null)
        throw Error.IncorrectNumberOfTypeArgsForFunc();
      else
        return funcType;
    }

    [__DynamicallyInvokable]
    public static bool TryGetFuncType(Type[] typeArgs, out Type funcType)
    {
      if (Expression.ValidateTryGetFuncActionArgs(typeArgs))
        return (funcType = DelegateHelpers.GetFuncType(typeArgs)) != (Type) null;
      funcType = (Type) null;
      return false;
    }

    [__DynamicallyInvokable]
    public static Type GetActionType(params Type[] typeArgs)
    {
      if (!Expression.ValidateTryGetFuncActionArgs(typeArgs))
        throw Error.TypeMustNotBeByRef();
      Type actionType = DelegateHelpers.GetActionType(typeArgs);
      if (actionType == (Type) null)
        throw Error.IncorrectNumberOfTypeArgsForAction();
      else
        return actionType;
    }

    [__DynamicallyInvokable]
    public static bool TryGetActionType(Type[] typeArgs, out Type actionType)
    {
      if (Expression.ValidateTryGetFuncActionArgs(typeArgs))
        return (actionType = DelegateHelpers.GetActionType(typeArgs)) != (Type) null;
      actionType = (Type) null;
      return false;
    }

    [__DynamicallyInvokable]
    public static Type GetDelegateType(params Type[] typeArgs)
    {
      ContractUtils.RequiresNotEmpty<Type>((ICollection<Type>) typeArgs, "typeArgs");
      ContractUtils.RequiresNotNullItems<Type>((IList<Type>) typeArgs, "typeArgs");
      return DelegateHelpers.MakeDelegateType(typeArgs);
    }

    [__DynamicallyInvokable]
    public static ListInitExpression ListInit(NewExpression newExpression, params Expression[] initializers)
    {
      ContractUtils.RequiresNotNull((object) newExpression, "newExpression");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      return Expression.ListInit(newExpression, (IEnumerable<Expression>) initializers);
    }

    [__DynamicallyInvokable]
    public static ListInitExpression ListInit(NewExpression newExpression, IEnumerable<Expression> initializers)
    {
      ContractUtils.RequiresNotNull((object) newExpression, "newExpression");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      ReadOnlyCollection<Expression> readOnlyCollection = CollectionExtensions.ToReadOnly<Expression>(initializers);
      if (readOnlyCollection.Count == 0)
        throw Error.ListInitializerWithZeroMembers();
      MethodInfo method = Expression.FindMethod(newExpression.Type, "Add", (Type[]) null, new Expression[1]
      {
        readOnlyCollection[0]
      }, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      return Expression.ListInit(newExpression, method, initializers);
    }

    [__DynamicallyInvokable]
    public static ListInitExpression ListInit(NewExpression newExpression, MethodInfo addMethod, params Expression[] initializers)
    {
      if (addMethod == (MethodInfo) null)
        return Expression.ListInit(newExpression, (IEnumerable<Expression>) initializers);
      ContractUtils.RequiresNotNull((object) newExpression, "newExpression");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      return Expression.ListInit(newExpression, addMethod, (IEnumerable<Expression>) initializers);
    }

    [__DynamicallyInvokable]
    public static ListInitExpression ListInit(NewExpression newExpression, MethodInfo addMethod, IEnumerable<Expression> initializers)
    {
      if (addMethod == (MethodInfo) null)
        return Expression.ListInit(newExpression, initializers);
      ContractUtils.RequiresNotNull((object) newExpression, "newExpression");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      ReadOnlyCollection<Expression> readOnlyCollection = CollectionExtensions.ToReadOnly<Expression>(initializers);
      if (readOnlyCollection.Count == 0)
        throw Error.ListInitializerWithZeroMembers();
      ElementInit[] list = new ElementInit[readOnlyCollection.Count];
      for (int index = 0; index < readOnlyCollection.Count; ++index)
        list[index] = Expression.ElementInit(addMethod, new Expression[1]
        {
          readOnlyCollection[index]
        });
      return Expression.ListInit(newExpression, (IEnumerable<ElementInit>) new TrueReadOnlyCollection<ElementInit>(list));
    }

    [__DynamicallyInvokable]
    public static ListInitExpression ListInit(NewExpression newExpression, params ElementInit[] initializers)
    {
      return Expression.ListInit(newExpression, (IEnumerable<ElementInit>) initializers);
    }

    [__DynamicallyInvokable]
    public static ListInitExpression ListInit(NewExpression newExpression, IEnumerable<ElementInit> initializers)
    {
      ContractUtils.RequiresNotNull((object) newExpression, "newExpression");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      ReadOnlyCollection<ElementInit> initializers1 = CollectionExtensions.ToReadOnly<ElementInit>(initializers);
      if (initializers1.Count == 0)
        throw Error.ListInitializerWithZeroMembers();
      Expression.ValidateListInitArgs(newExpression.Type, initializers1);
      return new ListInitExpression(newExpression, initializers1);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LoopExpression Loop(Expression body)
    {
      return Expression.Loop(body, (LabelTarget) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static LoopExpression Loop(Expression body, LabelTarget @break)
    {
      return Expression.Loop(body, @break, (LabelTarget) null);
    }

    [__DynamicallyInvokable]
    public static LoopExpression Loop(Expression body, LabelTarget @break, LabelTarget @continue)
    {
      Expression.RequiresCanRead(body, "body");
      if (@continue != null && @continue.Type != typeof (void))
        throw Error.LabelTypeMustBeVoid();
      else
        return new LoopExpression(body, @break, @continue);
    }

    [__DynamicallyInvokable]
    public static MemberAssignment Bind(MemberInfo member, Expression expression)
    {
      ContractUtils.RequiresNotNull((object) member, "member");
      Expression.RequiresCanRead(expression, "expression");
      Type memberType;
      Expression.ValidateSettableFieldOrPropertyMember(member, out memberType);
      if (!memberType.IsAssignableFrom(expression.Type))
        throw Error.ArgumentTypesMustMatch();
      else
        return new MemberAssignment(member, expression);
    }

    [__DynamicallyInvokable]
    public static MemberAssignment Bind(MethodInfo propertyAccessor, Expression expression)
    {
      ContractUtils.RequiresNotNull((object) propertyAccessor, "propertyAccessor");
      ContractUtils.RequiresNotNull((object) expression, "expression");
      Expression.ValidateMethodInfo(propertyAccessor);
      return Expression.Bind((MemberInfo) Expression.GetProperty(propertyAccessor), expression);
    }

    [__DynamicallyInvokable]
    public static MemberExpression Field(Expression expression, FieldInfo field)
    {
      ContractUtils.RequiresNotNull((object) field, "field");
      if (field.IsStatic)
      {
        if (expression != null)
          throw new ArgumentException(Strings.OnlyStaticFieldsHaveNullInstance, "expression");
      }
      else
      {
        if (expression == null)
          throw new ArgumentException(Strings.OnlyStaticFieldsHaveNullInstance, "field");
        Expression.RequiresCanRead(expression, "expression");
        if (!TypeUtils.AreReferenceAssignable(field.DeclaringType, expression.Type))
          throw Error.FieldInfoNotDefinedForType((object) field.DeclaringType, (object) field.Name, (object) expression.Type);
      }
      return MemberExpression.Make(expression, (MemberInfo) field);
    }

    [__DynamicallyInvokable]
    public static MemberExpression Field(Expression expression, string fieldName)
    {
      Expression.RequiresCanRead(expression, "expression");
      FieldInfo field = expression.Type.GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
      if (field == (FieldInfo) null)
        field = expression.Type.GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      if (field == (FieldInfo) null)
        throw Error.InstanceFieldNotDefinedForType((object) fieldName, (object) expression.Type);
      else
        return Expression.Field(expression, field);
    }

    [__DynamicallyInvokable]
    public static MemberExpression Field(Expression expression, Type type, string fieldName)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      FieldInfo field = type.GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
      if (field == (FieldInfo) null)
        field = type.GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      if (field == (FieldInfo) null)
        throw Error.FieldNotDefinedForType((object) fieldName, (object) type);
      else
        return Expression.Field(expression, field);
    }

    [__DynamicallyInvokable]
    public static MemberExpression Property(Expression expression, string propertyName)
    {
      Expression.RequiresCanRead(expression, "expression");
      ContractUtils.RequiresNotNull((object) propertyName, "propertyName");
      PropertyInfo property = expression.Type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
      if (property == (PropertyInfo) null)
        property = expression.Type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      if (property == (PropertyInfo) null)
        throw Error.InstancePropertyNotDefinedForType((object) propertyName, (object) expression.Type);
      else
        return Expression.Property(expression, property);
    }

    [__DynamicallyInvokable]
    public static MemberExpression Property(Expression expression, Type type, string propertyName)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      ContractUtils.RequiresNotNull((object) propertyName, "propertyName");
      PropertyInfo property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
      if (property == (PropertyInfo) null)
        property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      if (property == (PropertyInfo) null)
        throw Error.PropertyNotDefinedForType((object) propertyName, (object) type);
      else
        return Expression.Property(expression, property);
    }

    [__DynamicallyInvokable]
    public static MemberExpression Property(Expression expression, PropertyInfo property)
    {
      ContractUtils.RequiresNotNull((object) property, "property");
      MethodInfo methodInfo = property.GetGetMethod(true) ?? property.GetSetMethod(true);
      if (methodInfo == (MethodInfo) null)
        throw Error.PropertyDoesNotHaveAccessor((object) property);
      if (methodInfo.IsStatic)
      {
        if (expression != null)
          throw new ArgumentException(Strings.OnlyStaticPropertiesHaveNullInstance, "expression");
      }
      else
      {
        if (expression == null)
          throw new ArgumentException(Strings.OnlyStaticPropertiesHaveNullInstance, "property");
        Expression.RequiresCanRead(expression, "expression");
        if (!TypeUtils.IsValidInstanceType((MemberInfo) property, expression.Type))
          throw Error.PropertyNotDefinedForType((object) property, (object) expression.Type);
      }
      return MemberExpression.Make(expression, (MemberInfo) property);
    }

    [__DynamicallyInvokable]
    public static MemberExpression Property(Expression expression, MethodInfo propertyAccessor)
    {
      ContractUtils.RequiresNotNull((object) propertyAccessor, "propertyAccessor");
      Expression.ValidateMethodInfo(propertyAccessor);
      return Expression.Property(expression, Expression.GetProperty(propertyAccessor));
    }

    [__DynamicallyInvokable]
    public static MemberExpression PropertyOrField(Expression expression, string propertyOrFieldName)
    {
      Expression.RequiresCanRead(expression, "expression");
      PropertyInfo property1 = expression.Type.GetProperty(propertyOrFieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
      if (property1 != (PropertyInfo) null)
        return Expression.Property(expression, property1);
      FieldInfo field1 = expression.Type.GetField(propertyOrFieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
      if (field1 != (FieldInfo) null)
        return Expression.Field(expression, field1);
      PropertyInfo property2 = expression.Type.GetProperty(propertyOrFieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      if (property2 != (PropertyInfo) null)
        return Expression.Property(expression, property2);
      FieldInfo field2 = expression.Type.GetField(propertyOrFieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      if (field2 != (FieldInfo) null)
        return Expression.Field(expression, field2);
      else
        throw Error.NotAMemberOfType((object) propertyOrFieldName, (object) expression.Type);
    }

    [__DynamicallyInvokable]
    public static MemberExpression MakeMemberAccess(Expression expression, MemberInfo member)
    {
      ContractUtils.RequiresNotNull((object) member, "member");
      FieldInfo field = member as FieldInfo;
      if (field != (FieldInfo) null)
        return Expression.Field(expression, field);
      PropertyInfo property = member as PropertyInfo;
      if (property != (PropertyInfo) null)
        return Expression.Property(expression, property);
      else
        throw Error.MemberNotFieldOrProperty((object) member);
    }

    [__DynamicallyInvokable]
    public static MemberInitExpression MemberInit(NewExpression newExpression, params MemberBinding[] bindings)
    {
      return Expression.MemberInit(newExpression, (IEnumerable<MemberBinding>) bindings);
    }

    [__DynamicallyInvokable]
    public static MemberInitExpression MemberInit(NewExpression newExpression, IEnumerable<MemberBinding> bindings)
    {
      ContractUtils.RequiresNotNull((object) newExpression, "newExpression");
      ContractUtils.RequiresNotNull((object) bindings, "bindings");
      ReadOnlyCollection<MemberBinding> bindings1 = CollectionExtensions.ToReadOnly<MemberBinding>(bindings);
      Expression.ValidateMemberInitArgs(newExpression.Type, bindings1);
      return new MemberInitExpression(newExpression, bindings1);
    }

    [__DynamicallyInvokable]
    public static MemberListBinding ListBind(MemberInfo member, params ElementInit[] initializers)
    {
      ContractUtils.RequiresNotNull((object) member, "member");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      return Expression.ListBind(member, (IEnumerable<ElementInit>) initializers);
    }

    [__DynamicallyInvokable]
    public static MemberListBinding ListBind(MemberInfo member, IEnumerable<ElementInit> initializers)
    {
      ContractUtils.RequiresNotNull((object) member, "member");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      Type memberType;
      Expression.ValidateGettableFieldOrPropertyMember(member, out memberType);
      ReadOnlyCollection<ElementInit> initializers1 = CollectionExtensions.ToReadOnly<ElementInit>(initializers);
      Expression.ValidateListInitArgs(memberType, initializers1);
      return new MemberListBinding(member, initializers1);
    }

    [__DynamicallyInvokable]
    public static MemberListBinding ListBind(MethodInfo propertyAccessor, params ElementInit[] initializers)
    {
      ContractUtils.RequiresNotNull((object) propertyAccessor, "propertyAccessor");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      return Expression.ListBind(propertyAccessor, (IEnumerable<ElementInit>) initializers);
    }

    [__DynamicallyInvokable]
    public static MemberListBinding ListBind(MethodInfo propertyAccessor, IEnumerable<ElementInit> initializers)
    {
      ContractUtils.RequiresNotNull((object) propertyAccessor, "propertyAccessor");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      return Expression.ListBind((MemberInfo) Expression.GetProperty(propertyAccessor), initializers);
    }

    [__DynamicallyInvokable]
    public static MemberMemberBinding MemberBind(MemberInfo member, params MemberBinding[] bindings)
    {
      ContractUtils.RequiresNotNull((object) member, "member");
      ContractUtils.RequiresNotNull((object) bindings, "bindings");
      return Expression.MemberBind(member, (IEnumerable<MemberBinding>) bindings);
    }

    [__DynamicallyInvokable]
    public static MemberMemberBinding MemberBind(MemberInfo member, IEnumerable<MemberBinding> bindings)
    {
      ContractUtils.RequiresNotNull((object) member, "member");
      ContractUtils.RequiresNotNull((object) bindings, "bindings");
      ReadOnlyCollection<MemberBinding> bindings1 = CollectionExtensions.ToReadOnly<MemberBinding>(bindings);
      Type memberType;
      Expression.ValidateGettableFieldOrPropertyMember(member, out memberType);
      Expression.ValidateMemberInitArgs(memberType, bindings1);
      return new MemberMemberBinding(member, bindings1);
    }

    [__DynamicallyInvokable]
    public static MemberMemberBinding MemberBind(MethodInfo propertyAccessor, params MemberBinding[] bindings)
    {
      ContractUtils.RequiresNotNull((object) propertyAccessor, "propertyAccessor");
      return Expression.MemberBind((MemberInfo) Expression.GetProperty(propertyAccessor), bindings);
    }

    [__DynamicallyInvokable]
    public static MemberMemberBinding MemberBind(MethodInfo propertyAccessor, IEnumerable<MemberBinding> bindings)
    {
      ContractUtils.RequiresNotNull((object) propertyAccessor, "propertyAccessor");
      return Expression.MemberBind((MemberInfo) Expression.GetProperty(propertyAccessor), bindings);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(MethodInfo method, Expression arg0)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ContractUtils.RequiresNotNull((object) arg0, "arg0");
      ParameterInfo[] parameters = Expression.ValidateMethodAndGetParameters((Expression) null, method);
      Expression.ValidateArgumentCount((MethodBase) method, ExpressionType.Call, 1, parameters);
      arg0 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg0, parameters[0]);
      return (MethodCallExpression) new MethodCallExpression1(method, arg0);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ContractUtils.RequiresNotNull((object) arg0, "arg0");
      ContractUtils.RequiresNotNull((object) arg1, "arg1");
      ParameterInfo[] parameters = Expression.ValidateMethodAndGetParameters((Expression) null, method);
      Expression.ValidateArgumentCount((MethodBase) method, ExpressionType.Call, 2, parameters);
      arg0 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg0, parameters[0]);
      arg1 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg1, parameters[1]);
      return (MethodCallExpression) new MethodCallExpression2(method, arg0, arg1);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ContractUtils.RequiresNotNull((object) arg0, "arg0");
      ContractUtils.RequiresNotNull((object) arg1, "arg1");
      ContractUtils.RequiresNotNull((object) arg2, "arg2");
      ParameterInfo[] parameters = Expression.ValidateMethodAndGetParameters((Expression) null, method);
      Expression.ValidateArgumentCount((MethodBase) method, ExpressionType.Call, 3, parameters);
      arg0 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg0, parameters[0]);
      arg1 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg1, parameters[1]);
      arg2 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg2, parameters[2]);
      return (MethodCallExpression) new MethodCallExpression3(method, arg0, arg1, arg2);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ContractUtils.RequiresNotNull((object) arg0, "arg0");
      ContractUtils.RequiresNotNull((object) arg1, "arg1");
      ContractUtils.RequiresNotNull((object) arg2, "arg2");
      ContractUtils.RequiresNotNull((object) arg3, "arg3");
      ParameterInfo[] parameters = Expression.ValidateMethodAndGetParameters((Expression) null, method);
      Expression.ValidateArgumentCount((MethodBase) method, ExpressionType.Call, 4, parameters);
      arg0 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg0, parameters[0]);
      arg1 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg1, parameters[1]);
      arg2 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg2, parameters[2]);
      arg3 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg3, parameters[3]);
      return (MethodCallExpression) new MethodCallExpression4(method, arg0, arg1, arg2, arg3);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ContractUtils.RequiresNotNull((object) arg0, "arg0");
      ContractUtils.RequiresNotNull((object) arg1, "arg1");
      ContractUtils.RequiresNotNull((object) arg2, "arg2");
      ContractUtils.RequiresNotNull((object) arg3, "arg3");
      ContractUtils.RequiresNotNull((object) arg4, "arg4");
      ParameterInfo[] parameters = Expression.ValidateMethodAndGetParameters((Expression) null, method);
      Expression.ValidateArgumentCount((MethodBase) method, ExpressionType.Call, 5, parameters);
      arg0 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg0, parameters[0]);
      arg1 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg1, parameters[1]);
      arg2 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg2, parameters[2]);
      arg3 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg3, parameters[3]);
      arg4 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg4, parameters[4]);
      return (MethodCallExpression) new MethodCallExpression5(method, arg0, arg1, arg2, arg3, arg4);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MethodCallExpression Call(MethodInfo method, params Expression[] arguments)
    {
      return Expression.Call((Expression) null, method, arguments);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MethodCallExpression Call(MethodInfo method, IEnumerable<Expression> arguments)
    {
      return Expression.Call((Expression) null, method, arguments);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(Expression instance, MethodInfo method)
    {
      return Expression.Call(instance, method, (IEnumerable<Expression>) EmptyReadOnlyCollection<Expression>.Instance);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(Expression instance, MethodInfo method, params Expression[] arguments)
    {
      return Expression.Call(instance, method, (IEnumerable<Expression>) arguments);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(Expression instance, MethodInfo method, Expression arg0, Expression arg1)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ContractUtils.RequiresNotNull((object) arg0, "arg0");
      ContractUtils.RequiresNotNull((object) arg1, "arg1");
      ParameterInfo[] parameters = Expression.ValidateMethodAndGetParameters(instance, method);
      Expression.ValidateArgumentCount((MethodBase) method, ExpressionType.Call, 2, parameters);
      arg0 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg0, parameters[0]);
      arg1 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg1, parameters[1]);
      if (instance != null)
        return (MethodCallExpression) new InstanceMethodCallExpression2(method, instance, arg0, arg1);
      else
        return (MethodCallExpression) new MethodCallExpression2(method, arg0, arg1);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(Expression instance, MethodInfo method, Expression arg0, Expression arg1, Expression arg2)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ContractUtils.RequiresNotNull((object) arg0, "arg0");
      ContractUtils.RequiresNotNull((object) arg1, "arg1");
      ContractUtils.RequiresNotNull((object) arg2, "arg2");
      ParameterInfo[] parameters = Expression.ValidateMethodAndGetParameters(instance, method);
      Expression.ValidateArgumentCount((MethodBase) method, ExpressionType.Call, 3, parameters);
      arg0 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg0, parameters[0]);
      arg1 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg1, parameters[1]);
      arg2 = Expression.ValidateOneArgument((MethodBase) method, ExpressionType.Call, arg2, parameters[2]);
      if (instance != null)
        return (MethodCallExpression) new InstanceMethodCallExpression3(method, instance, arg0, arg1, arg2);
      else
        return (MethodCallExpression) new MethodCallExpression3(method, arg0, arg1, arg2);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(Expression instance, string methodName, Type[] typeArguments, params Expression[] arguments)
    {
      ContractUtils.RequiresNotNull((object) instance, "instance");
      ContractUtils.RequiresNotNull((object) methodName, "methodName");
      if (arguments == null)
        arguments = new Expression[0];
      BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
      return Expression.Call(instance, Expression.FindMethod(instance.Type, methodName, typeArguments, arguments, flags), arguments);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(Type type, string methodName, Type[] typeArguments, params Expression[] arguments)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      ContractUtils.RequiresNotNull((object) methodName, "methodName");
      if (arguments == null)
        arguments = new Expression[0];
      BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
      return Expression.Call((Expression) null, Expression.FindMethod(type, methodName, typeArguments, arguments, flags), arguments);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression Call(Expression instance, MethodInfo method, IEnumerable<Expression> arguments)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ReadOnlyCollection<Expression> arguments1 = CollectionExtensions.ToReadOnly<Expression>(arguments);
      Expression.ValidateMethodInfo(method);
      Expression.ValidateStaticOrInstanceMethod(instance, method);
      Expression.ValidateArgumentTypes((MethodBase) method, ExpressionType.Call, ref arguments1);
      if (instance == null)
        return (MethodCallExpression) new MethodCallExpressionN(method, (IList<Expression>) arguments1);
      else
        return (MethodCallExpression) new InstanceMethodCallExpressionN(method, instance, (IList<Expression>) arguments1);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression ArrayIndex(Expression array, params Expression[] indexes)
    {
      return Expression.ArrayIndex(array, (IEnumerable<Expression>) indexes);
    }

    [__DynamicallyInvokable]
    public static MethodCallExpression ArrayIndex(Expression array, IEnumerable<Expression> indexes)
    {
      Expression.RequiresCanRead(array, "array");
      ContractUtils.RequiresNotNull((object) indexes, "indexes");
      Type type = array.Type;
      if (!type.IsArray)
        throw Error.ArgumentMustBeArray();
      ReadOnlyCollection<Expression> readOnlyCollection = CollectionExtensions.ToReadOnly<Expression>(indexes);
      if (type.GetArrayRank() != readOnlyCollection.Count)
        throw Error.IncorrectNumberOfIndexes();
      foreach (Expression expression in readOnlyCollection)
      {
        Expression.RequiresCanRead(expression, "indexes");
        if (expression.Type != typeof (int))
          throw Error.ArgumentMustBeArrayIndexType();
      }
      MethodInfo method = array.Type.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public);
      return Expression.Call(array, method, (IEnumerable<Expression>) readOnlyCollection);
    }

    [__DynamicallyInvokable]
    public static NewArrayExpression NewArrayInit(Type type, params Expression[] initializers)
    {
      return Expression.NewArrayInit(type, (IEnumerable<Expression>) initializers);
    }

    [__DynamicallyInvokable]
    public static NewArrayExpression NewArrayInit(Type type, IEnumerable<Expression> initializers)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      ContractUtils.RequiresNotNull((object) initializers, "initializers");
      if (type.Equals(typeof (void)))
        throw Error.ArgumentCannotBeOfTypeVoid();
      ReadOnlyCollection<Expression> expressions = CollectionExtensions.ToReadOnly<Expression>(initializers);
      Expression[] list = (Expression[]) null;
      int index1 = 0;
      for (int count = expressions.Count; index1 < count; ++index1)
      {
        Expression expression = expressions[index1];
        Expression.RequiresCanRead(expression, "initializers");
        if (!TypeUtils.AreReferenceAssignable(type, expression.Type))
        {
          if (!Expression.TryQuote(type, ref expression))
            throw Error.ExpressionTypeCannotInitializeArrayType((object) expression.Type, (object) type);
          if (list == null)
          {
            list = new Expression[expressions.Count];
            for (int index2 = 0; index2 < index1; ++index2)
              list[index2] = expressions[index2];
          }
        }
        if (list != null)
          list[index1] = expression;
      }
      if (list != null)
        expressions = (ReadOnlyCollection<Expression>) new TrueReadOnlyCollection<Expression>(list);
      return NewArrayExpression.Make(ExpressionType.NewArrayInit, type.MakeArrayType(), expressions);
    }

    [__DynamicallyInvokable]
    public static NewArrayExpression NewArrayBounds(Type type, params Expression[] bounds)
    {
      return Expression.NewArrayBounds(type, (IEnumerable<Expression>) bounds);
    }

    [__DynamicallyInvokable]
    public static NewArrayExpression NewArrayBounds(Type type, IEnumerable<Expression> bounds)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      ContractUtils.RequiresNotNull((object) bounds, "bounds");
      if (type.Equals(typeof (void)))
        throw Error.ArgumentCannotBeOfTypeVoid();
      ReadOnlyCollection<Expression> readOnlyCollection = CollectionExtensions.ToReadOnly<Expression>(bounds);
      int count = readOnlyCollection.Count;
      if (count <= 0)
        throw Error.BoundsCannotBeLessThanOne();
      for (int index = 0; index < count; ++index)
      {
        Expression expression = readOnlyCollection[index];
        Expression.RequiresCanRead(expression, "bounds");
        if (!TypeUtils.IsInteger(expression.Type))
          throw Error.ArgumentMustBeInteger();
      }
      return NewArrayExpression.Make(ExpressionType.NewArrayBounds, count != 1 ? type.MakeArrayType(count) : type.MakeArrayType(), CollectionExtensions.ToReadOnly<Expression>(bounds));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static NewExpression New(ConstructorInfo constructor)
    {
      return Expression.New(constructor, (IEnumerable<Expression>) null);
    }

    [__DynamicallyInvokable]
    public static NewExpression New(ConstructorInfo constructor, params Expression[] arguments)
    {
      return Expression.New(constructor, (IEnumerable<Expression>) arguments);
    }

    [__DynamicallyInvokable]
    public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments)
    {
      ContractUtils.RequiresNotNull((object) constructor, "constructor");
      ContractUtils.RequiresNotNull((object) constructor.DeclaringType, "constructor.DeclaringType");
      TypeUtils.ValidateType(constructor.DeclaringType);
      ReadOnlyCollection<Expression> arguments1 = CollectionExtensions.ToReadOnly<Expression>(arguments);
      Expression.ValidateArgumentTypes((MethodBase) constructor, ExpressionType.New, ref arguments1);
      return new NewExpression(constructor, (IList<Expression>) arguments1, (ReadOnlyCollection<MemberInfo>) null);
    }

    [__DynamicallyInvokable]
    public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments, IEnumerable<MemberInfo> members)
    {
      ContractUtils.RequiresNotNull((object) constructor, "constructor");
      ReadOnlyCollection<MemberInfo> members1 = CollectionExtensions.ToReadOnly<MemberInfo>(members);
      ReadOnlyCollection<Expression> arguments1 = CollectionExtensions.ToReadOnly<Expression>(arguments);
      Expression.ValidateNewArgs(constructor, ref arguments1, ref members1);
      return new NewExpression(constructor, (IList<Expression>) arguments1, members1);
    }

    [__DynamicallyInvokable]
    public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments, params MemberInfo[] members)
    {
      return Expression.New(constructor, arguments, (IEnumerable<MemberInfo>) members);
    }

    [__DynamicallyInvokable]
    public static NewExpression New(Type type)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      if (type == typeof (void))
        throw Error.ArgumentCannotBeOfTypeVoid();
      if (type.IsValueType)
        return (NewExpression) new NewValueTypeExpression(type, EmptyReadOnlyCollection<Expression>.Instance, (ReadOnlyCollection<MemberInfo>) null);
      ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
      if (constructor == (ConstructorInfo) null)
        throw Error.TypeMissingDefaultConstructor((object) type);
      else
        return Expression.New(constructor);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParameterExpression Parameter(Type type)
    {
      return Expression.Parameter(type, (string) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParameterExpression Variable(Type type)
    {
      return Expression.Variable(type, (string) null);
    }

    [__DynamicallyInvokable]
    public static ParameterExpression Parameter(Type type, string name)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      if (type == typeof (void))
        throw Error.ArgumentCannotBeOfTypeVoid();
      bool isByRef = type.IsByRef;
      if (isByRef)
        type = type.GetElementType();
      return ParameterExpression.Make(type, name, isByRef);
    }

    [__DynamicallyInvokable]
    public static ParameterExpression Variable(Type type, string name)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      if (type == typeof (void))
        throw Error.ArgumentCannotBeOfTypeVoid();
      if (type.IsByRef)
        throw Error.TypeMustNotBeByRef();
      else
        return ParameterExpression.Make(type, name, false);
    }

    [__DynamicallyInvokable]
    public static RuntimeVariablesExpression RuntimeVariables(params ParameterExpression[] variables)
    {
      return Expression.RuntimeVariables((IEnumerable<ParameterExpression>) variables);
    }

    [__DynamicallyInvokable]
    public static RuntimeVariablesExpression RuntimeVariables(IEnumerable<ParameterExpression> variables)
    {
      ContractUtils.RequiresNotNull((object) variables, "variables");
      ReadOnlyCollection<ParameterExpression> variables1 = CollectionExtensions.ToReadOnly<ParameterExpression>(variables);
      for (int index = 0; index < variables1.Count; ++index)
      {
        if ((Expression) variables1[index] == null)
          throw new ArgumentNullException("variables[" + (object) index + "]");
      }
      return new RuntimeVariablesExpression(variables1);
    }

    [__DynamicallyInvokable]
    public static SwitchCase SwitchCase(Expression body, params Expression[] testValues)
    {
      return Expression.SwitchCase(body, (IEnumerable<Expression>) testValues);
    }

    [__DynamicallyInvokable]
    public static SwitchCase SwitchCase(Expression body, IEnumerable<Expression> testValues)
    {
      Expression.RequiresCanRead(body, "body");
      ReadOnlyCollection<Expression> testValues1 = CollectionExtensions.ToReadOnly<Expression>(testValues);
      Expression.RequiresCanRead((IEnumerable<Expression>) testValues1, "testValues");
      ContractUtils.RequiresNotEmpty<Expression>((ICollection<Expression>) testValues1, "testValues");
      return new SwitchCase(body, testValues1);
    }

    [__DynamicallyInvokable]
    public static SwitchExpression Switch(Expression switchValue, params SwitchCase[] cases)
    {
      return Expression.Switch(switchValue, (Expression) null, (MethodInfo) null, (IEnumerable<SwitchCase>) cases);
    }

    [__DynamicallyInvokable]
    public static SwitchExpression Switch(Expression switchValue, Expression defaultBody, params SwitchCase[] cases)
    {
      return Expression.Switch(switchValue, defaultBody, (MethodInfo) null, (IEnumerable<SwitchCase>) cases);
    }

    [__DynamicallyInvokable]
    public static SwitchExpression Switch(Expression switchValue, Expression defaultBody, MethodInfo comparison, params SwitchCase[] cases)
    {
      return Expression.Switch(switchValue, defaultBody, comparison, (IEnumerable<SwitchCase>) cases);
    }

    [__DynamicallyInvokable]
    public static SwitchExpression Switch(Type type, Expression switchValue, Expression defaultBody, MethodInfo comparison, params SwitchCase[] cases)
    {
      return Expression.Switch(type, switchValue, defaultBody, comparison, (IEnumerable<SwitchCase>) cases);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static SwitchExpression Switch(Expression switchValue, Expression defaultBody, MethodInfo comparison, IEnumerable<SwitchCase> cases)
    {
      return Expression.Switch((Type) null, switchValue, defaultBody, comparison, cases);
    }

    [__DynamicallyInvokable]
    public static SwitchExpression Switch(Type type, Expression switchValue, Expression defaultBody, MethodInfo comparison, IEnumerable<SwitchCase> cases)
    {
      Expression.RequiresCanRead(switchValue, "switchValue");
      if (switchValue.Type == typeof (void))
        throw Error.ArgumentCannotBeOfTypeVoid();
      ReadOnlyCollection<SwitchCase> cases1 = CollectionExtensions.ToReadOnly<SwitchCase>(cases);
      ContractUtils.RequiresNotEmpty<SwitchCase>((ICollection<SwitchCase>) cases1, "cases");
      ContractUtils.RequiresNotNullItems<SwitchCase>((IList<SwitchCase>) cases1, "cases");
      Type type1 = type ?? cases1[0].Body.Type;
      bool customType = type != (Type) null;
      if (comparison != (MethodInfo) null)
      {
        ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) comparison);
        if (parametersCached.Length != 2)
          throw Error.IncorrectNumberOfMethodCallArguments((object) comparison);
        ParameterInfo pi1 = parametersCached[0];
        bool flag = false;
        if (!Expression.ParameterIsAssignable(pi1, switchValue.Type))
        {
          flag = Expression.ParameterIsAssignable(pi1, TypeUtils.GetNonNullableType(switchValue.Type));
          if (!flag)
            throw Error.SwitchValueTypeDoesNotMatchComparisonMethodParameter((object) switchValue.Type, (object) pi1.ParameterType);
        }
        ParameterInfo pi2 = parametersCached[1];
        foreach (SwitchCase switchCase in cases1)
        {
          ContractUtils.RequiresNotNull((object) switchCase, "cases");
          Expression.ValidateSwitchCaseType(switchCase.Body, customType, type1, "cases");
          for (int index = 0; index < switchCase.TestValues.Count; ++index)
          {
            Type type2 = switchCase.TestValues[index].Type;
            if (flag)
            {
              if (!TypeUtils.IsNullableType(type2))
                throw Error.TestValueTypeDoesNotMatchComparisonMethodParameter((object) type2, (object) pi2.ParameterType);
              type2 = TypeUtils.GetNonNullableType(type2);
            }
            if (!Expression.ParameterIsAssignable(pi2, type2))
              throw Error.TestValueTypeDoesNotMatchComparisonMethodParameter((object) type2, (object) pi2.ParameterType);
          }
        }
      }
      else
      {
        Expression right = cases1[0].TestValues[0];
        foreach (SwitchCase switchCase in cases1)
        {
          ContractUtils.RequiresNotNull((object) switchCase, "cases");
          Expression.ValidateSwitchCaseType(switchCase.Body, customType, type1, "cases");
          for (int index = 0; index < switchCase.TestValues.Count; ++index)
          {
            if (!TypeUtils.AreEquivalent(right.Type, switchCase.TestValues[index].Type))
              throw new ArgumentException(Strings.AllTestValuesMustHaveSameType, "cases");
          }
        }
        comparison = Expression.Equal(switchValue, right, false, comparison).Method;
      }
      if (defaultBody == null)
      {
        if (type1 != typeof (void))
          throw Error.DefaultBodyMustBeSupplied();
      }
      else
        Expression.ValidateSwitchCaseType(defaultBody, customType, type1, "defaultBody");
      if (comparison != (MethodInfo) null && comparison.ReturnType != typeof (bool))
        throw Error.EqualityMustReturnBoolean((object) comparison);
      else
        return new SwitchExpression(type1, switchValue, defaultBody, comparison, cases1);
    }

    [__DynamicallyInvokable]
    public static SymbolDocumentInfo SymbolDocument(string fileName)
    {
      return new SymbolDocumentInfo(fileName);
    }

    [__DynamicallyInvokable]
    public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language)
    {
      return (SymbolDocumentInfo) new SymbolDocumentWithGuids(fileName, ref language);
    }

    [__DynamicallyInvokable]
    public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language, Guid languageVendor)
    {
      return (SymbolDocumentInfo) new SymbolDocumentWithGuids(fileName, ref language, ref languageVendor);
    }

    [__DynamicallyInvokable]
    public static SymbolDocumentInfo SymbolDocument(string fileName, Guid language, Guid languageVendor, Guid documentType)
    {
      return (SymbolDocumentInfo) new SymbolDocumentWithGuids(fileName, ref language, ref languageVendor, ref documentType);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static TryExpression TryFault(Expression body, Expression fault)
    {
      return Expression.MakeTry((Type) null, body, (Expression) null, fault, (IEnumerable<CatchBlock>) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static TryExpression TryFinally(Expression body, Expression @finally)
    {
      return Expression.MakeTry((Type) null, body, @finally, (Expression) null, (IEnumerable<CatchBlock>) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static TryExpression TryCatch(Expression body, params CatchBlock[] handlers)
    {
      return Expression.MakeTry((Type) null, body, (Expression) null, (Expression) null, (IEnumerable<CatchBlock>) handlers);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static TryExpression TryCatchFinally(Expression body, Expression @finally, params CatchBlock[] handlers)
    {
      return Expression.MakeTry((Type) null, body, @finally, (Expression) null, (IEnumerable<CatchBlock>) handlers);
    }

    [__DynamicallyInvokable]
    public static TryExpression MakeTry(Type type, Expression body, Expression @finally, Expression fault, IEnumerable<CatchBlock> handlers)
    {
      Expression.RequiresCanRead(body, "body");
      ReadOnlyCollection<CatchBlock> handlers1 = CollectionExtensions.ToReadOnly<CatchBlock>(handlers);
      ContractUtils.RequiresNotNullItems<CatchBlock>((IList<CatchBlock>) handlers1, "handlers");
      Expression.ValidateTryAndCatchHaveSameType(type, body, handlers1);
      if (fault != null)
      {
        if (@finally != null || handlers1.Count > 0)
          throw Error.FaultCannotHaveCatchOrFinally();
        Expression.RequiresCanRead(fault, "fault");
      }
      else if (@finally != null)
        Expression.RequiresCanRead(@finally, "finally");
      else if (handlers1.Count == 0)
        throw Error.TryMustHaveCatchFinallyOrFault();
      return new TryExpression(type ?? body.Type, body, @finally, fault, handlers1);
    }

    [__DynamicallyInvokable]
    public static TypeBinaryExpression TypeIs(Expression expression, Type type)
    {
      Expression.RequiresCanRead(expression, "expression");
      ContractUtils.RequiresNotNull((object) type, "type");
      if (type.IsByRef)
        throw Error.TypeMustNotBeByRef();
      else
        return new TypeBinaryExpression(expression, type, ExpressionType.TypeIs);
    }

    [__DynamicallyInvokable]
    public static TypeBinaryExpression TypeEqual(Expression expression, Type type)
    {
      Expression.RequiresCanRead(expression, "expression");
      ContractUtils.RequiresNotNull((object) type, "type");
      if (type.IsByRef)
        throw Error.TypeMustNotBeByRef();
      else
        return new TypeBinaryExpression(expression, type, ExpressionType.TypeEqual);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type)
    {
      return Expression.MakeUnary(unaryType, operand, type, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type, MethodInfo method)
    {
      switch (unaryType)
      {
        case ExpressionType.Increment:
          return Expression.Increment(operand, method);
        case ExpressionType.Throw:
          return Expression.Throw(operand, type);
        case ExpressionType.Unbox:
          return Expression.Unbox(operand, type);
        case ExpressionType.PreIncrementAssign:
          return Expression.PreIncrementAssign(operand, method);
        case ExpressionType.PreDecrementAssign:
          return Expression.PreDecrementAssign(operand, method);
        case ExpressionType.PostIncrementAssign:
          return Expression.PostIncrementAssign(operand, method);
        case ExpressionType.PostDecrementAssign:
          return Expression.PostDecrementAssign(operand, method);
        case ExpressionType.OnesComplement:
          return Expression.OnesComplement(operand, method);
        case ExpressionType.IsTrue:
          return Expression.IsTrue(operand, method);
        case ExpressionType.IsFalse:
          return Expression.IsFalse(operand, method);
        case ExpressionType.TypeAs:
          return Expression.TypeAs(operand, type);
        case ExpressionType.Decrement:
          return Expression.Decrement(operand, method);
        case ExpressionType.Negate:
          return Expression.Negate(operand, method);
        case ExpressionType.UnaryPlus:
          return Expression.UnaryPlus(operand, method);
        case ExpressionType.NegateChecked:
          return Expression.NegateChecked(operand, method);
        case ExpressionType.Not:
          return Expression.Not(operand, method);
        case ExpressionType.Quote:
          return Expression.Quote(operand);
        case ExpressionType.ArrayLength:
          return Expression.ArrayLength(operand);
        case ExpressionType.Convert:
          return Expression.Convert(operand, type, method);
        case ExpressionType.ConvertChecked:
          return Expression.ConvertChecked(operand, type, method);
        default:
          throw Error.UnhandledUnary((object) unaryType);
      }
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression Negate(Expression expression)
    {
      return Expression.Negate(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Negate(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.Negate, expression, method);
      if (TypeUtils.IsArithmetic(expression.Type) && !TypeUtils.IsUnsignedInt(expression.Type))
        return new UnaryExpression(ExpressionType.Negate, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.Negate, "op_UnaryNegation", expression);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression UnaryPlus(Expression expression)
    {
      return Expression.UnaryPlus(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression UnaryPlus(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.UnaryPlus, expression, method);
      if (TypeUtils.IsArithmetic(expression.Type))
        return new UnaryExpression(ExpressionType.UnaryPlus, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.UnaryPlus, "op_UnaryPlus", expression);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression NegateChecked(Expression expression)
    {
      return Expression.NegateChecked(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression NegateChecked(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.NegateChecked, expression, method);
      if (TypeUtils.IsArithmetic(expression.Type) && !TypeUtils.IsUnsignedInt(expression.Type))
        return new UnaryExpression(ExpressionType.NegateChecked, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.NegateChecked, "op_UnaryNegation", expression);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression Not(Expression expression)
    {
      return Expression.Not(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Not(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.Not, expression, method);
      if (TypeUtils.IsIntegerOrBool(expression.Type))
        return new UnaryExpression(ExpressionType.Not, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperator(ExpressionType.Not, "op_LogicalNot", expression) ?? Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.Not, "op_OnesComplement", expression);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression IsFalse(Expression expression)
    {
      return Expression.IsFalse(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression IsFalse(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.IsFalse, expression, method);
      if (TypeUtils.IsBool(expression.Type))
        return new UnaryExpression(ExpressionType.IsFalse, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.IsFalse, "op_False", expression);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression IsTrue(Expression expression)
    {
      return Expression.IsTrue(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression IsTrue(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.IsTrue, expression, method);
      if (TypeUtils.IsBool(expression.Type))
        return new UnaryExpression(ExpressionType.IsTrue, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.IsTrue, "op_True", expression);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression OnesComplement(Expression expression)
    {
      return Expression.OnesComplement(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression OnesComplement(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.OnesComplement, expression, method);
      if (TypeUtils.IsInteger(expression.Type))
        return new UnaryExpression(ExpressionType.OnesComplement, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.OnesComplement, "op_OnesComplement", expression);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression TypeAs(Expression expression, Type type)
    {
      Expression.RequiresCanRead(expression, "expression");
      ContractUtils.RequiresNotNull((object) type, "type");
      TypeUtils.ValidateType(type);
      if (type.IsValueType && !TypeUtils.IsNullableType(type))
        throw Error.IncorrectTypeForTypeAs((object) type);
      else
        return new UnaryExpression(ExpressionType.TypeAs, expression, type, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Unbox(Expression expression, Type type)
    {
      Expression.RequiresCanRead(expression, "expression");
      ContractUtils.RequiresNotNull((object) type, "type");
      if (!expression.Type.IsInterface && expression.Type != typeof (object))
        throw Error.InvalidUnboxType();
      if (!type.IsValueType)
        throw Error.InvalidUnboxType();
      TypeUtils.ValidateType(type);
      return new UnaryExpression(ExpressionType.Unbox, expression, type, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression Convert(Expression expression, Type type)
    {
      return Expression.Convert(expression, type, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Convert(Expression expression, Type type, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      ContractUtils.RequiresNotNull((object) type, "type");
      TypeUtils.ValidateType(type);
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedCoercionOperator(ExpressionType.Convert, expression, type, method);
      if (TypeUtils.HasIdentityPrimitiveOrNullableConversion(expression.Type, type) || TypeUtils.HasReferenceConversion(expression.Type, type))
        return new UnaryExpression(ExpressionType.Convert, expression, type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedCoercionOrThrow(ExpressionType.Convert, expression, type);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression ConvertChecked(Expression expression, Type type)
    {
      return Expression.ConvertChecked(expression, type, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression ConvertChecked(Expression expression, Type type, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      ContractUtils.RequiresNotNull((object) type, "type");
      TypeUtils.ValidateType(type);
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedCoercionOperator(ExpressionType.ConvertChecked, expression, type, method);
      if (TypeUtils.HasIdentityPrimitiveOrNullableConversion(expression.Type, type))
        return new UnaryExpression(ExpressionType.ConvertChecked, expression, type, (MethodInfo) null);
      if (TypeUtils.HasReferenceConversion(expression.Type, type))
        return new UnaryExpression(ExpressionType.Convert, expression, type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedCoercionOrThrow(ExpressionType.ConvertChecked, expression, type);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression ArrayLength(Expression array)
    {
      ContractUtils.RequiresNotNull((object) array, "array");
      if (!array.Type.IsArray || !typeof (Array).IsAssignableFrom(array.Type))
        throw Error.ArgumentMustBeArray();
      if (array.Type.GetArrayRank() != 1)
        throw Error.ArgumentMustBeSingleDimensionalArrayType();
      else
        return new UnaryExpression(ExpressionType.ArrayLength, array, typeof (int), (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Quote(Expression expression)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(expression is LambdaExpression))
        throw Error.QuotedExpressionMustBeLambda();
      else
        return new UnaryExpression(ExpressionType.Quote, expression, expression.GetType(), (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression Rethrow()
    {
      return Expression.Throw((Expression) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression Rethrow(Type type)
    {
      return Expression.Throw((Expression) null, type);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Throw(Expression value)
    {
      return Expression.Throw(value, typeof (void));
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Throw(Expression value, Type type)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      TypeUtils.ValidateType(type);
      if (value != null)
      {
        Expression.RequiresCanRead(value, "value");
        if (value.Type.IsValueType)
          throw Error.ArgumentMustNotHaveValueType();
      }
      return new UnaryExpression(ExpressionType.Throw, value, type, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression Increment(Expression expression)
    {
      return Expression.Increment(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Increment(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.Increment, expression, method);
      if (TypeUtils.IsArithmetic(expression.Type))
        return new UnaryExpression(ExpressionType.Increment, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.Increment, "op_Increment", expression);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression Decrement(Expression expression)
    {
      return Expression.Decrement(expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    public static UnaryExpression Decrement(Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      if (!(method == (MethodInfo) null))
        return Expression.GetMethodBasedUnaryOperator(ExpressionType.Decrement, expression, method);
      if (TypeUtils.IsArithmetic(expression.Type))
        return new UnaryExpression(ExpressionType.Decrement, expression, expression.Type, (MethodInfo) null);
      else
        return Expression.GetUserDefinedUnaryOperatorOrThrow(ExpressionType.Decrement, "op_Decrement", expression);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression PreIncrementAssign(Expression expression)
    {
      return Expression.MakeOpAssignUnary(ExpressionType.PreIncrementAssign, expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression PreIncrementAssign(Expression expression, MethodInfo method)
    {
      return Expression.MakeOpAssignUnary(ExpressionType.PreIncrementAssign, expression, method);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression PreDecrementAssign(Expression expression)
    {
      return Expression.MakeOpAssignUnary(ExpressionType.PreDecrementAssign, expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression PreDecrementAssign(Expression expression, MethodInfo method)
    {
      return Expression.MakeOpAssignUnary(ExpressionType.PreDecrementAssign, expression, method);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression PostIncrementAssign(Expression expression)
    {
      return Expression.MakeOpAssignUnary(ExpressionType.PostIncrementAssign, expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression PostIncrementAssign(Expression expression, MethodInfo method)
    {
      return Expression.MakeOpAssignUnary(ExpressionType.PostIncrementAssign, expression, method);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression PostDecrementAssign(Expression expression)
    {
      return Expression.MakeOpAssignUnary(ExpressionType.PostDecrementAssign, expression, (MethodInfo) null);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static UnaryExpression PostDecrementAssign(Expression expression, MethodInfo method)
    {
      return Expression.MakeOpAssignUnary(ExpressionType.PostDecrementAssign, expression, method);
    }

    private static UnaryExpression MakeOpAssignUnary(ExpressionType kind, Expression expression, MethodInfo method)
    {
      Expression.RequiresCanRead(expression, "expression");
      Expression.RequiresCanWrite(expression, "expression");
      UnaryExpression unaryExpression;
      if (method == (MethodInfo) null)
      {
        if (TypeUtils.IsArithmetic(expression.Type))
          return new UnaryExpression(kind, expression, expression.Type, (MethodInfo) null);
        string name = kind == ExpressionType.PreIncrementAssign || kind == ExpressionType.PostIncrementAssign ? "op_Increment" : "op_Decrement";
        unaryExpression = Expression.GetUserDefinedUnaryOperatorOrThrow(kind, name, expression);
      }
      else
        unaryExpression = Expression.GetMethodBasedUnaryOperator(kind, expression, method);
      if (!TypeUtils.AreReferenceAssignable(expression.Type, unaryExpression.Type))
        throw Error.UserDefinedOpMustHaveValidReturnType((object) kind, (object) method.Name);
      else
        return unaryExpression;
    }

    private static BinaryExpression GetUserDefinedBinaryOperator(ExpressionType binaryType, string name, Expression left, Expression right, bool liftToNull)
    {
      MethodInfo definedBinaryOperator1 = Expression.GetUserDefinedBinaryOperator(binaryType, left.Type, right.Type, name);
      if (definedBinaryOperator1 != (MethodInfo) null)
        return (BinaryExpression) new MethodBinaryExpression(binaryType, left, right, definedBinaryOperator1.ReturnType, definedBinaryOperator1);
      if (TypeUtils.IsNullableType(left.Type) && TypeUtils.IsNullableType(right.Type))
      {
        Type nonNullableType1 = TypeUtils.GetNonNullableType(left.Type);
        Type nonNullableType2 = TypeUtils.GetNonNullableType(right.Type);
        MethodInfo definedBinaryOperator2 = Expression.GetUserDefinedBinaryOperator(binaryType, nonNullableType1, nonNullableType2, name);
        if (definedBinaryOperator2 != (MethodInfo) null && definedBinaryOperator2.ReturnType.IsValueType && !TypeUtils.IsNullableType(definedBinaryOperator2.ReturnType))
        {
          if (definedBinaryOperator2.ReturnType != typeof (bool) || liftToNull)
            return (BinaryExpression) new MethodBinaryExpression(binaryType, left, right, TypeUtils.GetNullableType(definedBinaryOperator2.ReturnType), definedBinaryOperator2);
          else
            return (BinaryExpression) new MethodBinaryExpression(binaryType, left, right, typeof (bool), definedBinaryOperator2);
        }
      }
      return (BinaryExpression) null;
    }

    private static BinaryExpression GetMethodBasedBinaryOperator(ExpressionType binaryType, Expression left, Expression right, MethodInfo method, bool liftToNull)
    {
      Expression.ValidateOperator(method);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method);
      if (parametersCached.Length != 2)
        throw Error.IncorrectNumberOfMethodCallArguments((object) method);
      if (Expression.ParameterIsAssignable(parametersCached[0], left.Type) && Expression.ParameterIsAssignable(parametersCached[1], right.Type))
      {
        Expression.ValidateParamswithOperandsOrThrow(parametersCached[0].ParameterType, left.Type, binaryType, method.Name);
        Expression.ValidateParamswithOperandsOrThrow(parametersCached[1].ParameterType, right.Type, binaryType, method.Name);
        return (BinaryExpression) new MethodBinaryExpression(binaryType, left, right, method.ReturnType, method);
      }
      else
      {
        if (!TypeUtils.IsNullableType(left.Type) || !TypeUtils.IsNullableType(right.Type) || (!Expression.ParameterIsAssignable(parametersCached[0], TypeUtils.GetNonNullableType(left.Type)) || !Expression.ParameterIsAssignable(parametersCached[1], TypeUtils.GetNonNullableType(right.Type))) || (!method.ReturnType.IsValueType || TypeUtils.IsNullableType(method.ReturnType)))
          throw Error.OperandTypesDoNotMatchParameters((object) binaryType, (object) method.Name);
        if (method.ReturnType != typeof (bool) || liftToNull)
          return (BinaryExpression) new MethodBinaryExpression(binaryType, left, right, TypeUtils.GetNullableType(method.ReturnType), method);
        else
          return (BinaryExpression) new MethodBinaryExpression(binaryType, left, right, typeof (bool), method);
      }
    }

    private static BinaryExpression GetMethodBasedAssignOperator(ExpressionType binaryType, Expression left, Expression right, MethodInfo method, LambdaExpression conversion, bool liftToNull)
    {
      BinaryExpression binaryExpression = Expression.GetMethodBasedBinaryOperator(binaryType, left, right, method, liftToNull);
      if (conversion == null)
      {
        if (!TypeUtils.AreReferenceAssignable(left.Type, binaryExpression.Type))
          throw Error.UserDefinedOpMustHaveValidReturnType((object) binaryType, (object) binaryExpression.Method.Name);
      }
      else
      {
        Expression.ValidateOpAssignConversionLambda(conversion, binaryExpression.Left, binaryExpression.Method, binaryExpression.NodeType);
        binaryExpression = (BinaryExpression) new OpAssignMethodConversionBinaryExpression(binaryExpression.NodeType, binaryExpression.Left, binaryExpression.Right, binaryExpression.Left.Type, binaryExpression.Method, conversion);
      }
      return binaryExpression;
    }

    private static BinaryExpression GetUserDefinedBinaryOperatorOrThrow(ExpressionType binaryType, string name, Expression left, Expression right, bool liftToNull)
    {
      BinaryExpression definedBinaryOperator = Expression.GetUserDefinedBinaryOperator(binaryType, name, left, right, liftToNull);
      if (definedBinaryOperator == null)
        throw Error.BinaryOperatorNotDefined((object) binaryType, (object) left.Type, (object) right.Type);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) definedBinaryOperator.Method);
      Expression.ValidateParamswithOperandsOrThrow(parametersCached[0].ParameterType, left.Type, binaryType, name);
      Expression.ValidateParamswithOperandsOrThrow(parametersCached[1].ParameterType, right.Type, binaryType, name);
      return definedBinaryOperator;
    }

    private static BinaryExpression GetUserDefinedAssignOperatorOrThrow(ExpressionType binaryType, string name, Expression left, Expression right, LambdaExpression conversion, bool liftToNull)
    {
      BinaryExpression binaryExpression = Expression.GetUserDefinedBinaryOperatorOrThrow(binaryType, name, left, right, liftToNull);
      if (conversion == null)
      {
        if (!TypeUtils.AreReferenceAssignable(left.Type, binaryExpression.Type))
          throw Error.UserDefinedOpMustHaveValidReturnType((object) binaryType, (object) binaryExpression.Method.Name);
      }
      else
      {
        Expression.ValidateOpAssignConversionLambda(conversion, binaryExpression.Left, binaryExpression.Method, binaryExpression.NodeType);
        binaryExpression = (BinaryExpression) new OpAssignMethodConversionBinaryExpression(binaryExpression.NodeType, binaryExpression.Left, binaryExpression.Right, binaryExpression.Left.Type, binaryExpression.Method, conversion);
      }
      return binaryExpression;
    }

    private static MethodInfo GetUserDefinedBinaryOperator(ExpressionType binaryType, Type leftType, Type rightType, string name)
    {
      Type[] types = new Type[2]
      {
        leftType,
        rightType
      };
      Type nonNullableType1 = TypeUtils.GetNonNullableType(leftType);
      Type nonNullableType2 = TypeUtils.GetNonNullableType(rightType);
      BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
      MethodInfo method = TypeExtensions.GetMethodValidated(nonNullableType1, name, bindingAttr, (Binder) null, types, (ParameterModifier[]) null);
      if (method == (MethodInfo) null && !TypeUtils.AreEquivalent(leftType, rightType))
        method = TypeExtensions.GetMethodValidated(nonNullableType2, name, bindingAttr, (Binder) null, types, (ParameterModifier[]) null);
      if (Expression.IsLiftingConditionalLogicalOperator(leftType, rightType, method, binaryType))
        method = Expression.GetUserDefinedBinaryOperator(binaryType, nonNullableType1, nonNullableType2, name);
      return method;
    }

    private static bool IsLiftingConditionalLogicalOperator(Type left, Type right, MethodInfo method, ExpressionType binaryType)
    {
      if (!TypeUtils.IsNullableType(right) || !TypeUtils.IsNullableType(left) || !(method == (MethodInfo) null))
        return false;
      if (binaryType != ExpressionType.AndAlso)
        return binaryType == ExpressionType.OrElse;
      else
        return true;
    }

    internal static bool ParameterIsAssignable(ParameterInfo pi, Type argType)
    {
      Type dest = pi.ParameterType;
      if (dest.IsByRef)
        dest = dest.GetElementType();
      return TypeUtils.AreReferenceAssignable(dest, argType);
    }

    private static void ValidateParamswithOperandsOrThrow(Type paramType, Type operandType, ExpressionType exprType, string name)
    {
      if (TypeUtils.IsNullableType(paramType) && !TypeUtils.IsNullableType(operandType))
        throw Error.OperandTypesDoNotMatchParameters((object) exprType, (object) name);
    }

    private static void ValidateOperator(MethodInfo method)
    {
      Expression.ValidateMethodInfo(method);
      if (!method.IsStatic)
        throw Error.UserDefinedOperatorMustBeStatic((object) method);
      if (method.ReturnType == typeof (void))
        throw Error.UserDefinedOperatorMustNotBeVoid((object) method);
    }

    private static void ValidateMethodInfo(MethodInfo method)
    {
      if (method.IsGenericMethodDefinition)
        throw Error.MethodIsGeneric((object) method);
      if (method.ContainsGenericParameters)
        throw Error.MethodContainsGenericParameters((object) method);
    }

    private static bool IsNullComparison(Expression left, Expression right)
    {
      return Expression.IsNullConstant(left) && !Expression.IsNullConstant(right) && TypeUtils.IsNullableType(right.Type) || Expression.IsNullConstant(right) && !Expression.IsNullConstant(left) && TypeUtils.IsNullableType(left.Type);
    }

    private static bool IsNullConstant(Expression e)
    {
      ConstantExpression constantExpression = e as ConstantExpression;
      if (constantExpression != null)
        return constantExpression.Value == null;
      else
        return false;
    }

    private static void ValidateUserDefinedConditionalLogicOperator(ExpressionType nodeType, Type left, Type right, MethodInfo method)
    {
      Expression.ValidateOperator(method);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method);
      if (parametersCached.Length != 2)
        throw Error.IncorrectNumberOfMethodCallArguments((object) method);
      if (!Expression.ParameterIsAssignable(parametersCached[0], left) && (!TypeUtils.IsNullableType(left) || !Expression.ParameterIsAssignable(parametersCached[0], TypeUtils.GetNonNullableType(left))))
        throw Error.OperandTypesDoNotMatchParameters((object) nodeType, (object) method.Name);
      if (!Expression.ParameterIsAssignable(parametersCached[1], right) && (!TypeUtils.IsNullableType(right) || !Expression.ParameterIsAssignable(parametersCached[1], TypeUtils.GetNonNullableType(right))))
        throw Error.OperandTypesDoNotMatchParameters((object) nodeType, (object) method.Name);
      if (parametersCached[0].ParameterType != parametersCached[1].ParameterType)
        throw Error.UserDefinedOpMustHaveConsistentTypes((object) nodeType, (object) method.Name);
      if (method.ReturnType != parametersCached[0].ParameterType)
        throw Error.UserDefinedOpMustHaveConsistentTypes((object) nodeType, (object) method.Name);
      if (Expression.IsValidLiftedConditionalLogicalOperator(left, right, parametersCached))
      {
        left = TypeUtils.GetNonNullableType(left);
        right = TypeUtils.GetNonNullableType(left);
      }
      MethodInfo booleanOperator1 = TypeUtils.GetBooleanOperator(method.DeclaringType, "op_True");
      MethodInfo booleanOperator2 = TypeUtils.GetBooleanOperator(method.DeclaringType, "op_False");
      if (booleanOperator1 == (MethodInfo) null || booleanOperator1.ReturnType != typeof (bool) || (booleanOperator2 == (MethodInfo) null || booleanOperator2.ReturnType != typeof (bool)))
        throw Error.LogicalOperatorMustHaveBooleanOperators((object) nodeType, (object) method.Name);
      Expression.VerifyOpTrueFalse(nodeType, left, booleanOperator2);
      Expression.VerifyOpTrueFalse(nodeType, left, booleanOperator1);
    }

    private static void VerifyOpTrueFalse(ExpressionType nodeType, Type left, MethodInfo opTrue)
    {
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) opTrue);
      if (parametersCached.Length != 1)
        throw Error.IncorrectNumberOfMethodCallArguments((object) opTrue);
      if (!Expression.ParameterIsAssignable(parametersCached[0], left) && (!TypeUtils.IsNullableType(left) || !Expression.ParameterIsAssignable(parametersCached[0], TypeUtils.GetNonNullableType(left))))
        throw Error.OperandTypesDoNotMatchParameters((object) nodeType, (object) opTrue.Name);
    }

    private static bool IsValidLiftedConditionalLogicalOperator(Type left, Type right, ParameterInfo[] pms)
    {
      if (TypeUtils.AreEquivalent(left, right) && TypeUtils.IsNullableType(right))
        return TypeUtils.AreEquivalent(pms[1].ParameterType, TypeUtils.GetNonNullableType(right));
      else
        return false;
    }

    private static BinaryExpression GetEqualityComparisonOperator(ExpressionType binaryType, string opName, Expression left, Expression right, bool liftToNull)
    {
      if (left.Type == right.Type && (TypeUtils.IsNumeric(left.Type) || left.Type == typeof (object) || (TypeUtils.IsBool(left.Type) || TypeUtils.GetNonNullableType(left.Type).IsEnum)))
      {
        if (TypeUtils.IsNullableType(left.Type) && liftToNull)
          return (BinaryExpression) new SimpleBinaryExpression(binaryType, left, right, typeof (bool?));
        else
          return (BinaryExpression) new LogicalBinaryExpression(binaryType, left, right);
      }
      else
      {
        BinaryExpression definedBinaryOperator = Expression.GetUserDefinedBinaryOperator(binaryType, opName, left, right, liftToNull);
        if (definedBinaryOperator != null)
          return definedBinaryOperator;
        if (!TypeUtils.HasBuiltInEqualityOperator(left.Type, right.Type) && !Expression.IsNullComparison(left, right))
          throw Error.BinaryOperatorNotDefined((object) binaryType, (object) left.Type, (object) right.Type);
        if (TypeUtils.IsNullableType(left.Type) && liftToNull)
          return (BinaryExpression) new SimpleBinaryExpression(binaryType, left, right, typeof (bool?));
        else
          return (BinaryExpression) new LogicalBinaryExpression(binaryType, left, right);
      }
    }

    private static BinaryExpression GetComparisonOperator(ExpressionType binaryType, string opName, Expression left, Expression right, bool liftToNull)
    {
      if (!(left.Type == right.Type) || !TypeUtils.IsNumeric(left.Type))
        return Expression.GetUserDefinedBinaryOperatorOrThrow(binaryType, opName, left, right, liftToNull);
      if (TypeUtils.IsNullableType(left.Type) && liftToNull)
        return (BinaryExpression) new SimpleBinaryExpression(binaryType, left, right, typeof (bool?));
      else
        return (BinaryExpression) new LogicalBinaryExpression(binaryType, left, right);
    }

    private static Type ValidateCoalesceArgTypes(Type left, Type right)
    {
      Type nonNullableType = TypeUtils.GetNonNullableType(left);
      if (left.IsValueType && !TypeUtils.IsNullableType(left))
        throw Error.CoalesceUsedOnNonNullType();
      if (TypeUtils.IsNullableType(left) && TypeUtils.IsImplicitlyConvertible(right, nonNullableType))
        return nonNullableType;
      if (TypeUtils.IsImplicitlyConvertible(right, left))
        return left;
      if (TypeUtils.IsImplicitlyConvertible(nonNullableType, right))
        return right;
      else
        throw Error.ArgumentTypesMustMatch();
    }

    private static void ValidateOpAssignConversionLambda(LambdaExpression conversion, Expression left, MethodInfo method, ExpressionType nodeType)
    {
      MethodInfo method1 = conversion.Type.GetMethod("Invoke");
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method1);
      if (parametersCached.Length != 1)
        throw Error.IncorrectNumberOfMethodCallArguments((object) conversion);
      if (!TypeUtils.AreEquivalent(method1.ReturnType, left.Type))
        throw Error.OperandTypesDoNotMatchParameters((object) nodeType, (object) conversion.ToString());
      if (method != (MethodInfo) null && !TypeUtils.AreEquivalent(parametersCached[0].ParameterType, method.ReturnType))
        throw Error.OverloadOperatorTypeDoesNotMatchConversionType((object) nodeType, (object) conversion.ToString());
    }

    private static bool IsSimpleShift(Type left, Type right)
    {
      if (TypeUtils.IsInteger(left))
        return TypeUtils.GetNonNullableType(right) == typeof (int);
      else
        return false;
    }

    private static Type GetResultTypeOfShift(Type left, Type right)
    {
      if (TypeUtils.IsNullableType(left) || !TypeUtils.IsNullableType(right))
        return left;
      return typeof (Nullable<>).MakeGenericType(new Type[1]
      {
        left
      });
    }

    internal static void ValidateVariables(ReadOnlyCollection<ParameterExpression> varList, string collectionName)
    {
      if (varList.Count == 0)
        return;
      int count = varList.Count;
      Set<ParameterExpression> set = new Set<ParameterExpression>(count);
      for (int index = 0; index < count; ++index)
      {
        ParameterExpression parameterExpression = varList[index];
        if (parameterExpression == null)
        {
          throw new ArgumentNullException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}[{1}]", new object[2]
          {
            (object) collectionName,
            (object) set.Count
          }));
        }
        else
        {
          if (parameterExpression.IsByRef)
            throw Error.VariableMustNotBeByRef((object) parameterExpression, (object) parameterExpression.Type);
          if (set.Contains(parameterExpression))
            throw Error.DuplicateVariable((object) parameterExpression);
          set.Add(parameterExpression);
        }
      }
    }

    private static void ValidateSpan(int startLine, int startColumn, int endLine, int endColumn)
    {
      if (startLine < 1)
        throw Error.OutOfRange((object) "startLine", (object) 1);
      if (startColumn < 1)
        throw Error.OutOfRange((object) "startColumn", (object) 1);
      if (endLine < 1)
        throw Error.OutOfRange((object) "endLine", (object) 1);
      if (endColumn < 1)
        throw Error.OutOfRange((object) "endColumn", (object) 1);
      if (startLine > endLine)
        throw Error.StartEndMustBeOrdered();
      if (startLine == endLine && startColumn > endColumn)
        throw Error.StartEndMustBeOrdered();
    }

    private static MethodInfo GetValidMethodForDynamic(Type delegateType)
    {
      MethodInfo method = delegateType.GetMethod("Invoke");
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method);
      if (parametersCached.Length == 0 || parametersCached[0].ParameterType != typeof (CallSite))
        throw Error.FirstArgumentMustBeCallSite();
      else
        return method;
    }

    private static DynamicExpression MakeDynamic(CallSiteBinder binder, Type returnType, ReadOnlyCollection<Expression> args)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      for (int index = 0; index < args.Count; ++index)
        Expression.ValidateDynamicArgument(args[index]);
      Type delegateType = DelegateHelpers.MakeCallSiteDelegate(args, returnType);
      switch (args.Count)
      {
        case 1:
          return DynamicExpression.Make(returnType, delegateType, binder, args[0]);
        case 2:
          return DynamicExpression.Make(returnType, delegateType, binder, args[0], args[1]);
        case 3:
          return DynamicExpression.Make(returnType, delegateType, binder, args[0], args[1], args[2]);
        case 4:
          return DynamicExpression.Make(returnType, delegateType, binder, args[0], args[1], args[2], args[3]);
        default:
          return DynamicExpression.Make(returnType, delegateType, binder, args);
      }
    }

    private static void ValidateDynamicArgument(Expression arg)
    {
      Expression.RequiresCanRead(arg, "arguments");
      Type type = arg.Type;
      ContractUtils.RequiresNotNull((object) type, "type");
      TypeUtils.ValidateType(type);
      if (type == typeof (void))
        throw Error.ArgumentTypeCannotBeVoid();
    }

    private static void ValidateElementInitAddMethodInfo(MethodInfo addMethod)
    {
      Expression.ValidateMethodInfo(addMethod);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) addMethod);
      if (parametersCached.Length == 0)
        throw Error.ElementInitializerMethodWithZeroArgs();
      if (!addMethod.Name.Equals("Add", StringComparison.OrdinalIgnoreCase))
        throw Error.ElementInitializerMethodNotAdd();
      if (addMethod.IsStatic)
        throw Error.ElementInitializerMethodStatic();
      foreach (ParameterInfo parameterInfo in parametersCached)
      {
        if (parameterInfo.ParameterType.IsByRef)
          throw Error.ElementInitializerMethodNoRefOutParam((object) parameterInfo.Name, (object) addMethod.Name);
      }
    }

    internal static ReadOnlyCollection<Expression> ReturnReadOnly(IArgumentProvider provider, ref object collection)
    {
      Expression expression = collection as Expression;
      if (expression != null)
        Interlocked.CompareExchange(ref collection, (object) new ReadOnlyCollection<Expression>((IList<Expression>) new ListArgumentProvider(provider, expression)), (object) expression);
      return (ReadOnlyCollection<Expression>) collection;
    }

    private static void RequiresCanRead(Expression expression, string paramName)
    {
      if (expression == null)
        throw new ArgumentNullException(paramName);
      switch (expression.NodeType)
      {
        case ExpressionType.MemberAccess:
          MemberInfo member = ((MemberExpression) expression).Member;
          if (member.MemberType != MemberTypes.Property || ((PropertyInfo) member).CanRead)
            break;
          else
            throw new ArgumentException(Strings.ExpressionMustBeReadable, paramName);
        case ExpressionType.Index:
          IndexExpression indexExpression = (IndexExpression) expression;
          if (!(indexExpression.Indexer != (PropertyInfo) null) || indexExpression.Indexer.CanRead)
            break;
          else
            throw new ArgumentException(Strings.ExpressionMustBeReadable, paramName);
      }
    }

    private static void RequiresCanRead(IEnumerable<Expression> items, string paramName)
    {
      if (items == null)
        return;
      IList<Expression> list = items as IList<Expression>;
      if (list != null)
      {
        for (int index = 0; index < list.Count; ++index)
          Expression.RequiresCanRead(list[index], paramName);
      }
      else
      {
        foreach (Expression expression in items)
          Expression.RequiresCanRead(expression, paramName);
      }
    }

    private static void RequiresCanWrite(Expression expression, string paramName)
    {
      if (expression == null)
        throw new ArgumentNullException(paramName);
      bool flag = false;
      switch (expression.NodeType)
      {
        case ExpressionType.MemberAccess:
          MemberExpression memberExpression = (MemberExpression) expression;
          switch (memberExpression.Member.MemberType)
          {
            case MemberTypes.Field:
              FieldInfo fieldInfo = (FieldInfo) memberExpression.Member;
              flag = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;
              break;
            case MemberTypes.Property:
              flag = ((PropertyInfo) memberExpression.Member).CanWrite;
              break;
          }
        case ExpressionType.Parameter:
          flag = true;
          break;
        case ExpressionType.Index:
          IndexExpression indexExpression = (IndexExpression) expression;
          flag = !(indexExpression.Indexer != (PropertyInfo) null) || indexExpression.Indexer.CanWrite;
          break;
      }
      if (!flag)
        throw new ArgumentException(Strings.ExpressionMustBeWriteable, paramName);
    }

    private static void ValidateGoto(LabelTarget target, ref Expression value, string targetParameter, string valueParameter)
    {
      ContractUtils.RequiresNotNull((object) target, targetParameter);
      if (value == null)
      {
        if (target.Type != typeof (void))
          throw Error.LabelMustBeVoidOrHaveExpression();
      }
      else
        Expression.ValidateGotoType(target.Type, ref value, valueParameter);
    }

    private static void ValidateGotoType(Type expectedType, ref Expression value, string paramName)
    {
      Expression.RequiresCanRead(value, paramName);
      if (expectedType != typeof (void) && !TypeUtils.AreReferenceAssignable(expectedType, value.Type) && !Expression.TryQuote(expectedType, ref value))
        throw Error.ExpressionTypeDoesNotMatchLabel((object) value.Type, (object) expectedType);
    }

    private static PropertyInfo FindInstanceProperty(Type type, string propertyName, Expression[] arguments)
    {
      BindingFlags flags1 = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
      PropertyInfo property = Expression.FindProperty(type, propertyName, arguments, flags1);
      if (property == (PropertyInfo) null)
      {
        BindingFlags flags2 = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        property = Expression.FindProperty(type, propertyName, arguments, flags2);
      }
      if (!(property == (PropertyInfo) null))
        return property;
      if (arguments == null || arguments.Length == 0)
        throw Error.InstancePropertyWithoutParameterNotDefinedForType((object) propertyName, (object) type);
      else
        throw Error.InstancePropertyWithSpecifiedParametersNotDefinedForType((object) propertyName, (object) Expression.GetArgTypesString(arguments), (object) type);
    }

    private static string GetArgTypesString(Expression[] arguments)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      stringBuilder.Append("(");
      foreach (Type type in Enumerable.Select<Expression, Type>((IEnumerable<Expression>) arguments, (Func<Expression, Type>) (arg => arg.Type)))
      {
        if (!flag)
          stringBuilder.Append(", ");
        stringBuilder.Append(type.Name);
        flag = false;
      }
      stringBuilder.Append(")");
      return ((object) stringBuilder).ToString();
    }

    private static PropertyInfo FindProperty(Type type, string propertyName, Expression[] arguments, BindingFlags flags)
    {
      MemberInfo[] members = type.FindMembers(MemberTypes.Property, flags, Type.FilterNameIgnoreCase, (object) propertyName);
      if (members == null || members.Length == 0)
        return (PropertyInfo) null;
      PropertyInfo property;
      int bestProperty = Expression.FindBestProperty((IEnumerable<PropertyInfo>) CollectionExtensions.Map<MemberInfo, PropertyInfo>((ICollection<MemberInfo>) members, (Func<MemberInfo, PropertyInfo>) (t => (PropertyInfo) t)), arguments, out property);
      if (bestProperty == 0)
        return (PropertyInfo) null;
      if (bestProperty > 1)
        throw Error.PropertyWithMoreThanOneMatch((object) propertyName, (object) type);
      else
        return property;
    }

    private static int FindBestProperty(IEnumerable<PropertyInfo> properties, Expression[] args, out PropertyInfo property)
    {
      int num = 0;
      property = (PropertyInfo) null;
      foreach (PropertyInfo pi in properties)
      {
        if (pi != (PropertyInfo) null && Expression.IsCompatible(pi, args))
        {
          if (property == (PropertyInfo) null)
          {
            property = pi;
            num = 1;
          }
          else
            ++num;
        }
      }
      return num;
    }

    private static bool IsCompatible(PropertyInfo pi, Expression[] args)
    {
      MethodInfo methodInfo = pi.GetGetMethod(true);
      ParameterInfo[] parameterInfoArray;
      if (methodInfo != (MethodInfo) null)
      {
        parameterInfoArray = TypeExtensions.GetParametersCached((MethodBase) methodInfo);
      }
      else
      {
        methodInfo = pi.GetSetMethod(true);
        parameterInfoArray = CollectionExtensions.RemoveLast<ParameterInfo>(TypeExtensions.GetParametersCached((MethodBase) methodInfo));
      }
      if (methodInfo == (MethodInfo) null)
        return false;
      if (args == null)
        return parameterInfoArray.Length == 0;
      if (parameterInfoArray.Length != args.Length)
        return false;
      for (int index = 0; index < args.Length; ++index)
      {
        if (args[index] == null || !TypeUtils.AreReferenceAssignable(parameterInfoArray[index].ParameterType, args[index].Type))
          return false;
      }
      return true;
    }

    private static void ValidateIndexedProperty(Expression instance, PropertyInfo property, ref ReadOnlyCollection<Expression> argList)
    {
      ContractUtils.RequiresNotNull((object) property, "property");
      if (property.PropertyType.IsByRef)
        throw Error.PropertyCannotHaveRefType();
      if (property.PropertyType == typeof (void))
        throw Error.PropertyTypeCannotBeVoid();
      ParameterInfo[] indexes = (ParameterInfo[]) null;
      MethodInfo getMethod = property.GetGetMethod(true);
      if (getMethod != (MethodInfo) null)
      {
        indexes = TypeExtensions.GetParametersCached((MethodBase) getMethod);
        Expression.ValidateAccessor(instance, getMethod, indexes, ref argList);
      }
      MethodInfo setMethod = property.GetSetMethod(true);
      if (setMethod != (MethodInfo) null)
      {
        ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) setMethod);
        if (parametersCached.Length == 0)
          throw Error.SetterHasNoParams();
        Type parameterType = parametersCached[parametersCached.Length - 1].ParameterType;
        if (parameterType.IsByRef)
          throw Error.PropertyCannotHaveRefType();
        if (setMethod.ReturnType != typeof (void))
          throw Error.SetterMustBeVoid();
        if (property.PropertyType != parameterType)
          throw Error.PropertyTyepMustMatchSetter();
        if (getMethod != (MethodInfo) null)
        {
          if (getMethod.IsStatic ^ setMethod.IsStatic)
            throw Error.BothAccessorsMustBeStatic();
          if (indexes.Length != parametersCached.Length - 1)
            throw Error.IndexesOfSetGetMustMatch();
          for (int index = 0; index < indexes.Length; ++index)
          {
            if (indexes[index].ParameterType != parametersCached[index].ParameterType)
              throw Error.IndexesOfSetGetMustMatch();
          }
        }
        else
          Expression.ValidateAccessor(instance, setMethod, CollectionExtensions.RemoveLast<ParameterInfo>(parametersCached), ref argList);
      }
      if (getMethod == (MethodInfo) null && setMethod == (MethodInfo) null)
        throw Error.PropertyDoesNotHaveAccessor((object) property);
    }

    private static void ValidateAccessor(Expression instance, MethodInfo method, ParameterInfo[] indexes, ref ReadOnlyCollection<Expression> arguments)
    {
      ContractUtils.RequiresNotNull((object) arguments, "arguments");
      Expression.ValidateMethodInfo(method);
      if ((method.CallingConvention & CallingConventions.VarArgs) != (CallingConventions) 0)
        throw Error.AccessorsCannotHaveVarArgs();
      if (method.IsStatic)
      {
        if (instance != null)
          throw Error.OnlyStaticMethodsHaveNullInstance();
      }
      else
      {
        if (instance == null)
          throw Error.OnlyStaticMethodsHaveNullInstance();
        Expression.RequiresCanRead(instance, "instance");
        Expression.ValidateCallInstanceType(instance.Type, method);
      }
      Expression.ValidateAccessorArgumentTypes(method, indexes, ref arguments);
    }

    private static void ValidateAccessorArgumentTypes(MethodInfo method, ParameterInfo[] indexes, ref ReadOnlyCollection<Expression> arguments)
    {
      if (indexes.Length > 0)
      {
        if (indexes.Length != arguments.Count)
          throw Error.IncorrectNumberOfMethodCallArguments((object) method);
        Expression[] list = (Expression[]) null;
        int index1 = 0;
        for (int length = indexes.Length; index1 < length; ++index1)
        {
          Expression expression = arguments[index1];
          ParameterInfo parameterInfo = indexes[index1];
          Expression.RequiresCanRead(expression, "arguments");
          Type parameterType = parameterInfo.ParameterType;
          if (parameterType.IsByRef)
            throw Error.AccessorsCannotHaveByRefArgs();
          TypeUtils.ValidateType(parameterType);
          if (!TypeUtils.AreReferenceAssignable(parameterType, expression.Type) && !Expression.TryQuote(parameterType, ref expression))
            throw Error.ExpressionTypeDoesNotMatchMethodParameter((object) expression.Type, (object) parameterType, (object) method);
          if (list == null && expression != arguments[index1])
          {
            list = new Expression[arguments.Count];
            for (int index2 = 0; index2 < index1; ++index2)
              list[index2] = arguments[index2];
          }
          if (list != null)
            list[index1] = expression;
        }
        if (list == null)
          return;
        arguments = (ReadOnlyCollection<Expression>) new TrueReadOnlyCollection<Expression>(list);
      }
      else if (arguments.Count > 0)
        throw Error.IncorrectNumberOfMethodCallArguments((object) method);
    }

    internal static MethodInfo GetInvokeMethod(Expression expression)
    {
      Type type = expression.Type;
      if (!expression.Type.IsSubclassOf(typeof (MulticastDelegate)))
      {
        Type genericType = TypeUtils.FindGenericType(typeof (Expression<>), expression.Type);
        if (genericType == (Type) null)
          throw Error.ExpressionTypeNotInvocable((object) expression.Type);
        type = genericType.GetGenericArguments()[0];
      }
      return type.GetMethod("Invoke");
    }

    internal static LambdaExpression CreateLambda(Type delegateType, Expression body, string name, bool tailCall, ReadOnlyCollection<ParameterExpression> parameters)
    {
      CacheDict<Type, Expression.LambdaFactory> cacheDict = Expression._LambdaFactories;
      if (cacheDict == null)
        Expression._LambdaFactories = cacheDict = new CacheDict<Type, Expression.LambdaFactory>(50);
      MethodInfo method = (MethodInfo) null;
      Expression.LambdaFactory lambdaFactory;
      if (!cacheDict.TryGetValue(delegateType, out lambdaFactory))
      {
        method = typeof (Expression<>).MakeGenericType(new Type[1]
        {
          delegateType
        }).GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic);
        if (TypeUtils.CanCache(delegateType))
          cacheDict[delegateType] = lambdaFactory = (Expression.LambdaFactory) Delegate.CreateDelegate(typeof (Expression.LambdaFactory), method);
      }
      if (lambdaFactory != null)
        return lambdaFactory(body, name, tailCall, parameters);
      return (LambdaExpression) method.Invoke((object) null, new object[4]
      {
        (object) body,
        (object) name,
        (object) (bool) (tailCall ? 1 : 0),
        (object) parameters
      });
    }

    private static bool ValidateTryGetFuncActionArgs(Type[] typeArgs)
    {
      if (typeArgs == null)
        throw new ArgumentNullException("typeArgs");
      int index = 0;
      for (int length = typeArgs.Length; index < length; ++index)
      {
        Type type = typeArgs[index];
        if (type == (Type) null)
          throw new ArgumentNullException("typeArgs");
        if (type.IsByRef)
          return false;
      }
      return true;
    }

    private static void ValidateSettableFieldOrPropertyMember(MemberInfo member, out Type memberType)
    {
      FieldInfo fieldInfo = member as FieldInfo;
      if (fieldInfo == (FieldInfo) null)
      {
        PropertyInfo propertyInfo = member as PropertyInfo;
        if (propertyInfo == (PropertyInfo) null)
          throw Error.ArgumentMustBeFieldInfoOrPropertInfo();
        if (!propertyInfo.CanWrite)
          throw Error.PropertyDoesNotHaveSetter((object) propertyInfo);
        memberType = propertyInfo.PropertyType;
      }
      else
        memberType = fieldInfo.FieldType;
    }

    private static PropertyInfo GetProperty(MethodInfo mi)
    {
      foreach (PropertyInfo propertyInfo in mi.DeclaringType.GetProperties((BindingFlags) (48 | (mi.IsStatic ? 8 : 4))))
      {
        if (propertyInfo.CanRead && Expression.CheckMethod(mi, propertyInfo.GetGetMethod(true)) || propertyInfo.CanWrite && Expression.CheckMethod(mi, propertyInfo.GetSetMethod(true)))
          return propertyInfo;
      }
      throw Error.MethodNotPropertyAccessor((object) mi.DeclaringType, (object) mi.Name);
    }

    private static bool CheckMethod(MethodInfo method, MethodInfo propertyMethod)
    {
      if (method == propertyMethod)
        return true;
      Type declaringType = method.DeclaringType;
      return declaringType.IsInterface && method.Name == propertyMethod.Name && declaringType.GetMethod(method.Name) == propertyMethod;
    }

    private static void ValidateListInitArgs(Type listType, ReadOnlyCollection<ElementInit> initializers)
    {
      if (!typeof (IEnumerable).IsAssignableFrom(listType))
        throw Error.TypeNotIEnumerable((object) listType);
      int index = 0;
      for (int count = initializers.Count; index < count; ++index)
      {
        ElementInit elementInit = initializers[index];
        ContractUtils.RequiresNotNull((object) elementInit, "initializers");
        Expression.ValidateCallInstanceType(listType, elementInit.AddMethod);
      }
    }

    private static void ValidateGettableFieldOrPropertyMember(MemberInfo member, out Type memberType)
    {
      FieldInfo fieldInfo = member as FieldInfo;
      if (fieldInfo == (FieldInfo) null)
      {
        PropertyInfo propertyInfo = member as PropertyInfo;
        if (propertyInfo == (PropertyInfo) null)
          throw Error.ArgumentMustBeFieldInfoOrPropertInfo();
        if (!propertyInfo.CanRead)
          throw Error.PropertyDoesNotHaveGetter((object) propertyInfo);
        memberType = propertyInfo.PropertyType;
      }
      else
        memberType = fieldInfo.FieldType;
    }

    private static void ValidateMemberInitArgs(Type type, ReadOnlyCollection<MemberBinding> bindings)
    {
      int index = 0;
      for (int count = bindings.Count; index < count; ++index)
      {
        MemberBinding memberBinding = bindings[index];
        ContractUtils.RequiresNotNull((object) memberBinding, "bindings");
        if (!memberBinding.Member.DeclaringType.IsAssignableFrom(type))
          throw Error.NotAMemberOfType((object) memberBinding.Member.Name, (object) type);
      }
    }

    private static ParameterInfo[] ValidateMethodAndGetParameters(Expression instance, MethodInfo method)
    {
      Expression.ValidateMethodInfo(method);
      Expression.ValidateStaticOrInstanceMethod(instance, method);
      return Expression.GetParametersForValidation((MethodBase) method, ExpressionType.Call);
    }

    private static void ValidateStaticOrInstanceMethod(Expression instance, MethodInfo method)
    {
      if (method.IsStatic)
      {
        if (instance != null)
          throw new ArgumentException(Strings.OnlyStaticMethodsHaveNullInstance, "instance");
      }
      else
      {
        if (instance == null)
          throw new ArgumentException(Strings.OnlyStaticMethodsHaveNullInstance, "method");
        Expression.RequiresCanRead(instance, "instance");
        Expression.ValidateCallInstanceType(instance.Type, method);
      }
    }

    private static void ValidateCallInstanceType(Type instanceType, MethodInfo method)
    {
      if (!TypeUtils.IsValidInstanceType((MemberInfo) method, instanceType))
        throw Error.InstanceAndMethodTypeMismatch((object) method, (object) method.DeclaringType, (object) instanceType);
    }

    private static void ValidateArgumentTypes(MethodBase method, ExpressionType nodeKind, ref ReadOnlyCollection<Expression> arguments)
    {
      ParameterInfo[] parametersForValidation = Expression.GetParametersForValidation(method, nodeKind);
      Expression.ValidateArgumentCount(method, nodeKind, arguments.Count, parametersForValidation);
      Expression[] list = (Expression[]) null;
      int index1 = 0;
      for (int length = parametersForValidation.Length; index1 < length; ++index1)
      {
        Expression expression1 = arguments[index1];
        ParameterInfo pi = parametersForValidation[index1];
        Expression expression2 = Expression.ValidateOneArgument(method, nodeKind, expression1, pi);
        if (list == null && expression2 != arguments[index1])
        {
          list = new Expression[arguments.Count];
          for (int index2 = 0; index2 < index1; ++index2)
            list[index2] = arguments[index2];
        }
        if (list != null)
          list[index1] = expression2;
      }
      if (list == null)
        return;
      arguments = (ReadOnlyCollection<Expression>) new TrueReadOnlyCollection<Expression>(list);
    }

    private static ParameterInfo[] GetParametersForValidation(MethodBase method, ExpressionType nodeKind)
    {
      ParameterInfo[] array = TypeExtensions.GetParametersCached(method);
      if (nodeKind == ExpressionType.Dynamic)
        array = CollectionExtensions.RemoveFirst<ParameterInfo>(array);
      return array;
    }

    private static void ValidateArgumentCount(MethodBase method, ExpressionType nodeKind, int count, ParameterInfo[] pis)
    {
      if (pis.Length == count)
        return;
      switch (nodeKind)
      {
        case ExpressionType.New:
          throw Error.IncorrectNumberOfConstructorArguments();
        case ExpressionType.Dynamic:
        case ExpressionType.Call:
          throw Error.IncorrectNumberOfMethodCallArguments((object) method);
        case ExpressionType.Invoke:
          throw Error.IncorrectNumberOfLambdaArguments();
        default:
          throw ContractUtils.Unreachable;
      }
    }

    private static Expression ValidateOneArgument(MethodBase method, ExpressionType nodeKind, Expression arg, ParameterInfo pi)
    {
      Expression.RequiresCanRead(arg, "arguments");
      Type type = pi.ParameterType;
      if (type.IsByRef)
        type = type.GetElementType();
      TypeUtils.ValidateType(type);
      if (TypeUtils.AreReferenceAssignable(type, arg.Type) || Expression.TryQuote(type, ref arg))
        return arg;
      switch (nodeKind)
      {
        case ExpressionType.New:
          throw Error.ExpressionTypeDoesNotMatchConstructorParameter((object) arg.Type, (object) type);
        case ExpressionType.Dynamic:
        case ExpressionType.Call:
          throw Error.ExpressionTypeDoesNotMatchMethodParameter((object) arg.Type, (object) type, (object) method);
        case ExpressionType.Invoke:
          throw Error.ExpressionTypeDoesNotMatchParameter((object) arg.Type, (object) type);
        default:
          throw ContractUtils.Unreachable;
      }
    }

    private static bool TryQuote(Type parameterType, ref Expression argument)
    {
      if (!TypeUtils.IsSameOrSubclass(typeof (LambdaExpression), parameterType) || !parameterType.IsAssignableFrom(argument.GetType()))
        return false;
      argument = (Expression) Expression.Quote(argument);
      return true;
    }

    private static MethodInfo FindMethod(Type type, string methodName, Type[] typeArgs, Expression[] args, BindingFlags flags)
    {
      MemberInfo[] members = type.FindMembers(MemberTypes.Method, flags, Type.FilterNameIgnoreCase, (object) methodName);
      if (members == null || members.Length == 0)
        throw Error.MethodDoesNotExistOnType((object) methodName, (object) type);
      MethodInfo method;
      int bestMethod = Expression.FindBestMethod((IEnumerable<MethodInfo>) CollectionExtensions.Map<MemberInfo, MethodInfo>((ICollection<MemberInfo>) members, (Func<MemberInfo, MethodInfo>) (t => (MethodInfo) t)), typeArgs, args, out method);
      if (bestMethod == 0)
      {
        if (typeArgs != null && typeArgs.Length > 0)
          throw Error.GenericMethodWithArgsDoesNotExistOnType((object) methodName, (object) type);
        else
          throw Error.MethodWithArgsDoesNotExistOnType((object) methodName, (object) type);
      }
      else if (bestMethod > 1)
        throw Error.MethodWithMoreThanOneMatch((object) methodName, (object) type);
      else
        return method;
    }

    private static int FindBestMethod(IEnumerable<MethodInfo> methods, Type[] typeArgs, Expression[] args, out MethodInfo method)
    {
      int num = 0;
      method = (MethodInfo) null;
      foreach (MethodInfo m in methods)
      {
        MethodInfo methodInfo = Expression.ApplyTypeArgs(m, typeArgs);
        if (methodInfo != (MethodInfo) null && Expression.IsCompatible((MethodBase) methodInfo, args))
        {
          if (method == (MethodInfo) null || !method.IsPublic && methodInfo.IsPublic)
          {
            method = methodInfo;
            num = 1;
          }
          else if (method.IsPublic == methodInfo.IsPublic)
            ++num;
        }
      }
      return num;
    }

    private static bool IsCompatible(MethodBase m, Expression[] args)
    {
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached(m);
      if (parametersCached.Length != args.Length)
        return false;
      for (int index = 0; index < args.Length; ++index)
      {
        Expression expression = args[index];
        ContractUtils.RequiresNotNull((object) expression, "argument");
        Type type1 = expression.Type;
        Type type2 = parametersCached[index].ParameterType;
        if (type2.IsByRef)
          type2 = type2.GetElementType();
        if (!TypeUtils.AreReferenceAssignable(type2, type1) && (!TypeUtils.IsSameOrSubclass(typeof (LambdaExpression), type2) || !type2.IsAssignableFrom(expression.GetType())))
          return false;
      }
      return true;
    }

    private static MethodInfo ApplyTypeArgs(MethodInfo m, Type[] typeArgs)
    {
      if (typeArgs == null || typeArgs.Length == 0)
      {
        if (!m.IsGenericMethodDefinition)
          return m;
      }
      else if (m.IsGenericMethodDefinition && m.GetGenericArguments().Length == typeArgs.Length)
        return m.MakeGenericMethod(typeArgs);
      return (MethodInfo) null;
    }

    private static void ValidateNewArgs(ConstructorInfo constructor, ref ReadOnlyCollection<Expression> arguments, ref ReadOnlyCollection<MemberInfo> members)
    {
      ParameterInfo[] parametersCached;
      if ((parametersCached = TypeExtensions.GetParametersCached((MethodBase) constructor)).Length > 0)
      {
        if (arguments.Count != parametersCached.Length)
          throw Error.IncorrectNumberOfConstructorArguments();
        if (arguments.Count != members.Count)
          throw Error.IncorrectNumberOfArgumentsForMembers();
        Expression[] list1 = (Expression[]) null;
        MemberInfo[] list2 = (MemberInfo[]) null;
        int index1 = 0;
        for (int count = arguments.Count; index1 < count; ++index1)
        {
          Expression expression = arguments[index1];
          Expression.RequiresCanRead(expression, "argument");
          MemberInfo member = members[index1];
          ContractUtils.RequiresNotNull((object) member, "member");
          if (!TypeUtils.AreEquivalent(member.DeclaringType, constructor.DeclaringType))
            throw Error.ArgumentMemberNotDeclOnType((object) member.Name, (object) constructor.DeclaringType.Name);
          Type memberType;
          Expression.ValidateAnonymousTypeMember(ref member, out memberType);
          if (!TypeUtils.AreReferenceAssignable(memberType, expression.Type) && !Expression.TryQuote(memberType, ref expression))
            throw Error.ArgumentTypeDoesNotMatchMember((object) expression.Type, (object) memberType);
          Type type = parametersCached[index1].ParameterType;
          if (type.IsByRef)
            type = type.GetElementType();
          if (!TypeUtils.AreReferenceAssignable(type, expression.Type) && !Expression.TryQuote(type, ref expression))
            throw Error.ExpressionTypeDoesNotMatchConstructorParameter((object) expression.Type, (object) type);
          if (list1 == null && expression != arguments[index1])
          {
            list1 = new Expression[arguments.Count];
            for (int index2 = 0; index2 < index1; ++index2)
              list1[index2] = arguments[index2];
          }
          if (list1 != null)
            list1[index1] = expression;
          if (list2 == null && member != members[index1])
          {
            list2 = new MemberInfo[members.Count];
            for (int index2 = 0; index2 < index1; ++index2)
              list2[index2] = members[index2];
          }
          if (list2 != null)
            list2[index1] = member;
        }
        if (list1 != null)
          arguments = (ReadOnlyCollection<Expression>) new TrueReadOnlyCollection<Expression>(list1);
        if (list2 == null)
          return;
        members = (ReadOnlyCollection<MemberInfo>) new TrueReadOnlyCollection<MemberInfo>(list2);
      }
      else
      {
        if (arguments != null && arguments.Count > 0)
          throw Error.IncorrectNumberOfConstructorArguments();
        if (members != null && members.Count > 0)
          throw Error.IncorrectNumberOfMembersForGivenConstructor();
      }
    }

    private static void ValidateAnonymousTypeMember(ref MemberInfo member, out Type memberType)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          FieldInfo fieldInfo = member as FieldInfo;
          if (fieldInfo.IsStatic)
            throw Error.ArgumentMustBeInstanceMember();
          memberType = fieldInfo.FieldType;
          break;
        case MemberTypes.Method:
          MethodInfo mi = member as MethodInfo;
          if (mi.IsStatic)
            throw Error.ArgumentMustBeInstanceMember();
          PropertyInfo property = Expression.GetProperty(mi);
          member = (MemberInfo) property;
          memberType = property.PropertyType;
          break;
        case MemberTypes.Property:
          PropertyInfo propertyInfo = member as PropertyInfo;
          if (!propertyInfo.CanRead)
            throw Error.PropertyDoesNotHaveGetter((object) propertyInfo);
          if (propertyInfo.GetGetMethod().IsStatic)
            throw Error.ArgumentMustBeInstanceMember();
          memberType = propertyInfo.PropertyType;
          break;
        default:
          throw Error.ArgumentMustBeFieldInfoOrPropertInfoOrMethod();
      }
    }

    private static void ValidateSwitchCaseType(Expression @case, bool customType, Type resultType, string parameterName)
    {
      if (customType)
      {
        if (resultType != typeof (void) && !TypeUtils.AreReferenceAssignable(resultType, @case.Type))
          throw new ArgumentException(Strings.ArgumentTypesMustMatch, parameterName);
      }
      else if (!TypeUtils.AreEquivalent(resultType, @case.Type))
        throw new ArgumentException(Strings.AllCaseBodiesMustHaveSameType, parameterName);
    }

    private static void ValidateTryAndCatchHaveSameType(Type type, Expression tryBody, ReadOnlyCollection<CatchBlock> handlers)
    {
      if (type != (Type) null)
      {
        if (!(type != typeof (void)))
          return;
        if (!TypeUtils.AreReferenceAssignable(type, tryBody.Type))
          throw Error.ArgumentTypesMustMatch();
        foreach (CatchBlock catchBlock in handlers)
        {
          if (!TypeUtils.AreReferenceAssignable(type, catchBlock.Body.Type))
            throw Error.ArgumentTypesMustMatch();
        }
      }
      else if (tryBody == null || tryBody.Type == typeof (void))
      {
        foreach (CatchBlock catchBlock in handlers)
        {
          if (catchBlock.Body != null && catchBlock.Body.Type != typeof (void))
            throw Error.BodyOfCatchMustHaveSameTypeAsBodyOfTry();
        }
      }
      else
      {
        type = tryBody.Type;
        foreach (CatchBlock catchBlock in handlers)
        {
          if (catchBlock.Body == null || !TypeUtils.AreEquivalent(catchBlock.Body.Type, type))
            throw Error.BodyOfCatchMustHaveSameTypeAsBodyOfTry();
        }
      }
    }

    private static UnaryExpression GetUserDefinedUnaryOperatorOrThrow(ExpressionType unaryType, string name, Expression operand)
    {
      UnaryExpression definedUnaryOperator = Expression.GetUserDefinedUnaryOperator(unaryType, name, operand);
      if (definedUnaryOperator == null)
        throw Error.UnaryOperatorNotDefined((object) unaryType, (object) operand.Type);
      Expression.ValidateParamswithOperandsOrThrow(TypeExtensions.GetParametersCached((MethodBase) definedUnaryOperator.Method)[0].ParameterType, operand.Type, unaryType, name);
      return definedUnaryOperator;
    }

    private static UnaryExpression GetUserDefinedUnaryOperator(ExpressionType unaryType, string name, Expression operand)
    {
      Type type = operand.Type;
      Type[] types = new Type[1]
      {
        type
      };
      Type nonNullableType = TypeUtils.GetNonNullableType(type);
      BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
      MethodInfo methodValidated1 = TypeExtensions.GetMethodValidated(nonNullableType, name, bindingAttr, (Binder) null, types, (ParameterModifier[]) null);
      if (methodValidated1 != (MethodInfo) null)
        return new UnaryExpression(unaryType, operand, methodValidated1.ReturnType, methodValidated1);
      if (TypeUtils.IsNullableType(type))
      {
        types[0] = nonNullableType;
        MethodInfo methodValidated2 = TypeExtensions.GetMethodValidated(nonNullableType, name, bindingAttr, (Binder) null, types, (ParameterModifier[]) null);
        if (methodValidated2 != (MethodInfo) null && methodValidated2.ReturnType.IsValueType && !TypeUtils.IsNullableType(methodValidated2.ReturnType))
          return new UnaryExpression(unaryType, operand, TypeUtils.GetNullableType(methodValidated2.ReturnType), methodValidated2);
      }
      return (UnaryExpression) null;
    }

    private static UnaryExpression GetMethodBasedUnaryOperator(ExpressionType unaryType, Expression operand, MethodInfo method)
    {
      Expression.ValidateOperator(method);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method);
      if (parametersCached.Length != 1)
        throw Error.IncorrectNumberOfMethodCallArguments((object) method);
      if (Expression.ParameterIsAssignable(parametersCached[0], operand.Type))
      {
        Expression.ValidateParamswithOperandsOrThrow(parametersCached[0].ParameterType, operand.Type, unaryType, method.Name);
        return new UnaryExpression(unaryType, operand, method.ReturnType, method);
      }
      else if (TypeUtils.IsNullableType(operand.Type) && Expression.ParameterIsAssignable(parametersCached[0], TypeUtils.GetNonNullableType(operand.Type)) && (method.ReturnType.IsValueType && !TypeUtils.IsNullableType(method.ReturnType)))
        return new UnaryExpression(unaryType, operand, TypeUtils.GetNullableType(method.ReturnType), method);
      else
        throw Error.OperandTypesDoNotMatchParameters((object) unaryType, (object) method.Name);
    }

    private static UnaryExpression GetUserDefinedCoercionOrThrow(ExpressionType coercionType, Expression expression, Type convertToType)
    {
      UnaryExpression userDefinedCoercion = Expression.GetUserDefinedCoercion(coercionType, expression, convertToType);
      if (userDefinedCoercion != null)
        return userDefinedCoercion;
      else
        throw Error.CoercionOperatorNotDefined((object) expression.Type, (object) convertToType);
    }

    private static UnaryExpression GetUserDefinedCoercion(ExpressionType coercionType, Expression expression, Type convertToType)
    {
      MethodInfo definedCoercionMethod = TypeUtils.GetUserDefinedCoercionMethod(expression.Type, convertToType, false);
      if (definedCoercionMethod != (MethodInfo) null)
        return new UnaryExpression(coercionType, expression, convertToType, definedCoercionMethod);
      else
        return (UnaryExpression) null;
    }

    private static UnaryExpression GetMethodBasedCoercionOperator(ExpressionType unaryType, Expression operand, Type convertToType, MethodInfo method)
    {
      Expression.ValidateOperator(method);
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method);
      if (parametersCached.Length != 1)
        throw Error.IncorrectNumberOfMethodCallArguments((object) method);
      if (Expression.ParameterIsAssignable(parametersCached[0], operand.Type) && TypeUtils.AreEquivalent(method.ReturnType, convertToType))
        return new UnaryExpression(unaryType, operand, method.ReturnType, method);
      if ((TypeUtils.IsNullableType(operand.Type) || TypeUtils.IsNullableType(convertToType)) && (Expression.ParameterIsAssignable(parametersCached[0], TypeUtils.GetNonNullableType(operand.Type)) && TypeUtils.AreEquivalent(method.ReturnType, TypeUtils.GetNonNullableType(convertToType))))
        return new UnaryExpression(unaryType, operand, convertToType, method);
      else
        throw Error.OperandTypesDoNotMatchParameters((object) unaryType, (object) method.Name);
    }

    private delegate LambdaExpression LambdaFactory(Expression body, string name, bool tailCall, ReadOnlyCollection<ParameterExpression> parameters);

    private class ExtensionInfo
    {
      internal readonly ExpressionType NodeType;
      internal readonly Type Type;

      public ExtensionInfo(ExpressionType nodeType, Type type)
      {
        this.NodeType = nodeType;
        this.Type = type;
      }
    }

    internal class BinaryExpressionProxy
    {
      private readonly BinaryExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public LambdaExpression Conversion
      {
        get
        {
          return this._node.Conversion;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public bool IsLifted
      {
        get
        {
          return this._node.IsLifted;
        }
      }

      public bool IsLiftedToNull
      {
        get
        {
          return this._node.IsLiftedToNull;
        }
      }

      public Expression Left
      {
        get
        {
          return this._node.Left;
        }
      }

      public MethodInfo Method
      {
        get
        {
          return this._node.Method;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Expression Right
      {
        get
        {
          return this._node.Right;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public BinaryExpressionProxy(BinaryExpression node)
      {
        this._node = node;
      }
    }

    internal class BlockExpressionProxy
    {
      private readonly BlockExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public ReadOnlyCollection<Expression> Expressions
      {
        get
        {
          return this._node.Expressions;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Expression Result
      {
        get
        {
          return this._node.Result;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public ReadOnlyCollection<ParameterExpression> Variables
      {
        get
        {
          return this._node.Variables;
        }
      }

      public BlockExpressionProxy(BlockExpression node)
      {
        this._node = node;
      }
    }

    internal class CatchBlockProxy
    {
      private readonly CatchBlock _node;

      public Expression Body
      {
        get
        {
          return this._node.Body;
        }
      }

      public Expression Filter
      {
        get
        {
          return this._node.Filter;
        }
      }

      public Type Test
      {
        get
        {
          return this._node.Test;
        }
      }

      public ParameterExpression Variable
      {
        get
        {
          return this._node.Variable;
        }
      }

      public CatchBlockProxy(CatchBlock node)
      {
        this._node = node;
      }
    }

    internal class ConditionalExpressionProxy
    {
      private readonly ConditionalExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public Expression IfFalse
      {
        get
        {
          return this._node.IfFalse;
        }
      }

      public Expression IfTrue
      {
        get
        {
          return this._node.IfTrue;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Expression Test
      {
        get
        {
          return this._node.Test;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public ConditionalExpressionProxy(ConditionalExpression node)
      {
        this._node = node;
      }
    }

    internal class ConstantExpressionProxy
    {
      private readonly ConstantExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public object Value
      {
        get
        {
          return this._node.Value;
        }
      }

      public ConstantExpressionProxy(ConstantExpression node)
      {
        this._node = node;
      }
    }

    internal class DebugInfoExpressionProxy
    {
      private readonly DebugInfoExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public SymbolDocumentInfo Document
      {
        get
        {
          return this._node.Document;
        }
      }

      public int EndColumn
      {
        get
        {
          return this._node.EndColumn;
        }
      }

      public int EndLine
      {
        get
        {
          return this._node.EndLine;
        }
      }

      public bool IsClear
      {
        get
        {
          return this._node.IsClear;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public int StartColumn
      {
        get
        {
          return this._node.StartColumn;
        }
      }

      public int StartLine
      {
        get
        {
          return this._node.StartLine;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public DebugInfoExpressionProxy(DebugInfoExpression node)
      {
        this._node = node;
      }
    }

    internal class DefaultExpressionProxy
    {
      private readonly DefaultExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public DefaultExpressionProxy(DefaultExpression node)
      {
        this._node = node;
      }
    }

    internal class DynamicExpressionProxy
    {
      private readonly DynamicExpression _node;

      public ReadOnlyCollection<Expression> Arguments
      {
        get
        {
          return this._node.Arguments;
        }
      }

      public CallSiteBinder Binder
      {
        get
        {
          return this._node.Binder;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public Type DelegateType
      {
        get
        {
          return this._node.DelegateType;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public DynamicExpressionProxy(DynamicExpression node)
      {
        this._node = node;
      }
    }

    internal class GotoExpressionProxy
    {
      private readonly GotoExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public GotoExpressionKind Kind
      {
        get
        {
          return this._node.Kind;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public LabelTarget Target
      {
        get
        {
          return this._node.Target;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public Expression Value
      {
        get
        {
          return this._node.Value;
        }
      }

      public GotoExpressionProxy(GotoExpression node)
      {
        this._node = node;
      }
    }

    internal class IndexExpressionProxy
    {
      private readonly IndexExpression _node;

      public ReadOnlyCollection<Expression> Arguments
      {
        get
        {
          return this._node.Arguments;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public PropertyInfo Indexer
      {
        get
        {
          return this._node.Indexer;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Expression Object
      {
        get
        {
          return this._node.Object;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public IndexExpressionProxy(IndexExpression node)
      {
        this._node = node;
      }
    }

    internal class InvocationExpressionProxy
    {
      private readonly InvocationExpression _node;

      public ReadOnlyCollection<Expression> Arguments
      {
        get
        {
          return this._node.Arguments;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public Expression Expression
      {
        get
        {
          return this._node.Expression;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public InvocationExpressionProxy(InvocationExpression node)
      {
        this._node = node;
      }
    }

    internal class LabelExpressionProxy
    {
      private readonly LabelExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public Expression DefaultValue
      {
        get
        {
          return this._node.DefaultValue;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public LabelTarget Target
      {
        get
        {
          return this._node.Target;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public LabelExpressionProxy(LabelExpression node)
      {
        this._node = node;
      }
    }

    internal class LambdaExpressionProxy
    {
      private readonly LambdaExpression _node;

      public Expression Body
      {
        get
        {
          return this._node.Body;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public string Name
      {
        get
        {
          return this._node.Name;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public ReadOnlyCollection<ParameterExpression> Parameters
      {
        get
        {
          return this._node.Parameters;
        }
      }

      public Type ReturnType
      {
        get
        {
          return this._node.ReturnType;
        }
      }

      public bool TailCall
      {
        get
        {
          return this._node.TailCall;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public LambdaExpressionProxy(LambdaExpression node)
      {
        this._node = node;
      }
    }

    internal class ListInitExpressionProxy
    {
      private readonly ListInitExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public ReadOnlyCollection<ElementInit> Initializers
      {
        get
        {
          return this._node.Initializers;
        }
      }

      public NewExpression NewExpression
      {
        get
        {
          return this._node.NewExpression;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public ListInitExpressionProxy(ListInitExpression node)
      {
        this._node = node;
      }
    }

    internal class LoopExpressionProxy
    {
      private readonly LoopExpression _node;

      public Expression Body
      {
        get
        {
          return this._node.Body;
        }
      }

      public LabelTarget BreakLabel
      {
        get
        {
          return this._node.BreakLabel;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public LabelTarget ContinueLabel
      {
        get
        {
          return this._node.ContinueLabel;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public LoopExpressionProxy(LoopExpression node)
      {
        this._node = node;
      }
    }

    internal class MemberExpressionProxy
    {
      private readonly MemberExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public Expression Expression
      {
        get
        {
          return this._node.Expression;
        }
      }

      public MemberInfo Member
      {
        get
        {
          return this._node.Member;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public MemberExpressionProxy(MemberExpression node)
      {
        this._node = node;
      }
    }

    internal class MemberInitExpressionProxy
    {
      private readonly MemberInitExpression _node;

      public ReadOnlyCollection<MemberBinding> Bindings
      {
        get
        {
          return this._node.Bindings;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public NewExpression NewExpression
      {
        get
        {
          return this._node.NewExpression;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public MemberInitExpressionProxy(MemberInitExpression node)
      {
        this._node = node;
      }
    }

    internal class MethodCallExpressionProxy
    {
      private readonly MethodCallExpression _node;

      public ReadOnlyCollection<Expression> Arguments
      {
        get
        {
          return this._node.Arguments;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public MethodInfo Method
      {
        get
        {
          return this._node.Method;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Expression Object
      {
        get
        {
          return this._node.Object;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public MethodCallExpressionProxy(MethodCallExpression node)
      {
        this._node = node;
      }
    }

    internal class NewArrayExpressionProxy
    {
      private readonly NewArrayExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public ReadOnlyCollection<Expression> Expressions
      {
        get
        {
          return this._node.Expressions;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public NewArrayExpressionProxy(NewArrayExpression node)
      {
        this._node = node;
      }
    }

    internal class NewExpressionProxy
    {
      private readonly NewExpression _node;

      public ReadOnlyCollection<Expression> Arguments
      {
        get
        {
          return this._node.Arguments;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public ConstructorInfo Constructor
      {
        get
        {
          return this._node.Constructor;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public ReadOnlyCollection<MemberInfo> Members
      {
        get
        {
          return this._node.Members;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public NewExpressionProxy(NewExpression node)
      {
        this._node = node;
      }
    }

    internal class ParameterExpressionProxy
    {
      private readonly ParameterExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public bool IsByRef
      {
        get
        {
          return this._node.IsByRef;
        }
      }

      public string Name
      {
        get
        {
          return this._node.Name;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public ParameterExpressionProxy(ParameterExpression node)
      {
        this._node = node;
      }
    }

    internal class RuntimeVariablesExpressionProxy
    {
      private readonly RuntimeVariablesExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public ReadOnlyCollection<ParameterExpression> Variables
      {
        get
        {
          return this._node.Variables;
        }
      }

      public RuntimeVariablesExpressionProxy(RuntimeVariablesExpression node)
      {
        this._node = node;
      }
    }

    internal class SwitchCaseProxy
    {
      private readonly SwitchCase _node;

      public Expression Body
      {
        get
        {
          return this._node.Body;
        }
      }

      public ReadOnlyCollection<Expression> TestValues
      {
        get
        {
          return this._node.TestValues;
        }
      }

      public SwitchCaseProxy(SwitchCase node)
      {
        this._node = node;
      }
    }

    internal class SwitchExpressionProxy
    {
      private readonly SwitchExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public ReadOnlyCollection<SwitchCase> Cases
      {
        get
        {
          return this._node.Cases;
        }
      }

      public MethodInfo Comparison
      {
        get
        {
          return this._node.Comparison;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public Expression DefaultBody
      {
        get
        {
          return this._node.DefaultBody;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Expression SwitchValue
      {
        get
        {
          return this._node.SwitchValue;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public SwitchExpressionProxy(SwitchExpression node)
      {
        this._node = node;
      }
    }

    internal class TryExpressionProxy
    {
      private readonly TryExpression _node;

      public Expression Body
      {
        get
        {
          return this._node.Body;
        }
      }

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public Expression Fault
      {
        get
        {
          return this._node.Fault;
        }
      }

      public Expression Finally
      {
        get
        {
          return this._node.Finally;
        }
      }

      public ReadOnlyCollection<CatchBlock> Handlers
      {
        get
        {
          return this._node.Handlers;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public TryExpressionProxy(TryExpression node)
      {
        this._node = node;
      }
    }

    internal class TypeBinaryExpressionProxy
    {
      private readonly TypeBinaryExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public Expression Expression
      {
        get
        {
          return this._node.Expression;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public Type TypeOperand
      {
        get
        {
          return this._node.TypeOperand;
        }
      }

      public TypeBinaryExpressionProxy(TypeBinaryExpression node)
      {
        this._node = node;
      }
    }

    internal class UnaryExpressionProxy
    {
      private readonly UnaryExpression _node;

      public bool CanReduce
      {
        get
        {
          return this._node.CanReduce;
        }
      }

      public string DebugView
      {
        get
        {
          return this._node.DebugView;
        }
      }

      public bool IsLifted
      {
        get
        {
          return this._node.IsLifted;
        }
      }

      public bool IsLiftedToNull
      {
        get
        {
          return this._node.IsLiftedToNull;
        }
      }

      public MethodInfo Method
      {
        get
        {
          return this._node.Method;
        }
      }

      public ExpressionType NodeType
      {
        get
        {
          return this._node.NodeType;
        }
      }

      public Expression Operand
      {
        get
        {
          return this._node.Operand;
        }
      }

      public Type Type
      {
        get
        {
          return this._node.Type;
        }
      }

      public UnaryExpressionProxy(UnaryExpression node)
      {
        this._node = node;
      }
    }
  }
}
