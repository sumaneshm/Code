// Type: System.Linq.OrderedEnumerable`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq
{
  internal abstract class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>, IEnumerable<TElement>, IEnumerable
  {
    internal IEnumerable<TElement> source;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected OrderedEnumerable()
    {
    }

    public IEnumerator<TElement> GetEnumerator()
    {
      Buffer<TElement> buffer = new Buffer<TElement>(this.source);
      if (buffer.count > 0)
      {
        EnumerableSorter<TElement> sorter = this.GetEnumerableSorter((EnumerableSorter<TElement>) null);
        int[] map = sorter.Sort(buffer.items, buffer.count);
        sorter = (EnumerableSorter<TElement>) null;
        for (int i = 0; i < buffer.count; ++i)
          yield return buffer.items[map[i]];
      }
    }

    internal abstract EnumerableSorter<TElement> GetEnumerableSorter(EnumerableSorter<TElement> next);

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    IOrderedEnumerable<TElement> IOrderedEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
    {
      return (IOrderedEnumerable<TElement>) new OrderedEnumerable<TElement, TKey>(this.source, keySelector, comparer, descending)
      {
        parent = this
      };
    }
  }
}
