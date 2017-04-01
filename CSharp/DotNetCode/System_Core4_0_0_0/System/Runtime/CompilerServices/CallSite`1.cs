// Type: System.Runtime.CompilerServices.CallSite`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Linq.Expressions.Compiler;
using System.Reflection;

namespace System.Runtime.CompilerServices
{
  [__DynamicallyInvokable]
  public class CallSite<T> : CallSite where T : class
  {
    [__DynamicallyInvokable]
    public T Target;
    internal T[] Rules;
    private static T _CachedUpdate;
    private static volatile T _CachedNoMatch;
    private const int MaxRules = 10;

    [__DynamicallyInvokable]
    public T Update
    {
      [__DynamicallyInvokable] get
      {
        if (this._match)
          return CallSite<T>._CachedNoMatch;
        else
          return CallSite<T>._CachedUpdate;
      }
    }

    private CallSite(CallSiteBinder binder)
      : base(binder)
    {
      this.Target = this.GetUpdateDelegate();
    }

    private CallSite()
      : base((CallSiteBinder) null)
    {
    }

    internal CallSite<T> CreateMatchMaker()
    {
      return new CallSite<T>();
    }

    [__DynamicallyInvokable]
    public static CallSite<T> Create(CallSiteBinder binder)
    {
      if (!typeof (T).IsSubclassOf(typeof (MulticastDelegate)))
        throw Error.TypeMustBeDerivedFromSystemDelegate();
      else
        return new CallSite<T>(binder);
    }

    private T GetUpdateDelegate()
    {
      return this.GetUpdateDelegate(ref CallSite<T>._CachedUpdate);
    }

    private T GetUpdateDelegate(ref T addr)
    {
      if ((object) addr == null)
        addr = this.MakeUpdateDelegate();
      return addr;
    }

    private void ClearRuleCache()
    {
      this.Binder.GetRuleCache<T>();
      Dictionary<Type, object> dictionary = this.Binder.Cache;
      if (dictionary == null)
        return;
      lock (dictionary)
        dictionary.Clear();
    }

    internal void AddRule(T newRule)
    {
      T[] objArray1 = this.Rules;
      if (objArray1 == null)
      {
        this.Rules = new T[1]
        {
          newRule
        };
      }
      else
      {
        T[] objArray2;
        if (objArray1.Length < 9)
        {
          objArray2 = new T[objArray1.Length + 1];
          Array.Copy((Array) objArray1, 0, (Array) objArray2, 1, objArray1.Length);
        }
        else
        {
          objArray2 = new T[10];
          Array.Copy((Array) objArray1, 0, (Array) objArray2, 1, 9);
        }
        objArray2[0] = newRule;
        this.Rules = objArray2;
      }
    }

    internal void MoveRule(int i)
    {
      T[] objArray = this.Rules;
      T obj = objArray[i];
      objArray[i] = objArray[i - 1];
      objArray[i - 1] = objArray[i - 2];
      objArray[i - 2] = obj;
    }

