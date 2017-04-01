// Type: System.Linq.Parallel.OrderedHashRepartitionEnumerator`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class OrderedHashRepartitionEnumerator<TInputOutput, THashKey, TOrderKey> : QueryOperatorEnumerator<Pair<TInputOutput, THashKey>, TOrderKey>
  {
    private readonly int m_partitionCount;
    private readonly int m_partitionIndex;
    private readonly Func<TInputOutput, THashKey> m_keySelector;
    private readonly HashRepartitionStream<TInputOutput, THashKey, TOrderKey> m_repartitionStream;
    private readonly ListChunk<Pair<TInputOutput, THashKey>>[,] m_valueExchangeMatrix;
    private readonly ListChunk<TOrderKey>[,] m_keyExchangeMatrix;
    private readonly QueryOperatorEnumerator<TInputOutput, TOrderKey> m_source;
    private CountdownEvent m_barrier;
    private readonly CancellationToken m_cancellationToken;
    private OrderedHashRepartitionEnumerator<TInputOutput, THashKey, TOrderKey>.Mutables m_mutables;
    private const int ENUMERATION_NOT_STARTED = -1;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal OrderedHashRepartitionEnumerator(QueryOperatorEnumerator<TInputOutput, TOrderKey> source, int partitionCount, int partitionIndex, Func<TInputOutput, THashKey> keySelector, OrderedHashRepartitionStream<TInputOutput, THashKey, TOrderKey> repartitionStream, CountdownEvent barrier, ListChunk<Pair<TInputOutput, THashKey>>[,] valueExchangeMatrix, ListChunk<TOrderKey>[,] keyExchangeMatrix, CancellationToken cancellationToken)
    {
      this.m_source = source;
      this.m_partitionCount = partitionCount;
      this.m_partitionIndex = partitionIndex;
      this.m_keySelector = keySelector;
      this.m_repartitionStream = (HashRepartitionStream<TInputOutput, THashKey, TOrderKey>) repartitionStream;
      this.m_barrier = barrier;
      this.m_valueExchangeMatrix = valueExchangeMatrix;
      this.m_keyExchangeMatrix = keyExchangeMatrix;
      this.m_cancellationToken = cancellationToken;
    }

    internal override bool MoveNext(ref Pair<TInputOutput, THashKey> currentElement, ref TOrderKey currentKey)
    {
      if (this.m_partitionCount == 1)
      {
        TInputOutput currentElement1 = default (TInputOutput);
        if (!this.m_source.MoveNext(ref currentElement1, ref currentKey))
          return false;
        currentElement = new Pair<TInputOutput, THashKey>(currentElement1, this.m_keySelector == null ? default (THashKey) : this.m_keySelector(currentElement1));
        return true;
      }
      else
      {
        OrderedHashRepartitionEnumerator<TInputOutput, THashKey, TOrderKey>.Mutables mutables = this.m_mutables ?? (this.m_mutables = new OrderedHashRepartitionEnumerator<TInputOutput, THashKey, TOrderKey>.Mutables());
        if (mutables.m_currentBufferIndex == -1)
          this.EnumerateAndRedistributeElements();
        while (mutables.m_currentBufferIndex < this.m_partitionCount)
        {
          if (mutables.m_currentBuffer != null)
          {
            if (++mutables.m_currentIndex < mutables.m_currentBuffer.Count)
            {
              currentElement = mutables.m_currentBuffer.m_chunk[mutables.m_currentIndex];
              currentKey = mutables.m_currentKeyBuffer.m_chunk[mutables.m_currentIndex];
              return true;
            }
            else
            {
              mutables.m_currentIndex = -1;
              mutables.m_currentBuffer = mutables.m_currentBuffer.Next;
              mutables.m_currentKeyBuffer = mutables.m_currentKeyBuffer.Next;
            }
          }
          else
          {
            if (mutables.m_currentBufferIndex == this.m_partitionIndex)
            {
              this.m_barrier.Wait(this.m_cancellationToken);
              mutables.m_currentBufferIndex = -1;
            }
            ++mutables.m_currentBufferIndex;
            mutables.m_currentIndex = -1;
            if (mutables.m_currentBufferIndex == this.m_partitionIndex)
              ++mutables.m_currentBufferIndex;
            if (mutables.m_currentBufferIndex < this.m_partitionCount)
            {
              mutables.m_currentBuffer = this.m_valueExchangeMatrix[mutables.m_currentBufferIndex, this.m_partitionIndex];
              mutables.m_currentKeyBuffer = this.m_keyExchangeMatrix[mutables.m_currentBufferIndex, this.m_partitionIndex];
            }
          }
        }
        return false;
      }
    }

    private void EnumerateAndRedistributeElements()
    {
      OrderedHashRepartitionEnumerator<TInputOutput, THashKey, TOrderKey>.Mutables mutables = this.m_mutables;
      ListChunk<Pair<TInputOutput, THashKey>>[] listChunkArray1 = new ListChunk<Pair<TInputOutput, THashKey>>[this.m_partitionCount];
      ListChunk<TOrderKey>[] listChunkArray2 = new ListChunk<TOrderKey>[this.m_partitionCount];
      TInputOutput currentElement = default (TInputOutput);
      TOrderKey currentKey = default (TOrderKey);
      int num = 0;
      while (this.m_source.MoveNext(ref currentElement, ref currentKey))
      {
        if ((num++ & 63) == 0)
          CancellationState.ThrowIfCanceled(this.m_cancellationToken);
        THashKey hashKey = default (THashKey);
        int index;
        if (this.m_keySelector != null)
        {
          hashKey = this.m_keySelector(currentElement);
          index = this.m_repartitionStream.GetHashCode(hashKey) % this.m_partitionCount;
        }
        else
          index = this.m_repartitionStream.GetHashCode(currentElement) % this.m_partitionCount;
        ListChunk<Pair<TInputOutput, THashKey>> listChunk1 = listChunkArray1[index];
        ListChunk<TOrderKey> listChunk2 = listChunkArray2[index];
        if (listChunk1 == null)
        {
          listChunkArray1[index] = listChunk1 = new ListChunk<Pair<TInputOutput, THashKey>>(128);
          listChunkArray2[index] = listChunk2 = new ListChunk<TOrderKey>(128);
        }
        listChunk1.Add(new Pair<TInputOutput, THashKey>(currentElement, hashKey));
        listChunk2.Add(currentKey);
      }
      for (int index = 0; index < this.m_partitionCount; ++index)
      {
        this.m_valueExchangeMatrix[this.m_partitionIndex, index] = listChunkArray1[index];
        this.m_keyExchangeMatrix[this.m_partitionIndex, index] = listChunkArray2[index];
      }
      this.m_barrier.Signal();
      mutables.m_currentBufferIndex = this.m_partitionIndex;
      mutables.m_currentBuffer = listChunkArray1[this.m_partitionIndex];
      mutables.m_currentKeyBuffer = listChunkArray2[this.m_partitionIndex];
      mutables.m_currentIndex = -1;
    }

    protected override void Dispose(bool disposing)
    {
      if (this.m_barrier == null)
        return;
      if (this.m_mutables == null || this.m_mutables.m_currentBufferIndex == -1)
      {
        this.m_barrier.Signal();
        this.m_barrier = (CountdownEvent) null;
      }
      this.m_source.Dispose();
    }

    private class Mutables
    {
      internal int m_currentBufferIndex;
      internal ListChunk<Pair<TInputOutput, THashKey>> m_currentBuffer;
      internal ListChunk<TOrderKey> m_currentKeyBuffer;
      internal int m_currentIndex;

      internal Mutables()
      {
        this.m_currentBufferIndex = -1;
      }
    }
  }
}
