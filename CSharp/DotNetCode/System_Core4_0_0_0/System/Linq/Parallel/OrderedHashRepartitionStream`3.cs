// Type: System.Linq.Parallel.OrderedHashRepartitionStream`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class OrderedHashRepartitionStream<TInputOutput, THashKey, TOrderKey> : HashRepartitionStream<TInputOutput, THashKey, TOrderKey>
  {
    internal OrderedHashRepartitionStream(PartitionedStream<TInputOutput, TOrderKey> inputStream, Func<TInputOutput, THashKey> hashKeySelector, IEqualityComparer<THashKey> hashKeyComparer, IEqualityComparer<TInputOutput> elementComparer, CancellationToken cancellationToken)
      : base(inputStream.PartitionCount, inputStream.KeyComparer, hashKeyComparer, elementComparer)
    {
      this.m_partitions = (QueryOperatorEnumerator<Pair<TInputOutput, THashKey>, TOrderKey>[]) new OrderedHashRepartitionEnumerator<TInputOutput, THashKey, TOrderKey>[inputStream.PartitionCount];
      CountdownEvent barrier = new CountdownEvent(inputStream.PartitionCount);
      ListChunk<Pair<TInputOutput, THashKey>>[,] valueExchangeMatrix = new ListChunk<Pair<TInputOutput, THashKey>>[inputStream.PartitionCount, inputStream.PartitionCount];
      ListChunk<TOrderKey>[,] keyExchangeMatrix = new ListChunk<TOrderKey>[inputStream.PartitionCount, inputStream.PartitionCount];
      for (int partitionIndex = 0; partitionIndex < inputStream.PartitionCount; ++partitionIndex)
        this.m_partitions[partitionIndex] = (QueryOperatorEnumerator<Pair<TInputOutput, THashKey>, TOrderKey>) new OrderedHashRepartitionEnumerator<TInputOutput, THashKey, TOrderKey>(inputStream[partitionIndex], inputStream.PartitionCount, partitionIndex, hashKeySelector, this, barrier, valueExchangeMatrix, keyExchangeMatrix, cancellationToken);
    }
  }
}