    internal T MakeUpdateDelegate()
    {
      Type delegateType = typeof (T);
      MethodInfo method = delegateType.GetMethod("Invoke");
      Type[] sig;
      if (delegateType.IsGenericType && CallSite<T>.IsSimpleSignature(method, out sig))
      {
        MethodInfo methodInfo1 = (MethodInfo) null;
        MethodInfo methodInfo2 = (MethodInfo) null;
        if (method.ReturnType == typeof (void))
        {
          if (delegateType == DelegateHelpers.GetActionType(CollectionExtensions.AddFirst<Type>((IList<Type>) sig, typeof (CallSite))))
          {
            methodInfo1 = typeof (UpdateDelegates).GetMethod("UpdateAndExecuteVoid" + (object) sig.Length, BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo2 = typeof (UpdateDelegates).GetMethod("NoMatchVoid" + (object) sig.Length, BindingFlags.Static | BindingFlags.NonPublic);
          }
        }
        else if (delegateType == DelegateHelpers.GetFuncType(CollectionExtensions.AddFirst<Type>((IList<Type>) sig, typeof (CallSite))))
        {
          methodInfo1 = typeof (UpdateDelegates).GetMethod("UpdateAndExecute" + (object) (sig.Length - 1), BindingFlags.Static | BindingFlags.NonPublic);
          methodInfo2 = typeof (UpdateDelegates).GetMethod("NoMatch" + (object) (sig.Length - 1), BindingFlags.Static | BindingFlags.NonPublic);
        }
        if (methodInfo1 != (MethodInfo) null)
        {
          CallSite<T>._CachedNoMatch = (T) CallSite<T>.CreateDelegateHelper(delegateType, methodInfo2.MakeGenericMethod(sig));
          return (T) CallSite<T>.CreateDelegateHelper(delegateType, methodInfo1.MakeGenericMethod(sig));
        }
      }
      CallSite<T>._CachedNoMatch = this.CreateCustomNoMatchDelegate(method);
      return this.CreateCustomUpdateDelegate(method);
    }

    private static Delegate CreateDelegateHelper(Type delegateType, MethodInfo method)
    {
      return Delegate.CreateDelegate(delegateType, method);
    }

    private static bool IsSimpleSignature(MethodInfo invoke, out Type[] sig)
    {
      ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) invoke);
      ContractUtils.Requires(parametersCached.Length > 0 && parametersCached[0].ParameterType == typeof (CallSite), "T");
      Type[] typeArray = new Type[invoke.ReturnType != typeof (void) ? parametersCached.Length : parametersCached.Length - 1];
      bool flag = true;
      for (int index = 1; index < parametersCached.Length; ++index)
      {
        ParameterInfo pi = parametersCached[index];
        if (TypeExtensions.IsByRefParameter(pi))
          flag = false;
        typeArray[index - 1] = pi.ParameterType;
      }
      if (invoke.ReturnType != typeof (void))
        typeArray[typeArray.Length - 1] = invoke.ReturnType;
      sig = typeArray;
      return flag;
    }

    private T CreateCustomNoMatchDelegate(MethodInfo invoke)
    {
      return ((Expression<T>) (() => Expression.Block(Expression.Call(typeof (CallSiteOps).GetMethod("SetNotMatched"), (Expression) CollectionExtensions.First<ParameterExpression>((IEnumerable<ParameterExpression>) CollectionExtensions.Map<ParameterInfo, ParameterExpression>((ICollection<ParameterInfo>) TypeExtensions.GetParametersCached((MethodBase) invoke), (Func<ParameterInfo, ParameterExpression>) (p => Expression.Parameter(p.ParameterType, p.Name))))), Expression.Default(TypeExtensions.GetReturnType((MethodBase) invoke))))).Compile();
    }

