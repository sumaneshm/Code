// Type: System.Linq.Parallel.HashRepartitionEnumerator`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class HashRepartitionEnumerator<TInputOutput, THashKey, TIgnoreKey> : QueryOperatorEnumerator<Pair<TInputOutput, THashKey>, int>
  {
    private readonly int m_partitionCount;
    private readonly int m_partitionIndex;
    private readonly Func<TInputOutput, THashKey> m_keySelector;
    private readonly HashRepartitionStream<TInputOutput, THashKey, int> m_repartitionStream;
    private readonly ListChunk<Pair<TInputOutput, THashKey>>[,] m_valueExchangeMatrix;
    private readonly QueryOperatorEnumerator<TInputOutput, TIgnoreKey> m_source;
    private CountdownEvent m_barrier;
    private readonly CancellationToken m_cancellationToken;
    private HashRepartitionEnumerator<TInputOutput, THashKey, TIgnoreKey>.Mutables m_mutables;
    private const int ENUMERATION_NOT_STARTED = -1;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal HashRepartitionEnumerator(QueryOperatorEnumerator<TInputOutput, TIgnoreKey> source, int partitionCount, int partitionIndex, Func<TInputOutput, THashKey> keySelector, HashRepartitionStream<TInputOutput, THashKey, int> repartitionStream, CountdownEvent barrier, ListChunk<Pair<TInputOutput, THashKey>>[,] valueExchangeMatrix, CancellationToken cancellationToken)
    {
      this.m_source = source;
      this.m_partitionCount = partitionCount;
      this.m_partitionIndex = partitionIndex;
      this.m_keySelector = keySelector;
      this.m_repartitionStream = repartitionStream;
      this.m_barrier = barrier;
      this.m_valueExchangeMatrix = valueExchangeMatrix;
      this.m_cancellationToken = cancellationToken;
    }

    internal override bool MoveNext(ref Pair<TInputOutput, THashKey> currentElement, ref int currentKey)
    {
      if (this.m_partitionCount == 1)
      {
        TIgnoreKey currentKey1 = default (TIgnoreKey);
        TInputOutput currentElement1 = default (TInputOutput);
        if (!this.m_source.MoveNext(ref currentElement1, ref currentKey1))
          return false;
        currentElement = new Pair<TInputOutput, THashKey>(currentElement1, this.m_keySelector == null ? default (THashKey) : this.m_keySelector(currentElement1));
        return true;
      }
      else
      {
        HashRepartitionEnumerator<TInputOutput, THashKey, TIgnoreKey>.Mutables mutables = this.m_mutables ?? (this.m_mutables = new HashRepartitionEnumerator<TInputOutput, THashKey, TIgnoreKey>.Mutables());
        if (mutables.m_currentBufferIndex == -1)
          this.EnumerateAndRedistributeElements();
        while (mutables.m_currentBufferIndex < this.m_partitionCount)
        {
          if (mutables.m_currentBuffer != null)
          {
            if (++mutables.m_currentIndex < mutables.m_currentBuffer.Count)
            {
              currentElement = mutables.m_currentBuffer.m_chunk[mutables.m_currentIndex];
              return true;
            }
            else
            {
              mutables.m_currentIndex = -1;
              mutables.m_currentBuffer = mutables.m_currentBuffer.Next;
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
              mutables.m_currentBuffer = this.m_valueExchangeMatrix[mutables.m_currentBufferIndex, this.m_partitionIndex];
          }
        }
        return false;
      }
    }

    private void EnumerateAndRedistributeElements()
    {
      HashRepartitionEnumerator<TInputOutput, THashKey, TIgnoreKey>.Mutables mutables = this.m_mutables;
      ListChunk<Pair<TInputOutput, THashKey>>[] listChunkArray = new ListChunk<Pair<TInputOutput, THashKey>>[this.m_partitionCount];
      TInputOutput currentElement = default (TInputOutput);
      TIgnoreKey currentKey = default (TIgnoreKey);
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
        ListChunk<Pair<TInputOutput, THashKey>> listChunk = listChunkArray[index];
        if (listChunk == null)
          listChunkArray[index] = listChunk = new ListChunk<Pair<TInputOutput, THashKey>>(128);
        listChunk.Add(new Pair<TInputOutput, THashKey>(currentElement, hashKey));
      }
      for (int index = 0; index < this.m_partitionCount; ++index)
        this.m_valueExchangeMatrix[this.m_partitionIndex, index] = listChunkArray[index];
      this.m_barrier.Signal();
      mutables.m_currentBufferIndex = this.m_partitionIndex;
      mutables.m_currentBuffer = listChunkArray[this.m_partitionIndex];
      mutables.m_currentIndex = -1;
    }

    protected override void Dispose(bool disposed)
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
      internal int m_currentIndex;

      internal Mutables()
      {
        this.m_currentBufferIndex = -1;
      }
    }
  }
}
