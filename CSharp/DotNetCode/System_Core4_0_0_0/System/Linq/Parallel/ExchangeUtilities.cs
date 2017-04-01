// Type: System.Linq.Parallel.ExchangeUtilities
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
  internal static class ExchangeUtilities
  {
    internal static PartitionedStream<T, int> PartitionDataSource<T>(IEnumerable<T> source, int partitionCount, bool useStriping)
    {
      IParallelPartitionable<T> parallelPartitionable = source as IParallelPartitionable<T>;
      PartitionedStream<T, int> partitionedStream1;
      if (parallelPartitionable != null)
      {
        QueryOperatorEnumerator<T, int>[] partitions = parallelPartitionable.GetPartitions(partitionCount);
        if (partitions == null)
          throw new InvalidOperationException(System.Linq.SR.GetString("ParallelPartitionable_NullReturn"));
        if (partitions.Length != partitionCount)
          throw new InvalidOperationException(System.Linq.SR.GetString("ParallelPartitionable_IncorretElementCount"));
        PartitionedStream<T, int> partitionedStream2 = new PartitionedStream<T, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
        for (int index = 0; index < partitionCount; ++index)
        {
          QueryOperatorEnumerator<T, int> operatorEnumerator = partitions[index];
          if (operatorEnumerator == null)
            throw new InvalidOperationException(System.Linq.SR.GetString("ParallelPartitionable_NullElement"));
          partitionedStream2[index] = operatorEnumerator;
        }
        partitionedStream1 = partitionedStream2;
      }
      else
        partitionedStream1 = (PartitionedStream<T, int>) new PartitionedDataSource<T>(source, partitionCount, useStriping);
      return partitionedStream1;
    }

    internal static PartitionedStream<Pair<TElement, THashKey>, int> HashRepartition<TElement, THashKey, TIgnoreKey>(PartitionedStream<TElement, TIgnoreKey> source, Func<TElement, THashKey> keySelector, IEqualityComparer<THashKey> keyComparer, IEqualityComparer<TElement> elementComparer, CancellationToken cancellationToken)
    {
      return (PartitionedStream<Pair<TElement, THashKey>, int>) new UnorderedHashRepartitionStream<TElement, THashKey, TIgnoreKey>(source, keySelector, keyComparer, elementComparer, cancellationToken);
    }

    internal static PartitionedStream<Pair<TElement, THashKey>, TOrderKey> HashRepartitionOrdered<TElement, THashKey, TOrderKey>(PartitionedStream<TElement, TOrderKey> source, Func<TElement, THashKey> keySelector, IEqualityComparer<THashKey> keyComparer, IEqualityComparer<TElement> elementComparer, CancellationToken cancellationToken)
    {
      return (PartitionedStream<Pair<TElement, THashKey>, TOrderKey>) new OrderedHashRepartitionStream<TElement, THashKey, TOrderKey>(source, keySelector, keyComparer, elementComparer, cancellationToken);
    }

    internal static OrdinalIndexState Worse(this OrdinalIndexState state1, OrdinalIndexState state2)
    {
      if (state1 <= state2)
        return state2;
      else
        return state1;
    }

    internal static bool IsWorseThan(this OrdinalIndexState state1, OrdinalIndexState state2)
    {
      return state1 > state2;
    }
  }
}
