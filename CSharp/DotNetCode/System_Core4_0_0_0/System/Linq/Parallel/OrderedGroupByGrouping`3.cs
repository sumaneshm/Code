// Type: System.Linq.Parallel.OrderedGroupByGrouping`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq.Parallel
{
  internal class OrderedGroupByGrouping<TGroupKey, TOrderKey, TElement> : IGrouping<TGroupKey, TElement>, IEnumerable<TElement>, IEnumerable
  {
    private TGroupKey m_groupKey;
    private GrowingArray<TElement> m_values;
    private GrowingArray<TOrderKey> m_orderKeys;
    private IComparer<TOrderKey> m_orderComparer;

    TGroupKey IGrouping<TGroupKey, TElement>.Key
    {
      get
      {
        return this.m_groupKey;
      }
    }

    internal OrderedGroupByGrouping(TGroupKey groupKey, IComparer<TOrderKey> orderComparer)
    {
      this.m_groupKey = groupKey;
      this.m_values = new GrowingArray<TElement>();
      this.m_orderKeys = new GrowingArray<TOrderKey>();
      this.m_orderComparer = orderComparer;
    }

    IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
    {
      int valueCount = this.m_values.Count;
      TElement[] valueArray = this.m_values.InternalArray;
      for (int i = 0; i < valueCount; ++i)
        yield return valueArray[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    internal void Add(TElement value, TOrderKey orderKey)
    {
      this.m_values.Add(value);
      this.m_orderKeys.Add(orderKey);
    }

    internal void DoneAdding()
    {
      Array.Sort<TOrderKey, TElement>(this.m_orderKeys.InternalArray, this.m_values.InternalArray, 0, this.m_values.Count, this.m_orderComparer);
    }
  }
}
