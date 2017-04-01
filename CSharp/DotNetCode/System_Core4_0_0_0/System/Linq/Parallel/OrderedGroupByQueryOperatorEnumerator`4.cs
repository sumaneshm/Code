// Type: System.Linq.Parallel.OrderedGroupByQueryOperatorEnumerator`4
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal abstract class OrderedGroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey> : QueryOperatorEnumerator<IGrouping<TGroupKey, TElement>, TOrderKey>
  {
    protected readonly QueryOperatorEnumerator<Pair<TSource, TGroupKey>, TOrderKey> m_source;
    private readonly Func<TSource, TGroupKey> m_keySelector;
    protected readonly IEqualityComparer<TGroupKey> m_keyComparer;
    protected readonly IComparer<TOrderKey> m_orderComparer;
    protected readonly CancellationToken m_cancellationToken;
    private OrderedGroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.Mutables m_mutables;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected OrderedGroupByQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TSource, TGroupKey>, TOrderKey> source, Func<TSource, TGroupKey> keySelector, IEqualityComparer<TGroupKey> keyComparer, IComparer<TOrderKey> orderComparer, CancellationToken cancellationToken)
    {
      this.m_source = source;
      this.m_keySelector = keySelector;
      this.m_keyComparer = keyComparer;
      this.m_orderComparer = orderComparer;
      this.m_cancellationToken = cancellationToken;
    }

    internal override bool MoveNext(ref IGrouping<TGroupKey, TElement> currentElement, ref TOrderKey currentKey)
    {
      OrderedGroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.Mutables mutables = this.m_mutables;
      if (mutables == null)
      {
        mutables = this.m_mutables = new OrderedGroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.Mutables();
        mutables.m_hashLookup = this.BuildHashLookup();
        mutables.m_hashLookupIndex = -1;
      }
      if (++mutables.m_hashLookupIndex >= mutables.m_hashLookup.Count)
        return false;
      OrderedGroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.GroupKeyData groupKeyData = mutables.m_hashLookup[mutables.m_hashLookupIndex].Value;
      currentElement = (IGrouping<TGroupKey, TElement>) groupKeyData.m_grouping;
      currentKey = groupKeyData.m_orderKey;
      return true;
    }

    protected abstract HashLookup<Wrapper<TGroupKey>, OrderedGroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.GroupKeyData> BuildHashLookup();

    protected override void Dispose(bool disposing)
    {
      this.m_source.Dispose();
    }

    private class Mutables
    {
      internal HashLookup<Wrapper<TGroupKey>, OrderedGroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.GroupKeyData> m_hashLookup;
      internal int m_hashLookupIndex;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      public Mutables()
      {
      }
    }

    protected class GroupKeyData
    {
      internal TOrderKey m_orderKey;
      internal OrderedGroupByGrouping<TGroupKey, TOrderKey, TElement> m_grouping;

      internal GroupKeyData(TOrderKey orderKey, TGroupKey hashKey, IComparer<TOrderKey> orderComparer)
      {
        this.m_orderKey = orderKey;
        this.m_grouping = new OrderedGroupByGrouping<TGroupKey, TOrderKey, TElement>(hashKey, orderComparer);
      }
    }
  }
}
