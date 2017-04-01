// Type: System.Linq.Parallel.SortQueryOperator`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class SortQueryOperator<TInputOutput, TSortKey> : UnaryQueryOperator<TInputOutput, TInputOutput>, IOrderedEnumerable<TInputOutput>, IEnumerable<TInputOutput>, IEnumerable
  {
    private readonly Func<TInputOutput, TSortKey> m_keySelector;
    private readonly IComparer<TSortKey> m_comparer;

    internal Func<TInputOutput, TSortKey> KeySelector
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_keySelector;
      }
    }

    internal IComparer<TSortKey> KeyComparer
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_comparer;
      }
    }

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal SortQueryOperator(IEnumerable<TInputOutput> source, Func<TInputOutput, TSortKey> keySelector, IComparer<TSortKey> comparer, bool descending)
      : base(source, true)
    {
      this.m_keySelector = keySelector;
      this.m_comparer = comparer != null ? comparer : (IComparer<TSortKey>) Util.GetDefaultComparer<TSortKey>();
      if (descending)
        this.m_comparer = (IComparer<TSortKey>) new ReverseComparer<TSortKey>(this.m_comparer);
      this.SetOrdinalIndexState(OrdinalIndexState.Shuffled);
    }

    IOrderedEnumerable<TInputOutput> IOrderedEnumerable<TInputOutput>.CreateOrderedEnumerable<TKey2>(Func<TInputOutput, TKey2> key2Selector, IComparer<TKey2> key2Comparer, bool descending)
    {
      key2Comparer = key2Comparer ?? (IComparer<TKey2>) Util.GetDefaultComparer<TKey2>();
      if (descending)
        key2Comparer = (IComparer<TKey2>) new ReverseComparer<TKey2>(key2Comparer);
      IComparer<Pair<TSortKey, TKey2>> comparer = (IComparer<Pair<TSortKey, TKey2>>) new PairComparer<TSortKey, TKey2>(this.m_comparer, key2Comparer);
      return (IOrderedEnumerable<TInputOutput>) new SortQueryOperator<TInputOutput, Pair<TSortKey, TKey2>>((IEnumerable<TInputOutput>) this.Child, (Func<TInputOutput, Pair<TSortKey, TKey2>>) (elem => new Pair<TSortKey, TKey2>(this.m_keySelector(elem), key2Selector(elem))), comparer, false);
    }

    internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TInputOutput>) new SortQueryOperatorResults<TInputOutput, TSortKey>(this.Child.Open(settings, false), this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInputOutput, TKey> inputStream, IPartitionedStreamRecipient<TInputOutput> recipient, bool preferStriping, QuerySettings settings)
    {
      PartitionedStream<TInputOutput, TSortKey> partitionedStream = new PartitionedStream<TInputOutput, TSortKey>(inputStream.PartitionCount, this.m_comparer, this.OrdinalIndexState);
      for (int index = 0; index < partitionedStream.PartitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TInputOutput, TSortKey>) new SortQueryOperatorEnumerator<TInputOutput, TKey, TSortKey>(inputStream[index], this.m_keySelector, this.m_comparer);
      recipient.Receive<TSortKey>(partitionedStream);
    }

    internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
    {
      return (IEnumerable<TInputOutput>) Enumerable.OrderBy<TInputOutput, TSortKey>(CancellableEnumerable.Wrap<TInputOutput>(this.Child.AsSequentialQuery(token), token), this.m_keySelector, this.m_comparer);
    }
  }
}
