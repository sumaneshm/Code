// Type: System.Dynamic.DynamicObject
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  [Serializable]
  public class DynamicObject : IDynamicMetaObjectProvider
  {
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected DynamicObject()
    {
    }

    [__DynamicallyInvokable]
    public virtual bool TryGetMember(GetMemberBinder binder, out object result)
    {
      result = (object) null;
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TrySetMember(SetMemberBinder binder, object value)
    {
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryDeleteMember(DeleteMemberBinder binder)
    {
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
    {
      result = (object) null;
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryConvert(ConvertBinder binder, out object result)
    {
      result = (object) null;
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
    {
      result = (object) null;
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryInvoke(InvokeBinder binder, object[] args, out object result)
    {
      result = (object) null;
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
    {
      result = (object) null;
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
    {
      result = (object) null;
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
      result = (object) null;
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
    {
      return false;
    }

    [__DynamicallyInvokable]
    public virtual bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
    {
      return false;
    }

    [__DynamicallyInvokable]
    public virtual IEnumerable<string> GetDynamicMemberNames()
    {
      return (IEnumerable<string>) new string[0];
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject GetMetaObject(Expression parameter)
    {
      return (DynamicMetaObject) new DynamicObject.MetaDynamic(parameter, this);
    }

    private sealed class MetaDynamic : DynamicMetaObject
    {
      private static readonly Expression[] NoArgs = new Expression[0];

      private DynamicObject Value
      {
        get
        {
          return (DynamicObject) base.Value;
        }
      }

      static MetaDynamic()
      {
      }

      internal MetaDynamic(Expression expression, DynamicObject value)
        : base(expression, BindingRestrictions.Empty, (object) value)
      {
      }

      public override IEnumerable<string> GetDynamicMemberNames()
      {
        return this.Value.GetDynamicMemberNames();
      }

      public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
      {
        if (this.IsOverridden("TryGetMember"))
          return this.CallMethodWithResult("TryGetMember", (DynamicMetaObjectBinder) binder, DynamicObject.MetaDynamic.NoArgs, (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackGetMember((DynamicMetaObject) this, e)));
        else
          return base.BindGetMember(binder);
      }

      public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
      {
        if (this.IsOverridden("TrySetMember"))
          return this.CallMethodReturnLast("TrySetMember", (DynamicMetaObjectBinder) binder, DynamicObject.MetaDynamic.NoArgs, value.Expression, (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackSetMember((DynamicMetaObject) this, value, e)));
        else
          return base.BindSetMember(binder, value);
      }

      public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
      {
        if (this.IsOverridden("TryDeleteMember"))
          return this.CallMethodNoResult("TryDeleteMember", (DynamicMetaObjectBinder) binder, DynamicObject.MetaDynamic.NoArgs, (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackDeleteMember((DynamicMetaObject) this, e)));
        else
          return base.BindDeleteMember(binder);
      }

      public override DynamicMetaObject BindConvert(ConvertBinder binder)
      {
        if (this.IsOverridden("TryConvert"))
          return this.CallMethodWithResult("TryConvert", (DynamicMetaObjectBinder) binder, DynamicObject.MetaDynamic.NoArgs, (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackConvert((DynamicMetaObject) this, e)));
        else
          return base.BindConvert(binder);
      }

      public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
      {
        DynamicObject.MetaDynamic.Fallback fallback = (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackInvokeMember((DynamicMetaObject) this, args, e));
        DynamicMetaObject errorSuggestion = this.BuildCallMethodWithResult("TryInvokeMember", (DynamicMetaObjectBinder) binder, DynamicMetaObject.GetExpressions(args), this.BuildCallMethodWithResult("TryGetMember", (DynamicMetaObjectBinder) new DynamicObject.MetaDynamic.GetBinderAdapter(binder), DynamicObject.MetaDynamic.NoArgs, fallback((DynamicMetaObject) null), (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackInvoke(e, args, (DynamicMetaObject) null))), (DynamicObject.MetaDynamic.Fallback) null);
        return fallback(errorSuggestion);
      }

      public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
      {
        if (this.IsOverridden("TryCreateInstance"))
          return this.CallMethodWithResult("TryCreateInstance", (DynamicMetaObjectBinder) binder, DynamicMetaObject.GetExpressions(args), (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackCreateInstance((DynamicMetaObject) this, args, e)));
        else
          return base.BindCreateInstance(binder, args);
      }

      public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
      {
        if (this.IsOverridden("TryInvoke"))
          return this.CallMethodWithResult("TryInvoke", (DynamicMetaObjectBinder) binder, DynamicMetaObject.GetExpressions(args), (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackInvoke((DynamicMetaObject) this, args, e)));
        else
          return base.BindInvoke(binder, args);
      }

      public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
      {
        if (!this.IsOverridden("TryBinaryOperation"))
          return base.BindBinaryOperation(binder, arg);
        return this.CallMethodWithResult("TryBinaryOperation", (DynamicMetaObjectBinder) binder, DynamicMetaObject.GetExpressions(new DynamicMetaObject[1]
        {
          arg
        }), (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackBinaryOperation((DynamicMetaObject) this, arg, e)));
      }

      public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
      {
        if (this.IsOverridden("TryUnaryOperation"))
          return this.CallMethodWithResult("TryUnaryOperation", (DynamicMetaObjectBinder) binder, DynamicObject.MetaDynamic.NoArgs, (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackUnaryOperation((DynamicMetaObject) this, e)));
        else
          return base.BindUnaryOperation(binder);
      }

      public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
      {
        if (this.IsOverridden("TryGetIndex"))
          return this.CallMethodWithResult("TryGetIndex", (DynamicMetaObjectBinder) binder, DynamicMetaObject.GetExpressions(indexes), (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackGetIndex((DynamicMetaObject) this, indexes, e)));
        else
          return base.BindGetIndex(binder, indexes);
      }

      public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
      {
        if (this.IsOverridden("TrySetIndex"))
          return this.CallMethodReturnLast("TrySetIndex", (DynamicMetaObjectBinder) binder, DynamicMetaObject.GetExpressions(indexes), value.Expression, (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackSetIndex((DynamicMetaObject) this, indexes, value, e)));
        else
          return base.BindSetIndex(binder, indexes, value);
      }

      public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
      {
        if (this.IsOverridden("TryDeleteIndex"))
          return this.CallMethodNoResult("TryDeleteIndex", (DynamicMetaObjectBinder) binder, DynamicMetaObject.GetExpressions(indexes), (DynamicObject.MetaDynamic.Fallback) (e => binder.FallbackDeleteIndex((DynamicMetaObject) this, indexes, e)));
        else
          return base.BindDeleteIndex(binder, indexes);
      }

      private static Expression[] GetConvertedArgs(params Expression[] args)
      {
        ReadOnlyCollectionBuilder<Expression> collectionBuilder = new ReadOnlyCollectionBuilder<Expression>(args.Length);
        for (int index = 0; index < args.Length; ++index)
          collectionBuilder.Add((Expression) Expression.Convert(args[index], typeof (object)));
        return collectionBuilder.ToArray();
      }

      private static Expression ReferenceArgAssign(Expression callArgs, Expression[] args)
      {
        ReadOnlyCollectionBuilder<Expression> collectionBuilder = (ReadOnlyCollectionBuilder<Expression>) null;
        for (int index = 0; index < args.Length; ++index)
        {
          ContractUtils.Requires(args[index] is ParameterExpression);
          if (((ParameterExpression) args[index]).IsByRef)
          {
            if (collectionBuilder == null)
              collectionBuilder = new ReadOnlyCollectionBuilder<Expression>();
            collectionBuilder.Add((Expression) Expression.Assign(args[index], (Expression) Expression.Convert((Expression) Expression.ArrayIndex(callArgs, (Expression) Expression.Constant((object) index)), args[index].Type)));
          }
        }
        if (collectionBuilder != null)
          return (Expression) Expression.Block((IEnumerable<Expression>) collectionBuilder);
        else
          return (Expression) Expression.Empty();
      }

      private static Expression[] BuildCallArgs(DynamicMetaObjectBinder binder, Expression[] parameters, Expression arg0, Expression arg1)
      {
        if (!object.ReferenceEquals((object) parameters, (object) DynamicObject.MetaDynamic.NoArgs))
        {
          if (arg1 == null)
            return new Expression[2]
            {
              (Expression) DynamicObject.MetaDynamic.Constant(binder),
              arg0
            };
          else
            return new Expression[3]
            {
              (Expression) DynamicObject.MetaDynamic.Constant(binder),
              arg0,
              arg1
            };
        }
        else if (arg1 == null)
          return new Expression[1]
          {
            (Expression) DynamicObject.MetaDynamic.Constant(binder)
          };
        else
          return new Expression[2]
          {
            (Expression) DynamicObject.MetaDynamic.Constant(binder),
            arg1
          };
      }

      private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
      {
        Type type = binder.GetType();
        while (!type.IsVisible)
          type = type.BaseType;
        return Expression.Constant((object) binder, type);
      }

      private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicObject.MetaDynamic.Fallback fallback)
      {
        return this.CallMethodWithResult(methodName, binder, args, fallback, (DynamicObject.MetaDynamic.Fallback) null);
      }

      private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicObject.MetaDynamic.Fallback fallback, DynamicObject.MetaDynamic.Fallback fallbackInvoke)
      {
        DynamicMetaObject fallbackResult = fallback((DynamicMetaObject) null);
        DynamicMetaObject errorSuggestion = this.BuildCallMethodWithResult(methodName, binder, args, fallbackResult, fallbackInvoke);
        return fallback(errorSuggestion);
      }

      private DynamicMetaObject BuildCallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicMetaObject fallbackResult, DynamicObject.MetaDynamic.Fallback fallbackInvoke)
      {
        if (!this.IsOverridden(methodName))
          return fallbackResult;
        ParameterExpression parameterExpression1 = Expression.Parameter(typeof (object), (string) null);
        ParameterExpression parameterExpression2 = methodName != "TryBinaryOperation" ? Expression.Parameter(typeof (object[]), (string) null) : Expression.Parameter(typeof (object), (string) null);
        Expression[] convertedArgs = DynamicObject.MetaDynamic.GetConvertedArgs(args);
        DynamicMetaObject errorSuggestion = new DynamicMetaObject((Expression) parameterExpression1, BindingRestrictions.Empty);
        if (binder.ReturnType != typeof (object))
        {
          UnaryExpression unaryExpression = Expression.Convert(errorSuggestion.Expression, binder.ReturnType);
          string str = Strings.DynamicObjectResultNotAssignable((object) "{0}", (object) this.Value.GetType(), (object) binder.GetType(), (object) binder.ReturnType);
          errorSuggestion = new DynamicMetaObject((Expression) Expression.Condition(!binder.ReturnType.IsValueType || !(Nullable.GetUnderlyingType(binder.ReturnType) == (Type) null) ? (Expression) Expression.OrElse((Expression) Expression.Equal(errorSuggestion.Expression, (Expression) Expression.Constant((object) null)), (Expression) Expression.TypeIs(errorSuggestion.Expression, binder.ReturnType)) : (Expression) Expression.TypeIs(errorSuggestion.Expression, binder.ReturnType), (Expression) unaryExpression, (Expression) Expression.Throw((Expression) Expression.New(typeof (InvalidCastException).GetConstructor(new Type[1]
          {
            typeof (string)
          }), new Expression[1]
          {
            (Expression) Expression.Call(typeof (string).GetMethod("Format", new Type[2]
            {
              typeof (string),
              typeof (object[])
            }), (Expression) Expression.Constant((object) str), (Expression) Expression.NewArrayInit(typeof (object), new Expression[1]
            {
              (Expression) Expression.Condition((Expression) Expression.Equal(errorSuggestion.Expression, (Expression) Expression.Constant((object) null)), (Expression) Expression.Constant((object) "null"), (Expression) Expression.Call(errorSuggestion.Expression, typeof (object).GetMethod("GetType")), typeof (object))
            }))
          }), binder.ReturnType), binder.ReturnType), errorSuggestion.Restrictions);
        }
        if (fallbackInvoke != null)
          errorSuggestion = fallbackInvoke(errorSuggestion);
        return new DynamicMetaObject((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[2]
        {
          parameterExpression1,
          parameterExpression2
        }, new Expression[2]
        {
          methodName != "TryBinaryOperation" ? (Expression) Expression.Assign((Expression) parameterExpression2, (Expression) Expression.NewArrayInit(typeof (object), convertedArgs)) : (Expression) Expression.Assign((Expression) parameterExpression2, convertedArgs[0]),
          (Expression) Expression.Condition((Expression) Expression.Call(this.GetLimitedSelf(), typeof (DynamicObject).GetMethod(methodName), DynamicObject.MetaDynamic.BuildCallArgs(binder, args, (Expression) parameterExpression2, (Expression) parameterExpression1)), (Expression) Expression.Block(methodName != "TryBinaryOperation" ? DynamicObject.MetaDynamic.ReferenceArgAssign((Expression) parameterExpression2, args) : (Expression) Expression.Empty(), errorSuggestion.Expression), fallbackResult.Expression, binder.ReturnType)
        }), this.GetRestrictions().Merge(errorSuggestion.Restrictions).Merge(fallbackResult.Restrictions));
      }

      private DynamicMetaObject CallMethodReturnLast(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Expression value, DynamicObject.MetaDynamic.Fallback fallback)
      {
        DynamicMetaObject dynamicMetaObject = fallback((DynamicMetaObject) null);
        ParameterExpression parameterExpression1 = Expression.Parameter(typeof (object), (string) null);
        ParameterExpression parameterExpression2 = Expression.Parameter(typeof (object[]), (string) null);
        Expression[] convertedArgs = DynamicObject.MetaDynamic.GetConvertedArgs(args);
        DynamicMetaObject errorSuggestion = new DynamicMetaObject((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[2]
        {
          parameterExpression1,
          parameterExpression2
        }, new Expression[2]
        {
          (Expression) Expression.Assign((Expression) parameterExpression2, (Expression) Expression.NewArrayInit(typeof (object), convertedArgs)),
          (Expression) Expression.Condition((Expression) Expression.Call(this.GetLimitedSelf(), typeof (DynamicObject).GetMethod(methodName), DynamicObject.MetaDynamic.BuildCallArgs(binder, args, (Expression) parameterExpression2, (Expression) Expression.Assign((Expression) parameterExpression1, (Expression) Expression.Convert(value, typeof (object))))), (Expression) Expression.Block(DynamicObject.MetaDynamic.ReferenceArgAssign((Expression) parameterExpression2, args), (Expression) parameterExpression1), dynamicMetaObject.Expression, typeof (object))
        }), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
        return fallback(errorSuggestion);
      }

      private DynamicMetaObject CallMethodNoResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicObject.MetaDynamic.Fallback fallback)
      {
        DynamicMetaObject dynamicMetaObject = fallback((DynamicMetaObject) null);
        ParameterExpression parameterExpression = Expression.Parameter(typeof (object[]), (string) null);
        Expression[] convertedArgs = DynamicObject.MetaDynamic.GetConvertedArgs(args);
        DynamicMetaObject errorSuggestion = new DynamicMetaObject((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
        {
          parameterExpression
        }, new Expression[2]
        {
          (Expression) Expression.Assign((Expression) parameterExpression, (Expression) Expression.NewArrayInit(typeof (object), convertedArgs)),
          (Expression) Expression.Condition((Expression) Expression.Call(this.GetLimitedSelf(), typeof (DynamicObject).GetMethod(methodName), DynamicObject.MetaDynamic.BuildCallArgs(binder, args, (Expression) parameterExpression, (Expression) null)), (Expression) Expression.Block(DynamicObject.MetaDynamic.ReferenceArgAssign((Expression) parameterExpression, args), (Expression) Expression.Empty()), dynamicMetaObject.Expression, typeof (void))
        }), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
        return fallback(errorSuggestion);
      }

      private bool IsOverridden(string method)
      {
        foreach (MethodInfo methodInfo in this.Value.GetType().GetMember(method, MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public))
        {
          if (methodInfo.DeclaringType != typeof (DynamicObject) && methodInfo.GetBaseDefinition().DeclaringType == typeof (DynamicObject))
            return true;
        }
        return false;
      }

      private BindingRestrictions GetRestrictions()
      {
        return BindingRestrictions.GetTypeRestriction((DynamicMetaObject) this);
      }

      private Expression GetLimitedSelf()
      {
        if (TypeUtils.AreEquivalent(this.Expression.Type, typeof (DynamicObject)))
          return this.Expression;
        else
          return (Expression) Expression.Convert(this.Expression, typeof (DynamicObject));
      }

      private delegate DynamicMetaObject Fallback(DynamicMetaObject errorSuggestion);

      private sealed class GetBinderAdapter : GetMemberBinder
      {
        internal GetBinderAdapter(InvokeMemberBinder binder)
          : base(binder.Name, binder.IgnoreCase)
        {
        }

        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
          throw new NotSupportedException();
        }
      }
    }
  }
}
