// Type: System.Linq.Expressions.Compiler.LambdaCompiler
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class LambdaCompiler
  {
    private LabelScopeInfo _labelBlock = new LabelScopeInfo((LabelScopeInfo) null, LabelScopeKind.Lambda);
    private readonly Dictionary<LabelTarget, LabelInfo> _labelInfo = new Dictionary<LabelTarget, LabelInfo>();
    private readonly KeyedQueue<Type, LocalBuilder> _freeLocals = new KeyedQueue<Type, LocalBuilder>();
    private readonly AnalyzedTree _tree;
    private readonly ILGenerator _ilg;
    private readonly TypeBuilder _typeBuilder;
    private readonly MethodInfo _method;
    private CompilerScope _scope;
    private readonly LambdaExpression _lambda;
    private readonly bool _hasClosureArgument;
    private readonly BoundConstants _boundConstants;
    private bool _sequencePointCleared;
    private static int _Counter;

    private bool EmitDebugSymbols
    {
      get
      {
        return this._tree.DebugInfoGenerator != null;
      }
    }

    internal ILGenerator IL
    {
      get
      {
        return this._ilg;
      }
    }

    internal ReadOnlyCollection<ParameterExpression> Parameters
    {
      get
      {
        return this._lambda.Parameters;
      }
    }

    internal bool CanEmitBoundConstants
    {
      get
      {
        return this._method is DynamicMethod;
      }
    }

    private LambdaCompiler(AnalyzedTree tree, LambdaExpression lambda)
    {
      Type[] parameterTypes = CollectionExtensions.AddFirst<Type>((IList<Type>) LambdaCompiler.GetParameterTypes(lambda), typeof (Closure));
      DynamicMethod dynamicMethod = new DynamicMethod(lambda.Name ?? "lambda_method", lambda.ReturnType, parameterTypes, true);
      this._tree = tree;
      this._lambda = lambda;
      this._method = (MethodInfo) dynamicMethod;
      dynamicMethod.ProfileAPICheck = true;
      this._ilg = dynamicMethod.GetILGenerator();
      this._hasClosureArgument = true;
      this._scope = tree.Scopes[(object) lambda];
      this._boundConstants = tree.Constants[lambda];
      this.InitializeMethod();
    }

    private LambdaCompiler(AnalyzedTree tree, LambdaExpression lambda, MethodBuilder method)
    {
      this._hasClosureArgument = tree.Scopes[(object) lambda].NeedsClosure;
      Type[] typeArray = LambdaCompiler.GetParameterTypes(lambda);
      if (this._hasClosureArgument)
        typeArray = CollectionExtensions.AddFirst<Type>((IList<Type>) typeArray, typeof (Closure));
      method.SetReturnType(lambda.ReturnType);
      method.SetParameters(typeArray);
      string[] strArray = CollectionExtensions.Map<ParameterExpression, string>((ICollection<ParameterExpression>) lambda.Parameters, (Func<ParameterExpression, string>) (p => p.Name));
      int num = this._hasClosureArgument ? 2 : 1;
      for (int index = 0; index < strArray.Length; ++index)
        method.DefineParameter(index + num, ParameterAttributes.None, strArray[index]);
      this._tree = tree;
      this._lambda = lambda;
      this._typeBuilder = (TypeBuilder) method.DeclaringType;
      this._method = (MethodInfo) method;
      this._ilg = method.GetILGenerator();
      this._scope = tree.Scopes[(object) lambda];
      this._boundConstants = tree.Constants[lambda];
      this.InitializeMethod();
    }

    private LambdaCompiler(LambdaCompiler parent, LambdaExpression lambda)
    {
      this._tree = parent._tree;
      this._lambda = lambda;
      this._method = parent._method;
      this._ilg = parent._ilg;
      this._hasClosureArgument = parent._hasClosureArgument;
      this._typeBuilder = parent._typeBuilder;
      this._scope = this._tree.Scopes[(object) lambda];
      this._boundConstants = parent._boundConstants;
    }

    public override string ToString()
    {
      return this._method.ToString();
    }

    internal static Delegate Compile(LambdaExpression lambda, DebugInfoGenerator debugInfoGenerator)
    {
      AnalyzedTree tree = LambdaCompiler.AnalyzeLambda(ref lambda);
      tree.DebugInfoGenerator = debugInfoGenerator;
      LambdaCompiler lambdaCompiler = new LambdaCompiler(tree, lambda);
      lambdaCompiler.EmitLambdaBody();
      return lambdaCompiler.CreateDelegate();
    }

    private FieldBuilder CreateStaticField(string name, Type type)
    {
      return this._typeBuilder.DefineField(string.Concat(new object[4]
      {
        (object) "<ExpressionCompilerImplementationDetails>{",
        (object) Interlocked.Increment(ref LambdaCompiler._Counter),
        (object) "}",
        (object) name
      }), type, FieldAttributes.Private | FieldAttributes.Static);
    }

    private MemberExpression CreateLazyInitializedField<T>(string name)
    {
      if (this._method is DynamicMethod)
        return Expression.Field((Expression) Expression.Constant((object) new StrongBox<T>(default (T))), "Value");
      else
        return Expression.Field((Expression) null, (FieldInfo) this.CreateStaticField(name, typeof (T)));
    }

    private void EmitConstant(object value, Type type)
    {
      if (ILGen.CanEmitConstant(value, type))
        ILGen.EmitConstant(this._ilg, value, type);
      else
        this._boundConstants.EmitConstant(this, value, type);
    }

    internal void EmitConstantArray<T>(T[] array)
    {
      if (this._method is DynamicMethod)
        this.EmitConstant((object) array, typeof (T[]));
      else if ((Type) this._typeBuilder != (Type) null)
      {
        FieldBuilder staticField = this.CreateStaticField("ConstantArray", typeof (T[]));
        Label label = this._ilg.DefineLabel();
        this._ilg.Emit(OpCodes.Ldsfld, (FieldInfo) staticField);
        this._ilg.Emit(OpCodes.Ldnull);
        this._ilg.Emit(OpCodes.Bne_Un, label);
        ILGen.EmitArray<T>(this._ilg, (IList<T>) array);
        this._ilg.Emit(OpCodes.Stsfld, (FieldInfo) staticField);
        this._ilg.MarkLabel(label);
        this._ilg.Emit(OpCodes.Ldsfld, (FieldInfo) staticField);
      }
      else
        ILGen.EmitArray<T>(this._ilg, (IList<T>) array);
    }

    private void EmitAddress(Expression node, Type type)
    {
      this.EmitAddress(node, type, LambdaCompiler.CompilationFlags.EmitExpressionStart);
    }

    private void EmitAddress(Expression node, Type type, LambdaCompiler.CompilationFlags flags)
    {
      bool flag = (flags & LambdaCompiler.CompilationFlags.EmitExpressionStartMask) == LambdaCompiler.CompilationFlags.EmitExpressionStart;
      LambdaCompiler.CompilationFlags flags1 = flag ? this.EmitExpressionStart(node) : LambdaCompiler.CompilationFlags.EmitNoExpressionStart;
      switch (node.NodeType)
      {
        case ExpressionType.Parameter:
          this.AddressOf((ParameterExpression) node, type);
          break;
        case ExpressionType.Index:
          this.AddressOf((IndexExpression) node, type);
          break;
        case ExpressionType.Unbox:
          this.AddressOf((UnaryExpression) node, type);
          break;
        case ExpressionType.ArrayIndex:
          this.AddressOf((BinaryExpression) node, type);
          break;
        case ExpressionType.Call:
          this.AddressOf((MethodCallExpression) node, type);
          break;
        case ExpressionType.MemberAccess:
          this.AddressOf((MemberExpression) node, type);
          break;
        default:
          this.EmitExpressionAddress(node, type);
          break;
      }
      if (!flag)
        return;
      this.EmitExpressionEnd(flags1);
    }

    private void AddressOf(BinaryExpression node, Type type)
    {
      if (TypeUtils.AreEquivalent(type, node.Type))
      {
        this.EmitExpression(node.Left);
        this.EmitExpression(node.Right);
        Type type1 = node.Right.Type;
        if (TypeUtils.IsNullableType(type1))
        {
          LocalBuilder local = this.GetLocal(type1);
          this._ilg.Emit(OpCodes.Stloc, local);
          this._ilg.Emit(OpCodes.Ldloca, local);
          ILGen.EmitGetValue(this._ilg, type1);
          this.FreeLocal(local);
        }
        Type nonNullableType = TypeUtils.GetNonNullableType(type1);
        if (nonNullableType != typeof (int))
          ILGen.EmitConvertToType(this._ilg, nonNullableType, typeof (int), true);
        this._ilg.Emit(OpCodes.Ldelema, node.Type);
      }
      else
        this.EmitExpressionAddress((Expression) node, type);
    }

    private void AddressOf(ParameterExpression node, Type type)
    {
      if (TypeUtils.AreEquivalent(type, node.Type))
      {
        if (node.IsByRef)
          this._scope.EmitGet(node);
        else
          this._scope.EmitAddressOf(node);
      }
      else
        this.EmitExpressionAddress((Expression) node, type);
    }

    private void AddressOf(MemberExpression node, Type type)
    {
      if (TypeUtils.AreEquivalent(type, node.Type))
      {
        Type objectType = (Type) null;
        if (node.Expression != null)
          this.EmitInstance(node.Expression, objectType = node.Expression.Type);
        this.EmitMemberAddress(node.Member, objectType);
      }
      else
        this.EmitExpressionAddress((Expression) node, type);
    }

    private void EmitMemberAddress(MemberInfo member, Type objectType)
    {
      if (member.MemberType == MemberTypes.Field)
      {
        FieldInfo fi = (FieldInfo) member;
        if (!fi.IsLiteral && !fi.IsInitOnly)
        {
          ILGen.EmitFieldAddress(this._ilg, fi);
          return;
        }
      }
      this.EmitMemberGet(member, objectType);
      LocalBuilder local = this.GetLocal(LambdaCompiler.GetMemberType(member));
      this._ilg.Emit(OpCodes.Stloc, local);
      this._ilg.Emit(OpCodes.Ldloca, local);
    }

    private void AddressOf(MethodCallExpression node, Type type)
    {
      if (!node.Method.IsStatic && node.Object.Type.IsArray && node.Method == node.Object.Type.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public))
      {
        MethodInfo method = node.Object.Type.GetMethod("Address", BindingFlags.Instance | BindingFlags.Public);
        this.EmitMethodCall(node.Object, method, (IArgumentProvider) node);
      }
      else
        this.EmitExpressionAddress((Expression) node, type);
    }

    private void AddressOf(IndexExpression node, Type type)
    {
      if (!TypeUtils.AreEquivalent(type, node.Type) || node.Indexer != (PropertyInfo) null)
        this.EmitExpressionAddress((Expression) node, type);
      else if (node.Arguments.Count == 1)
      {
        this.EmitExpression(node.Object);
        this.EmitExpression(node.Arguments[0]);
        this._ilg.Emit(OpCodes.Ldelema, node.Type);
      }
      else
      {
        MethodInfo method = node.Object.Type.GetMethod("Address", BindingFlags.Instance | BindingFlags.Public);
        this.EmitMethodCall(node.Object, method, (IArgumentProvider) node);
      }
    }

    private void AddressOf(UnaryExpression node, Type type)
    {
      this.EmitExpression(node.Operand);
      this._ilg.Emit(OpCodes.Unbox, type);
    }

    private void EmitExpressionAddress(Expression node, Type type)
    {
      this.EmitExpression(node, LambdaCompiler.CompilationFlags.EmitNoExpressionStart | LambdaCompiler.CompilationFlags.EmitAsNoTail);
      LocalBuilder local = this.GetLocal(type);
      this._ilg.Emit(OpCodes.Stloc, local);
      this._ilg.Emit(OpCodes.Ldloca, local);
    }

    private LambdaCompiler.WriteBack EmitAddressWriteBack(Expression node, Type type)
    {
      LambdaCompiler.CompilationFlags flags = this.EmitExpressionStart(node);
      LambdaCompiler.WriteBack writeBack = (LambdaCompiler.WriteBack) null;
      if (TypeUtils.AreEquivalent(type, node.Type))
      {
        switch (node.NodeType)
        {
          case ExpressionType.MemberAccess:
            writeBack = this.AddressOfWriteBack((MemberExpression) node);
            break;
          case ExpressionType.Index:
            writeBack = this.AddressOfWriteBack((IndexExpression) node);
            break;
        }
      }
      if (writeBack == null)
        this.EmitAddress(node, type, LambdaCompiler.CompilationFlags.EmitNoExpressionStart | LambdaCompiler.CompilationFlags.EmitAsNoTail);
      this.EmitExpressionEnd(flags);
      return writeBack;
    }

    private LambdaCompiler.WriteBack AddressOfWriteBack(MemberExpression node)
    {
      if (node.Member.MemberType != MemberTypes.Property || !((PropertyInfo) node.Member).CanWrite)
        return (LambdaCompiler.WriteBack) null;
      LocalBuilder instanceLocal = (LocalBuilder) null;
      Type instanceType = (Type) null;
      if (node.Expression != null)
      {
        this.EmitInstance(node.Expression, instanceType = node.Expression.Type);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Stloc, instanceLocal = this.GetLocal(instanceType));
      }
      PropertyInfo pi = (PropertyInfo) node.Member;
      this.EmitCall(instanceType, pi.GetGetMethod(true));
      LocalBuilder valueLocal = this.GetLocal(node.Type);
      this._ilg.Emit(OpCodes.Stloc, valueLocal);
      this._ilg.Emit(OpCodes.Ldloca, valueLocal);
      return (LambdaCompiler.WriteBack) (() =>
      {
        if (instanceLocal != null)
        {
          this._ilg.Emit(OpCodes.Ldloc, instanceLocal);
          this.FreeLocal(instanceLocal);
        }
        this._ilg.Emit(OpCodes.Ldloc, valueLocal);
        this.FreeLocal(valueLocal);
        this.EmitCall(instanceType, pi.GetSetMethod(true));
      });
    }

    private LambdaCompiler.WriteBack AddressOfWriteBack(IndexExpression node)
    {
      if (node.Indexer == (PropertyInfo) null || !node.Indexer.CanWrite)
        return (LambdaCompiler.WriteBack) null;
      LocalBuilder instanceLocal = (LocalBuilder) null;
      Type instanceType = (Type) null;
      if (node.Object != null)
      {
        this.EmitInstance(node.Object, instanceType = node.Object.Type);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Stloc, instanceLocal = this.GetLocal(instanceType));
      }
      List<LocalBuilder> args = new List<LocalBuilder>();
      foreach (Expression node1 in node.Arguments)
      {
        this.EmitExpression(node1);
        LocalBuilder local = this.GetLocal(node1.Type);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Stloc, local);
        args.Add(local);
      }
      this.EmitGetIndexCall(node, instanceType);
      LocalBuilder valueLocal = this.GetLocal(node.Type);
      this._ilg.Emit(OpCodes.Stloc, valueLocal);
      this._ilg.Emit(OpCodes.Ldloca, valueLocal);
      return (LambdaCompiler.WriteBack) (() =>
      {
        if (instanceLocal != null)
        {
          this._ilg.Emit(OpCodes.Ldloc, instanceLocal);
          this.FreeLocal(instanceLocal);
        }
        foreach (LocalBuilder item_1 in args)
        {
          this._ilg.Emit(OpCodes.Ldloc, item_1);
          this.FreeLocal(item_1);
        }
        this._ilg.Emit(OpCodes.Ldloc, valueLocal);
        this.FreeLocal(valueLocal);
        this.EmitSetIndexCall(node, instanceType);
      });
    }

    private void EmitBinaryExpression(Expression expr)
    {
      this.EmitBinaryExpression(expr, LambdaCompiler.CompilationFlags.EmitAsNoTail);
    }

    private void EmitBinaryExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      BinaryExpression b = (BinaryExpression) expr;
      if (b.Method != (MethodInfo) null)
      {
        this.EmitBinaryMethod(b, flags);
      }
      else
      {
        if ((b.NodeType == ExpressionType.Equal || b.NodeType == ExpressionType.NotEqual) && (b.Type == typeof (bool) || b.Type == typeof (bool?)))
        {
          if (ConstantCheck.IsNull(b.Left) && !ConstantCheck.IsNull(b.Right) && TypeUtils.IsNullableType(b.Right.Type))
          {
            this.EmitNullEquality(b.NodeType, b.Right, b.IsLiftedToNull);
            return;
          }
          else if (ConstantCheck.IsNull(b.Right) && !ConstantCheck.IsNull(b.Left) && TypeUtils.IsNullableType(b.Left.Type))
          {
            this.EmitNullEquality(b.NodeType, b.Left, b.IsLiftedToNull);
            return;
          }
          else
          {
            this.EmitExpression(LambdaCompiler.GetEqualityOperand(b.Left));
            this.EmitExpression(LambdaCompiler.GetEqualityOperand(b.Right));
          }
        }
        else
        {
          this.EmitExpression(b.Left);
          this.EmitExpression(b.Right);
        }
        this.EmitBinaryOperator(b.NodeType, b.Left.Type, b.Right.Type, b.Type, b.IsLiftedToNull);
      }
    }

    private void EmitNullEquality(ExpressionType op, Expression e, bool isLiftedToNull)
    {
      if (isLiftedToNull)
      {
        this.EmitExpressionAsVoid(e);
        ILGen.EmitDefault(this._ilg, typeof (bool?));
      }
      else
      {
        this.EmitAddress(e, e.Type);
        ILGen.EmitHasValue(this._ilg, e.Type);
        if (op != ExpressionType.Equal)
          return;
        this._ilg.Emit(OpCodes.Ldc_I4_0);
        this._ilg.Emit(OpCodes.Ceq);
      }
    }

    private void EmitBinaryMethod(BinaryExpression b, LambdaCompiler.CompilationFlags flags)
    {
      if (b.IsLifted)
      {
        ParameterExpression parameterExpression1 = Expression.Variable(TypeUtils.GetNonNullableType(b.Left.Type), (string) null);
        ParameterExpression parameterExpression2 = Expression.Variable(TypeUtils.GetNonNullableType(b.Right.Type), (string) null);
        MethodCallExpression mc = Expression.Call((Expression) null, b.Method, (Expression) parameterExpression1, (Expression) parameterExpression2);
        Type resultType;
        if (b.IsLiftedToNull)
        {
          resultType = TypeUtils.GetNullableType(mc.Type);
        }
        else
        {
          switch (b.NodeType)
          {
            case ExpressionType.Equal:
            case ExpressionType.GreaterThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.LessThan:
            case ExpressionType.LessThanOrEqual:
            case ExpressionType.NotEqual:
              if (mc.Type != typeof (bool))
                throw System.Linq.Expressions.Error.ArgumentMustBeBoolean();
              resultType = typeof (bool);
              break;
            default:
              resultType = TypeUtils.GetNullableType(mc.Type);
              break;
          }
        }
        ParameterExpression[] paramList = new ParameterExpression[2]
        {
          parameterExpression1,
          parameterExpression2
        };
        Expression[] argList = new Expression[2]
        {
          b.Left,
          b.Right
        };
        LambdaCompiler.ValidateLift((IList<ParameterExpression>) paramList, (IList<Expression>) argList);
        this.EmitLift(b.NodeType, resultType, mc, paramList, argList);
      }
      else
        this.EmitMethodCallExpression((Expression) Expression.Call((Expression) null, b.Method, b.Left, b.Right), flags);
    }

    private void EmitBinaryOperator(ExpressionType op, Type leftType, Type rightType, Type resultType, bool liftedToNull)
    {
      bool flag1 = TypeUtils.IsNullableType(leftType);
      bool flag2 = TypeUtils.IsNullableType(rightType);
      switch (op)
      {
        case ExpressionType.ArrayIndex:
          if (rightType != typeof (int))
            throw ContractUtils.Unreachable;
          ILGen.EmitLoadElement(this._ilg, leftType.GetElementType());
          break;
        case ExpressionType.Coalesce:
          throw System.Linq.Expressions.Error.UnexpectedCoalesceOperator();
        default:
          if (flag1 || flag2)
          {
            this.EmitLiftedBinaryOp(op, leftType, rightType, resultType, liftedToNull);
            break;
          }
          else
          {
            this.EmitUnliftedBinaryOp(op, leftType, rightType);
            this.EmitConvertArithmeticResult(op, resultType);
            break;
          }
      }
    }

    private void EmitUnliftedBinaryOp(ExpressionType op, Type leftType, Type rightType)
    {
      if (op == ExpressionType.Equal || op == ExpressionType.NotEqual)
      {
        this.EmitUnliftedEquality(op, leftType);
      }
      else
      {
        if (!leftType.IsPrimitive)
          throw System.Linq.Expressions.Error.OperatorNotImplementedForType((object) op, (object) leftType);
        switch (op)
        {
          case ExpressionType.Add:
            this._ilg.Emit(OpCodes.Add);
            break;
          case ExpressionType.AddChecked:
            if (TypeUtils.IsFloatingPoint(leftType))
            {
              this._ilg.Emit(OpCodes.Add);
              break;
            }
            else if (TypeUtils.IsUnsigned(leftType))
            {
              this._ilg.Emit(OpCodes.Add_Ovf_Un);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Add_Ovf);
              break;
            }
          case ExpressionType.And:
          case ExpressionType.AndAlso:
            this._ilg.Emit(OpCodes.And);
            break;
          case ExpressionType.Divide:
            if (TypeUtils.IsUnsigned(leftType))
            {
              this._ilg.Emit(OpCodes.Div_Un);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Div);
              break;
            }
          case ExpressionType.ExclusiveOr:
            this._ilg.Emit(OpCodes.Xor);
            break;
          case ExpressionType.GreaterThan:
            if (TypeUtils.IsUnsigned(leftType))
            {
              this._ilg.Emit(OpCodes.Cgt_Un);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Cgt);
              break;
            }
          case ExpressionType.GreaterThanOrEqual:
            Label label1 = this._ilg.DefineLabel();
            Label label2 = this._ilg.DefineLabel();
            if (TypeUtils.IsUnsigned(leftType))
              this._ilg.Emit(OpCodes.Bge_Un_S, label1);
            else
              this._ilg.Emit(OpCodes.Bge_S, label1);
            this._ilg.Emit(OpCodes.Ldc_I4_0);
            this._ilg.Emit(OpCodes.Br_S, label2);
            this._ilg.MarkLabel(label1);
            this._ilg.Emit(OpCodes.Ldc_I4_1);
            this._ilg.MarkLabel(label2);
            break;
          case ExpressionType.LeftShift:
            if (rightType != typeof (int))
              throw ContractUtils.Unreachable;
            this._ilg.Emit(OpCodes.Shl);
            break;
          case ExpressionType.LessThan:
            if (TypeUtils.IsUnsigned(leftType))
            {
              this._ilg.Emit(OpCodes.Clt_Un);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Clt);
              break;
            }
          case ExpressionType.LessThanOrEqual:
            Label label3 = this._ilg.DefineLabel();
            Label label4 = this._ilg.DefineLabel();
            if (TypeUtils.IsUnsigned(leftType))
              this._ilg.Emit(OpCodes.Ble_Un_S, label3);
            else
              this._ilg.Emit(OpCodes.Ble_S, label3);
            this._ilg.Emit(OpCodes.Ldc_I4_0);
            this._ilg.Emit(OpCodes.Br_S, label4);
            this._ilg.MarkLabel(label3);
            this._ilg.Emit(OpCodes.Ldc_I4_1);
            this._ilg.MarkLabel(label4);
            break;
          case ExpressionType.Modulo:
            if (TypeUtils.IsUnsigned(leftType))
            {
              this._ilg.Emit(OpCodes.Rem_Un);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Rem);
              break;
            }
          case ExpressionType.Multiply:
            this._ilg.Emit(OpCodes.Mul);
            break;
          case ExpressionType.MultiplyChecked:
            if (TypeUtils.IsFloatingPoint(leftType))
            {
              this._ilg.Emit(OpCodes.Mul);
              break;
            }
            else if (TypeUtils.IsUnsigned(leftType))
            {
              this._ilg.Emit(OpCodes.Mul_Ovf_Un);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Mul_Ovf);
              break;
            }
          case ExpressionType.Or:
          case ExpressionType.OrElse:
            this._ilg.Emit(OpCodes.Or);
            break;
          case ExpressionType.RightShift:
            if (rightType != typeof (int))
              throw ContractUtils.Unreachable;
            if (TypeUtils.IsUnsigned(leftType))
            {
              this._ilg.Emit(OpCodes.Shr_Un);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Shr);
              break;
            }
          case ExpressionType.Subtract:
            this._ilg.Emit(OpCodes.Sub);
            break;
          case ExpressionType.SubtractChecked:
            if (TypeUtils.IsFloatingPoint(leftType))
            {
              this._ilg.Emit(OpCodes.Sub);
              break;
            }
            else if (TypeUtils.IsUnsigned(leftType))
            {
              this._ilg.Emit(OpCodes.Sub_Ovf_Un);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Sub_Ovf);
              break;
            }
          default:
            throw System.Linq.Expressions.Error.UnhandledBinary((object) op);
        }
      }
    }

    private void EmitConvertArithmeticResult(ExpressionType op, Type resultType)
    {
      switch (Type.GetTypeCode(resultType))
      {
        case TypeCode.SByte:
          this._ilg.Emit(LambdaCompiler.IsChecked(op) ? OpCodes.Conv_Ovf_I1 : OpCodes.Conv_I1);
          break;
        case TypeCode.Byte:
          this._ilg.Emit(LambdaCompiler.IsChecked(op) ? OpCodes.Conv_Ovf_U1 : OpCodes.Conv_U1);
          break;
        case TypeCode.Int16:
          this._ilg.Emit(LambdaCompiler.IsChecked(op) ? OpCodes.Conv_Ovf_I2 : OpCodes.Conv_I2);
          break;
        case TypeCode.UInt16:
          this._ilg.Emit(LambdaCompiler.IsChecked(op) ? OpCodes.Conv_Ovf_U2 : OpCodes.Conv_U2);
          break;
      }
    }

    private void EmitUnliftedEquality(ExpressionType op, Type type)
    {
      if (!type.IsPrimitive && type.IsValueType && !type.IsEnum)
        throw System.Linq.Expressions.Error.OperatorNotImplementedForType((object) op, (object) type);
      this._ilg.Emit(OpCodes.Ceq);
      if (op != ExpressionType.NotEqual)
        return;
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
    }

    private void EmitLiftedBinaryOp(ExpressionType op, Type leftType, Type rightType, Type resultType, bool liftedToNull)
    {
      switch (op)
      {
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.Divide:
        case ExpressionType.ExclusiveOr:
        case ExpressionType.LeftShift:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
        case ExpressionType.RightShift:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
          this.EmitLiftedBinaryArithmetic(op, leftType, rightType, resultType);
          break;
        case ExpressionType.And:
          if (leftType == typeof (bool?))
          {
            this.EmitLiftedBooleanAnd();
            break;
          }
          else
          {
            this.EmitLiftedBinaryArithmetic(op, leftType, rightType, resultType);
            break;
          }
        case ExpressionType.Equal:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.NotEqual:
          this.EmitLiftedRelational(op, leftType, rightType, resultType, liftedToNull);
          break;
        case ExpressionType.Or:
          if (leftType == typeof (bool?))
          {
            this.EmitLiftedBooleanOr();
            break;
          }
          else
          {
            this.EmitLiftedBinaryArithmetic(op, leftType, rightType, resultType);
            break;
          }
        default:
          throw ContractUtils.Unreachable;
      }
    }

    private void EmitLiftedRelational(ExpressionType op, Type leftType, Type rightType, Type resultType, bool liftedToNull)
    {
      Label label1 = this._ilg.DefineLabel();
      LocalBuilder local1 = this.GetLocal(leftType);
      LocalBuilder local2 = this.GetLocal(rightType);
      this._ilg.Emit(OpCodes.Stloc, local2);
      this._ilg.Emit(OpCodes.Stloc, local1);
      if (op == ExpressionType.Equal)
      {
        this._ilg.Emit(OpCodes.Ldloca, local1);
        ILGen.EmitHasValue(this._ilg, leftType);
        this._ilg.Emit(OpCodes.Ldc_I4_0);
        this._ilg.Emit(OpCodes.Ceq);
        this._ilg.Emit(OpCodes.Ldloca, local2);
        ILGen.EmitHasValue(this._ilg, rightType);
        this._ilg.Emit(OpCodes.Ldc_I4_0);
        this._ilg.Emit(OpCodes.Ceq);
        this._ilg.Emit(OpCodes.And);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Brtrue_S, label1);
        this._ilg.Emit(OpCodes.Pop);
        this._ilg.Emit(OpCodes.Ldloca, local1);
        ILGen.EmitHasValue(this._ilg, leftType);
        this._ilg.Emit(OpCodes.Ldloca, local2);
        ILGen.EmitHasValue(this._ilg, rightType);
        this._ilg.Emit(OpCodes.And);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Brfalse_S, label1);
        this._ilg.Emit(OpCodes.Pop);
      }
      else if (op == ExpressionType.NotEqual)
      {
        this._ilg.Emit(OpCodes.Ldloca, local1);
        ILGen.EmitHasValue(this._ilg, leftType);
        this._ilg.Emit(OpCodes.Ldloca, local2);
        ILGen.EmitHasValue(this._ilg, rightType);
        this._ilg.Emit(OpCodes.Or);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Brfalse_S, label1);
        this._ilg.Emit(OpCodes.Pop);
        this._ilg.Emit(OpCodes.Ldloca, local1);
        ILGen.EmitHasValue(this._ilg, leftType);
        this._ilg.Emit(OpCodes.Ldc_I4_0);
        this._ilg.Emit(OpCodes.Ceq);
        this._ilg.Emit(OpCodes.Ldloca, local2);
        ILGen.EmitHasValue(this._ilg, rightType);
        this._ilg.Emit(OpCodes.Ldc_I4_0);
        this._ilg.Emit(OpCodes.Ceq);
        this._ilg.Emit(OpCodes.Or);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Brtrue_S, label1);
        this._ilg.Emit(OpCodes.Pop);
      }
      else
      {
        this._ilg.Emit(OpCodes.Ldloca, local1);
        ILGen.EmitHasValue(this._ilg, leftType);
        this._ilg.Emit(OpCodes.Ldloca, local2);
        ILGen.EmitHasValue(this._ilg, rightType);
        this._ilg.Emit(OpCodes.And);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Brfalse_S, label1);
        this._ilg.Emit(OpCodes.Pop);
      }
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitGetValueOrDefault(this._ilg, leftType);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      ILGen.EmitGetValueOrDefault(this._ilg, rightType);
      this.FreeLocal(local1);
      this.FreeLocal(local2);
      this.EmitBinaryOperator(op, TypeUtils.GetNonNullableType(leftType), TypeUtils.GetNonNullableType(rightType), TypeUtils.GetNonNullableType(resultType), false);
      if (!liftedToNull)
        this._ilg.MarkLabel(label1);
      if (!TypeUtils.AreEquivalent(resultType, TypeUtils.GetNonNullableType(resultType)))
        ILGen.EmitConvertToType(this._ilg, TypeUtils.GetNonNullableType(resultType), resultType, true);
      if (!liftedToNull)
        return;
      Label label2 = this._ilg.DefineLabel();
      this._ilg.Emit(OpCodes.Br, label2);
      this._ilg.MarkLabel(label1);
      this._ilg.Emit(OpCodes.Pop);
      this._ilg.Emit(OpCodes.Ldnull);
      this._ilg.Emit(OpCodes.Unbox_Any, resultType);
      this._ilg.MarkLabel(label2);
    }

    private void EmitLiftedBinaryArithmetic(ExpressionType op, Type leftType, Type rightType, Type resultType)
    {
      bool flag1 = TypeUtils.IsNullableType(leftType);
      bool flag2 = TypeUtils.IsNullableType(rightType);
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      LocalBuilder local1 = this.GetLocal(leftType);
      LocalBuilder local2 = this.GetLocal(rightType);
      LocalBuilder local3 = this.GetLocal(resultType);
      this._ilg.Emit(OpCodes.Stloc, local2);
      this._ilg.Emit(OpCodes.Stloc, local1);
      if (flag1)
      {
        this._ilg.Emit(OpCodes.Ldloca, local1);
        ILGen.EmitHasValue(this._ilg, leftType);
        this._ilg.Emit(OpCodes.Brfalse_S, label1);
      }
      if (flag2)
      {
        this._ilg.Emit(OpCodes.Ldloca, local2);
        ILGen.EmitHasValue(this._ilg, rightType);
        this._ilg.Emit(OpCodes.Brfalse_S, label1);
      }
      if (flag1)
      {
        this._ilg.Emit(OpCodes.Ldloca, local1);
        ILGen.EmitGetValueOrDefault(this._ilg, leftType);
      }
      else
        this._ilg.Emit(OpCodes.Ldloc, local1);
      if (flag2)
      {
        this._ilg.Emit(OpCodes.Ldloca, local2);
        ILGen.EmitGetValueOrDefault(this._ilg, rightType);
      }
      else
        this._ilg.Emit(OpCodes.Ldloc, local2);
      this.FreeLocal(local1);
      this.FreeLocal(local2);
      this.EmitBinaryOperator(op, TypeUtils.GetNonNullableType(leftType), TypeUtils.GetNonNullableType(rightType), TypeUtils.GetNonNullableType(resultType), false);
      ConstructorInfo constructor = resultType.GetConstructor(new Type[1]
      {
        TypeUtils.GetNonNullableType(resultType)
      });
      this._ilg.Emit(OpCodes.Newobj, constructor);
      this._ilg.Emit(OpCodes.Stloc, local3);
      this._ilg.Emit(OpCodes.Br_S, label2);
      this._ilg.MarkLabel(label1);
      this._ilg.Emit(OpCodes.Ldloca, local3);
      this._ilg.Emit(OpCodes.Initobj, resultType);
      this._ilg.MarkLabel(label2);
      this._ilg.Emit(OpCodes.Ldloc, local3);
      this.FreeLocal(local3);
    }

    private void EmitLiftedBooleanAnd()
    {
      Type type = typeof (bool?);
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      Label label3 = this._ilg.DefineLabel();
      Label label4 = this._ilg.DefineLabel();
      Label label5 = this._ilg.DefineLabel();
      LocalBuilder local1 = this.GetLocal(type);
      LocalBuilder local2 = this.GetLocal(type);
      this._ilg.Emit(OpCodes.Stloc, local2);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse, label1);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitGetValueOrDefault(this._ilg, type);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brtrue, label2);
      this._ilg.MarkLabel(label1);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse_S, label3);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      this.FreeLocal(local2);
      ILGen.EmitGetValueOrDefault(this._ilg, type);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brtrue_S, label2);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse, label3);
      this._ilg.Emit(OpCodes.Ldc_I4_1);
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label2);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label4);
      ConstructorInfo constructor = type.GetConstructor(new Type[1]
      {
        typeof (bool)
      });
      this._ilg.Emit(OpCodes.Newobj, constructor);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this._ilg.Emit(OpCodes.Br, label5);
      this._ilg.MarkLabel(label3);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      this._ilg.Emit(OpCodes.Initobj, type);
      this._ilg.MarkLabel(label5);
      this._ilg.Emit(OpCodes.Ldloc, local1);
      this.FreeLocal(local1);
    }

    private void EmitLiftedBooleanOr()
    {
      Type type = typeof (bool?);
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      Label label3 = this._ilg.DefineLabel();
      Label label4 = this._ilg.DefineLabel();
      Label label5 = this._ilg.DefineLabel();
      LocalBuilder local1 = this.GetLocal(type);
      LocalBuilder local2 = this.GetLocal(type);
      this._ilg.Emit(OpCodes.Stloc, local2);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse, label1);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitGetValueOrDefault(this._ilg, type);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brfalse, label2);
      this._ilg.MarkLabel(label1);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse_S, label3);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      this.FreeLocal(local2);
      ILGen.EmitGetValueOrDefault(this._ilg, type);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brfalse_S, label2);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse, label3);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label2);
      this._ilg.Emit(OpCodes.Ldc_I4_1);
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label4);
      ConstructorInfo constructor = type.GetConstructor(new Type[1]
      {
        typeof (bool)
      });
      this._ilg.Emit(OpCodes.Newobj, constructor);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this._ilg.Emit(OpCodes.Br, label5);
      this._ilg.MarkLabel(label3);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      this._ilg.Emit(OpCodes.Initobj, type);
      this._ilg.MarkLabel(label5);
      this._ilg.Emit(OpCodes.Ldloc, local1);
      this.FreeLocal(local1);
    }

    private LabelInfo EnsureLabel(LabelTarget node)
    {
      LabelInfo labelInfo;
      if (!this._labelInfo.TryGetValue(node, out labelInfo))
        this._labelInfo.Add(node, labelInfo = new LabelInfo(this._ilg, node, false));
      return labelInfo;
    }

    private LabelInfo ReferenceLabel(LabelTarget node)
    {
      LabelInfo labelInfo = this.EnsureLabel(node);
      labelInfo.Reference(this._labelBlock);
      return labelInfo;
    }

    private LabelInfo DefineLabel(LabelTarget node)
    {
      if (node == null)
        return new LabelInfo(this._ilg, (LabelTarget) null, false);
      LabelInfo labelInfo = this.EnsureLabel(node);
      labelInfo.Define(this._labelBlock);
      return labelInfo;
    }

    private void PushLabelBlock(LabelScopeKind type)
    {
      this._labelBlock = new LabelScopeInfo(this._labelBlock, type);
    }

    private void PopLabelBlock(LabelScopeKind kind)
    {
      this._labelBlock = this._labelBlock.Parent;
    }

    private void EmitLabelExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      LabelExpression labelExpression = (LabelExpression) expr;
      LabelInfo info = (LabelInfo) null;
      if (this._labelBlock.Kind == LabelScopeKind.Block)
      {
        this._labelBlock.TryGetLabelInfo(labelExpression.Target, out info);
        if (info == null && this._labelBlock.Parent.Kind == LabelScopeKind.Switch)
          this._labelBlock.Parent.TryGetLabelInfo(labelExpression.Target, out info);
      }
      if (info == null)
        info = this.DefineLabel(labelExpression.Target);
      if (labelExpression.DefaultValue != null)
      {
        if (labelExpression.Target.Type == typeof (void))
        {
          this.EmitExpressionAsVoid(labelExpression.DefaultValue, flags);
        }
        else
        {
          flags = LambdaCompiler.UpdateEmitExpressionStartFlag(flags, LambdaCompiler.CompilationFlags.EmitExpressionStart);
          this.EmitExpression(labelExpression.DefaultValue, flags);
        }
      }
      info.Mark();
    }

    private void EmitGotoExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      GotoExpression gotoExpression = (GotoExpression) expr;
      LabelInfo labelInfo = this.ReferenceLabel(gotoExpression.Target);
      if ((flags & LambdaCompiler.CompilationFlags.EmitAsTailCallMask) != LambdaCompiler.CompilationFlags.EmitAsNoTail)
      {
        LambdaCompiler.CompilationFlags newValue = labelInfo.CanReturn ? LambdaCompiler.CompilationFlags.EmitAsTail : LambdaCompiler.CompilationFlags.EmitAsNoTail;
        flags = LambdaCompiler.UpdateEmitAsTailCallFlag(flags, newValue);
      }
      if (gotoExpression.Value != null)
      {
        if (gotoExpression.Target.Type == typeof (void))
        {
          this.EmitExpressionAsVoid(gotoExpression.Value, flags);
        }
        else
        {
          flags = LambdaCompiler.UpdateEmitExpressionStartFlag(flags, LambdaCompiler.CompilationFlags.EmitExpressionStart);
          this.EmitExpression(gotoExpression.Value, flags);
        }
      }
      labelInfo.EmitJump();
      this.EmitUnreachable((Expression) gotoExpression, flags);
    }

    private void EmitUnreachable(Expression node, LambdaCompiler.CompilationFlags flags)
    {
      if (!(node.Type != typeof (void)) || (flags & LambdaCompiler.CompilationFlags.EmitAsVoidType) != (LambdaCompiler.CompilationFlags) 0)
        return;
      ILGen.EmitDefault(this._ilg, node.Type);
    }

    private bool TryPushLabelBlock(Expression node)
    {
      switch (node.NodeType)
      {
        case ExpressionType.Conditional:
        case ExpressionType.Goto:
        case ExpressionType.Loop:
          this.PushLabelBlock(LabelScopeKind.Statement);
          return true;
        case ExpressionType.Convert:
          if (!(node.Type != typeof (void)))
          {
            this.PushLabelBlock(LabelScopeKind.Statement);
            return true;
          }
          else
            break;
        case ExpressionType.Block:
          if (!(node is SpilledExpressionBlock))
          {
            this.PushLabelBlock(LabelScopeKind.Block);
            if (this._labelBlock.Parent.Kind != LabelScopeKind.Switch)
              this.DefineBlockLabels(node);
            return true;
          }
          else
            break;
        case ExpressionType.Label:
          if (this._labelBlock.Kind == LabelScopeKind.Block)
          {
            LabelTarget target = ((LabelExpression) node).Target;
            if (this._labelBlock.ContainsTarget(target) || this._labelBlock.Parent.Kind == LabelScopeKind.Switch && this._labelBlock.Parent.ContainsTarget(target))
              return false;
          }
          this.PushLabelBlock(LabelScopeKind.Statement);
          return true;
        case ExpressionType.Switch:
          this.PushLabelBlock(LabelScopeKind.Switch);
          SwitchExpression switchExpression = (SwitchExpression) node;
          foreach (SwitchCase switchCase in switchExpression.Cases)
            this.DefineBlockLabels(switchCase.Body);
          this.DefineBlockLabels(switchExpression.DefaultBody);
          return true;
      }
      if (this._labelBlock.Kind == LabelScopeKind.Expression)
        return false;
      this.PushLabelBlock(LabelScopeKind.Expression);
      return true;
    }

    private void DefineBlockLabels(Expression node)
    {
      BlockExpression blockExpression = node as BlockExpression;
      if (blockExpression == null || blockExpression is SpilledExpressionBlock)
        return;
      int index = 0;
      for (int expressionCount = blockExpression.ExpressionCount; index < expressionCount; ++index)
      {
        LabelExpression labelExpression = blockExpression.GetExpression(index) as LabelExpression;
        if (labelExpression != null)
          this.DefineLabel(labelExpression.Target);
      }
    }

    private void AddReturnLabel(LambdaExpression lambda)
    {
      Expression node = lambda.Body;
label_1:
      switch (node.NodeType)
      {
        case ExpressionType.Block:
          BlockExpression blockExpression = (BlockExpression) node;
          for (int index = blockExpression.ExpressionCount - 1; index >= 0; --index)
          {
            node = blockExpression.GetExpression(index);
            if (LambdaCompiler.Significant(node))
              break;
          }
          goto label_1;
        case ExpressionType.Label:
          LabelTarget target = ((LabelExpression) node).Target;
          this._labelInfo.Add(target, new LabelInfo(this._ilg, target, TypeUtils.AreReferenceAssignable(lambda.ReturnType, target.Type)));
          break;
      }
    }

    private void InitializeMethod()
    {
      this.AddReturnLabel(this._lambda);
      this._boundConstants.EmitCacheConstants(this);
    }

    internal static void Compile(LambdaExpression lambda, MethodBuilder method, DebugInfoGenerator debugInfoGenerator)
    {
      AnalyzedTree tree = LambdaCompiler.AnalyzeLambda(ref lambda);
      tree.DebugInfoGenerator = debugInfoGenerator;
      new LambdaCompiler(tree, lambda, method).EmitLambdaBody();
    }

    private static AnalyzedTree AnalyzeLambda(ref LambdaExpression lambda)
    {
      lambda = StackSpiller.AnalyzeLambda(lambda);
      return VariableBinder.Bind(lambda);
    }

    internal LocalBuilder GetLocal(Type type)
    {
      LocalBuilder localBuilder;
      if (this._freeLocals.TryDequeue(type, out localBuilder))
        return localBuilder;
      else
        return this._ilg.DeclareLocal(type);
    }

    internal void FreeLocal(LocalBuilder local)
    {
      if (local == null)
        return;
      this._freeLocals.Enqueue(local.LocalType, local);
    }

    internal LocalBuilder GetNamedLocal(Type type, ParameterExpression variable)
    {
      LocalBuilder localBuilder = this._ilg.DeclareLocal(type);
      if (this.EmitDebugSymbols && variable.Name != null)
        this._tree.DebugInfoGenerator.SetLocalName(localBuilder, variable.Name);
      return localBuilder;
    }

    internal int GetLambdaArgument(int index)
    {
      return index + (this._hasClosureArgument ? 1 : 0) + (this._method.IsStatic ? 0 : 1);
    }

    internal void EmitLambdaArgument(int index)
    {
      ILGen.EmitLoadArg(this._ilg, this.GetLambdaArgument(index));
    }

    internal void EmitClosureArgument()
    {
      ILGen.EmitLoadArg(this._ilg, 0);
    }

    private Delegate CreateDelegate()
    {
      return this._method.CreateDelegate(this._lambda.Type, (object) new Closure(this._boundConstants.ToArray(), (object[]) null));
    }

    private static LambdaCompiler.CompilationFlags UpdateEmitAsTailCallFlag(LambdaCompiler.CompilationFlags flags, LambdaCompiler.CompilationFlags newValue)
    {
      LambdaCompiler.CompilationFlags compilationFlags = flags & LambdaCompiler.CompilationFlags.EmitAsTailCallMask;
      return flags ^ compilationFlags | newValue;
    }

    private static LambdaCompiler.CompilationFlags UpdateEmitExpressionStartFlag(LambdaCompiler.CompilationFlags flags, LambdaCompiler.CompilationFlags newValue)
    {
      LambdaCompiler.CompilationFlags compilationFlags = flags & LambdaCompiler.CompilationFlags.EmitExpressionStartMask;
      return flags ^ compilationFlags | newValue;
    }

    private static LambdaCompiler.CompilationFlags UpdateEmitAsTypeFlag(LambdaCompiler.CompilationFlags flags, LambdaCompiler.CompilationFlags newValue)
    {
      LambdaCompiler.CompilationFlags compilationFlags = flags & LambdaCompiler.CompilationFlags.EmitAsTypeMask;
      return flags ^ compilationFlags | newValue;
    }

    internal void EmitExpression(Expression node)
    {
      this.EmitExpression(node, LambdaCompiler.CompilationFlags.EmitExpressionStart | LambdaCompiler.CompilationFlags.EmitAsNoTail);
    }

    private void EmitExpressionAsVoid(Expression node)
    {
      this.EmitExpressionAsVoid(node, LambdaCompiler.CompilationFlags.EmitAsNoTail);
    }

    private void EmitExpressionAsVoid(Expression node, LambdaCompiler.CompilationFlags flags)
    {
      LambdaCompiler.CompilationFlags flags1 = this.EmitExpressionStart(node);
      switch (node.NodeType)
      {
        case ExpressionType.Assign:
          this.EmitAssign((BinaryExpression) node, LambdaCompiler.CompilationFlags.EmitAsVoidType);
          goto case ExpressionType.Default;
        case ExpressionType.Block:
          this.Emit((BlockExpression) node, LambdaCompiler.UpdateEmitAsTypeFlag(flags, LambdaCompiler.CompilationFlags.EmitAsVoidType));
          goto case ExpressionType.Default;
        case ExpressionType.Default:
        case ExpressionType.Constant:
        case ExpressionType.Parameter:
          this.EmitExpressionEnd(flags1);
          break;
        case ExpressionType.Goto:
          this.EmitGotoExpression(node, LambdaCompiler.UpdateEmitAsTypeFlag(flags, LambdaCompiler.CompilationFlags.EmitAsVoidType));
          goto case ExpressionType.Default;
        case ExpressionType.Throw:
          this.EmitThrow((UnaryExpression) node, LambdaCompiler.CompilationFlags.EmitAsVoidType);
          goto case ExpressionType.Default;
        default:
          if (node.Type == typeof (void))
          {
            this.EmitExpression(node, LambdaCompiler.UpdateEmitExpressionStartFlag(flags, LambdaCompiler.CompilationFlags.EmitNoExpressionStart));
            goto case ExpressionType.Default;
          }
          else
          {
            this.EmitExpression(node, LambdaCompiler.CompilationFlags.EmitNoExpressionStart | LambdaCompiler.CompilationFlags.EmitAsNoTail);
            this._ilg.Emit(OpCodes.Pop);
            goto case ExpressionType.Default;
          }
      }
    }

    private void EmitExpressionAsType(Expression node, Type type, LambdaCompiler.CompilationFlags flags)
    {
      if (type == typeof (void))
        this.EmitExpressionAsVoid(node, flags);
      else if (!TypeUtils.AreEquivalent(node.Type, type))
      {
        this.EmitExpression(node);
        this._ilg.Emit(OpCodes.Castclass, type);
      }
      else
        this.EmitExpression(node, LambdaCompiler.UpdateEmitExpressionStartFlag(flags, LambdaCompiler.CompilationFlags.EmitExpressionStart));
    }

    private LambdaCompiler.CompilationFlags EmitExpressionStart(Expression node)
    {
      return this.TryPushLabelBlock(node) ? LambdaCompiler.CompilationFlags.EmitExpressionStart : LambdaCompiler.CompilationFlags.EmitNoExpressionStart;
    }

    private void EmitExpressionEnd(LambdaCompiler.CompilationFlags flags)
    {
      if ((flags & LambdaCompiler.CompilationFlags.EmitExpressionStartMask) != LambdaCompiler.CompilationFlags.EmitExpressionStart)
        return;
      this.PopLabelBlock(this._labelBlock.Kind);
    }

    private void EmitInvocationExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      InvocationExpression invoke = (InvocationExpression) expr;
      if (invoke.LambdaOperand != null)
      {
        this.EmitInlinedInvoke(invoke, flags);
      }
      else
      {
        expr = invoke.Expression;
        if (typeof (LambdaExpression).IsAssignableFrom(expr.Type))
          expr = (Expression) Expression.Call(expr, expr.Type.GetMethod("Compile", new Type[0]));
        expr = (Expression) Expression.Call(expr, expr.Type.GetMethod("Invoke"), (IEnumerable<Expression>) invoke.Arguments);
        this.EmitExpression(expr);
      }
    }

    private void EmitInlinedInvoke(InvocationExpression invoke, LambdaCompiler.CompilationFlags flags)
    {
      LambdaExpression lambdaOperand = invoke.LambdaOperand;
      List<LambdaCompiler.WriteBack> list = this.EmitArguments((MethodBase) lambdaOperand.Type.GetMethod("Invoke"), (IArgumentProvider) invoke);
      LambdaCompiler lambdaCompiler = new LambdaCompiler(this, lambdaOperand);
      if (list.Count != 0)
        flags = LambdaCompiler.UpdateEmitAsTailCallFlag(flags, LambdaCompiler.CompilationFlags.EmitAsNoTail);
      lambdaCompiler.EmitLambdaBody(this._scope, true, flags);
      LambdaCompiler.EmitWriteBack((IList<LambdaCompiler.WriteBack>) list);
    }

    private void EmitIndexExpression(Expression expr)
    {
      IndexExpression node1 = (IndexExpression) expr;
      Type objectType = (Type) null;
      if (node1.Object != null)
        this.EmitInstance(node1.Object, objectType = node1.Object.Type);
      foreach (Expression node2 in node1.Arguments)
        this.EmitExpression(node2);
      this.EmitGetIndexCall(node1, objectType);
    }

    private void EmitIndexAssignment(BinaryExpression node, LambdaCompiler.CompilationFlags flags)
    {
      IndexExpression node1 = (IndexExpression) node.Left;
      LambdaCompiler.CompilationFlags compilationFlags = flags & LambdaCompiler.CompilationFlags.EmitAsTypeMask;
      Type objectType = (Type) null;
      if (node1.Object != null)
        this.EmitInstance(node1.Object, objectType = node1.Object.Type);
      foreach (Expression node2 in node1.Arguments)
        this.EmitExpression(node2);
      this.EmitExpression(node.Right);
      LocalBuilder local = (LocalBuilder) null;
      if (compilationFlags != LambdaCompiler.CompilationFlags.EmitAsVoidType)
      {
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Stloc, local = this.GetLocal(node.Type));
      }
      this.EmitSetIndexCall(node1, objectType);
      if (compilationFlags == LambdaCompiler.CompilationFlags.EmitAsVoidType)
        return;
      this._ilg.Emit(OpCodes.Ldloc, local);
      this.FreeLocal(local);
    }

    private void EmitGetIndexCall(IndexExpression node, Type objectType)
    {
      if (node.Indexer != (PropertyInfo) null)
      {
        MethodInfo getMethod = node.Indexer.GetGetMethod(true);
        this.EmitCall(objectType, getMethod);
      }
      else if (node.Arguments.Count != 1)
        this._ilg.Emit(OpCodes.Call, node.Object.Type.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public));
      else
        ILGen.EmitLoadElement(this._ilg, node.Type);
    }

    private void EmitSetIndexCall(IndexExpression node, Type objectType)
    {
      if (node.Indexer != (PropertyInfo) null)
      {
        MethodInfo setMethod = node.Indexer.GetSetMethod(true);
        this.EmitCall(objectType, setMethod);
      }
      else if (node.Arguments.Count != 1)
        this._ilg.Emit(OpCodes.Call, node.Object.Type.GetMethod("Set", BindingFlags.Instance | BindingFlags.Public));
      else
        ILGen.EmitStoreElement(this._ilg, node.Type);
    }

    private void EmitMethodCallExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      MethodCallExpression methodCallExpression = (MethodCallExpression) expr;
      this.EmitMethodCall(methodCallExpression.Object, methodCallExpression.Method, (IArgumentProvider) methodCallExpression, flags);
    }

    private void EmitMethodCallExpression(Expression expr)
    {
      this.EmitMethodCallExpression(expr, LambdaCompiler.CompilationFlags.EmitAsNoTail);
    }

    private void EmitMethodCall(Expression obj, MethodInfo method, IArgumentProvider methodCallExpr)
    {
      this.EmitMethodCall(obj, method, methodCallExpr, LambdaCompiler.CompilationFlags.EmitAsNoTail);
    }

    private void EmitMethodCall(Expression obj, MethodInfo method, IArgumentProvider methodCallExpr, LambdaCompiler.CompilationFlags flags)
    {
      Type objectType = (Type) null;
      if (!method.IsStatic)
        this.EmitInstance(obj, objectType = obj.Type);
      if (obj != null && obj.Type.IsValueType)
        this.EmitMethodCall(method, methodCallExpr, objectType);
      else
        this.EmitMethodCall(method, methodCallExpr, objectType, flags);
    }

    private void EmitMethodCall(MethodInfo mi, IArgumentProvider args, Type objectType)
    {
      this.EmitMethodCall(mi, args, objectType, LambdaCompiler.CompilationFlags.EmitAsNoTail);
    }

    private void EmitMethodCall(MethodInfo mi, IArgumentProvider args, Type objectType, LambdaCompiler.CompilationFlags flags)
    {
      List<LambdaCompiler.WriteBack> list = this.EmitArguments((MethodBase) mi, args);
      OpCode opcode = LambdaCompiler.UseVirtual(mi) ? OpCodes.Callvirt : OpCodes.Call;
      if (opcode == OpCodes.Callvirt && objectType.IsValueType)
        this._ilg.Emit(OpCodes.Constrained, objectType);
      if ((flags & LambdaCompiler.CompilationFlags.EmitAsTailCallMask) == LambdaCompiler.CompilationFlags.EmitAsTail && !LambdaCompiler.MethodHasByRefParameter(mi))
        this._ilg.Emit(OpCodes.Tailcall);
      if (mi.CallingConvention == CallingConventions.VarArgs)
        this._ilg.EmitCall(opcode, mi, ArgumentProviderOps.Map<Type>(args, (Func<Expression, Type>) (a => a.Type)));
      else
        this._ilg.Emit(opcode, mi);
      LambdaCompiler.EmitWriteBack((IList<LambdaCompiler.WriteBack>) list);
    }

    private static bool MethodHasByRefParameter(MethodInfo mi)
    {
      foreach (ParameterInfo pi in TypeExtensions.GetParametersCached((MethodBase) mi))
      {
        if (TypeExtensions.IsByRefParameter(pi))
          return true;
      }
      return false;
    }

    private void EmitCall(Type objectType, MethodInfo method)
    {
      if (method.CallingConvention == CallingConventions.VarArgs)
        throw System.Linq.Expressions.Error.UnexpectedVarArgsCall((object) method);
      OpCode opcode = LambdaCompiler.UseVirtual(method) ? OpCodes.Callvirt : OpCodes.Call;
      if (opcode == OpCodes.Callvirt && objectType.IsValueType)
        this._ilg.Emit(OpCodes.Constrained, objectType);
      this._ilg.Emit(opcode, method);
    }

    private static bool UseVirtual(MethodInfo mi)
    {
      return !mi.IsStatic && !mi.DeclaringType.IsValueType;
    }

    private List<LambdaCompiler.WriteBack> EmitArguments(MethodBase method, IArgumentProvider args)
    {
      return this.EmitArguments(method, args, 0);
    }

    private List<LambdaCompiler.WriteBack> EmitArguments(MethodBase method, IArgumentProvider args, int skipParameters)
    {
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached(method);
      List<LambdaCompiler.WriteBack> list = new List<LambdaCompiler.WriteBack>();
      int index = skipParameters;
      for (int length = parametersCached.Length; index < length; ++index)
      {
        ParameterInfo parameterInfo = parametersCached[index];
        Expression node = args.GetArgument(index - skipParameters);
        Type parameterType = parameterInfo.ParameterType;
        if (parameterType.IsByRef)
        {
          Type elementType = parameterType.GetElementType();
          LambdaCompiler.WriteBack writeBack = this.EmitAddressWriteBack(node, elementType);
          if (writeBack != null)
            list.Add(writeBack);
        }
        else
          this.EmitExpression(node);
      }
      return list;
    }

    private static void EmitWriteBack(IList<LambdaCompiler.WriteBack> writeBacks)
    {
      foreach (LambdaCompiler.WriteBack writeBack in (IEnumerable<LambdaCompiler.WriteBack>) writeBacks)
        writeBack();
    }

    private void EmitConstantExpression(Expression expr)
    {
      ConstantExpression constantExpression = (ConstantExpression) expr;
      this.EmitConstant(constantExpression.Value, constantExpression.Type);
    }

    private void EmitDynamicExpression(Expression expr)
    {
      if (!(this._method is DynamicMethod))
        throw System.Linq.Expressions.Error.CannotCompileDynamic();
      DynamicExpression dynamicExpression = (DynamicExpression) expr;
      CallSite callSite = CallSite.Create(dynamicExpression.DelegateType, dynamicExpression.Binder);
      Type type = callSite.GetType();
      MethodInfo method = dynamicExpression.DelegateType.GetMethod("Invoke");
      this.EmitConstant((object) callSite, type);
      this._ilg.Emit(OpCodes.Dup);
      LocalBuilder local = this.GetLocal(typeof (CallSite));
      this._ilg.Emit(OpCodes.Stloc, local);
      this._ilg.Emit(OpCodes.Ldfld, type.GetField("Target"));
      this._ilg.Emit(OpCodes.Ldloc, local);
      this.FreeLocal(local);
      List<LambdaCompiler.WriteBack> list = this.EmitArguments((MethodBase) method, (IArgumentProvider) dynamicExpression, 1);
      this._ilg.Emit(OpCodes.Callvirt, method);
      LambdaCompiler.EmitWriteBack((IList<LambdaCompiler.WriteBack>) list);
    }

    private void EmitNewExpression(Expression expr)
    {
      NewExpression newExpression = (NewExpression) expr;
      if (newExpression.Constructor != (ConstructorInfo) null)
      {
        List<LambdaCompiler.WriteBack> list = this.EmitArguments((MethodBase) newExpression.Constructor, (IArgumentProvider) newExpression);
        this._ilg.Emit(OpCodes.Newobj, newExpression.Constructor);
        LambdaCompiler.EmitWriteBack((IList<LambdaCompiler.WriteBack>) list);
      }
      else
      {
        LocalBuilder local = this.GetLocal(newExpression.Type);
        this._ilg.Emit(OpCodes.Ldloca, local);
        this._ilg.Emit(OpCodes.Initobj, newExpression.Type);
        this._ilg.Emit(OpCodes.Ldloc, local);
        this.FreeLocal(local);
      }
    }

    private void EmitTypeBinaryExpression(Expression expr)
    {
      TypeBinaryExpression typeIs = (TypeBinaryExpression) expr;
      if (typeIs.NodeType == ExpressionType.TypeEqual)
      {
        this.EmitExpression(typeIs.ReduceTypeEqual());
      }
      else
      {
        Type type = typeIs.Expression.Type;
        AnalyzeTypeIsResult analyzeTypeIsResult = ConstantCheck.AnalyzeTypeIs(typeIs);
        switch (analyzeTypeIsResult)
        {
          case AnalyzeTypeIsResult.KnownTrue:
          case AnalyzeTypeIsResult.KnownFalse:
            this.EmitExpressionAsVoid(typeIs.Expression);
            ILGen.EmitBoolean(this._ilg, analyzeTypeIsResult == AnalyzeTypeIsResult.KnownTrue);
            break;
          case AnalyzeTypeIsResult.KnownAssignable:
            if (TypeUtils.IsNullableType(type))
            {
              this.EmitAddress(typeIs.Expression, type);
              ILGen.EmitHasValue(this._ilg, type);
              break;
            }
            else
            {
              this.EmitExpression(typeIs.Expression);
              this._ilg.Emit(OpCodes.Ldnull);
              this._ilg.Emit(OpCodes.Ceq);
              this._ilg.Emit(OpCodes.Ldc_I4_0);
              this._ilg.Emit(OpCodes.Ceq);
              break;
            }
          default:
            this.EmitExpression(typeIs.Expression);
            if (type.IsValueType)
              this._ilg.Emit(OpCodes.Box, type);
            this._ilg.Emit(OpCodes.Isinst, typeIs.TypeOperand);
            this._ilg.Emit(OpCodes.Ldnull);
            this._ilg.Emit(OpCodes.Cgt_Un);
            break;
        }
      }
    }

    private void EmitVariableAssignment(BinaryExpression node, LambdaCompiler.CompilationFlags flags)
    {
      ParameterExpression variable = (ParameterExpression) node.Left;
      LambdaCompiler.CompilationFlags compilationFlags = flags & LambdaCompiler.CompilationFlags.EmitAsTypeMask;
      this.EmitExpression(node.Right);
      if (compilationFlags != LambdaCompiler.CompilationFlags.EmitAsVoidType)
        this._ilg.Emit(OpCodes.Dup);
      if (variable.IsByRef)
      {
        LocalBuilder local = this.GetLocal(variable.Type);
        this._ilg.Emit(OpCodes.Stloc, local);
        this._scope.EmitGet(variable);
        this._ilg.Emit(OpCodes.Ldloc, local);
        this.FreeLocal(local);
        ILGen.EmitStoreValueIndirect(this._ilg, variable.Type);
      }
      else
        this._scope.EmitSet(variable);
    }

    private void EmitAssignBinaryExpression(Expression expr)
    {
      this.EmitAssign((BinaryExpression) expr, LambdaCompiler.CompilationFlags.EmitAsDefaultType);
    }

    private void EmitAssign(BinaryExpression node, LambdaCompiler.CompilationFlags emitAs)
    {
      switch (node.Left.NodeType)
      {
        case ExpressionType.MemberAccess:
          this.EmitMemberAssignment(node, emitAs);
          break;
        case ExpressionType.Parameter:
          this.EmitVariableAssignment(node, emitAs);
          break;
        case ExpressionType.Index:
          this.EmitIndexAssignment(node, emitAs);
          break;
        default:
          throw System.Linq.Expressions.Error.InvalidLvalue((object) node.Left.NodeType);
      }
    }

    private void EmitParameterExpression(Expression expr)
    {
      ParameterExpression variable = (ParameterExpression) expr;
      this._scope.EmitGet(variable);
      if (!variable.IsByRef)
        return;
      ILGen.EmitLoadValueIndirect(this._ilg, variable.Type);
    }

    private void EmitLambdaExpression(Expression expr)
    {
      this.EmitDelegateConstruction((LambdaExpression) expr);
    }

    private void EmitRuntimeVariablesExpression(Expression expr)
    {
      this._scope.EmitVariableAccess(this, ((RuntimeVariablesExpression) expr).Variables);
    }

    private void EmitMemberAssignment(BinaryExpression node, LambdaCompiler.CompilationFlags flags)
    {
      MemberExpression memberExpression = (MemberExpression) node.Left;
      MemberInfo member = memberExpression.Member;
      Type objectType = (Type) null;
      if (memberExpression.Expression != null)
        this.EmitInstance(memberExpression.Expression, objectType = memberExpression.Expression.Type);
      this.EmitExpression(node.Right);
      LocalBuilder local = (LocalBuilder) null;
      LambdaCompiler.CompilationFlags compilationFlags = flags & LambdaCompiler.CompilationFlags.EmitAsTypeMask;
      if (compilationFlags != LambdaCompiler.CompilationFlags.EmitAsVoidType)
      {
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Stloc, local = this.GetLocal(node.Type));
      }
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          ILGen.EmitFieldSet(this._ilg, (FieldInfo) member);
          break;
        case MemberTypes.Property:
          this.EmitCall(objectType, ((PropertyInfo) member).GetSetMethod(true));
          break;
        default:
          throw System.Linq.Expressions.Error.InvalidMemberType((object) member.MemberType);
      }
      if (compilationFlags == LambdaCompiler.CompilationFlags.EmitAsVoidType)
        return;
      this._ilg.Emit(OpCodes.Ldloc, local);
      this.FreeLocal(local);
    }

    private void EmitMemberExpression(Expression expr)
    {
      MemberExpression memberExpression = (MemberExpression) expr;
      Type objectType = (Type) null;
      if (memberExpression.Expression != null)
        this.EmitInstance(memberExpression.Expression, objectType = memberExpression.Expression.Type);
      this.EmitMemberGet(memberExpression.Member, objectType);
    }

    private void EmitMemberGet(MemberInfo member, Type objectType)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          FieldInfo fi = (FieldInfo) member;
          if (fi.IsLiteral)
          {
            this.EmitConstant(fi.GetRawConstantValue(), fi.FieldType);
            break;
          }
          else
          {
            ILGen.EmitFieldGet(this._ilg, fi);
            break;
          }
        case MemberTypes.Property:
          this.EmitCall(objectType, ((PropertyInfo) member).GetGetMethod(true));
          break;
        default:
          throw ContractUtils.Unreachable;
      }
    }

    private void EmitInstance(Expression instance, Type type)
    {
      if (instance == null)
        return;
      if (type.IsValueType)
        this.EmitAddress(instance, type);
      else
        this.EmitExpression(instance);
    }

    private void EmitNewArrayExpression(Expression expr)
    {
      NewArrayExpression node = (NewArrayExpression) expr;
      if (node.NodeType == ExpressionType.NewArrayInit)
      {
        ILGen.EmitArray(this._ilg, node.Type.GetElementType(), node.Expressions.Count, (Action<int>) (index => this.EmitExpression(node.Expressions[index])));
      }
      else
      {
        ReadOnlyCollection<Expression> expressions = node.Expressions;
        for (int index = 0; index < expressions.Count; ++index)
        {
          Expression node1 = expressions[index];
          this.EmitExpression(node1);
          ILGen.EmitConvertToType(this._ilg, node1.Type, typeof (int), true);
        }
        ILGen.EmitArray(this._ilg, node.Type);
      }
    }

    private void EmitDebugInfoExpression(Expression expr)
    {
      if (!this.EmitDebugSymbols)
        return;
      DebugInfoExpression sequencePoint = (DebugInfoExpression) expr;
      if (sequencePoint.IsClear && this._sequencePointCleared)
        return;
      this._tree.DebugInfoGenerator.MarkSequencePoint(this._lambda, (MethodBase) this._method, this._ilg, sequencePoint);
      this._ilg.Emit(OpCodes.Nop);
      this._sequencePointCleared = sequencePoint.IsClear;
    }

    private static void EmitExtensionExpression(Expression expr)
    {
      throw System.Linq.Expressions.Error.ExtensionNotReduced();
    }

    private void EmitListInitExpression(Expression expr)
    {
      this.EmitListInit((ListInitExpression) expr);
    }

    private void EmitMemberInitExpression(Expression expr)
    {
      this.EmitMemberInit((MemberInitExpression) expr);
    }

    private void EmitBinding(MemberBinding binding, Type objectType)
    {
      switch (binding.BindingType)
      {
        case MemberBindingType.Assignment:
          this.EmitMemberAssignment((MemberAssignment) binding, objectType);
          break;
        case MemberBindingType.MemberBinding:
          this.EmitMemberMemberBinding((MemberMemberBinding) binding);
          break;
        case MemberBindingType.ListBinding:
          this.EmitMemberListBinding((MemberListBinding) binding);
          break;
        default:
          throw System.Linq.Expressions.Error.UnknownBindingType();
      }
    }

    private void EmitMemberAssignment(MemberAssignment binding, Type objectType)
    {
      this.EmitExpression(binding.Expression);
      FieldInfo field = binding.Member as FieldInfo;
      if (field != (FieldInfo) null)
      {
        this._ilg.Emit(OpCodes.Stfld, field);
      }
      else
      {
        PropertyInfo propertyInfo = binding.Member as PropertyInfo;
        if (!(propertyInfo != (PropertyInfo) null))
          throw System.Linq.Expressions.Error.UnhandledBinding();
        this.EmitCall(objectType, propertyInfo.GetSetMethod(true));
      }
    }

    private void EmitMemberMemberBinding(MemberMemberBinding binding)
    {
      Type memberType = LambdaCompiler.GetMemberType(binding.Member);
      if (binding.Member is PropertyInfo && memberType.IsValueType)
        throw System.Linq.Expressions.Error.CannotAutoInitializeValueTypeMemberThroughProperty((object) binding.Member);
      if (memberType.IsValueType)
        this.EmitMemberAddress(binding.Member, binding.Member.DeclaringType);
      else
        this.EmitMemberGet(binding.Member, binding.Member.DeclaringType);
      this.EmitMemberInit(binding.Bindings, false, memberType);
    }

    private void EmitMemberListBinding(MemberListBinding binding)
    {
      Type memberType = LambdaCompiler.GetMemberType(binding.Member);
      if (binding.Member is PropertyInfo && memberType.IsValueType)
        throw System.Linq.Expressions.Error.CannotAutoInitializeValueTypeElementThroughProperty((object) binding.Member);
      if (memberType.IsValueType)
        this.EmitMemberAddress(binding.Member, binding.Member.DeclaringType);
      else
        this.EmitMemberGet(binding.Member, binding.Member.DeclaringType);
      this.EmitListInit(binding.Initializers, false, memberType);
    }

    private void EmitMemberInit(MemberInitExpression init)
    {
      this.EmitExpression((Expression) init.NewExpression);
      LocalBuilder local = (LocalBuilder) null;
      if (init.NewExpression.Type.IsValueType && init.Bindings.Count > 0)
      {
        local = this._ilg.DeclareLocal(init.NewExpression.Type);
        this._ilg.Emit(OpCodes.Stloc, local);
        this._ilg.Emit(OpCodes.Ldloca, local);
      }
      this.EmitMemberInit(init.Bindings, local == null, init.NewExpression.Type);
      if (local == null)
        return;
      this._ilg.Emit(OpCodes.Ldloc, local);
    }

    private void EmitMemberInit(ReadOnlyCollection<MemberBinding> bindings, bool keepOnStack, Type objectType)
    {
      int count = bindings.Count;
      if (count == 0)
      {
        if (keepOnStack)
          return;
        this._ilg.Emit(OpCodes.Pop);
      }
      else
      {
        for (int index = 0; index < count; ++index)
        {
          if (keepOnStack || index < count - 1)
            this._ilg.Emit(OpCodes.Dup);
          this.EmitBinding(bindings[index], objectType);
        }
      }
    }

    private void EmitListInit(ListInitExpression init)
    {
      this.EmitExpression((Expression) init.NewExpression);
      LocalBuilder local = (LocalBuilder) null;
      if (init.NewExpression.Type.IsValueType)
      {
        local = this._ilg.DeclareLocal(init.NewExpression.Type);
        this._ilg.Emit(OpCodes.Stloc, local);
        this._ilg.Emit(OpCodes.Ldloca, local);
      }
      this.EmitListInit(init.Initializers, local == null, init.NewExpression.Type);
      if (local == null)
        return;
      this._ilg.Emit(OpCodes.Ldloc, local);
    }

    private void EmitListInit(ReadOnlyCollection<ElementInit> initializers, bool keepOnStack, Type objectType)
    {
      int count = initializers.Count;
      if (count == 0)
      {
        if (keepOnStack)
          return;
        this._ilg.Emit(OpCodes.Pop);
      }
      else
      {
        for (int index = 0; index < count; ++index)
        {
          if (keepOnStack || index < count - 1)
            this._ilg.Emit(OpCodes.Dup);
          this.EmitMethodCall(initializers[index].AddMethod, (IArgumentProvider) initializers[index], objectType);
          if (initializers[index].AddMethod.ReturnType != typeof (void))
            this._ilg.Emit(OpCodes.Pop);
        }
      }
    }

    private static Type GetMemberType(MemberInfo member)
    {
      FieldInfo fieldInfo = member as FieldInfo;
      if (fieldInfo != (FieldInfo) null)
        return fieldInfo.FieldType;
      PropertyInfo propertyInfo = member as PropertyInfo;
      if (propertyInfo != (PropertyInfo) null)
        return propertyInfo.PropertyType;
      else
        throw System.Linq.Expressions.Error.MemberNotFieldOrProperty((object) member);
    }

    internal static void ValidateLift(IList<ParameterExpression> variables, IList<Expression> arguments)
    {
      if (variables.Count != arguments.Count)
        throw System.Linq.Expressions.Error.IncorrectNumberOfIndexes();
      int index = 0;
      for (int count = variables.Count; index < count; ++index)
      {
        if (!TypeUtils.AreReferenceAssignable(variables[index].Type, TypeUtils.GetNonNullableType(arguments[index].Type)))
          throw System.Linq.Expressions.Error.ArgumentTypesMustMatch();
      }
    }

    private void EmitLift(ExpressionType nodeType, Type resultType, MethodCallExpression mc, ParameterExpression[] paramList, Expression[] argList)
    {
      switch (nodeType)
      {
        case ExpressionType.Equal:
        case ExpressionType.NotEqual:
          if (!TypeUtils.AreEquivalent(resultType, TypeUtils.GetNullableType(mc.Type)))
          {
            Label label1 = this._ilg.DefineLabel();
            Label label2 = this._ilg.DefineLabel();
            Label label3 = this._ilg.DefineLabel();
            LocalBuilder local1 = this._ilg.DeclareLocal(typeof (bool));
            LocalBuilder local2 = this._ilg.DeclareLocal(typeof (bool));
            this._ilg.Emit(OpCodes.Ldc_I4_0);
            this._ilg.Emit(OpCodes.Stloc, local1);
            this._ilg.Emit(OpCodes.Ldc_I4_1);
            this._ilg.Emit(OpCodes.Stloc, local2);
            int index = 0;
            for (int length = paramList.Length; index < length; ++index)
            {
              ParameterExpression variable = paramList[index];
              Expression node = argList[index];
              this._scope.AddLocal(this, variable);
              if (TypeUtils.IsNullableType(node.Type))
              {
                this.EmitAddress(node, node.Type);
                this._ilg.Emit(OpCodes.Dup);
                ILGen.EmitHasValue(this._ilg, node.Type);
                this._ilg.Emit(OpCodes.Ldc_I4_0);
                this._ilg.Emit(OpCodes.Ceq);
                this._ilg.Emit(OpCodes.Dup);
                this._ilg.Emit(OpCodes.Ldloc, local1);
                this._ilg.Emit(OpCodes.Or);
                this._ilg.Emit(OpCodes.Stloc, local1);
                this._ilg.Emit(OpCodes.Ldloc, local2);
                this._ilg.Emit(OpCodes.And);
                this._ilg.Emit(OpCodes.Stloc, local2);
                ILGen.EmitGetValueOrDefault(this._ilg, node.Type);
              }
              else
              {
                this.EmitExpression(node);
                if (!node.Type.IsValueType)
                {
                  this._ilg.Emit(OpCodes.Dup);
                  this._ilg.Emit(OpCodes.Ldnull);
                  this._ilg.Emit(OpCodes.Ceq);
                  this._ilg.Emit(OpCodes.Dup);
                  this._ilg.Emit(OpCodes.Ldloc, local1);
                  this._ilg.Emit(OpCodes.Or);
                  this._ilg.Emit(OpCodes.Stloc, local1);
                  this._ilg.Emit(OpCodes.Ldloc, local2);
                  this._ilg.Emit(OpCodes.And);
                  this._ilg.Emit(OpCodes.Stloc, local2);
                }
                else
                {
                  this._ilg.Emit(OpCodes.Ldc_I4_0);
                  this._ilg.Emit(OpCodes.Stloc, local2);
                }
              }
              this._scope.EmitSet(variable);
            }
            this._ilg.Emit(OpCodes.Ldloc, local2);
            this._ilg.Emit(OpCodes.Brtrue, label2);
            this._ilg.Emit(OpCodes.Ldloc, local1);
            this._ilg.Emit(OpCodes.Brtrue, label3);
            this.EmitMethodCallExpression((Expression) mc);
            if (TypeUtils.IsNullableType(resultType) && !TypeUtils.AreEquivalent(resultType, mc.Type))
            {
              ConstructorInfo constructor = resultType.GetConstructor(new Type[1]
              {
                mc.Type
              });
              this._ilg.Emit(OpCodes.Newobj, constructor);
            }
            this._ilg.Emit(OpCodes.Br_S, label1);
            this._ilg.MarkLabel(label2);
            ILGen.EmitBoolean(this._ilg, nodeType == ExpressionType.Equal);
            this._ilg.Emit(OpCodes.Br_S, label1);
            this._ilg.MarkLabel(label3);
            ILGen.EmitBoolean(this._ilg, nodeType == ExpressionType.NotEqual);
            this._ilg.MarkLabel(label1);
            return;
          }
          else
            break;
      }
      Label label4 = this._ilg.DefineLabel();
      Label label5 = this._ilg.DefineLabel();
      LocalBuilder local3 = this._ilg.DeclareLocal(typeof (bool));
      int index1 = 0;
      for (int length = paramList.Length; index1 < length; ++index1)
      {
        ParameterExpression variable = paramList[index1];
        Expression node = argList[index1];
        if (TypeUtils.IsNullableType(node.Type))
        {
          this._scope.AddLocal(this, variable);
          this.EmitAddress(node, node.Type);
          this._ilg.Emit(OpCodes.Dup);
          ILGen.EmitHasValue(this._ilg, node.Type);
          this._ilg.Emit(OpCodes.Ldc_I4_0);
          this._ilg.Emit(OpCodes.Ceq);
          this._ilg.Emit(OpCodes.Stloc, local3);
          ILGen.EmitGetValueOrDefault(this._ilg, node.Type);
          this._scope.EmitSet(variable);
        }
        else
        {
          this._scope.AddLocal(this, variable);
          this.EmitExpression(node);
          if (!node.Type.IsValueType)
          {
            this._ilg.Emit(OpCodes.Dup);
            this._ilg.Emit(OpCodes.Ldnull);
            this._ilg.Emit(OpCodes.Ceq);
            this._ilg.Emit(OpCodes.Stloc, local3);
          }
          this._scope.EmitSet(variable);
        }
        this._ilg.Emit(OpCodes.Ldloc, local3);
        this._ilg.Emit(OpCodes.Brtrue, label5);
      }
      this.EmitMethodCallExpression((Expression) mc);
      if (TypeUtils.IsNullableType(resultType) && !TypeUtils.AreEquivalent(resultType, mc.Type))
      {
        ConstructorInfo constructor = resultType.GetConstructor(new Type[1]
        {
          mc.Type
        });
        this._ilg.Emit(OpCodes.Newobj, constructor);
      }
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label5);
      if (TypeUtils.AreEquivalent(resultType, TypeUtils.GetNullableType(mc.Type)))
      {
        if (resultType.IsValueType)
        {
          LocalBuilder local1 = this.GetLocal(resultType);
          this._ilg.Emit(OpCodes.Ldloca, local1);
          this._ilg.Emit(OpCodes.Initobj, resultType);
          this._ilg.Emit(OpCodes.Ldloc, local1);
          this.FreeLocal(local1);
        }
        else
          this._ilg.Emit(OpCodes.Ldnull);
      }
      else
      {
        switch (nodeType)
        {
          case ExpressionType.GreaterThan:
          case ExpressionType.GreaterThanOrEqual:
          case ExpressionType.LessThan:
          case ExpressionType.LessThanOrEqual:
            this._ilg.Emit(OpCodes.Ldc_I4_0);
            break;
          default:
            throw System.Linq.Expressions.Error.UnknownLiftType((object) nodeType);
        }
      }
      this._ilg.MarkLabel(label4);
    }

    private void EmitExpression(Expression node, LambdaCompiler.CompilationFlags flags)
    {
      bool flag = (flags & LambdaCompiler.CompilationFlags.EmitExpressionStartMask) == LambdaCompiler.CompilationFlags.EmitExpressionStart;
      LambdaCompiler.CompilationFlags flags1 = flag ? this.EmitExpressionStart(node) : LambdaCompiler.CompilationFlags.EmitNoExpressionStart;
      flags &= LambdaCompiler.CompilationFlags.EmitAsTailCallMask;
      switch (node.NodeType)
      {
        case ExpressionType.Add:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.AddChecked:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.And:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.AndAlso:
          this.EmitAndAlsoBinaryExpression(node, flags);
          break;
        case ExpressionType.ArrayLength:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.ArrayIndex:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.Call:
          this.EmitMethodCallExpression(node, flags);
          break;
        case ExpressionType.Coalesce:
          this.EmitCoalesceBinaryExpression(node);
          break;
        case ExpressionType.Conditional:
          this.EmitConditionalExpression(node, flags);
          break;
        case ExpressionType.Constant:
          this.EmitConstantExpression(node);
          break;
        case ExpressionType.Convert:
          this.EmitConvertUnaryExpression(node, flags);
          break;
        case ExpressionType.ConvertChecked:
          this.EmitConvertUnaryExpression(node, flags);
          break;
        case ExpressionType.Divide:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.Equal:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.ExclusiveOr:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.GreaterThan:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.GreaterThanOrEqual:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.Invoke:
          this.EmitInvocationExpression(node, flags);
          break;
        case ExpressionType.Lambda:
          this.EmitLambdaExpression(node);
          break;
        case ExpressionType.LeftShift:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.LessThan:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.LessThanOrEqual:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.ListInit:
          this.EmitListInitExpression(node);
          break;
        case ExpressionType.MemberAccess:
          this.EmitMemberExpression(node);
          break;
        case ExpressionType.MemberInit:
          this.EmitMemberInitExpression(node);
          break;
        case ExpressionType.Modulo:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.Multiply:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.MultiplyChecked:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.Negate:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.UnaryPlus:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.NegateChecked:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.New:
          this.EmitNewExpression(node);
          break;
        case ExpressionType.NewArrayInit:
          this.EmitNewArrayExpression(node);
          break;
        case ExpressionType.NewArrayBounds:
          this.EmitNewArrayExpression(node);
          break;
        case ExpressionType.Not:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.NotEqual:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.Or:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.OrElse:
          this.EmitOrElseBinaryExpression(node, flags);
          break;
        case ExpressionType.Parameter:
          this.EmitParameterExpression(node);
          break;
        case ExpressionType.Power:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.Quote:
          this.EmitQuoteUnaryExpression(node);
          break;
        case ExpressionType.RightShift:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.Subtract:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.SubtractChecked:
          this.EmitBinaryExpression(node, flags);
          break;
        case ExpressionType.TypeAs:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.TypeIs:
          this.EmitTypeBinaryExpression(node);
          break;
        case ExpressionType.Assign:
          this.EmitAssignBinaryExpression(node);
          break;
        case ExpressionType.Block:
          this.EmitBlockExpression(node, flags);
          break;
        case ExpressionType.DebugInfo:
          this.EmitDebugInfoExpression(node);
          break;
        case ExpressionType.Decrement:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.Dynamic:
          this.EmitDynamicExpression(node);
          break;
        case ExpressionType.Default:
          this.EmitDefaultExpression(node);
          break;
        case ExpressionType.Extension:
          LambdaCompiler.EmitExtensionExpression(node);
          break;
        case ExpressionType.Goto:
          this.EmitGotoExpression(node, flags);
          break;
        case ExpressionType.Increment:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.Index:
          this.EmitIndexExpression(node);
          break;
        case ExpressionType.Label:
          this.EmitLabelExpression(node, flags);
          break;
        case ExpressionType.RuntimeVariables:
          this.EmitRuntimeVariablesExpression(node);
          break;
        case ExpressionType.Loop:
          this.EmitLoopExpression(node);
          break;
        case ExpressionType.Switch:
          this.EmitSwitchExpression(node, flags);
          break;
        case ExpressionType.Throw:
          this.EmitThrowUnaryExpression(node);
          break;
        case ExpressionType.Try:
          this.EmitTryExpression(node);
          break;
        case ExpressionType.Unbox:
          this.EmitUnboxUnaryExpression(node);
          break;
        case ExpressionType.TypeEqual:
          this.EmitTypeBinaryExpression(node);
          break;
        case ExpressionType.OnesComplement:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.IsTrue:
          this.EmitUnaryExpression(node, flags);
          break;
        case ExpressionType.IsFalse:
          this.EmitUnaryExpression(node, flags);
          break;
        default:
          throw ContractUtils.Unreachable;
      }
      if (!flag)
        return;
      this.EmitExpressionEnd(flags1);
    }

    private static bool IsChecked(ExpressionType op)
    {
      switch (op)
      {
        case ExpressionType.NegateChecked:
        case ExpressionType.SubtractChecked:
        case ExpressionType.AddAssignChecked:
        case ExpressionType.MultiplyAssignChecked:
        case ExpressionType.SubtractAssignChecked:
        case ExpressionType.AddChecked:
        case ExpressionType.ConvertChecked:
        case ExpressionType.MultiplyChecked:
          return true;
        default:
          return false;
      }
    }

    private void EmitClosureCreation(LambdaCompiler inner)
    {
      bool flag1 = inner._scope.NeedsClosure;
      bool flag2 = inner._boundConstants.Count > 0;
      if (!flag1 && !flag2)
      {
        ILGen.EmitNull(this._ilg);
      }
      else
      {
        if (flag2)
          this._boundConstants.EmitConstant(this, (object) inner._boundConstants.ToArray(), typeof (object[]));
        else
          ILGen.EmitNull(this._ilg);
        if (flag1)
          this._scope.EmitGet(this._scope.NearestHoistedLocals.SelfVariable);
        else
          ILGen.EmitNull(this._ilg);
        ILGen.EmitNew(this._ilg, typeof (Closure).GetConstructor(new Type[2]
        {
          typeof (object[]),
          typeof (object[])
        }));
      }
    }

    private void EmitDelegateConstruction(LambdaCompiler inner)
    {
      Type type = inner._lambda.Type;
      DynamicMethod dynamicMethod = inner._method as DynamicMethod;
      if ((MethodInfo) dynamicMethod != (MethodInfo) null)
      {
        this._boundConstants.EmitConstant(this, (object) dynamicMethod, typeof (MethodInfo));
        ILGen.EmitType(this._ilg, type);
        this.EmitClosureCreation(inner);
        this._ilg.Emit(OpCodes.Callvirt, typeof (MethodInfo).GetMethod("CreateDelegate", new Type[2]
        {
          typeof (Type),
          typeof (object)
        }));
        this._ilg.Emit(OpCodes.Castclass, type);
      }
      else
      {
        this.EmitClosureCreation(inner);
        this._ilg.Emit(OpCodes.Ldftn, inner._method);
        this._ilg.Emit(OpCodes.Newobj, (ConstructorInfo) type.GetMember(".ctor")[0]);
      }
    }

    private void EmitDelegateConstruction(LambdaExpression lambda)
    {
      LambdaCompiler inner;
      if (this._method is DynamicMethod)
      {
        inner = new LambdaCompiler(this._tree, lambda);
      }
      else
      {
        MethodBuilder method = this._typeBuilder.DefineMethod(string.IsNullOrEmpty(lambda.Name) ? LambdaCompiler.GetUniqueMethodName() : lambda.Name, MethodAttributes.Private | MethodAttributes.Static);
        inner = new LambdaCompiler(this._tree, lambda, method);
      }
      inner.EmitLambdaBody(this._scope, false, LambdaCompiler.CompilationFlags.EmitAsNoTail);
      this.EmitDelegateConstruction(inner);
    }

    private static Type[] GetParameterTypes(LambdaExpression lambda)
    {
      return CollectionExtensions.Map<ParameterExpression, Type>((ICollection<ParameterExpression>) lambda.Parameters, (Func<ParameterExpression, Type>) (p =>
      {
        if (!p.IsByRef)
          return p.Type;
        else
          return p.Type.MakeByRefType();
      }));
    }

    private static string GetUniqueMethodName()
    {
      return "<ExpressionCompilerImplementationDetails>{" + (object) Interlocked.Increment(ref LambdaCompiler._Counter) + "}lambda_method";
    }

    private void EmitLambdaBody()
    {
      this.EmitLambdaBody((CompilerScope) null, false, this._lambda.TailCall ? LambdaCompiler.CompilationFlags.EmitAsTail : LambdaCompiler.CompilationFlags.EmitAsNoTail);
    }

    private void EmitLambdaBody(CompilerScope parent, bool inlined, LambdaCompiler.CompilationFlags flags)
    {
      this._scope.Enter(this, parent);
      if (inlined)
      {
        for (int index = this._lambda.Parameters.Count - 1; index >= 0; --index)
          this._scope.EmitSet(this._lambda.Parameters[index]);
      }
      flags = LambdaCompiler.UpdateEmitExpressionStartFlag(flags, LambdaCompiler.CompilationFlags.EmitExpressionStart);
      if (this._lambda.ReturnType == typeof (void))
        this.EmitExpressionAsVoid(this._lambda.Body, flags);
      else
        this.EmitExpression(this._lambda.Body, flags);
      if (!inlined)
        this._ilg.Emit(OpCodes.Ret);
      this._scope.Exit();
      foreach (LabelInfo labelInfo in this._labelInfo.Values)
        labelInfo.ValidateFinish();
    }

    private void EmitConditionalExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      ConditionalExpression conditionalExpression = (ConditionalExpression) expr;
      Label label1 = this._ilg.DefineLabel();
      this.EmitExpressionAndBranch(false, conditionalExpression.Test, label1);
      this.EmitExpressionAsType(conditionalExpression.IfTrue, conditionalExpression.Type, flags);
      if (LambdaCompiler.NotEmpty(conditionalExpression.IfFalse))
      {
        Label label2 = this._ilg.DefineLabel();
        if ((flags & LambdaCompiler.CompilationFlags.EmitAsTailCallMask) == LambdaCompiler.CompilationFlags.EmitAsTail)
          this._ilg.Emit(OpCodes.Ret);
        else
          this._ilg.Emit(OpCodes.Br, label2);
        this._ilg.MarkLabel(label1);
        this.EmitExpressionAsType(conditionalExpression.IfFalse, conditionalExpression.Type, flags);
        this._ilg.MarkLabel(label2);
      }
      else
        this._ilg.MarkLabel(label1);
    }

    private static bool NotEmpty(Expression node)
    {
      DefaultExpression defaultExpression = node as DefaultExpression;
      return defaultExpression == null || defaultExpression.Type != typeof (void);
    }

    private static bool Significant(Expression node)
    {
      BlockExpression blockExpression = node as BlockExpression;
      if (blockExpression != null)
      {
        for (int index = 0; index < blockExpression.ExpressionCount; ++index)
        {
          if (LambdaCompiler.Significant(blockExpression.GetExpression(index)))
            return true;
        }
        return false;
      }
      else if (LambdaCompiler.NotEmpty(node))
        return !(node is DebugInfoExpression);
      else
        return false;
    }

    private void EmitCoalesceBinaryExpression(Expression expr)
    {
      BinaryExpression b = (BinaryExpression) expr;
      if (TypeUtils.IsNullableType(b.Left.Type))
      {
        this.EmitNullableCoalesce(b);
      }
      else
      {
        if (b.Left.Type.IsValueType)
          throw System.Linq.Expressions.Error.CoalesceUsedOnNonNullType();
        if (b.Conversion != null)
          this.EmitLambdaReferenceCoalesce(b);
        else
          this.EmitReferenceCoalesceWithoutConversion(b);
      }
    }

    private void EmitNullableCoalesce(BinaryExpression b)
    {
      LocalBuilder local = this.GetLocal(b.Left.Type);
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      this.EmitExpression(b.Left);
      this._ilg.Emit(OpCodes.Stloc, local);
      this._ilg.Emit(OpCodes.Ldloca, local);
      ILGen.EmitHasValue(this._ilg, b.Left.Type);
      this._ilg.Emit(OpCodes.Brfalse, label1);
      Type nonNullableType = TypeUtils.GetNonNullableType(b.Left.Type);
      if (b.Conversion != null)
      {
        ParameterExpression parameterExpression = b.Conversion.Parameters[0];
        this.EmitLambdaExpression((Expression) b.Conversion);
        if (!parameterExpression.Type.IsAssignableFrom(b.Left.Type))
        {
          this._ilg.Emit(OpCodes.Ldloca, local);
          ILGen.EmitGetValueOrDefault(this._ilg, b.Left.Type);
        }
        else
          this._ilg.Emit(OpCodes.Ldloc, local);
        this._ilg.Emit(OpCodes.Callvirt, b.Conversion.Type.GetMethod("Invoke"));
      }
      else if (!TypeUtils.AreEquivalent(b.Type, nonNullableType))
      {
        this._ilg.Emit(OpCodes.Ldloca, local);
        ILGen.EmitGetValueOrDefault(this._ilg, b.Left.Type);
        ILGen.EmitConvertToType(this._ilg, nonNullableType, b.Type, true);
      }
      else
      {
        this._ilg.Emit(OpCodes.Ldloca, local);
        ILGen.EmitGetValueOrDefault(this._ilg, b.Left.Type);
      }
      this.FreeLocal(local);
      this._ilg.Emit(OpCodes.Br, label2);
      this._ilg.MarkLabel(label1);
      this.EmitExpression(b.Right);
      if (!TypeUtils.AreEquivalent(b.Right.Type, b.Type))
        ILGen.EmitConvertToType(this._ilg, b.Right.Type, b.Type, true);
      this._ilg.MarkLabel(label2);
    }

    private void EmitLambdaReferenceCoalesce(BinaryExpression b)
    {
      LocalBuilder local = this.GetLocal(b.Left.Type);
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      this.EmitExpression(b.Left);
      this._ilg.Emit(OpCodes.Dup);
      this._ilg.Emit(OpCodes.Stloc, local);
      this._ilg.Emit(OpCodes.Ldnull);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brfalse, label2);
      this.EmitExpression(b.Right);
      this._ilg.Emit(OpCodes.Br, label1);
      this._ilg.MarkLabel(label2);
      this.EmitLambdaExpression((Expression) b.Conversion);
      this._ilg.Emit(OpCodes.Ldloc, local);
      this.FreeLocal(local);
      this._ilg.Emit(OpCodes.Callvirt, b.Conversion.Type.GetMethod("Invoke"));
      this._ilg.MarkLabel(label1);
    }

    private void EmitReferenceCoalesceWithoutConversion(BinaryExpression b)
    {
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      this.EmitExpression(b.Left);
      this._ilg.Emit(OpCodes.Dup);
      this._ilg.Emit(OpCodes.Ldnull);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brfalse, label2);
      this._ilg.Emit(OpCodes.Pop);
      this.EmitExpression(b.Right);
      if (!TypeUtils.AreEquivalent(b.Right.Type, b.Type))
      {
        if (b.Right.Type.IsValueType)
          this._ilg.Emit(OpCodes.Box, b.Right.Type);
        this._ilg.Emit(OpCodes.Castclass, b.Type);
      }
      this._ilg.Emit(OpCodes.Br_S, label1);
      this._ilg.MarkLabel(label2);
      if (!TypeUtils.AreEquivalent(b.Left.Type, b.Type))
        this._ilg.Emit(OpCodes.Castclass, b.Type);
      this._ilg.MarkLabel(label1);
    }

    private void EmitLiftedAndAlso(BinaryExpression b)
    {
      Type type = typeof (bool?);
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      Label label3 = this._ilg.DefineLabel();
      Label label4 = this._ilg.DefineLabel();
      Label label5 = this._ilg.DefineLabel();
      LocalBuilder local1 = this.GetLocal(type);
      LocalBuilder local2 = this.GetLocal(type);
      this.EmitExpression(b.Left);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse, label1);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitGetValueOrDefault(this._ilg, type);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brtrue, label2);
      this._ilg.MarkLabel(label1);
      this.EmitExpression(b.Right);
      this._ilg.Emit(OpCodes.Stloc, local2);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse_S, label3);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      ILGen.EmitGetValueOrDefault(this._ilg, type);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brtrue_S, label2);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse, label3);
      this._ilg.Emit(OpCodes.Ldc_I4_1);
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label2);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label4);
      ConstructorInfo constructor = type.GetConstructor(new Type[1]
      {
        typeof (bool)
      });
      this._ilg.Emit(OpCodes.Newobj, constructor);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this._ilg.Emit(OpCodes.Br, label5);
      this._ilg.MarkLabel(label3);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      this._ilg.Emit(OpCodes.Initobj, type);
      this._ilg.MarkLabel(label5);
      this._ilg.Emit(OpCodes.Ldloc, local1);
      this.FreeLocal(local1);
      this.FreeLocal(local2);
    }

    private void EmitMethodAndAlso(BinaryExpression b, LambdaCompiler.CompilationFlags flags)
    {
      Label label = this._ilg.DefineLabel();
      this.EmitExpression(b.Left);
      this._ilg.Emit(OpCodes.Dup);
      MethodInfo booleanOperator = TypeUtils.GetBooleanOperator(b.Method.DeclaringType, "op_False");
      this._ilg.Emit(OpCodes.Call, booleanOperator);
      this._ilg.Emit(OpCodes.Brtrue, label);
      LocalBuilder local1 = this.GetLocal(b.Left.Type);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this.EmitExpression(b.Right);
      LocalBuilder local2 = this.GetLocal(b.Right.Type);
      this._ilg.Emit(OpCodes.Stloc, local2);
      this._ilg.Emit(OpCodes.Ldloc, local1);
      this._ilg.Emit(OpCodes.Ldloc, local2);
      if ((flags & LambdaCompiler.CompilationFlags.EmitAsTailCallMask) == LambdaCompiler.CompilationFlags.EmitAsTail)
        this._ilg.Emit(OpCodes.Tailcall);
      this._ilg.Emit(OpCodes.Call, b.Method);
      this.FreeLocal(local1);
      this.FreeLocal(local2);
      this._ilg.MarkLabel(label);
    }

    private void EmitUnliftedAndAlso(BinaryExpression b)
    {
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      this.EmitExpressionAndBranch(false, b.Left, label1);
      this.EmitExpression(b.Right);
      this._ilg.Emit(OpCodes.Br, label2);
      this._ilg.MarkLabel(label1);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.MarkLabel(label2);
    }

    private void EmitAndAlsoBinaryExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      BinaryExpression b = (BinaryExpression) expr;
      if (b.Method != (MethodInfo) null && !b.IsLiftedLogical)
        this.EmitMethodAndAlso(b, flags);
      else if (b.Left.Type == typeof (bool?))
        this.EmitLiftedAndAlso(b);
      else if (b.IsLiftedLogical)
        this.EmitExpression(b.ReduceUserdefinedLifted());
      else
        this.EmitUnliftedAndAlso(b);
    }

    private void EmitLiftedOrElse(BinaryExpression b)
    {
      Type type = typeof (bool?);
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      Label label3 = this._ilg.DefineLabel();
      Label label4 = this._ilg.DefineLabel();
      Label label5 = this._ilg.DefineLabel();
      LocalBuilder local1 = this.GetLocal(type);
      LocalBuilder local2 = this.GetLocal(type);
      this.EmitExpression(b.Left);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse, label1);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitGetValueOrDefault(this._ilg, type);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brfalse, label2);
      this._ilg.MarkLabel(label1);
      this.EmitExpression(b.Right);
      this._ilg.Emit(OpCodes.Stloc, local2);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse_S, label3);
      this._ilg.Emit(OpCodes.Ldloca, local2);
      ILGen.EmitGetValueOrDefault(this._ilg, type);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Ceq);
      this._ilg.Emit(OpCodes.Brfalse_S, label2);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(this._ilg, type);
      this._ilg.Emit(OpCodes.Brfalse, label3);
      this._ilg.Emit(OpCodes.Ldc_I4_0);
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label2);
      this._ilg.Emit(OpCodes.Ldc_I4_1);
      this._ilg.Emit(OpCodes.Br_S, label4);
      this._ilg.MarkLabel(label4);
      ConstructorInfo constructor = type.GetConstructor(new Type[1]
      {
        typeof (bool)
      });
      this._ilg.Emit(OpCodes.Newobj, constructor);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this._ilg.Emit(OpCodes.Br, label5);
      this._ilg.MarkLabel(label3);
      this._ilg.Emit(OpCodes.Ldloca, local1);
      this._ilg.Emit(OpCodes.Initobj, type);
      this._ilg.MarkLabel(label5);
      this._ilg.Emit(OpCodes.Ldloc, local1);
      this.FreeLocal(local1);
      this.FreeLocal(local2);
    }

    private void EmitUnliftedOrElse(BinaryExpression b)
    {
      Label label1 = this._ilg.DefineLabel();
      Label label2 = this._ilg.DefineLabel();
      this.EmitExpressionAndBranch(false, b.Left, label1);
      this._ilg.Emit(OpCodes.Ldc_I4_1);
      this._ilg.Emit(OpCodes.Br, label2);
      this._ilg.MarkLabel(label1);
      this.EmitExpression(b.Right);
      this._ilg.MarkLabel(label2);
    }

    private void EmitMethodOrElse(BinaryExpression b, LambdaCompiler.CompilationFlags flags)
    {
      Label label = this._ilg.DefineLabel();
      this.EmitExpression(b.Left);
      this._ilg.Emit(OpCodes.Dup);
      MethodInfo booleanOperator = TypeUtils.GetBooleanOperator(b.Method.DeclaringType, "op_True");
      this._ilg.Emit(OpCodes.Call, booleanOperator);
      this._ilg.Emit(OpCodes.Brtrue, label);
      LocalBuilder local1 = this.GetLocal(b.Left.Type);
      this._ilg.Emit(OpCodes.Stloc, local1);
      this.EmitExpression(b.Right);
      LocalBuilder local2 = this.GetLocal(b.Right.Type);
      this._ilg.Emit(OpCodes.Stloc, local2);
      this._ilg.Emit(OpCodes.Ldloc, local1);
      this._ilg.Emit(OpCodes.Ldloc, local2);
      if ((flags & LambdaCompiler.CompilationFlags.EmitAsTailCallMask) == LambdaCompiler.CompilationFlags.EmitAsTail)
        this._ilg.Emit(OpCodes.Tailcall);
      this._ilg.Emit(OpCodes.Call, b.Method);
      this.FreeLocal(local1);
      this.FreeLocal(local2);
      this._ilg.MarkLabel(label);
    }

    private void EmitOrElseBinaryExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      BinaryExpression b = (BinaryExpression) expr;
      if (b.Method != (MethodInfo) null && !b.IsLiftedLogical)
        this.EmitMethodOrElse(b, flags);
      else if (b.Left.Type == typeof (bool?))
        this.EmitLiftedOrElse(b);
      else if (b.IsLiftedLogical)
        this.EmitExpression(b.ReduceUserdefinedLifted());
      else
        this.EmitUnliftedOrElse(b);
    }

    private void EmitExpressionAndBranch(bool branchValue, Expression node, Label label)
    {
      LambdaCompiler.CompilationFlags flags = this.EmitExpressionStart(node);
      try
      {
        if (node.Type == typeof (bool))
        {
          switch (node.NodeType)
          {
            case ExpressionType.Not:
              this.EmitBranchNot(branchValue, (UnaryExpression) node, label);
              return;
            case ExpressionType.NotEqual:
            case ExpressionType.Equal:
              this.EmitBranchComparison(branchValue, (BinaryExpression) node, label);
              return;
            case ExpressionType.OrElse:
            case ExpressionType.AndAlso:
              this.EmitBranchLogical(branchValue, (BinaryExpression) node, label);
              return;
            case ExpressionType.Block:
              this.EmitBranchBlock(branchValue, (BlockExpression) node, label);
              return;
          }
        }
        this.EmitExpression(node, LambdaCompiler.CompilationFlags.EmitNoExpressionStart | LambdaCompiler.CompilationFlags.EmitAsNoTail);
        this.EmitBranchOp(branchValue, label);
      }
      finally
      {
        this.EmitExpressionEnd(flags);
      }
    }

    private void EmitBranchOp(bool branch, Label label)
    {
      this._ilg.Emit(branch ? OpCodes.Brtrue : OpCodes.Brfalse, label);
    }

    private void EmitBranchNot(bool branch, UnaryExpression node, Label label)
    {
      if (node.Method != (MethodInfo) null)
      {
        this.EmitExpression((Expression) node, LambdaCompiler.CompilationFlags.EmitNoExpressionStart | LambdaCompiler.CompilationFlags.EmitAsNoTail);
        this.EmitBranchOp(branch, label);
      }
      else
        this.EmitExpressionAndBranch(!branch, node.Operand, label);
    }

    private void EmitBranchComparison(bool branch, BinaryExpression node, Label label)
    {
      bool flag = branch == (node.NodeType == ExpressionType.Equal);
      if (node.Method != (MethodInfo) null)
      {
        this.EmitBinaryMethod(node, LambdaCompiler.CompilationFlags.EmitAsNoTail);
        this.EmitBranchOp(branch, label);
      }
      else if (ConstantCheck.IsNull(node.Left))
      {
        if (TypeUtils.IsNullableType(node.Right.Type))
        {
          this.EmitAddress(node.Right, node.Right.Type);
          ILGen.EmitHasValue(this._ilg, node.Right.Type);
        }
        else
          this.EmitExpression(LambdaCompiler.GetEqualityOperand(node.Right));
        this.EmitBranchOp(!flag, label);
      }
      else if (ConstantCheck.IsNull(node.Right))
      {
        if (TypeUtils.IsNullableType(node.Left.Type))
        {
          this.EmitAddress(node.Left, node.Left.Type);
          ILGen.EmitHasValue(this._ilg, node.Left.Type);
        }
        else
          this.EmitExpression(LambdaCompiler.GetEqualityOperand(node.Left));
        this.EmitBranchOp(!flag, label);
      }
      else if (TypeUtils.IsNullableType(node.Left.Type) || TypeUtils.IsNullableType(node.Right.Type))
      {
        this.EmitBinaryExpression((Expression) node);
        this.EmitBranchOp(branch, label);
      }
      else
      {
        this.EmitExpression(LambdaCompiler.GetEqualityOperand(node.Left));
        this.EmitExpression(LambdaCompiler.GetEqualityOperand(node.Right));
        if (flag)
        {
          this._ilg.Emit(OpCodes.Beq, label);
        }
        else
        {
          this._ilg.Emit(OpCodes.Ceq);
          this._ilg.Emit(OpCodes.Brfalse, label);
        }
      }
    }

    private static Expression GetEqualityOperand(Expression expression)
    {
      if (expression.NodeType == ExpressionType.Convert)
      {
        UnaryExpression unaryExpression = (UnaryExpression) expression;
        if (TypeUtils.AreReferenceAssignable(unaryExpression.Type, unaryExpression.Operand.Type))
          return unaryExpression.Operand;
      }
      return expression;
    }

    private void EmitBranchLogical(bool branch, BinaryExpression node, Label label)
    {
      if (node.Method != (MethodInfo) null || node.IsLifted)
      {
        this.EmitExpression((Expression) node);
        this.EmitBranchOp(branch, label);
      }
      else
      {
        bool flag = node.NodeType == ExpressionType.AndAlso;
        if (branch == flag)
          this.EmitBranchAnd(branch, node, label);
        else
          this.EmitBranchOr(branch, node, label);
      }
    }

    private void EmitBranchAnd(bool branch, BinaryExpression node, Label label)
    {
      Label label1 = this._ilg.DefineLabel();
      this.EmitExpressionAndBranch(!branch, node.Left, label1);
      this.EmitExpressionAndBranch(branch, node.Right, label);
      this._ilg.MarkLabel(label1);
    }

    private void EmitBranchOr(bool branch, BinaryExpression node, Label label)
    {
      this.EmitExpressionAndBranch(branch, node.Left, label);
      this.EmitExpressionAndBranch(branch, node.Right, label);
    }

    private void EmitBranchBlock(bool branch, BlockExpression node, Label label)
    {
      this.EnterScope((object) node);
      int expressionCount = node.ExpressionCount;
      for (int index = 0; index < expressionCount - 1; ++index)
        this.EmitExpressionAsVoid(node.GetExpression(index));
      this.EmitExpressionAndBranch(branch, node.GetExpression(expressionCount - 1), label);
      this.ExitScope((object) node);
    }

    private void EmitBlockExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      this.Emit((BlockExpression) expr, LambdaCompiler.UpdateEmitAsTypeFlag(flags, LambdaCompiler.CompilationFlags.EmitAsDefaultType));
    }

    private void Emit(BlockExpression node, LambdaCompiler.CompilationFlags flags)
    {
      this.EnterScope((object) node);
      LambdaCompiler.CompilationFlags compilationFlags = flags & LambdaCompiler.CompilationFlags.EmitAsTypeMask;
      int expressionCount = node.ExpressionCount;
      LambdaCompiler.CompilationFlags flags1 = flags & LambdaCompiler.CompilationFlags.EmitAsTailCallMask;
      for (int index = 0; index < expressionCount - 1; ++index)
      {
        Expression expression1 = node.GetExpression(index);
        Expression expression2 = node.GetExpression(index + 1);
        if (this.EmitDebugSymbols)
        {
          DebugInfoExpression debugInfoExpression = expression1 as DebugInfoExpression;
          if (debugInfoExpression != null && debugInfoExpression.IsClear && expression2 is DebugInfoExpression)
            continue;
        }
        LambdaCompiler.CompilationFlags newValue;
        if (flags1 != LambdaCompiler.CompilationFlags.EmitAsNoTail)
        {
          GotoExpression gotoExpression = expression2 as GotoExpression;
          newValue = gotoExpression == null || gotoExpression.Value != null && LambdaCompiler.Significant(gotoExpression.Value) || !this.ReferenceLabel(gotoExpression.Target).CanReturn ? LambdaCompiler.CompilationFlags.EmitAsMiddle : LambdaCompiler.CompilationFlags.EmitAsTail;
        }
        else
          newValue = LambdaCompiler.CompilationFlags.EmitAsNoTail;
        flags = LambdaCompiler.UpdateEmitAsTailCallFlag(flags, newValue);
        this.EmitExpressionAsVoid(expression1, flags);
      }
      if (compilationFlags == LambdaCompiler.CompilationFlags.EmitAsVoidType || node.Type == typeof (void))
        this.EmitExpressionAsVoid(node.GetExpression(expressionCount - 1), flags1);
      else
        this.EmitExpressionAsType(node.GetExpression(expressionCount - 1), node.Type, flags1);
      this.ExitScope((object) node);
    }

    private void EnterScope(object node)
    {
      if (!LambdaCompiler.HasVariables(node) || this._scope.MergedScopes != null && this._scope.MergedScopes.Contains(node))
        return;
      CompilerScope compilerScope;
      if (!this._tree.Scopes.TryGetValue(node, out compilerScope))
        compilerScope = new CompilerScope(node, false)
        {
          NeedsClosure = this._scope.NeedsClosure
        };
      this._scope = compilerScope.Enter(this, this._scope);
    }

    private static bool HasVariables(object node)
    {
      BlockExpression blockExpression = node as BlockExpression;
      if (blockExpression != null)
        return blockExpression.Variables.Count > 0;
      else
        return ((CatchBlock) node).Variable != null;
    }

    private void ExitScope(object node)
    {
      if (this._scope.Node != node)
        return;
      this._scope = this._scope.Exit();
    }

    private void EmitDefaultExpression(Expression expr)
    {
      DefaultExpression defaultExpression = (DefaultExpression) expr;
      if (!(defaultExpression.Type != typeof (void)))
        return;
      ILGen.EmitDefault(this._ilg, defaultExpression.Type);
    }

    private void EmitLoopExpression(Expression expr)
    {
      LoopExpression loopExpression = (LoopExpression) expr;
      this.PushLabelBlock(LabelScopeKind.Statement);
      LabelInfo labelInfo1 = this.DefineLabel(loopExpression.BreakLabel);
      LabelInfo labelInfo2 = this.DefineLabel(loopExpression.ContinueLabel);
      labelInfo2.MarkWithEmptyStack();
      this.EmitExpressionAsVoid(loopExpression.Body);
      this._ilg.Emit(OpCodes.Br, labelInfo2.Label);
      this.PopLabelBlock(LabelScopeKind.Statement);
      labelInfo1.MarkWithEmptyStack();
    }

    private void EmitSwitchExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      SwitchExpression node1 = (SwitchExpression) expr;
      if (this.TryEmitSwitchInstruction(node1, flags) || this.TryEmitHashtableSwitch(node1, flags))
        return;
      ParameterExpression variable1 = Expression.Parameter(node1.SwitchValue.Type, "switchValue");
      ParameterExpression variable2 = Expression.Parameter(LambdaCompiler.GetTestValueType(node1), "testValue");
      this._scope.AddLocal(this, variable1);
      this._scope.AddLocal(this, variable2);
      this.EmitExpression(node1.SwitchValue);
      this._scope.EmitSet(variable1);
      Label[] labels = new Label[node1.Cases.Count];
      bool[] isGoto = new bool[node1.Cases.Count];
      int index = 0;
      for (int count = node1.Cases.Count; index < count; ++index)
      {
        this.DefineSwitchCaseLabel(node1.Cases[index], out labels[index], out isGoto[index]);
        foreach (Expression node2 in node1.Cases[index].TestValues)
        {
          this.EmitExpression(node2);
          this._scope.EmitSet(variable2);
          this.EmitExpressionAndBranch(true, (Expression) Expression.Equal((Expression) variable1, (Expression) variable2, false, node1.Comparison), labels[index]);
        }
      }
      Label end = this._ilg.DefineLabel();
      Label @default = node1.DefaultBody == null ? end : this._ilg.DefineLabel();
      this.EmitSwitchCases(node1, labels, isGoto, @default, end, flags);
    }

    private static Type GetTestValueType(SwitchExpression node)
    {
      if (node.Comparison == (MethodInfo) null)
        return node.Cases[0].TestValues[0].Type;
      Type type = TypeUtils.GetNonRefType(TypeExtensions.GetParametersCached((MethodBase) node.Comparison)[1].ParameterType);
      if (node.IsLifted)
        type = TypeUtils.GetNullableType(type);
      return type;
    }

    private static bool FitsInBucket(List<LambdaCompiler.SwitchLabel> buckets, Decimal key, int count)
    {
      Decimal num = Decimal.op_Increment(key - buckets[0].Key);
      if (num > new Decimal(int.MaxValue))
        return false;
      else
        return (Decimal) ((buckets.Count + count) * 2) > num;
    }

    private static void MergeBuckets(List<List<LambdaCompiler.SwitchLabel>> buckets)
    {
      while (buckets.Count > 1)
      {
        List<LambdaCompiler.SwitchLabel> buckets1 = buckets[buckets.Count - 2];
        List<LambdaCompiler.SwitchLabel> list = buckets[buckets.Count - 1];
        if (!LambdaCompiler.FitsInBucket(buckets1, list[list.Count - 1].Key, list.Count))
          break;
        buckets1.AddRange((IEnumerable<LambdaCompiler.SwitchLabel>) list);
        buckets.RemoveAt(buckets.Count - 1);
      }
    }

    private static void AddToBuckets(List<List<LambdaCompiler.SwitchLabel>> buckets, LambdaCompiler.SwitchLabel key)
    {
      if (buckets.Count > 0)
      {
        List<LambdaCompiler.SwitchLabel> buckets1 = buckets[buckets.Count - 1];
        if (LambdaCompiler.FitsInBucket(buckets1, key.Key, 1))
        {
          buckets1.Add(key);
          LambdaCompiler.MergeBuckets(buckets);
          return;
        }
      }
      buckets.Add(new List<LambdaCompiler.SwitchLabel>()
      {
        key
      });
    }

    private static bool CanOptimizeSwitchType(Type valueType)
    {
      switch (Type.GetTypeCode(valueType))
      {
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return true;
        default:
          return false;
      }
    }

    private bool TryEmitSwitchInstruction(SwitchExpression node, LambdaCompiler.CompilationFlags flags)
    {
      if (node.Comparison != (MethodInfo) null)
        return false;
      Type type = node.SwitchValue.Type;
      if (!LambdaCompiler.CanOptimizeSwitchType(type) || !TypeUtils.AreEquivalent(type, node.Cases[0].TestValues[0].Type) || !Enumerable.All<SwitchCase>((IEnumerable<SwitchCase>) node.Cases, (Func<SwitchCase, bool>) (c => Enumerable.All<Expression>((IEnumerable<Expression>) c.TestValues, (Func<Expression, bool>) (t => t is ConstantExpression)))))
        return false;
      Label[] labels = new Label[node.Cases.Count];
      bool[] isGoto = new bool[node.Cases.Count];
      Set<Decimal> set = new Set<Decimal>();
      List<LambdaCompiler.SwitchLabel> list = new List<LambdaCompiler.SwitchLabel>();
      for (int index = 0; index < node.Cases.Count; ++index)
      {
        this.DefineSwitchCaseLabel(node.Cases[index], out labels[index], out isGoto[index]);
        foreach (ConstantExpression constantExpression in node.Cases[index].TestValues)
        {
          Decimal key = LambdaCompiler.ConvertSwitchValue(constantExpression.Value);
          if (!set.Contains(key))
          {
            list.Add(new LambdaCompiler.SwitchLabel(key, constantExpression.Value, labels[index]));
            set.Add(key);
          }
        }
      }
      list.Sort((Comparison<LambdaCompiler.SwitchLabel>) ((x, y) => Math.Sign(x.Key - y.Key)));
      List<List<LambdaCompiler.SwitchLabel>> buckets = new List<List<LambdaCompiler.SwitchLabel>>();
      foreach (LambdaCompiler.SwitchLabel key in list)
        LambdaCompiler.AddToBuckets(buckets, key);
      LocalBuilder local = this.GetLocal(node.SwitchValue.Type);
      this.EmitExpression(node.SwitchValue);
      this._ilg.Emit(OpCodes.Stloc, local);
      Label end = this._ilg.DefineLabel();
      Label @default = node.DefaultBody == null ? end : this._ilg.DefineLabel();
      this.EmitSwitchBuckets(new LambdaCompiler.SwitchInfo(node, local, @default), buckets, 0, buckets.Count - 1);
      this.EmitSwitchCases(node, labels, isGoto, @default, end, flags);
      this.FreeLocal(local);
      return true;
    }

    private static Decimal ConvertSwitchValue(object value)
    {
      if (value is char)
        return (Decimal) ((int) (char) value);
      else
        return Convert.ToDecimal(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    private void DefineSwitchCaseLabel(SwitchCase @case, out Label label, out bool isGoto)
    {
      GotoExpression gotoExpression = @case.Body as GotoExpression;
      if (gotoExpression != null && gotoExpression.Value == null)
      {
        LabelInfo labelInfo = this.ReferenceLabel(gotoExpression.Target);
        if (labelInfo.CanBranch)
        {
          label = labelInfo.Label;
          isGoto = true;
          return;
        }
      }
      label = this._ilg.DefineLabel();
      isGoto = false;
    }

    private void EmitSwitchCases(SwitchExpression node, Label[] labels, bool[] isGoto, Label @default, Label end, LambdaCompiler.CompilationFlags flags)
    {
      this._ilg.Emit(OpCodes.Br, @default);
      int index = 0;
      for (int count = node.Cases.Count; index < count; ++index)
      {
        if (!isGoto[index])
        {
          this._ilg.MarkLabel(labels[index]);
          this.EmitExpressionAsType(node.Cases[index].Body, node.Type, flags);
          if (node.DefaultBody != null || index < count - 1)
          {
            if ((flags & LambdaCompiler.CompilationFlags.EmitAsTailCallMask) == LambdaCompiler.CompilationFlags.EmitAsTail)
              this._ilg.Emit(OpCodes.Ret);
            else
              this._ilg.Emit(OpCodes.Br, end);
          }
        }
      }
      if (node.DefaultBody != null)
      {
        this._ilg.MarkLabel(@default);
        this.EmitExpressionAsType(node.DefaultBody, node.Type, flags);
      }
      this._ilg.MarkLabel(end);
    }

    private void EmitSwitchBuckets(LambdaCompiler.SwitchInfo info, List<List<LambdaCompiler.SwitchLabel>> buckets, int first, int last)
    {
      if (first == last)
      {
        this.EmitSwitchBucket(info, buckets[first]);
      }
      else
      {
        int first1 = (int) (((long) first + (long) last + 1L) / 2L);
        if (first == first1 - 1)
        {
          this.EmitSwitchBucket(info, buckets[first]);
        }
        else
        {
          Label label = this._ilg.DefineLabel();
          this._ilg.Emit(OpCodes.Ldloc, info.Value);
          ILGen.EmitConstant(this._ilg, Enumerable.Last<LambdaCompiler.SwitchLabel>((IEnumerable<LambdaCompiler.SwitchLabel>) buckets[first1 - 1]).Constant);
          this._ilg.Emit(info.IsUnsigned ? OpCodes.Bgt_Un : OpCodes.Bgt, label);
          this.EmitSwitchBuckets(info, buckets, first, first1 - 1);
          this._ilg.MarkLabel(label);
        }
        this.EmitSwitchBuckets(info, buckets, first1, last);
      }
    }

    private void EmitSwitchBucket(LambdaCompiler.SwitchInfo info, List<LambdaCompiler.SwitchLabel> bucket)
    {
      if (bucket.Count == 1)
      {
        this._ilg.Emit(OpCodes.Ldloc, info.Value);
        ILGen.EmitConstant(this._ilg, bucket[0].Constant);
        this._ilg.Emit(OpCodes.Beq, bucket[0].Label);
      }
      else
      {
        Label? nullable = new Label?();
        if (info.Is64BitSwitch)
        {
          nullable = new Label?(this._ilg.DefineLabel());
          this._ilg.Emit(OpCodes.Ldloc, info.Value);
          ILGen.EmitConstant(this._ilg, Enumerable.Last<LambdaCompiler.SwitchLabel>((IEnumerable<LambdaCompiler.SwitchLabel>) bucket).Constant);
          this._ilg.Emit(info.IsUnsigned ? OpCodes.Bgt_Un : OpCodes.Bgt, nullable.Value);
          this._ilg.Emit(OpCodes.Ldloc, info.Value);
          ILGen.EmitConstant(this._ilg, bucket[0].Constant);
          this._ilg.Emit(info.IsUnsigned ? OpCodes.Blt_Un : OpCodes.Blt, nullable.Value);
        }
        this._ilg.Emit(OpCodes.Ldloc, info.Value);
        Decimal num1 = bucket[0].Key;
        if (num1 != new Decimal(0))
        {
          ILGen.EmitConstant(this._ilg, bucket[0].Constant);
          this._ilg.Emit(OpCodes.Sub);
        }
        if (info.Is64BitSwitch)
          this._ilg.Emit(OpCodes.Conv_I4);
        Label[] labels = new Label[(int) Decimal.op_Increment(bucket[bucket.Count - 1].Key - bucket[0].Key)];
        int num2 = 0;
        foreach (LambdaCompiler.SwitchLabel switchLabel in bucket)
        {
          while (num1++ != switchLabel.Key)
            labels[num2++] = info.Default;
          labels[num2++] = switchLabel.Label;
        }
        this._ilg.Emit(OpCodes.Switch, labels);
        if (!info.Is64BitSwitch)
          return;
        this._ilg.MarkLabel(nullable.Value);
      }
    }

    private bool TryEmitHashtableSwitch(SwitchExpression node, LambdaCompiler.CompilationFlags flags)
    {
      if (node.Comparison != typeof (string).GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[2]
      {
        typeof (string),
        typeof (string)
      }, (ParameterModifier[]) null))
        return false;
      int capacity = 0;
      foreach (SwitchCase switchCase in node.Cases)
      {
        foreach (Expression expression in switchCase.TestValues)
        {
          if (!(expression is ConstantExpression))
            return false;
          ++capacity;
        }
      }
      if (capacity < 7)
        return false;
      List<ElementInit> list1 = new List<ElementInit>(capacity);
      List<SwitchCase> list2 = new List<SwitchCase>(node.Cases.Count);
      int num = -1;
      MethodInfo method = typeof (Dictionary<string, int>).GetMethod("Add", new Type[2]
      {
        typeof (string),
        typeof (int)
      });
      int index = 0;
      for (int count = node.Cases.Count; index < count; ++index)
      {
        foreach (ConstantExpression constantExpression in node.Cases[index].TestValues)
        {
          if (constantExpression.Value != null)
            list1.Add(Expression.ElementInit(method, (Expression) constantExpression, (Expression) Expression.Constant((object) index)));
          else
            num = index;
        }
        list2.Add(Expression.SwitchCase(node.Cases[index].Body, new Expression[1]
        {
          (Expression) Expression.Constant((object) index)
        }));
      }
      MemberExpression initializedField = this.CreateLazyInitializedField<Dictionary<string, int>>("dictionarySwitch");
      Expression instance = (Expression) Expression.Condition((Expression) Expression.Equal((Expression) initializedField, (Expression) Expression.Constant((object) null, initializedField.Type)), (Expression) Expression.Assign((Expression) initializedField, (Expression) Expression.ListInit(Expression.New(typeof (Dictionary<string, int>).GetConstructor(new Type[1]
      {
        typeof (int)
      }), new Expression[1]
      {
        (Expression) Expression.Constant((object) list1.Count)
      }), (IEnumerable<ElementInit>) list1)), (Expression) initializedField);
      ParameterExpression parameterExpression1 = Expression.Variable(typeof (string), "switchValue");
      ParameterExpression parameterExpression2 = Expression.Variable(typeof (int), "switchIndex");
      this.EmitExpression((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[2]
      {
        parameterExpression2,
        parameterExpression1
      }, new Expression[3]
      {
        (Expression) Expression.Assign((Expression) parameterExpression1, node.SwitchValue),
        (Expression) Expression.IfThenElse((Expression) Expression.Equal((Expression) parameterExpression1, (Expression) Expression.Constant((object) null, typeof (string))), (Expression) Expression.Assign((Expression) parameterExpression2, (Expression) Expression.Constant((object) num)), (Expression) Expression.IfThenElse((Expression) Expression.Call(instance, "TryGetValue", (Type[]) null, new Expression[2]
        {
          (Expression) parameterExpression1,
          (Expression) parameterExpression2
        }), (Expression) Expression.Empty(), (Expression) Expression.Assign((Expression) parameterExpression2, (Expression) Expression.Constant((object) -1)))),
        (Expression) Expression.Switch(node.Type, (Expression) parameterExpression2, node.DefaultBody, (MethodInfo) null, (IEnumerable<SwitchCase>) list2)
      }), flags);
      return true;
    }

    private void CheckRethrow()
    {
      for (LabelScopeInfo labelScopeInfo = this._labelBlock; labelScopeInfo != null; labelScopeInfo = labelScopeInfo.Parent)
      {
        if (labelScopeInfo.Kind == LabelScopeKind.Catch)
          return;
        if (labelScopeInfo.Kind == LabelScopeKind.Finally)
          break;
      }
      throw System.Linq.Expressions.Error.RethrowRequiresCatch();
    }

    private void CheckTry()
    {
      for (LabelScopeInfo labelScopeInfo = this._labelBlock; labelScopeInfo != null; labelScopeInfo = labelScopeInfo.Parent)
      {
        if (labelScopeInfo.Kind == LabelScopeKind.Filter)
          throw System.Linq.Expressions.Error.TryNotAllowedInFilter();
      }
    }

    private void EmitSaveExceptionOrPop(CatchBlock cb)
    {
      if (cb.Variable != null)
        this._scope.EmitSet(cb.Variable);
      else
        this._ilg.Emit(OpCodes.Pop);
    }

    private void EmitTryExpression(Expression expr)
    {
      TryExpression tryExpression = (TryExpression) expr;
      this.CheckTry();
      this.PushLabelBlock(LabelScopeKind.Try);
      this._ilg.BeginExceptionBlock();
      this.EmitExpression(tryExpression.Body);
      Type type = expr.Type;
      LocalBuilder local = (LocalBuilder) null;
      if (type != typeof (void))
      {
        local = this.GetLocal(type);
        this._ilg.Emit(OpCodes.Stloc, local);
      }
      foreach (CatchBlock cb in tryExpression.Handlers)
      {
        this.PushLabelBlock(LabelScopeKind.Catch);
        if (cb.Filter == null)
          this._ilg.BeginCatchBlock(cb.Test);
        else
          this._ilg.BeginExceptFilterBlock();
        this.EnterScope((object) cb);
        this.EmitCatchStart(cb);
        this.EmitExpression(cb.Body);
        if (type != typeof (void))
          this._ilg.Emit(OpCodes.Stloc, local);
        this.ExitScope((object) cb);
        this.PopLabelBlock(LabelScopeKind.Catch);
      }
      if (tryExpression.Finally != null || tryExpression.Fault != null)
      {
        this.PushLabelBlock(LabelScopeKind.Finally);
        if (tryExpression.Finally != null)
          this._ilg.BeginFinallyBlock();
        else
          this._ilg.BeginFaultBlock();
        this.EmitExpressionAsVoid(tryExpression.Finally ?? tryExpression.Fault);
        this._ilg.EndExceptionBlock();
        this.PopLabelBlock(LabelScopeKind.Finally);
      }
      else
        this._ilg.EndExceptionBlock();
      if (type != typeof (void))
      {
        this._ilg.Emit(OpCodes.Ldloc, local);
        this.FreeLocal(local);
      }
      this.PopLabelBlock(LabelScopeKind.Try);
    }

    private void EmitCatchStart(CatchBlock cb)
    {
      if (cb.Filter == null)
      {
        this.EmitSaveExceptionOrPop(cb);
      }
      else
      {
        Label label1 = this._ilg.DefineLabel();
        Label label2 = this._ilg.DefineLabel();
        this._ilg.Emit(OpCodes.Isinst, cb.Test);
        this._ilg.Emit(OpCodes.Dup);
        this._ilg.Emit(OpCodes.Brtrue, label2);
        this._ilg.Emit(OpCodes.Pop);
        this._ilg.Emit(OpCodes.Ldc_I4_0);
        this._ilg.Emit(OpCodes.Br, label1);
        this._ilg.MarkLabel(label2);
        this.EmitSaveExceptionOrPop(cb);
        this.PushLabelBlock(LabelScopeKind.Filter);
        this.EmitExpression(cb.Filter);
        this.PopLabelBlock(LabelScopeKind.Filter);
        this._ilg.MarkLabel(label1);
        this._ilg.BeginCatchBlock((Type) null);
        this._ilg.Emit(OpCodes.Pop);
      }
    }

    private void EmitQuoteUnaryExpression(Expression expr)
    {
      this.EmitQuote((UnaryExpression) expr);
    }

    private void EmitQuote(UnaryExpression quote)
    {
      this.EmitConstant((object) quote.Operand, quote.Type);
      if (this._scope.NearestHoistedLocals == null)
        return;
      this.EmitConstant((object) this._scope.NearestHoistedLocals, typeof (object));
      this._scope.EmitGet(this._scope.NearestHoistedLocals.SelfVariable);
      this._ilg.Emit(OpCodes.Call, typeof (RuntimeOps).GetMethod("Quote"));
      if (!(quote.Type != typeof (Expression)))
        return;
      this._ilg.Emit(OpCodes.Castclass, quote.Type);
    }

    private void EmitThrowUnaryExpression(Expression expr)
    {
      this.EmitThrow((UnaryExpression) expr, LambdaCompiler.CompilationFlags.EmitAsDefaultType);
    }

    private void EmitThrow(UnaryExpression expr, LambdaCompiler.CompilationFlags flags)
    {
      if (expr.Operand == null)
      {
        this.CheckRethrow();
        this._ilg.Emit(OpCodes.Rethrow);
      }
      else
      {
        this.EmitExpression(expr.Operand);
        this._ilg.Emit(OpCodes.Throw);
      }
      this.EmitUnreachable((Expression) expr, flags);
    }

    private void EmitUnaryExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      this.EmitUnary((UnaryExpression) expr, flags);
    }

    private void EmitUnary(UnaryExpression node, LambdaCompiler.CompilationFlags flags)
    {
      if (node.Method != (MethodInfo) null)
        this.EmitUnaryMethod(node, flags);
      else if (node.NodeType == ExpressionType.NegateChecked && TypeUtils.IsInteger(node.Operand.Type))
      {
        this.EmitExpression(node.Operand);
        LocalBuilder local = this.GetLocal(node.Operand.Type);
        this._ilg.Emit(OpCodes.Stloc, local);
        ILGen.EmitInt(this._ilg, 0);
        ILGen.EmitConvertToType(this._ilg, typeof (int), node.Operand.Type, false);
        this._ilg.Emit(OpCodes.Ldloc, local);
        this.FreeLocal(local);
        this.EmitBinaryOperator(ExpressionType.SubtractChecked, node.Operand.Type, node.Operand.Type, node.Type, false);
      }
      else
      {
        this.EmitExpression(node.Operand);
        this.EmitUnaryOperator(node.NodeType, node.Operand.Type, node.Type);
      }
    }

    private void EmitUnaryOperator(ExpressionType op, Type operandType, Type resultType)
    {
      bool flag = TypeUtils.IsNullableType(operandType);
      if (op == ExpressionType.ArrayLength)
        this._ilg.Emit(OpCodes.Ldlen);
      else if (flag)
      {
        switch (op)
        {
          case ExpressionType.Decrement:
          case ExpressionType.Increment:
          case ExpressionType.OnesComplement:
          case ExpressionType.IsTrue:
          case ExpressionType.IsFalse:
          case ExpressionType.Negate:
          case ExpressionType.UnaryPlus:
          case ExpressionType.NegateChecked:
            Label label1 = this._ilg.DefineLabel();
            Label label2 = this._ilg.DefineLabel();
            LocalBuilder local1 = this.GetLocal(operandType);
            this._ilg.Emit(OpCodes.Stloc, local1);
            this._ilg.Emit(OpCodes.Ldloca, local1);
            ILGen.EmitHasValue(this._ilg, operandType);
            this._ilg.Emit(OpCodes.Brfalse_S, label1);
            this._ilg.Emit(OpCodes.Ldloca, local1);
            ILGen.EmitGetValueOrDefault(this._ilg, operandType);
            Type nonNullableType1 = TypeUtils.GetNonNullableType(resultType);
            this.EmitUnaryOperator(op, nonNullableType1, nonNullableType1);
            ConstructorInfo constructor1 = resultType.GetConstructor(new Type[1]
            {
              nonNullableType1
            });
            this._ilg.Emit(OpCodes.Newobj, constructor1);
            this._ilg.Emit(OpCodes.Stloc, local1);
            this._ilg.Emit(OpCodes.Br_S, label2);
            this._ilg.MarkLabel(label1);
            this._ilg.Emit(OpCodes.Ldloca, local1);
            this._ilg.Emit(OpCodes.Initobj, resultType);
            this._ilg.MarkLabel(label2);
            this._ilg.Emit(OpCodes.Ldloc, local1);
            this.FreeLocal(local1);
            break;
          case ExpressionType.Not:
            if (!(operandType != typeof (bool?)))
            {
              Label label3 = this._ilg.DefineLabel();
              LocalBuilder local2 = this.GetLocal(operandType);
              this._ilg.Emit(OpCodes.Stloc, local2);
              this._ilg.Emit(OpCodes.Ldloca, local2);
              ILGen.EmitHasValue(this._ilg, operandType);
              this._ilg.Emit(OpCodes.Brfalse_S, label3);
              this._ilg.Emit(OpCodes.Ldloca, local2);
              ILGen.EmitGetValueOrDefault(this._ilg, operandType);
              Type nonNullableType2 = TypeUtils.GetNonNullableType(operandType);
              this.EmitUnaryOperator(op, nonNullableType2, typeof (bool));
              ConstructorInfo constructor2 = resultType.GetConstructor(new Type[1]
              {
                typeof (bool)
              });
              this._ilg.Emit(OpCodes.Newobj, constructor2);
              this._ilg.Emit(OpCodes.Stloc, local2);
              this._ilg.MarkLabel(label3);
              this._ilg.Emit(OpCodes.Ldloc, local2);
              this.FreeLocal(local2);
              break;
            }
            else
              goto case ExpressionType.Decrement;
          case ExpressionType.TypeAs:
            this._ilg.Emit(OpCodes.Box, operandType);
            this._ilg.Emit(OpCodes.Isinst, resultType);
            if (!TypeUtils.IsNullableType(resultType))
              break;
            this._ilg.Emit(OpCodes.Unbox_Any, resultType);
            break;
          default:
            throw System.Linq.Expressions.Error.UnhandledUnary((object) op);
        }
      }
      else
      {
        switch (op)
        {
          case ExpressionType.Decrement:
            this.EmitConstantOne(resultType);
            this._ilg.Emit(OpCodes.Sub);
            break;
          case ExpressionType.Increment:
            this.EmitConstantOne(resultType);
            this._ilg.Emit(OpCodes.Add);
            break;
          case ExpressionType.OnesComplement:
            this._ilg.Emit(OpCodes.Not);
            break;
          case ExpressionType.IsTrue:
            this._ilg.Emit(OpCodes.Ldc_I4_1);
            this._ilg.Emit(OpCodes.Ceq);
            return;
          case ExpressionType.IsFalse:
            this._ilg.Emit(OpCodes.Ldc_I4_0);
            this._ilg.Emit(OpCodes.Ceq);
            return;
          case ExpressionType.Negate:
          case ExpressionType.NegateChecked:
            this._ilg.Emit(OpCodes.Neg);
            break;
          case ExpressionType.UnaryPlus:
            this._ilg.Emit(OpCodes.Nop);
            break;
          case ExpressionType.Not:
            if (operandType == typeof (bool))
            {
              this._ilg.Emit(OpCodes.Ldc_I4_0);
              this._ilg.Emit(OpCodes.Ceq);
              break;
            }
            else
            {
              this._ilg.Emit(OpCodes.Not);
              break;
            }
          case ExpressionType.TypeAs:
            if (operandType.IsValueType)
              this._ilg.Emit(OpCodes.Box, operandType);
            this._ilg.Emit(OpCodes.Isinst, resultType);
            if (!TypeUtils.IsNullableType(resultType))
              return;
            this._ilg.Emit(OpCodes.Unbox_Any, resultType);
            return;
          default:
            throw System.Linq.Expressions.Error.UnhandledUnary((object) op);
        }
        this.EmitConvertArithmeticResult(op, resultType);
      }
    }

    private void EmitConstantOne(Type type)
    {
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
          this._ilg.Emit(OpCodes.Ldc_I4_1);
          break;
        case TypeCode.Int64:
        case TypeCode.UInt64:
          this._ilg.Emit(OpCodes.Ldc_I8, 1L);
          break;
        case TypeCode.Single:
          this._ilg.Emit(OpCodes.Ldc_R4, 1f);
          break;
        case TypeCode.Double:
          this._ilg.Emit(OpCodes.Ldc_R8, 1.0);
          break;
        default:
          throw ContractUtils.Unreachable;
      }
    }

    private void EmitUnboxUnaryExpression(Expression expr)
    {
      UnaryExpression unaryExpression = (UnaryExpression) expr;
      this.EmitExpression(unaryExpression.Operand);
      this._ilg.Emit(OpCodes.Unbox_Any, unaryExpression.Type);
    }

    private void EmitConvertUnaryExpression(Expression expr, LambdaCompiler.CompilationFlags flags)
    {
      this.EmitConvert((UnaryExpression) expr, flags);
    }

    private void EmitConvert(UnaryExpression node, LambdaCompiler.CompilationFlags flags)
    {
      if (node.Method != (MethodInfo) null)
      {
        if (node.IsLifted && (!node.Type.IsValueType || !node.Operand.Type.IsValueType))
        {
          ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) node.Method);
          Type parameterType = parametersCached[0].ParameterType;
          if (parameterType.IsByRef)
            parameterType.GetElementType();
          this.EmitConvert(Expression.Convert((Expression) Expression.Call(node.Method, (Expression) Expression.Convert(node.Operand, parametersCached[0].ParameterType)), node.Type), flags);
        }
        else
          this.EmitUnaryMethod(node, flags);
      }
      else if (node.Type == typeof (void))
        this.EmitExpressionAsVoid(node.Operand, flags);
      else if (TypeUtils.AreEquivalent(node.Operand.Type, node.Type))
      {
        this.EmitExpression(node.Operand, flags);
      }
      else
      {
        this.EmitExpression(node.Operand);
        ILGen.EmitConvertToType(this._ilg, node.Operand.Type, node.Type, node.NodeType == ExpressionType.ConvertChecked);
      }
    }

    private void EmitUnaryMethod(UnaryExpression node, LambdaCompiler.CompilationFlags flags)
    {
      if (node.IsLifted)
      {
        ParameterExpression parameterExpression = Expression.Variable(TypeUtils.GetNonNullableType(node.Operand.Type), (string) null);
        MethodCallExpression mc = Expression.Call(node.Method, (Expression) parameterExpression);
        Type nullableType = TypeUtils.GetNullableType(mc.Type);
        this.EmitLift(node.NodeType, nullableType, mc, new ParameterExpression[1]
        {
          parameterExpression
        }, new Expression[1]
        {
          node.Operand
        });
        ILGen.EmitConvertToType(this._ilg, nullableType, node.Type, false);
      }
      else
        this.EmitMethodCallExpression((Expression) Expression.Call(node.Method, node.Operand), flags);
    }

    private sealed class SwitchLabel
    {
      internal readonly Decimal Key;
      internal readonly Label Label;
      internal readonly object Constant;

      internal SwitchLabel(Decimal key, object constant, Label label)
      {
        this.Key = key;
        this.Constant = constant;
        this.Label = label;
      }
    }

    private delegate void WriteBack();

    [System.Flags]
    internal enum CompilationFlags
    {
      EmitExpressionStart = 1,
      EmitNoExpressionStart = 2,
      EmitAsDefaultType = 16,
      EmitAsVoidType = 32,
      EmitAsTail = 256,
      EmitAsMiddle = 512,
      EmitAsNoTail = 1024,
      EmitExpressionStartMask = 15,
      EmitAsTypeMask = 240,
      EmitAsTailCallMask = 3840,
    }

    private sealed class SwitchInfo
    {
      internal readonly SwitchExpression Node;
      internal readonly LocalBuilder Value;
      internal readonly Label Default;
      internal readonly Type Type;
      internal readonly bool IsUnsigned;
      internal readonly bool Is64BitSwitch;

      internal SwitchInfo(SwitchExpression node, LocalBuilder value, Label @default)
      {
        this.Node = node;
        this.Value = value;
        this.Default = @default;
        this.Type = this.Node.SwitchValue.Type;
        this.IsUnsigned = TypeUtils.IsUnsigned(this.Type);
        TypeCode typeCode = Type.GetTypeCode(this.Type);
        this.Is64BitSwitch = typeCode == TypeCode.UInt64 || typeCode == TypeCode.Int64;
      }
    }
  }
}