    private T CreateCustomUpdateDelegate(MethodInfo invoke)
    {
      List<Expression> list1 = new List<Expression>();
      List<ParameterExpression> list2 = new List<ParameterExpression>();
      ParameterExpression[] array = CollectionExtensions.Map<ParameterInfo, ParameterExpression>((ICollection<ParameterInfo>) TypeExtensions.GetParametersCached((MethodBase) invoke), (Func<ParameterInfo, ParameterExpression>) (p => Expression.Parameter(p.ParameterType, p.Name)));
      LabelTarget target = Expression.Label(TypeExtensions.GetReturnType((MethodBase) invoke));
      Type[] typeArguments = new Type[1]
      {
        typeof (T)
      };
      ParameterExpression parameterExpression1 = array[0];
      ParameterExpression[] parameterExpressionArray = CollectionExtensions.RemoveFirst<ParameterExpression>(array);
      ParameterExpression parameterExpression2 = Expression.Variable(typeof (CallSite<T>), "this");
      list2.Add(parameterExpression2);
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression2, (Expression) Expression.Convert((Expression) parameterExpression1, parameterExpression2.Type)));
      ParameterExpression parameterExpression3 = Expression.Variable(typeof (T[]), "applicable");
      list2.Add(parameterExpression3);
      ParameterExpression parameterExpression4 = Expression.Variable(typeof (T), "rule");
      list2.Add(parameterExpression4);
      ParameterExpression parameterExpression5 = Expression.Variable(typeof (T), "originalRule");
      list2.Add(parameterExpression5);
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression5, (Expression) Expression.Field((Expression) parameterExpression2, "Target")));
      ParameterExpression parameterExpression6 = (ParameterExpression) null;
      if (target.Type != typeof (void))
        list2.Add(parameterExpression6 = Expression.Variable(target.Type, "result"));
      ParameterExpression parameterExpression7 = Expression.Variable(typeof (int), "count");
      list2.Add(parameterExpression7);
      ParameterExpression parameterExpression8 = Expression.Variable(typeof (int), "index");
      list2.Add(parameterExpression8);
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression1, (Expression) Expression.Call(typeof (CallSiteOps), "CreateMatchmaker", typeArguments, new Expression[1]
      {
        (Expression) parameterExpression2
      })));
      Expression test = (Expression) Expression.Call(typeof (CallSiteOps).GetMethod("GetMatch"), (Expression) parameterExpression1);
      Expression expression1 = (Expression) Expression.Call(typeof (CallSiteOps).GetMethod("ClearMatch"), (Expression) parameterExpression1);
      MethodCallExpression methodCallExpression = Expression.Call(typeof (CallSiteOps), "UpdateRules", typeArguments, new Expression[2]
      {
        (Expression) parameterExpression2,
        (Expression) parameterExpression8
      });
      Expression expression2 = !(target.Type == typeof (void)) ? (Expression) Expression.Block((Expression) Expression.Assign((Expression) parameterExpression6, (Expression) Expression.Invoke((Expression) parameterExpression4, (IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>((Expression[]) array))), (Expression) Expression.IfThen(test, (Expression) Expression.Block((Expression) methodCallExpression, (Expression) Expression.Return(target, (Expression) parameterExpression6)))) : (Expression) Expression.Block((Expression) Expression.Invoke((Expression) parameterExpression4, (IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>((Expression[]) array)), (Expression) Expression.IfThen(test, (Expression) Expression.Block((Expression) methodCallExpression, (Expression) Expression.Return(target))));
      Expression expression3 = (Expression) Expression.Assign((Expression) parameterExpression4, (Expression) Expression.ArrayAccess((Expression) parameterExpression3, new Expression[1]
      {
        (Expression) parameterExpression8
      }));
      LabelTarget labelTarget = Expression.Label();
      ConditionalExpression conditionalExpression = Expression.IfThen((Expression) Expression.Equal((Expression) parameterExpression8, (Expression) parameterExpression7), (Expression) Expression.Break(labelTarget));
      UnaryExpression unaryExpression = Expression.PreIncrementAssign((Expression) parameterExpression8);
      list1.Add((Expression) Expression.IfThen((Expression) Expression.NotEqual((Expression) Expression.Assign((Expression) parameterExpression3, (Expression) Expression.Call(typeof (CallSiteOps), "GetRules", typeArguments, new Expression[1]
      {
        (Expression) parameterExpression2
      })), (Expression) Expression.Constant((object) null, parameterExpression3.Type)), (Expression) Expression.Block((Expression) Expression.Assign((Expression) parameterExpression7, (Expression) Expression.ArrayLength((Expression) parameterExpression3)), (Expression) Expression.Assign((Expression) parameterExpression8, (Expression) Expression.Constant((object) 0)), (Expression) Expression.Loop((Expression) Expression.Block((Expression) conditionalExpression, expression3, (Expression) Expression.IfThen((Expression) Expression.NotEqual((Expression) Expression.Convert((Expression) parameterExpression4, typeof (object)), (Expression) Expression.Convert((Expression) parameterExpression5, typeof (object))), (Expression) Expression.Block((Expression) Expression.Assign((Expression) Expression.Field((Expression) parameterExpression2, "Target"), (Expression) parameterExpression4), expression2, expression1)), (Expression) unaryExpression), labelTarget, (LabelTarget) null))));
      ParameterExpression parameterExpression9 = Expression.Variable(typeof (RuleCache<T>), "cache");
      list2.Add(parameterExpression9);
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression9, (Expression) Expression.Call(typeof (CallSiteOps), "GetRuleCache", typeArguments, new Expression[1]
      {
        (Expression) parameterExpression2
      })));
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression3, (Expression) Expression.Call(typeof (CallSiteOps), "GetCachedRules", typeArguments, new Expression[1]
      {
        (Expression) parameterExpression9
      })));
      Expression body = !(target.Type == typeof (void)) ? (Expression) Expression.Block((Expression) Expression.Assign((Expression) parameterExpression6, (Expression) Expression.Invoke((Expression) parameterExpression4, (IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>((Expression[]) array))), (Expression) Expression.IfThen(test, (Expression) Expression.Return(target, (Expression) parameterExpression6))) : (Expression) Expression.Block((Expression) Expression.Invoke((Expression) parameterExpression4, (IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>((Expression[]) array)), (Expression) Expression.IfThen(test, (Expression) Expression.Return(target)));
      TryExpression tryExpression1 = Expression.TryFinally(body, (Expression) Expression.IfThen(test, (Expression) Expression.Block((Expression) Expression.Call(typeof (CallSiteOps), "AddRule", typeArguments, new Expression[2]
      {
        (Expression) parameterExpression2,
        (Expression) parameterExpression4
      }), (Expression) Expression.Call(typeof (CallSiteOps), "MoveRule", typeArguments, new Expression[3]
      {
        (Expression) parameterExpression9,
        (Expression) parameterExpression4,
        (Expression) parameterExpression8
      }))));
      Expression expression4 = (Expression) Expression.Assign((Expression) Expression.Field((Expression) parameterExpression2, "Target"), (Expression) Expression.Assign((Expression) parameterExpression4, (Expression) Expression.ArrayAccess((Expression) parameterExpression3, new Expression[1]
      {
        (Expression) parameterExpression8
      })));
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression8, (Expression) Expression.Constant((object) 0)));
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression7, (Expression) Expression.ArrayLength((Expression) parameterExpression3)));
      list1.Add((Expression) Expression.Loop((Expression) Expression.Block((Expression) conditionalExpression, expression4, (Expression) tryExpression1, expression1, (Expression) unaryExpression), labelTarget, (LabelTarget) null));
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression4, (Expression) Expression.Constant((object) null, parameterExpression4.Type)));
      ParameterExpression parameterExpression10 = Expression.Variable(typeof (object[]), "args");
      list2.Add(parameterExpression10);
      list1.Add((Expression) Expression.Assign((Expression) parameterExpression10, (Expression) Expression.NewArrayInit(typeof (object), CollectionExtensions.Map<ParameterExpression, Expression>((ICollection<ParameterExpression>) parameterExpressionArray, (Func<ParameterExpression, Expression>) (p => CallSite<T>.Convert((Expression) p, typeof (object)))))));
      Expression expression5 = (Expression) Expression.Assign((Expression) Expression.Field((Expression) parameterExpression2, "Target"), (Expression) parameterExpression5);
      Expression expression6 = (Expression) Expression.Assign((Expression) Expression.Field((Expression) parameterExpression2, "Target"), (Expression) Expression.Assign((Expression) parameterExpression4, (Expression) Expression.Call(typeof (CallSiteOps), "Bind", typeArguments, new Expression[3]
      {
        (Expression) Expression.Property((Expression) parameterExpression2, "Binder"),
        (Expression) parameterExpression2,
        (Expression) parameterExpression10
      })));
      TryExpression tryExpression2 = Expression.TryFinally(body, (Expression) Expression.IfThen(test, (Expression) Expression.Call(typeof (CallSiteOps), "AddRule", typeArguments, new Expression[2]
      {
        (Expression) parameterExpression2,
        (Expression) parameterExpression4
      })));
      list1.Add((Expression) Expression.Loop((Expression) Expression.Block(expression5, expression6, (Expression) tryExpression2, expression1), (LabelTarget) null, (LabelTarget) null));
      list1.Add((Expression) Expression.Default(target.Type));
      return Expression.Lambda<T>((Expression) Expression.Label(target, (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ReadOnlyCollection<ParameterExpression>((IList<ParameterExpression>) list2), (IEnumerable<Expression>) new ReadOnlyCollection<Expression>((IList<Expression>) list1))), "CallSite.Target", true, (IEnumerable<ParameterExpression>) new ReadOnlyCollection<ParameterExpression>((IList<ParameterExpression>) array)).Compile();
    }

    private static Expression Convert(Expression arg, Type type)
    {
      if (TypeUtils.AreReferenceAssignable(type, arg.Type))
        return arg;
      else
        return (Expression) Expression.Convert(arg, type);
    }
  }
}
