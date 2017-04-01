// Type: System.Linq.OrderedEnumerable`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;

namespace System.Linq
{
  internal class OrderedEnumerable<TElement, TKey> : OrderedEnumerable<TElement>
  {
    internal OrderedEnumerable<TElement> parent;
    internal Func<TElement, TKey> keySelector;
    internal IComparer<TKey> comparer;
    internal bool descending;

    internal OrderedEnumerable(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      this.source = source;
      this.parent = (OrderedEnumerable<TElement>) null;
      this.keySelector = keySelector;
      this.comparer = comparer != null ? comparer : (IComparer<TKey>) Comparer<TKey>.Default;
      this.descending = descending;
    }

    internal override EnumerableSorter<TElement> GetEnumerableSorter(EnumerableSorter<TElement> next)
    {
      EnumerableSorter<TElement> next1 = (EnumerableSorter<TElement>) new EnumerableSorter<TElement, TKey>(this.keySelector, this.comparer, this.descending, next);
      if (this.parent != null)
        next1 = this.parent.GetEnumerableSorter(next1);
      return next1;
    }
  }
}
