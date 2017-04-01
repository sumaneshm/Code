// Type: System.Dynamic.ExpandoClass
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;

namespace System.Dynamic
{
  internal class ExpandoClass
  {
    internal static ExpandoClass Empty = new ExpandoClass();
    private readonly string[] _keys;
    private readonly int _hashCode;
    private Dictionary<int, List<WeakReference>> _transitions;
    private const int EmptyHashCode = 6551;

    internal string[] Keys
    {
      get
      {
        return this._keys;
      }
    }

    static ExpandoClass()
    {
    }

    internal ExpandoClass()
    {
      this._hashCode = 6551;
      this._keys = new string[0];
    }

    internal ExpandoClass(string[] keys, int hashCode)
    {
      this._hashCode = hashCode;
      this._keys = keys;
    }

    internal ExpandoClass FindNewClass(string newKey)
    {
      int hashCode = this._hashCode ^ newKey.GetHashCode();
      lock (this)
      {
        List<WeakReference> local_1 = this.GetTransitionList(hashCode);
        for (int local_2 = 0; local_2 < local_1.Count; ++local_2)
        {
          ExpandoClass local_3 = local_1[local_2].Target as ExpandoClass;
          if (local_3 == null)
          {
            local_1.RemoveAt(local_2);
            --local_2;
          }
          else if (string.Equals(local_3._keys[local_3._keys.Length - 1], newKey, StringComparison.Ordinal))
            return local_3;
        }
        string[] local_4 = new string[this._keys.Length + 1];
        Array.Copy((Array) this._keys, (Array) local_4, this._keys.Length);
        local_4[this._keys.Length] = newKey;
        ExpandoClass local_5 = new ExpandoClass(local_4, hashCode);
        local_1.Add(new WeakReference((object) local_5));
        return local_5;
      }
    }

    private List<WeakReference> GetTransitionList(int hashCode)
    {
      if (this._transitions == null)
        this._transitions = new Dictionary<int, List<WeakReference>>();
      List<WeakReference> list;
      if (!this._transitions.TryGetValue(hashCode, out list))
        this._transitions[hashCode] = list = new List<WeakReference>();
      return list;
    }

    internal int GetValueIndex(string name, bool caseInsensitive, ExpandoObject obj)
    {
      if (caseInsensitive)
        return this.GetValueIndexCaseInsensitive(name, obj);
      else
        return this.GetValueIndexCaseSensitive(name);
    }

    internal int GetValueIndexCaseSensitive(string name)
    {
      for (int index = 0; index < this._keys.Length; ++index)
      {
        if (string.Equals(this._keys[index], name, StringComparison.Ordinal))
          return index;
      }
      return -1;
    }

    private int GetValueIndexCaseInsensitive(string name, ExpandoObject obj)
    {
      int num = -1;
      lock (obj.LockObject)
      {
        for (int local_1 = this._keys.Length - 1; local_1 >= 0; --local_1)
        {
          if (string.Equals(this._keys[local_1], name, StringComparison.OrdinalIgnoreCase) && !obj.IsDeletedMember(local_1))
          {
            if (num != -1)
              return -2;
            num = local_1;
          }
        }
      }
      return num;
    }
  }
}
