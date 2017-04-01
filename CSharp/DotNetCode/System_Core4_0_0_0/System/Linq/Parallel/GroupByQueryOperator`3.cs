// Type: System.Linq.Parallel.GroupByQueryOperator`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class GroupByQueryOperator<TSource, TGroupKey, TElement> : UnaryQueryOperator<TSource, IGrouping<TGroupKey, TElement>>
  {
    private readonly Func<TSource, TGroupKey> m_keySelector;
    private readonly Func<TSource, TElement> m_elementSelector;
    private readonly IEqualityComparer<TGroupKey> m_keyComparer;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal GroupByQueryOperator(IEnumerable<TSource> child, Func<TSource, TGroupKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TGroupKey> keyComparer)
      : base(child)
    {
      this.m_keySelector = keySelector;
      this.m_elementSelector = elementSelector;
      this.m_keyComparer = keyComparer;
      this.SetOrdinalIndexState(OrdinalIndexState.Shuffled);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<IGrouping<TGroupKey, TElement>> recipient, bool preferStriping, QuerySettings settings)
    {
      if (this.Child.OutputOrdered)
        this.WrapPartitionedStreamHelperOrdered<TKey>(ExchangeUtilities.HashRepartitionOrdered<TSource, TGroupKey, TKey>(inputStream, this.m_keySelector, this.m_keyComparer, (IEqualityComparer<TSource>) null, settings.CancellationState.MergedCancellationToken), recipient, settings.CancellationState.MergedCancellationToken);
      else
        this.WrapPartitionedStreamHelper<TKey, int>(ExchangeUtilities.HashRepartition<TSource, TGroupKey, TKey>(inputStream, this.m_keySelector, this.m_keyComparer, (IEqualityComparer<TSource>) null, settings.CancellationState.MergedCancellationToken), recipient, settings.CancellationState.MergedCancellationToken);
    }

    private void WrapPartitionedStreamHelper<TIgnoreKey, TKey>(PartitionedStream<Pair<TSource, TGroupKey>, TKey> hashStream, IPartitionedStreamRecipient<IGrouping<TGroupKey, TElement>> recipient, CancellationToken cancellationToken)
    {
      int partitionCount = hashStream.PartitionCount;
      PartitionedStream<IGrouping<TGroupKey, TElement>, TKey> partitionedStream = new PartitionedStream<IGrouping<TGroupKey, TElement>, TKey>(partitionCount, hashStream.KeyComparer, OrdinalIndexState.Shuffled);
      for (int index = 0; index < partitionCount; ++index)
      {
        if (this.m_elementSelector == null)
        {
          GroupByIdentityQueryOperatorEnumerator<TSource, TGroupKey, TKey> operatorEnumerator = new GroupByIdentityQueryOperatorEnumerator<TSource, TGroupKey, TKey>(hashStream[index], this.m_keyComparer, cancellationToken);
          partitionedStream[index] = (QueryOperatorEnumerator<IGrouping<TGroupKey, TElement>, TKey>) operatorEnumerator;
        }
        else
          partitionedStream[index] = (QueryOperatorEnumerator<IGrouping<TGroupKey, TElement>, TKey>) new GroupByElementSelectorQueryOperatorEnumerator<TSource, TGroupKey, TElement, TKey>(hashStream[index], this.m_keyComparer, this.m_elementSelector, cancellationToken);
      }
      recipient.Receive<TKey>(partitionedStream);
    }

    private void WrapPartitionedStreamHelperOrdered<TKey>(PartitionedStream<Pair<TSource, TGroupKey>, TKey> hashStream, IPartitionedStreamRecipient<IGrouping<TGroupKey, TElement>> recipient, CancellationToken cancellationToken)
    {
      int partitionCount = hashStream.PartitionCount;
      PartitionedStream<IGrouping<TGroupKey, TElement>, TKey> partitionedStream = new PartitionedStream<IGrouping<TGroupKey, TElement>, TKey>(partitionCount, hashStream.KeyComparer, OrdinalIndexState.Shuffled);
      IComparer<TKey> keyComparer = hashStream.KeyComparer;
      for (int index = 0; index < partitionCount; ++index)
      {
        if (this.m_elementSelector == null)
        {
          OrderedGroupByIdentityQueryOperatorEnumerator<TSource, TGroupKey, TKey> operatorEnumerator = new OrderedGroupByIdentityQueryOperatorEnumerator<TSource, TGroupKey, TKey>(hashStream[index], this.m_keySelector, this.m_keyComparer, keyComparer, cancellationToken);
          partitionedStream[index] = (QueryOperatorEnumerator<IGrouping<TGroupKey, TElement>, TKey>) operatorEnumerator;
        }
        else
          partitionedStream[index] = (QueryOperatorEnumerator<IGrouping<TGroupKey, TElement>, TKey>) new OrderedGroupByElementSelectorQueryOperatorEnumerator<TSource, TGroupKey, TElement, TKey>(hashStream[index], this.m_keySelector, this.m_elementSelector, this.m_keyComparer, keyComparer, cancellationToken);
      }
      recipient.Receive<TKey>(partitionedStream);
    }

    internal override QueryResults<IGrouping<TGroupKey, TElement>> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<IGrouping<TGroupKey, TElement>>) new UnaryQueryOperator<TSource, IGrouping<TGroupKey, TElement>>.UnaryQueryOperatorResults(this.Child.Open(settings, false), (UnaryQueryOperator<TSource, IGrouping<TGroupKey, TElement>>) this, settings, false);
    }

    internal override IEnumerable<IGrouping<TGroupKey, TElement>> AsSequentialQuery(CancellationToken token)
    {
      IEnumerable<TSource> source = CancellableEnumerable.Wrap<TSource>(this.Child.AsSequentialQuery(token), token);
      if (this.m_elementSelector == null)
        return (IEnumerable<IGrouping<TGroupKey, TElement>>) Enumerable.GroupBy<TSource, TGroupKey>(source, this.m_keySelector, this.m_keyComparer);
      else
        return Enumerable.GroupBy<TSource, TGroupKey, TElement>(source, this.m_keySelector, this.m_elementSelector, this.m_keyComparer);
    }
  }
}
