// Type: System.Runtime.CompilerServices.CallSiteBinder
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;
using System.Threading;

namespace System.Runtime.CompilerServices
{
  [__DynamicallyInvokable]
  public abstract class CallSiteBinder
  {
    private static readonly LabelTarget _updateLabel = Expression.Label("CallSiteBinder.UpdateLabel");
    internal Dictionary<Type, object> Cache;

    [__DynamicallyInvokable]
    public static LabelTarget UpdateLabel
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return CallSiteBinder._updateLabel;
      }
    }

    static CallSiteBinder()
    {
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected CallSiteBinder()
    {
    }

    [__DynamicallyInvokable]
    public abstract Expression Bind(object[] args, ReadOnlyCollection<ParameterExpression> parameters, LabelTarget returnLabel);

    [__DynamicallyInvokable]
    public virtual T BindDelegate<T>(CallSite<T> site, object[] args) where T : class
    {
      return default (T);
    }

    internal T BindCore<T>(CallSite<T> site, object[] args) where T : class
    {
      T obj = this.BindDelegate<T>(site, args);
      if ((object) obj != null)
        return obj;
      CallSiteBinder.LambdaSignature<T> signature = CallSiteBinder.LambdaSignature<T>.Instance;
      Expression binding = this.Bind(args, signature.Parameters, signature.ReturnLabel);
      if (binding == null)
        throw Error.NoOrInvalidRuleProduced();
      if (!AppDomain.CurrentDomain.IsHomogenous)
        throw Error.HomogenousAppDomainRequired();
      T target = CallSiteBinder.Stitch<T>(binding, signature).Compile();
      this.CacheTarget<T>(target);
      return target;
    }

    [__DynamicallyInvokable]
    protected void CacheTarget<T>(T target) where T : class
    {
      this.GetRuleCache<T>().AddRule(target);
    }

    private static Expression<T> Stitch<T>(Expression binding, CallSiteBinder.LambdaSignature<T> signature) where T : class
    {
      Type type = typeof (CallSite<T>);
      ReadOnlyCollectionBuilder<Expression> collectionBuilder = new ReadOnlyCollectionBuilder<Expression>(3);
      collectionBuilder.Add(binding);
      ParameterExpression parameterExpression = Expression.Parameter(typeof (CallSite), "$site");
      ParameterExpression[] list = CollectionExtensions.AddFirst<ParameterExpression>((IList<ParameterExpression>) signature.Parameters, parameterExpression);
      Expression expression = (Expression) Expression.Label(CallSiteBinder.UpdateLabel);
      collectionBuilder.Add(expression);
      collectionBuilder.Add((Expression) Expression.Label(signature.ReturnLabel, (Expression) Expression.Condition((Expression) Expression.Call(typeof (CallSiteOps).GetMethod("SetNotMatched"), (Expression) CollectionExtensions.First<ParameterExpression>((IEnumerable<ParameterExpression>) list)), (Expression) Expression.Default(signature.ReturnLabel.Type), (Expression) Expression.Invoke((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, type), typeof (CallSite<T>).GetProperty("Update")), (IEnumerable<Expression>) new TrueReadOnlyCollection<Expression>((Expression[]) list)))));
      return new Expression<T>((Expression) Expression.Block((IEnumerable<Expression>) collectionBuilder), "CallSite.Target", true, (ReadOnlyCollection<ParameterExpression>) new TrueReadOnlyCollection<ParameterExpression>(list));
    }

    internal RuleCache<T> GetRuleCache<T>() where T : class
    {
      if (this.Cache == null)
        Interlocked.CompareExchange<Dictionary<Type, object>>(ref this.Cache, new Dictionary<Type, object>(), (Dictionary<Type, object>) null);
      Dictionary<Type, object> dictionary = this.Cache;
      object obj;
      lock (dictionary)
      {
        if (!dictionary.TryGetValue(typeof (T), out obj))
          dictionary[typeof (T)] = (object) (RuleCache<T>) (obj = (object) new RuleCache<T>());
      }
      return obj as RuleCache<T>;
    }

    private sealed class LambdaSignature<T> where T : class
    {
      internal static readonly CallSiteBinder.LambdaSignature<T> Instance = new CallSiteBinder.LambdaSignature<T>();
      internal readonly ReadOnlyCollection<ParameterExpression> Parameters;
      internal readonly LabelTarget ReturnLabel;

      static LambdaSignature()
      {
      }

      private LambdaSignature()
      {
        Type type = typeof (T);
        if (!type.IsSubclassOf(typeof (MulticastDelegate)))
          throw Error.TypeParameterIsNotDelegate((object) type);
        MethodInfo method = type.GetMethod("Invoke");
        ParameterInfo[] parametersCached = TypeExtensions.GetParametersCached((MethodBase) method);
        if (parametersCached[0].ParameterType != typeof (CallSite))
          throw Error.FirstArgumentMustBeCallSite();
        ParameterExpression[] list = new ParameterExpression[parametersCached.Length - 1];
        for (int index = 0; index < list.Length; ++index)
          list[index] = Expression.Parameter(parametersCached[index + 1].ParameterType, "$arg" + (object) index);
        this.Parameters = (ReadOnlyCollection<ParameterExpression>) new TrueReadOnlyCollection<ParameterExpression>(list);
        this.ReturnLabel = Expression.Label(TypeExtensions.GetReturnType((MethodBase) method));
      }
    }
  }
}
