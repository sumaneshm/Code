// Type: System.Runtime.CompilerServices.CallSiteOps
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Runtime.CompilerServices
{
  [DebuggerStepThrough]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [__DynamicallyInvokable]
  public static class CallSiteOps
  {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static CallSite<T> CreateMatchmaker<T>(CallSite<T> site) where T : class
    {
      CallSite<T> matchMaker = site.CreateMatchMaker();
      CallSiteOps.ClearMatch((CallSite) matchMaker);
      return matchMaker;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static bool SetNotMatched(CallSite site)
    {
      bool flag = site._match;
      site._match = false;
      return flag;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static bool GetMatch(CallSite site)
    {
      return site._match;
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static void ClearMatch(CallSite site)
    {
      site._match = true;
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static void AddRule<T>(CallSite<T> site, T rule) where T : class
    {
      site.AddRule(rule);
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static void UpdateRules<T>(CallSite<T> @this, int matched) where T : class
    {
      if (matched <= 1)
        return;
      @this.MoveRule(matched);
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static T[] GetRules<T>(CallSite<T> site) where T : class
    {
      return site.Rules;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static RuleCache<T> GetRuleCache<T>(CallSite<T> site) where T : class
    {
      return site.Binder.GetRuleCache<T>();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static void MoveRule<T>(RuleCache<T> cache, T rule, int i) where T : class
    {
      if (i <= 1)
        return;
      cache.MoveRule(rule, i);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static T[] GetCachedRules<T>(RuleCache<T> cache) where T : class
    {
      return cache.GetRules();
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static T Bind<T>(CallSiteBinder binder, CallSite<T> site, object[] args) where T : class
    {
      return binder.BindCore<T>(site, args);
    }
  }
}
