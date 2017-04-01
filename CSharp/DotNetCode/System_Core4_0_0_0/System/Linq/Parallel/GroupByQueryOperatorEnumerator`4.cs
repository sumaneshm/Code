// Type: System.Linq.Parallel.GroupByQueryOperatorEnumerator`4
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal abstract class GroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey> : QueryOperatorEnumerator<IGrouping<TGroupKey, TElement>, TOrderKey>
  {
    protected readonly QueryOperatorEnumerator<Pair<TSource, TGroupKey>, TOrderKey> m_source;
    protected readonly IEqualityComparer<TGroupKey> m_keyComparer;
    protected readonly CancellationToken m_cancellationToken;
    private GroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.Mutables m_mutables;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected GroupByQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TSource, TGroupKey>, TOrderKey> source, IEqualityComparer<TGroupKey> keyComparer, CancellationToken cancellationToken)
    {
      this.m_source = source;
      this.m_keyComparer = keyComparer;
      this.m_cancellationToken = cancellationToken;
    }

    internal override bool MoveNext(ref IGrouping<TGroupKey, TElement> currentElement, ref TOrderKey currentKey)
    {
      GroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.Mutables mutables = this.m_mutables;
      if (mutables == null)
      {
        mutables = this.m_mutables = new GroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>.Mutables();
        mutables.m_hashLookup = this.BuildHashLookup();
        mutables.m_hashLookupIndex = -1;
      }
      if (++mutables.m_hashLookupIndex >= mutables.m_hashLookup.Count)
        return false;
      currentElement = (IGrouping<TGroupKey, TElement>) new GroupByGrouping<TGroupKey, TElement>(mutables.m_hashLookup[mutables.m_hashLookupIndex]);
      return true;
    }

    protected abstract HashLookup<Wrapper<TGroupKey>, ListChunk<TElement>> BuildHashLookup();

    protected override void Dispose(bool disposing)
    {
      this.m_source.Dispose();
    }

    private class Mutables
    {
      internal HashLookup<Wrapper<TGroupKey>, ListChunk<TElement>> m_hashLookup;
      internal int m_hashLookupIndex;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      public Mutables()
      {
      }
    }
  }
}
