// Type: System.Linq.Parallel.UnorderedHashRepartitionStream`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class UnorderedHashRepartitionStream<TInputOutput, THashKey, TIgnoreKey> : HashRepartitionStream<TInputOutput, THashKey, int>
  {
    internal UnorderedHashRepartitionStream(PartitionedStream<TInputOutput, TIgnoreKey> inputStream, Func<TInputOutput, THashKey> keySelector, IEqualityComparer<THashKey> keyComparer, IEqualityComparer<TInputOutput> elementComparer, CancellationToken cancellationToken)
      : base(inputStream.PartitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), keyComparer, elementComparer)
    {
      this.m_partitions = (QueryOperatorEnumerator<Pair<TInputOutput, THashKey>, int>[]) new HashRepartitionEnumerator<TInputOutput, THashKey, TIgnoreKey>[inputStream.PartitionCount];
      CountdownEvent barrier = new CountdownEvent(inputStream.PartitionCount);
      ListChunk<Pair<TInputOutput, THashKey>>[,] valueExchangeMatrix = new ListChunk<Pair<TInputOutput, THashKey>>[inputStream.PartitionCount, inputStream.PartitionCount];
      for (int partitionIndex = 0; partitionIndex < inputStream.PartitionCount; ++partitionIndex)
        this.m_partitions[partitionIndex] = (QueryOperatorEnumerator<Pair<TInputOutput, THashKey>, int>) new HashRepartitionEnumerator<TInputOutput, THashKey, TIgnoreKey>(inputStream[partitionIndex], inputStream.PartitionCount, partitionIndex, keySelector, (HashRepartitionStream<TInputOutput, THashKey, int>) this, barrier, valueExchangeMatrix, cancellationToken);
    }
  }
}
