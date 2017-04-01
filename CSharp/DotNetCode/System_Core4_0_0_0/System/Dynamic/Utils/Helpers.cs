// Type: System.Dynamic.Utils.Helpers
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Dynamic.Utils
{
  internal static class Helpers
  {
    internal static T CommonNode<T>(T first, T second, Func<T, T> parent) where T : class
    {
      EqualityComparer<T> @default = EqualityComparer<T>.Default;
      if (@default.Equals(first, second))
        return first;
      Set<T> set = new Set<T>((IEqualityComparer<T>) @default);
      for (T obj = first; (object) obj != null; obj = parent(obj))
        set.Add(obj);
      for (T obj = second; (object) obj != null; obj = parent(obj))
      {
        if (set.Contains(obj))
          return obj;
      }
      return default (T);
    }

    internal static void IncrementCount<T>(T key, Dictionary<T, int> dict)
    {
      int num;
      dict.TryGetValue(key, out num);
      dict[key] = num + 1;
    }
  }
}
