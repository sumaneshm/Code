// Type: System.Runtime.CompilerServices.RuleCache`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic.Utils;

namespace System.Runtime.CompilerServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerStepThrough]
  [__DynamicallyInvokable]
  public class RuleCache<T> where T : class
  {
    private T[] _rules = new T[0];
    private readonly object cacheLock = new object();
    private const int MaxRules = 128;
    private const int InsertPosition = 64;

    internal RuleCache()
    {
    }

    internal T[] GetRules()
    {
      return this._rules;
    }

    internal void MoveRule(T rule, int i)
    {
      lock (this.cacheLock)
      {
        int local_0 = this._rules.Length - i;
        if (local_0 > 8)
          local_0 = 8;
        int local_1 = -1;
        int local_2 = Math.Min(this._rules.Length, i + local_0);
        for (int local_3 = i; local_3 < local_2; ++local_3)
        {
          if ((object) this._rules[local_3] == (object) rule)
          {
            local_1 = local_3;
            break;
          }
        }
        if (local_1 < 0)
          return;
        T local_4 = this._rules[local_1];
        this._rules[local_1] = this._rules[local_1 - 1];
        this._rules[local_1 - 1] = this._rules[local_1 - 2];
        this._rules[local_1 - 2] = local_4;
      }
    }

    internal void AddRule(T newRule)
    {
      lock (this.cacheLock)
        this._rules = RuleCache<T>.AddOrInsert(this._rules, newRule);
    }

    internal void ReplaceRule(T oldRule, T newRule)
    {
      lock (this.cacheLock)
      {
        int local_0 = Array.IndexOf<T>(this._rules, oldRule);
        if (local_0 >= 0)
          this._rules[local_0] = newRule;
        else
          this._rules = RuleCache<T>.AddOrInsert(this._rules, newRule);
      }
    }

    private static T[] AddOrInsert(T[] rules, T item)
    {
      if (rules.Length < 64)
        return CollectionExtensions.AddLast<T>((IList<T>) rules, item);
      int length = rules.Length + 1;
      T[] objArray;
      if (length > 128)
      {
        length = 128;
        objArray = rules;
      }
      else
        objArray = new T[length];
      Array.Copy((Array) rules, 0, (Array) objArray, 0, 64);
      objArray[64] = item;
      Array.Copy((Array) rules, 64, (Array) objArray, 65, length - 64 - 1);
      return objArray;
    }
  }
}
