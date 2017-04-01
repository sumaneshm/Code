// Type: System.Dynamic.UpdateDelegates
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.CompilerServices;

namespace System.Dynamic
{
  internal static class UpdateDelegates
  {
    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute0<TRet>(CallSite site)
    {
      CallSite<Func<CallSite, TRet>> callSite = (CallSite<Func<CallSite, TRet>>) site;
      Func<CallSite, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, TRet>>(callSite);
      Func<CallSite, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, TRet>>(callSite);
      Func<CallSite, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[0];
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch0<TRet>(CallSite site)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute1<T0, TRet>(CallSite site, T0 arg0)
    {
      CallSite<Func<CallSite, T0, TRet>> callSite = (CallSite<Func<CallSite, T0, TRet>>) site;
      Func<CallSite, T0, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, TRet>>(callSite);
      Func<CallSite, T0, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, TRet>>(callSite);
      Func<CallSite, T0, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[1]
      {
        (object) arg0
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch1<T0, TRet>(CallSite site, T0 arg0)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute2<T0, T1, TRet>(CallSite site, T0 arg0, T1 arg1)
    {
      CallSite<Func<CallSite, T0, T1, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, TRet>>) site;
      Func<CallSite, T0, T1, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, TRet>>(callSite);
      Func<CallSite, T0, T1, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, TRet>>(callSite);
      Func<CallSite, T0, T1, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[2]
      {
        (object) arg0,
        (object) arg1
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch2<T0, T1, TRet>(CallSite site, T0 arg0, T1 arg1)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute3<T0, T1, T2, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2)
    {
      CallSite<Func<CallSite, T0, T1, T2, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, T2, TRet>>) site;
      Func<CallSite, T0, T1, T2, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, T2, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, T2, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, T2, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1, arg2);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, T2, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, T2, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, T2, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, T2, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, T2, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[3]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, T2, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, T2, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch3<T0, T1, T2, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute4<T0, T1, T2, T3, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
      CallSite<Func<CallSite, T0, T1, T2, T3, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, T2, T3, TRet>>) site;
      Func<CallSite, T0, T1, T2, T3, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, T2, T3, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, T2, T3, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, T2, T3, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1, arg2, arg3);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, T2, T3, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, T2, T3, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, T2, T3, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, T2, T3, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, T2, T3, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[4]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, T2, T3, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, T2, T3, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch4<T0, T1, T2, T3, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute5<T0, T1, T2, T3, T4, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      CallSite<Func<CallSite, T0, T1, T2, T3, T4, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, T2, T3, T4, TRet>>) site;
      Func<CallSite, T0, T1, T2, T3, T4, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, T2, T3, T4, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, T2, T3, T4, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, T2, T3, T4, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1, arg2, arg3, arg4);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, T2, T3, T4, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, T2, T3, T4, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, T2, T3, T4, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, T2, T3, T4, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, T2, T3, T4, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[5]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, T2, T3, T4, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, T2, T3, T4, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch5<T0, T1, T2, T3, T4, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute6<T0, T1, T2, T3, T4, T5, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
      CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>) site;
      Func<CallSite, T0, T1, T2, T3, T4, T5, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, T2, T3, T4, T5, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1, arg2, arg3, arg4, arg5);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, T2, T3, T4, T5, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[6]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, T2, T3, T4, T5, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch6<T0, T1, T2, T3, T4, T5, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute7<T0, T1, T2, T3, T4, T5, T6, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
      CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>) site;
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[7]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch7<T0, T1, T2, T3, T4, T5, T6, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute8<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
      CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>) site;
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[8]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch8<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute9<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
      CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>) site;
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[9]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch9<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet UpdateAndExecute10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
      CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>> callSite = (CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>) site;
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> func1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> func2 = rules1[matched];
          if (func2 != func1)
          {
            callSite.Target = func2;
            TRet ret = func2(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>(callSite, matched);
              return ret;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>> ruleCache = CallSiteOps.GetRuleCache<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>(callSite);
      Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>(callSite, rule);
            CallSiteOps.MoveRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[10]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9
      };
      while (true)
      {
        callSite.Target = func1;
        Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> rule = callSite.Target = callSite.Binder.BindCore<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>(callSite, args);
        try
        {
          TRet ret = rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
          if (CallSiteOps.GetMatch(site))
            return ret;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static TRet NoMatch10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
      site._match = false;
      return default (TRet);
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid1<T0>(CallSite site, T0 arg0)
    {
      CallSite<Action<CallSite, T0>> callSite = (CallSite<Action<CallSite, T0>>) site;
      Action<CallSite, T0> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0>>(callSite);
      Action<CallSite, T0>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0>>(callSite);
      Action<CallSite, T0>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[1]
      {
        (object) arg0
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0>>(callSite, args);
        try
        {
          rule(site, arg0);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid1<T0>(CallSite site, T0 arg0)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid2<T0, T1>(CallSite site, T0 arg0, T1 arg1)
    {
      CallSite<Action<CallSite, T0, T1>> callSite = (CallSite<Action<CallSite, T0, T1>>) site;
      Action<CallSite, T0, T1> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1>>(callSite);
      Action<CallSite, T0, T1>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1>>(callSite);
      Action<CallSite, T0, T1>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[2]
      {
        (object) arg0,
        (object) arg1
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1>>(callSite, args);
        try
        {
          rule(site, arg0, arg1);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid2<T0, T1>(CallSite site, T0 arg0, T1 arg1)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid3<T0, T1, T2>(CallSite site, T0 arg0, T1 arg1, T2 arg2)
    {
      CallSite<Action<CallSite, T0, T1, T2>> callSite = (CallSite<Action<CallSite, T0, T1, T2>>) site;
      Action<CallSite, T0, T1, T2> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1, T2>>(callSite);
      Action<CallSite, T0, T1, T2>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1, T2>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1, T2> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1, arg2);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1, T2>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1, T2>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1, T2>>(callSite);
      Action<CallSite, T0, T1, T2>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1, T2> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1, arg2);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1, T2>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[3]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1, T2> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1, T2>>(callSite, args);
        try
        {
          rule(site, arg0, arg1, arg2);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid3<T0, T1, T2>(CallSite site, T0 arg0, T1 arg1, T2 arg2)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid4<T0, T1, T2, T3>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
      CallSite<Action<CallSite, T0, T1, T2, T3>> callSite = (CallSite<Action<CallSite, T0, T1, T2, T3>>) site;
      Action<CallSite, T0, T1, T2, T3> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1, T2, T3>>(callSite);
      Action<CallSite, T0, T1, T2, T3>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1, T2, T3>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1, T2, T3> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1, arg2, arg3);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1, T2, T3>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1, T2, T3>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1, T2, T3>>(callSite);
      Action<CallSite, T0, T1, T2, T3>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1, T2, T3> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1, arg2, arg3);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1, T2, T3>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[4]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1, T2, T3> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1, T2, T3>>(callSite, args);
        try
        {
          rule(site, arg0, arg1, arg2, arg3);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid4<T0, T1, T2, T3>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid5<T0, T1, T2, T3, T4>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      CallSite<Action<CallSite, T0, T1, T2, T3, T4>> callSite = (CallSite<Action<CallSite, T0, T1, T2, T3, T4>>) site;
      Action<CallSite, T0, T1, T2, T3, T4> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1, T2, T3, T4>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1, T2, T3, T4>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1, T2, T3, T4> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1, arg2, arg3, arg4);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1, T2, T3, T4>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1, T2, T3, T4>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1, T2, T3, T4>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1, T2, T3, T4> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1, T2, T3, T4>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[5]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1, T2, T3, T4> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1, T2, T3, T4>>(callSite, args);
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid5<T0, T1, T2, T3, T4>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid6<T0, T1, T2, T3, T4, T5>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
      CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5>> callSite = (CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5>>) site;
      Action<CallSite, T0, T1, T2, T3, T4, T5> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1, T2, T3, T4, T5>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1, T2, T3, T4, T5>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1, T2, T3, T4, T5> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1, arg2, arg3, arg4, arg5);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1, T2, T3, T4, T5>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1, T2, T3, T4, T5> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1, T2, T3, T4, T5>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[6]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1, T2, T3, T4, T5> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1, T2, T3, T4, T5>>(callSite, args);
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid6<T0, T1, T2, T3, T4, T5>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid7<T0, T1, T2, T3, T4, T5, T6>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
      CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>> callSite = (CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>) site;
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1, T2, T3, T4, T5, T6> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1, T2, T3, T4, T5, T6> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[7]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1, T2, T3, T4, T5, T6> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>(callSite, args);
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid7<T0, T1, T2, T3, T4, T5, T6>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid8<T0, T1, T2, T3, T4, T5, T6, T7>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
      CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>> callSite = (CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>) site;
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[8]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>(callSite, args);
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid8<T0, T1, T2, T3, T4, T5, T6, T7>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
      CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>> callSite = (CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>) site;
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[9]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>(callSite, args);
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
      site._match = false;
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void UpdateAndExecuteVoid10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
      CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>> callSite = (CallSite<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>) site;
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> action1 = callSite.Target;
      site = (CallSite) CallSiteOps.CreateMatchmaker<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>[] rules1;
      if ((rules1 = CallSiteOps.GetRules<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(callSite)) != null)
      {
        for (int matched = 0; matched < rules1.Length; ++matched)
        {
          Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> action2 = rules1[matched];
          if (action2 != action1)
          {
            callSite.Target = action2;
            action2(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            if (CallSiteOps.GetMatch(site))
            {
              CallSiteOps.UpdateRules<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(callSite, matched);
              return;
            }
            else
              CallSiteOps.ClearMatch(site);
          }
        }
      }
      RuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>> ruleCache = CallSiteOps.GetRuleCache<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(callSite);
      Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>[] rules2 = ruleCache.GetRules();
      for (int i = 0; i < rules2.Length; ++i)
      {
        Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> rule = rules2[i];
        callSite.Target = rule;
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
          if (CallSiteOps.GetMatch(site))
            return;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
          {
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(callSite, rule);
            CallSiteOps.MoveRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(ruleCache, rule, i);
          }
        }
        CallSiteOps.ClearMatch(site);
      }
      object[] args = new object[10]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9
      };
      while (true)
      {
        callSite.Target = action1;
        Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> rule = callSite.Target = callSite.Binder.BindCore<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(callSite, args);
        try
        {
          rule(site, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
          if (CallSiteOps.GetMatch(site))
            break;
        }
        finally
        {
          if (CallSiteOps.GetMatch(site))
            CallSiteOps.AddRule<Action<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(callSite, rule);
        }
        CallSiteOps.ClearMatch(site);
      }
    }

    [Obsolete("pregenerated CallSite<T>.Update delegate", true)]
    internal static void NoMatchVoid10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
      site._match = false;
    }
  }
}
