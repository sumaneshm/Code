// Type: System.Runtime.CompilerServices.CallSite
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;

namespace System.Runtime.CompilerServices
{
  [__DynamicallyInvokable]
  public class CallSite
  {
    private static volatile CacheDict<Type, Func<CallSiteBinder, CallSite>> _SiteCtors;
    internal readonly CallSiteBinder _binder;
    internal bool _match;

    [__DynamicallyInvokable]
    public CallSiteBinder Binder
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._binder;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal CallSite(CallSiteBinder binder)
    {
      this._binder = binder;
    }

    [__DynamicallyInvokable]
    public static CallSite Create(Type delegateType, CallSiteBinder binder)
    {
      ContractUtils.RequiresNotNull((object) delegateType, "delegateType");
      ContractUtils.RequiresNotNull((object) binder, "binder");
      if (!delegateType.IsSubclassOf(typeof (MulticastDelegate)))
        throw Error.TypeMustBeDerivedFromSystemDelegate();
      CacheDict<Type, Func<CallSiteBinder, CallSite>> cacheDict = CallSite._SiteCtors;
      if (cacheDict == null)
        CallSite._SiteCtors = cacheDict = new CacheDict<Type, Func<CallSiteBinder, CallSite>>(100);
      MethodInfo method = (MethodInfo) null;
      Func<CallSiteBinder, CallSite> func;
      if (!cacheDict.TryGetValue(delegateType, out func))
      {
        method = typeof (CallSite<>).MakeGenericType(new Type[1]
        {
          delegateType
        }).GetMethod("Create");
        if (TypeUtils.CanCache(delegateType))
        {
          func = (Func<CallSiteBinder, CallSite>) Delegate.CreateDelegate(typeof (Func<CallSiteBinder, CallSite>), method);
          cacheDict.Add(delegateType, func);
        }
      }
      if (func != null)
        return func(binder);
      return (CallSite) method.Invoke((object) null, new object[1]
      {
        (object) binder
      });
    }
  }
}
